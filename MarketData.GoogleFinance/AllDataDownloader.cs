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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MarketData.ToolBox;

namespace MarketData.GoogleFinance
{
    /// <summary>
    /// a class to download all End-Of-Day bars from GoogleFinance
    /// </summary>
    public class AllDataDownloader
    {
        #region properties
        /// <summary>
        /// The exchange where the instrument is traded
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// The ticker symbol for to be downloaded
        /// </summary>
        public string Ticker { get; set; }
        /// <summary>
        /// The output directory for the data.
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// The path to the csv file for ticker/exchange symbols
        /// </summary>
        public string SymbolList { get; set; }
        /// <summary>
        /// The last entry in the csv file.
        /// </summary>
        public DateTime lastentry { get; set; }
        private DownloadURIBuilder _uriBuilder;
        /// <summary>
        /// 
        /// </summary>
        //        public Boolean FormatAsMilliseconds { get; set; }
        //        public Boolean SplitDays { get; set; }
        /// <summary>
        /// If true, the output will be zipped.  If false, csv
        /// </summary>
        public Boolean ZipOutput { get; set; }
        /// <summary>
        /// Empty constructor
        /// </summary>
        public AllDataDownloader() { }
        /// <summary>
        /// Parameter constructor for use with a single symbol and the default output OutputDirectory
        /// </summary>
        /// <param name="exchange">string - the exchange symbol</param>
        /// <param name="ticker">string - the ticker symbol</param>
        public AllDataDownloader(string exchange, string ticker)
        {
            Exchange = exchange;
            Ticker = ticker;
            OutputDirectory = Config.GetDefaultDownloadDirectory();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Parameter constructor to use for a single ticker symbol
        /// </summary>
        /// <param name="exchange">string - the exchange symbol</param>
        /// <param name="ticker">string - the ticker symbol</param>
        /// <param name="outputDirectory">string - the full path to the target OutputDirectory</param>
        public AllDataDownloader(string exchange, string ticker, string outputDirectory)
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
        public AllDataDownloader(FileInfo symbolList, string targetOutputDirectory)
        {
            SymbolList = symbolList.FullName;
            OutputDirectory = targetOutputDirectory;
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// Main entry point for the class.  Downloads all data for all symbols in a list
        /// </summary>
        /// <returns>Task - not used</returns>
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

            await LoopSymbolList(symbolList);

        }
        #endregion

        #region "private methods"

        private DateTime GetLastEntryDate(string filepath, string ticker)
        {
            string data = Compression.Unzip(filepath, ticker + ".csv");
            int index = data.LastIndexOf("\n", System.StringComparison.Ordinal) - 100;
            string lastline = data.Substring(index);
            lastline = lastline.Substring(lastline.LastIndexOf("\n", System.StringComparison.Ordinal) + 1);

            int year = System.Convert.ToInt32(lastline.Substring(0, 4));
            int month = Convert.ToInt32(lastline.Substring(4, 2));
            int day = Convert.ToInt32(lastline.Substring(6, 2));
            return new DateTime(year, month, day);

        }

        /// <summary>
        /// Loops through a symbol list, looking up the exchange where the security is traded
        ///  and writing the files to an {exchange}\{firstletter}\{symbol} folder
        /// </summary>
        /// <param name="symbolList">A list of symbols</param>
        /// <returns>Task (void)</returns>
        private async Task LoopSymbolList(Dictionary<string, string> symbolList)
        {
            List<string> files = new List<string>();
            foreach (string ticker in symbolList.Keys)
            {

                string symbol = ticker.Replace("^", "-");
                if (ticker.Contains(@"\") || ticker.Contains(@"/"))
                    continue;

                string outputFolder;
                if (OutputDirectory != null || OutputDirectory.Length > 0)
                {
                    if (!OutputDirectory.EndsWith(@"\"))
                    {
                        OutputDirectory += @"\";
                    }
                    outputFolder = OutputDirectory; // The factory addes the daily
                }
                else
                {
                    outputFolder = Config.GetDefaultDownloadDirectory();
                }
                DirectoryInfo _qcInfo = new DirectoryInfo(outputFolder);
                DirectoryInfo dailyDirectoryInfo = DailyDirectoryFactory.Create(_qcInfo);

                string x = "";
                if (symbol == "ACST")
                    x = "here";

                var uri = _uriBuilder.GetGetPricesUrlToDownloadAllData(DateTime.Now);

                string fn = dailyDirectoryInfo.FullName + ticker.ToLower() + ".zip";
                if (File.Exists(fn))
                {
                    FileInfo f = new FileInfo(fn);
                    DateTime lastentry = GetLastEntryDate(fn, ticker);
                    DateTime endDateTime =
                        new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);  // the routine adds a day
                    if (lastentry.Equals(endDateTime))
                        continue;

                    uri = _uriBuilder.GetGetPricesUrlForRecentData(lastentry.AddDays(1), endDateTime);
                }
                _uriBuilder.SetTickerName(ticker);
                //_uriBuilder.SetExchangeName(exchangeDirectoryInfo.Name);

                // download Data
                Ticker = ticker;
                WebClient wClient = new WebClient();
                NameValueCollection myQueryStringCollection = new NameValueCollection();
                myQueryStringCollection.Add("symbol", ticker);
                myQueryStringCollection.Add("OutputDirectory", dailyDirectoryInfo.FullName);
                wClient.QueryString = myQueryStringCollection;
                wClient.DownloadDataCompleted += wClient_DownloadDataCompleted;
                await wClient.DownloadDataTaskAsync(uri);
            }
            return;
        }


        /// <summary>
        /// Handler for the Window Client Download Data Complete event.  Saves the historical data to a file
        /// </summary>
        /// <param name="sender">object - The WebClient</param>
        /// <param name="e">The data returned by the downloader.</param>
        private async void wClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            WebClient client = (WebClient)sender;
            var ticker = client.QueryString["symbol"];
            DirectoryInfo dir = new DirectoryInfo(client.QueryString["OutputDirectory"]);
            string errorMessage;
            DataProcessor processor = new DataProcessor();
            using (MemoryStream ms = new MemoryStream(e.Result))
            {
                string historicalData = processor.processStreamMadeOfOneDayLinesToExtractHistoricalData(ms, out errorMessage);
                System.Diagnostics.Debug.WriteLine(ticker);
                Console.WriteLine("D " + ticker);
                await SaveDataAsync(dir.FullName, historicalData, ticker);
            }
        }
        /// <summary>
        /// Saves the data to a csv or zip file
        /// </summary>
        /// <param name="directory">string - the OutputDirectory to write the data to</param>
        /// <param name="historicalData">string - the formatted data from the DataProcessor</param>
        /// <param name="ticker">string - the symbol to write</param>
        /// <returns>Task</returns>
        private async Task<int> SaveDataAsync(string directory, string historicalData, string ticker)
        {
            DateTime dt = DateTime.Now;
            string[] lines = historicalData.Split('\n');
            StringBuilder sb = new StringBuilder();
            string data = string.Empty;
            string filepath;
            var internalFilename = CreateFilename(directory, ticker, out filepath);
            filepath = filepath.Replace(".csv", ".zip");
            if (File.Exists(filepath))
            {
                data = Compression.Unzip(filepath, internalFilename);
                sb.AppendLine(data.Trim());
            }


            foreach (string line in lines)
            {
                try
                {
                    // Sometimes there is an extra CRLF at the end of the last record.  The last line is blank.
                    //  Treat it as though it is an EOF marker.
                    if (line.Length == 0)
                    {
                        await WriteStreamAsync(dt, sb.ToString(), directory, ticker);
                        return 1;
                    }
                    try
                    {
                        string[] elements = line.Split(',');
                        // skip the first row if it is header
                        if (elements[0] == "Date")
                            continue;

                        // skip if the open == 0.  The open on some symbols before 1/3/2000 is 0.  WTF?
                        try
                        {
                            if (Convert.ToDecimal(elements[1]) == 0)
                                continue;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        elements[elements.Length - 1] = elements[elements.Length - 1].Replace("\r", string.Empty);
                        string record = ColumnJoiner.JoinColumns(elements);
                        //20151030 00:00,
                        string datestring = line.Substring(0, 15);
                        if (!data.Contains(datestring))
                        {
                            sb.AppendLine(line.Trim());
                        }
                        //sb.AppendLine(record);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            await WriteStreamAsync(dt, sb.ToString().Trim(), directory, ticker);
            return 1;
        }
        /// <summary>
        /// Writes a zip or csv file if it does not exist. 
        /// </summary>
        /// <param name="tradingDate">DateTime - The trading date for the downloaded data; used for the file name</param>
        /// <param name="buffer">string - the contents of the file to be written</param>
        /// <param name="directory">string - the OutputDirectory.FullPath</param>
        /// <param name="ticker">string - the ticker symbol used in the OutputDirectory name and zipped file name</param>
        /// <returns>nothing</returns>
        public async Task<int> WriteStreamAsync(DateTime tradingDate, string buffer, string directory, string ticker)
        {
            string filepath;
            var filename = CreateFilename(directory, ticker, out filepath);

            // No point if writing if there is no data in the buffer 
            //  For example, on a bank holiday
            if (buffer.Length > 0)
            {
                if (ZipOutput)
                {
                    string zippath = filepath.Replace("csv", "zip");
                    Compression.Zip(zippath, filename, buffer);
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(filepath))
                    {
                        await sw.WriteAsync(buffer);
                        await sw.FlushAsync();
                    }
                }
            }
            return buffer.Length;
        }

        private static string CreateFilename(string directory, string ticker, out string filepath)
        {
            if (directory.Length == 0)
                directory = Config.GetDefaultDownloadDirectory();
            if (!directory.EndsWith(@"\"))
                directory += @"\";
            if (ticker.Length == 0)
                throw new NullReferenceException("Ticker symbol is null");

            string folder = directory;
            string filename = ticker.ToLower() + ".csv";

            filepath = folder + filename;
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            return filename;
        }

        #endregion
    }

}

