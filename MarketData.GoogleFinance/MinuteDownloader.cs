/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MarketData.ToolBox;

namespace MarketData.GoogleFinance
{
    /// <summary>
    /// Class for downloading minute data from GoogleFinance.  
    /// NOTE: GoogleFinance will give you a maximum of the last 15 days
    /// </summary>
    public class MinuteDownloader
    {
        /// <summary>
        /// The exchange used to download a single symbol
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// The ticker symbol used to download a single symbol
        /// </summary>
        public string Ticker { get; set; }
        /// <summary>
        /// The full path to the root output directory
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// The full path to the list of symbols and exchanges as a csv file
        /// </summary>
        public string SymbolList { get; set; }

        private WebClient _wClient;
        private DownloadURIBuilder _uriBuilder;
        /// <summary>
        /// If true use millisecond formatting as QuantConnect expects.  
        /// If false use date time formattting as other programs may expect.
        /// </summary>
        public Boolean FormatAsMilliseconds { get; set; }
        /// <summary>
        /// If true, minute data for multiple days in one download file will be split into
        /// separate day files yyyymmdd_trade.zip or .csv
        /// </summary>
        public Boolean SplitDays { get; set; }
        /// <summary>
        /// If true, the outputfile will be zipped.  If false, outputfile will be csv
        /// </summary>
        public Boolean ZipOutput { get; set; }
        public Logger logger { get; set; }
        /// <summary>
        /// Empty constructor
        /// </summary>
        public MinuteDownloader() { }
        /// <summary>
        /// Parameter constructor for use with a single symbol and the default output OutputDirectory
        /// </summary>
        /// <param name="exchange">string - the exchange symbol</param>
        /// <param name="ticker">string - the ticker symbol</param>
        public MinuteDownloader(string exchange, string ticker)
        {
            Exchange = exchange;
            Ticker = ticker;
            OutputDirectory = Config.GetDefaultDownloadDirectory();
        }
        /// <summary>
        /// Parameter constructor to use for a single ticker symbol
        /// </summary>
        /// <param name="exchange">string - the exchange symbol</param>
        /// <param name="ticker">string - the ticker symbol</param>
        /// <param name="outputDirectory">string - the full path to the target OutputDirectory</param>
        public MinuteDownloader(string exchange, string ticker, string outputDirectory)
        {
            Exchange = exchange;
            Ticker = ticker;
            OutputDirectory = outputDirectory;
        }
        /// <summary>
        /// Parameter Constructor
        /// </summary>
        /// <param name="symbolList">FileInfo - an info file pointing to the csv file with the list in it.</param>
        /// <param name="targetOutputDirectory">string - the name of the root OutputDirectory for output</param>
        /// <remarks>The list should be the ticker symbol in the first column and the exchange symbol in the second column.
        /// However, if the second column is blank, the program will ask GoogleFinance for the exchange using the symbol.
        /// </remarks>
        public MinuteDownloader(FileInfo symbolList, string targetOutputDirectory)
        {
            SymbolList = symbolList.FullName;
            OutputDirectory = targetOutputDirectory;
        }

        /// <summary>
        /// Entry point for downloading data from a list
        /// </summary>
        /// <returns>nothing</returns>
        public async Task DownloadDataFromListAsync()
        {
            FileInfo symbolFileInfo = new FileInfo(SymbolList);
            if (!symbolFileInfo.Exists)
            {
                throw new FileNotFoundException(symbolFileInfo.FullName);
            }
            // Build a list of symbols from the symbol file
            var symbolList = new SymbolListBuilder().BuildListFromFile(symbolFileInfo);

            // use a dummy symbol to create the object
            _uriBuilder = new DownloadURIBuilder(Exchange, "A");
            if (_wClient == null)
            {
                _wClient = new WebClient();
                _wClient.DownloadDataCompleted += wClient_DownloadDataCompleted;
            }
            await LoopSymbolList(symbolList);

        }

