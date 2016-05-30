using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MarketData.ToolBox;

namespace MarketData.GoogleFinance
{
    public static class FileMover
    {
        /// <summary>
        /// Copies all the files in a folder to a new folder overwritting the old
        /// </summary>
        /// <param name="sourceRoot"></param>
        /// <returns>bool true if sucessful</returns>
        public static bool CopyDailyFiles(DirectoryInfo sourceRoot)
        {
            DirectoryInfo destRoot = new DirectoryInfo(sourceRoot.FullName.Replace("L:", "H:"));

            if (!destRoot.Exists)
            {
                throw new DirectoryNotFoundException("Destination root not found");
            }


            var files = sourceRoot.GetFiles();
            foreach (var file3 in files)
            {
                try
                {
                    string oldname = file3.Name;
                    FileInfo oldpath = new FileInfo(sourceRoot.FullName + file3.Name);
                    FileInfo newpath = new FileInfo(destRoot.FullName + file3.Name);
                    oldpath.CopyTo(newpath.FullName, true);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            System.Threading.Thread.Sleep(250);


            return true;
        }
        /// <summary>
        /// Copies all the files in the minute folder.  No overwrite if destfile exists.
        /// </summary>
        /// <param name="sourceRoot"></param>
        /// <returns>bool true if sucessful</returns>
        public static bool CopyMinuteFiles(DirectoryInfo sourceRoot)
        {
            DirectoryInfo destRoot = new DirectoryInfo(sourceRoot.FullName.Replace("L:", "H:"));

            if (!destRoot.Exists)
            {
                throw new DirectoryNotFoundException("Destination root not found");
            }

            var dirs = sourceRoot.GetDirectories();
            foreach (var dir in dirs)
            {
                var files = dir.GetFiles();
                foreach (var file3 in files)
                {
                    try
                    {
                        FileInfo oldpath = new FileInfo(dir.FullName + "\\" + file3.Name);
                        FileInfo newpath = new FileInfo(oldpath.FullName.Replace("L:", "H:"));
                        if (!newpath.Exists)
                            oldpath.MoveTo(newpath.FullName);
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            System.Threading.Thread.Sleep(250);


            return true;
        }

        public static void RenameInteriorFiles(DirectoryInfo sourceRoot)
        {
            var subdirs1 = sourceRoot.GetDirectories();

            foreach (var file1 in subdirs1
                .Where(info1 => string.Compare(info1.Name, "ICB", StringComparison.Ordinal) >= 0)
                .Select(info1 => info1.GetFiles())
                .SelectMany(files => files))
            {
                try
                {
                    Compression.RenameInternal(file1);
                    //System.Threading.Thread.Sleep(150);
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message + file1.FullName);
                }
            }
        }

        /// <summary>
        /// Reads JJs symbols.txt and checks to see if the zip files exist.
        /// </summary>
        /// <param name="symbolFileInfo"></param>
        /// <returns>true if they all do</returns>
        public static bool CheckJJList(FileInfo symbolFileInfo)
        {
            List<string> noexchange = new List<string>();
            Dictionary<string, string> symbolDictionary = new Dictionary<string, string>();
            //SymbolListBuilder builder = new SymbolListBuilder();


            string symbols = string.Empty;
            using (StreamReader sr = new StreamReader(symbolFileInfo.FullName))
            {
                try
                {
                    while (!sr.EndOfStream)
                    {
                        string symbol = sr.ReadLine();
                        if (!string.IsNullOrEmpty(symbol))
                        {
                            symbolDictionary.Add(symbol, "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(ex.Message);
                }
            }
            var uriBuilder = new DownloadURIBuilder("NYSE", "AAMRQ");
            foreach (var ticker in symbolDictionary)
            {
                uriBuilder.SetTickerName(ticker.Key);
                uriBuilder.SetExchangeName(ticker.Value);
                var uri = uriBuilder.GetGetPricesUrlToDownloadAllData(DateTime.Now);
                var wClient = new WebClient();
                byte[] buf = wClient.DownloadData(uri);
                string rstring = Encoding.Default.GetString(buf);
                bool found = false;
                if (rstring.Length > 0)
                {
                    string[] lines = rstring.Split('\n');
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("a"))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        noexchange.Add(ticker.Key);
                }
            }

            // deletes any old bad csv files and writes new ones 
            SaveBadSymbolList(new FileInfo(symbolFileInfo.FullName.Replace("txt", "csv")), noexchange);
            if (noexchange.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine("There were bad symbols in JJs List");
                return false;
            }
            return true;
        }
        public static void SaveBadSymbolList(FileInfo symbolFileInfo, List<string> linelist)
        {
            // Save a sorted copy of the line list of symbols and names
            if (symbolFileInfo.Exists)
                symbolFileInfo.Delete();
            if (linelist.Count > 0)
                using (StreamWriter wr = new StreamWriter(symbolFileInfo.FullName.Replace("symbols", "badsymbols"), false))
                {
                    foreach (string line in linelist)
                    {
                        wr.WriteLine(line);
                    }
                    wr.Flush();
                }
        }
    }
}