        /// <summary>
        /// Loops through a symbol list, looking up the exchange where the security is traded
        ///  and writing the files to an {exchange}\{firstletter}\{symbol} folder
        /// </summary>
        /// <param name="symbolList">A list of symbols</param>
        /// <returns>Task (void)</returns>
        private async Task LoopSymbolList(Dictionary<string, string> symbolList)
        {
            foreach (string ticker in symbolList.Keys)
            {
                //if (System.String.Compare(ticker, "QCOM", System.StringComparison.Ordinal) <= 0)
                //    continue;
                //DirectoryInfo exchangeDirectoryInfo;
                string symbol = ticker.Replace("^", "-").Trim();
                if (symbol.Contains(@"\") || symbol.Contains(@"/"))
                    continue;

                // Look up the exchange on GoogleFinance if it is not in the symbolList item
                string exchange;
                var kvpair = symbolList.FirstOrDefault(s => s.Key == ticker);
                if (kvpair.Value == null)
                {
                    // Look up the exchange from GoogleFinance
                    ExchangeLookup exchangeLookup = new ExchangeLookup();
                    exchange = exchangeLookup.GetExchangeForSymbol(ticker);
                }
                else
                {
                    exchange = kvpair.Value;
                }
                // used in debugging
                if (ticker == "ABEV")
                    Debug.WriteLine("here");

                DirectoryInfo outputFolder;
                if (OutputDirectory != null || OutputDirectory.Length > 0)
                {
                    if (!OutputDirectory.EndsWith(@"\"))
                    {
                        OutputDirectory += @"\";
                    }
                    outputFolder = new DirectoryInfo(OutputDirectory);  // the factory adds the "minute";
                }
                else
                {
                    outputFolder = new DirectoryInfo(Config.GetDefaultDownloadDirectory());
                }
                
                DirectoryInfo symbolDirectoryInfo = SymbolDirectoryFactory.Create(MinuteDirectoryFactory.Create(outputFolder), symbol);
                // find out if files have been downloaded to this OutputDirectory before.  
                //  If not get the max available from Google Finance (15 days)
                //  Otherwise get the files that have not been downloaded.
                var files = symbolDirectoryInfo.GetFiles().OrderBy(f => f.Name);
                int numberOfDays;
                if (!files.Any())
                {
                    numberOfDays = 15;
                }
                else
                {
                    var lastfile = files.LastOrDefault();
                    numberOfDays = NumberOfDaysSinceLastDownload(lastfile);
                    //numberOfDays = 3;
                }


                _uriBuilder.SetTickerName(symbol);
                _uriBuilder.SetExchangeName(exchange);

                var uri = _uriBuilder.GetGetPricesUrlForLastNumberOfDays(numberOfDays);
                // download Data
                Ticker = ticker;       // this assignment is superflous because the ticker is returned in the header

                // Set the QueryString in the Header to the symbol and OutputDirectory
                //  so they will be returned in the DownloadDataCompleted event handler
                NameValueCollection myQueryStringCollection = new NameValueCollection
                {
                    {"symbol", ticker},
                    {"OutputDirectory", symbolDirectoryInfo.FullName}
                };
                _wClient.QueryString = myQueryStringCollection;
                // Get the data async
                await _wClient.DownloadDataTaskAsync(uri);

            }
        }

        /// <summary>
        /// Handle the DownloadDataCompleted event when the WebClient returns the data.
        /// </summary>
        /// <param name="sender">WebClient - the sender</param>
        /// <param name="e">the data from the return</param>
        /// <returns>nothing</returns>
        private async void wClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            WebClient client = (WebClient)sender;
            var ticker = client.QueryString["symbol"];

            DirectoryInfo dir = new DirectoryInfo(client.QueryString["OutputDirectory"]);

            using (MemoryStream ms = new MemoryStream(e.Result))
            {
                await ProcessAndSaveMinuteDataByDay(ms, ticker, dir);
            }
        }

        /// <summary>
        /// Process the downloaded minute data for one symbol and call the save method
        /// </summary>
        /// <param name="symbol">string - The symbols</param>
        /// <param name="singleLetterDirectoryInfo">DirectoryInfo - the folder to save minute files</param>
        private async Task ProcessAndSaveMinuteDataByDay(MemoryStream ms, string symbol, DirectoryInfo singleLetterDirectoryInfo)
        {
            DataProcessor dp = new DataProcessor();
            string errorMessage;
            string resultValue = dp.ProcessStreamOfOneMinuteBarsToReplaceGoogleDateWithFormatted(ms, "Millisecond", out errorMessage);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                string message = $"{symbol} {DateTime.Now} Minute Symbol had an error. {errorMessage}";
                logger.Log(message);

            }
            Console.WriteLine("M " + symbol);
            await SaveDataAsync(singleLetterDirectoryInfo.FullName, resultValue, symbol);

        }

        /// <summary>
        /// Saves the data day by day
        /// </summary>
        /// <param name="directory">string - the OutputDirectory to write the data to</param>
        /// <param name="historicalData">string - the formatted data from the DataProcessor</param>
        /// <param name="ticker">string - the symbol to write</param>
        /// <returns>Task</returns>
        private async Task SaveDataAsync(string directory, string historicalData, string ticker = @"")
        {
            DateTime tradingDate = DateTime.Now;
            int day = 0;
            string[] lines = historicalData.Split('\n');
            StringBuilder sb = new StringBuilder();

            foreach (string line in lines)
            {
                // When there is an extra CR at the end of the last line of the file,
                //  the final blank line is treated as an EOF
                if (line.Length == 0)
                {
                    await WriteStreamAsync(tradingDate, sb.ToString(), directory, ticker);
                }
                else
                {
                    string[] elements = line.Split(',');
                    if (FormatAsMilliseconds) // Milliseconds
                    {
                        tradingDate = System.Convert.ToDateTime(elements[elements.Length - 1]);  // If the format is milliseconds, get the date from the last column of the input file
                    }
                    else
                    {
                        DateTime.TryParse(elements[0], out tradingDate);
                        if (tradingDate.Year == 1)
                            throw new InvalidDataException("The date format is invalid.  You need to set FormatAsMilliseconds to true");
                    }
                    if (tradingDate.Day != day)
                    {
                        if (day > 0)
                        {
                            await WriteStreamAsync(tradingDate, sb.ToString(), directory, ticker);
                        }
                        sb = new StringBuilder();
                        day = tradingDate.Day;
                    }
                    string record = line.Substring(0, line.LastIndexOf(",", System.StringComparison.Ordinal));
                    sb.AppendLine(record);
                }
            }
        }
        #region "Recursive Zip files"
        /*
         * This section is not used in the test application.  
         * It should be run from the test runner.  
         * It's purpose is to clean up any csv files left over during development
         */
        /// <summary>
        /// Entry point to recursivly zip all csv files.
        /// </summary>
        public void FindAndZipCsvFiles(DirectoryInfo rootDownloadDirectory)
        {
            ZipCsvFiles(rootDownloadDirectory);
        }
        /// <summary>
        /// Recursively zip all the files in the dir and then do so for all the subdirs
        /// </summary>
        /// <param name="sourceDirectory">DirectoryInfo - root download dir</param>
        private void ZipCsvFiles(DirectoryInfo sourceDirectory)
        {
            // In the lowest level OutputDirectory the symbol is the name of the OutputDirectory
            string ticker = sourceDirectory.Name;

            // zip all the files in the current dir.
            //  In parent directories there probably will not be any csv files so this step will be skipped
            foreach (var file in sourceDirectory.EnumerateFiles("*.csv"))
            {
                ZipDirectoryFiles(sourceDirectory.FullName, ticker);
            }
            // If the OutputDirectory is empty delete it
            if (!sourceDirectory.GetDirectories().Any() && !sourceDirectory.GetFiles().Any())
            {
                Debug.WriteLine("Deleting " + sourceDirectory.FullName);
                sourceDirectory.Delete();
            }
            else
            {
                foreach (var directory in sourceDirectory.EnumerateDirectories())
                {
                    ZipCsvFiles(directory);     // recurse into each subdirectory
                }

            }
        }
        private void ZipDirectoryFiles(string directory, string ticker)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            foreach (FileInfo file in dir.GetFiles(@"*.csv"))
            {

                string filename = file.Name;
                string filepath = file.FullName;
                string zippath = file.FullName.Replace("csv", "zip");
                using (StreamReader reader = new StreamReader(filepath))
                {
                    string historicalData = reader.ReadToEnd();
                    if (historicalData.Length > 0)
                    {
                        Compression.Zip(zippath, filename, historicalData);
                        // byte[] buf = Encoding.ASCII.GetBytes(historicalData);
                        //using (var fs = File.Create(zippath, buf.Length))
                        //{
                        //    using (var s = new ZipOutputStream(fs))
                        //    {
                        //        var ret = filename.Replace("_trade.csv", "_" + ticker.ToLower() + "_Trade_Minute.csv");
                        //        s.PutNextEntry(ret);
                        //        s.Write(buf, 0, buf.Length);
                        //        s.Flush();
                        //    }
                        //}
                    }
                }
                System.Threading.Thread.Sleep(100);
                if (File.Exists(zippath))
                {
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }
                }
            }

        }
        #endregion

        /// <summary>
        /// Writes a zip or csv file if it does not exist. 
        /// </summary>
        /// <param name="tradingDate">DateTime - The trading date for the downloaded data; used for the file name</param>
        /// <param name="buffer">string - the contents of the file to be written</param>
        /// <param name="directory">string - the OutputDirectory.FullPath</param>
        /// <param name="ticker">string - the ticker symbol used in the OutputDirectory name and zipped file name</param>
        /// <returns>nothing</returns>
        public async Task WriteStreamAsync(DateTime tradingDate, string buffer, string directory, string ticker)
        {
            if (directory.Length == 0)
                directory = Config.GetDefaultDownloadDirectory();
            if (!directory.EndsWith(@"\"))
                directory += @"\";

            // No point if writing if there is no data in the buffer, or there is no symbol
            //  For example, on a bank holiday
            if (ticker.Length == 0 || buffer.Length == 0)
                //throw new NullReferenceException("Ticker symbol is null");
                return;  // cannot write

            string folder = directory;
            string filename = FilenameDateFormatter.FormatDt(tradingDate) + "_trade.csv";

            string filepath = folder + filename;
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);


            string zippath = filepath.Replace("csv", "zip");

            // Check to see if the trading day is today, and if so always overwrite the current day's file.  
            // If we download during the trading day, we will get the latest day's trades
            //  up to the amount of delay Google builds into their quote mechanism.  (I think 15 minutes)
            // If the current hour is after say 4:45 GMT-5 (East Coast time) we will get all of the day's prices
            if (tradingDate.Day == DateTime.Now.Day)
            {
                // If today is Saturday or Sunday, the equity markets are closed.
                // ********** To Do ***********
                // Check for bank holidays for the exchange
                if (!(DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday))
                {
                    if (ZipOutput)
                    {
                        Compression.Zip(zippath, filename, buffer);
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(filepath, false))
                        {
                            await sw.WriteAsync(buffer);
                            await sw.FlushAsync();
                        }
                    }

                }
            }

            // Because of the above if block, a file will have been written for today.
            if (ZipOutput)
            {
                // If a file has been written in the past and exists do not overwrite it.
                // Otherwise write the file.
                // ****  To overwrite all 15 days uncomment the following two lines *** //
                //if (File.Exists(zippath))
                //    File.Delete(zippath);
                if (!(File.Exists(zippath)))
                {
                    Compression.Zip(zippath, filename, buffer);
                }
            }
            else
            {
                // If a file has been written in the past and exists do not overwrite it.
                // Otherwise write the file.
                if (!File.Exists(filepath))
                {
                    using (StreamWriter sw = new StreamWriter(filepath))
                    {
                        sw.Write(buffer);
                        sw.Flush();
                    }
                }
            }
        }

        /// <summary>
        /// Counts the number of days since the last download by looking at the last file written
        /// and decomposing the file name yyyymmdd_trade.zip into a DateTime and counting the
        /// number of days up to Now
        /// </summary>
        /// <param name="lastfile">FileInfo - the last file written to the OutputDirectory</param>
        /// <returns>int - the number of days since the last file was written</returns>
        private static int NumberOfDaysSinceLastDownload(FileInfo lastfile)
        {
            string fn = lastfile.Name;
            int year = System.Convert.ToInt32(lastfile.Name.Substring(0, 4));
            int mo = System.Convert.ToInt32(lastfile.Name.Substring(4, 2));
            int day = System.Convert.ToInt32(lastfile.Name.Substring(6, 2));
            DateTime dt = new DateTime(year, mo, day);
            List<DateTime> daylist = new List<DateTime> { dt };
            while (dt.Day != DateTime.Now.Day)
            {
                dt = dt.AddDays(1.0);
                if (!(dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday))
                {
                    daylist.Add(dt);
                }
            }
            return daylist.Count;
        }

    }
}