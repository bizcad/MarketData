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
        public static void MoveFiles(DirectoryInfo sourceRoot)
        {
            DirectoryInfo destRoot = new DirectoryInfo(@"H:\GoogleFinanceData\equity\usa\minute\");
            //DirectoryInfo root;

            var subdirs1 = sourceRoot.GetDirectories();

            foreach (DirectoryInfo info1 in subdirs1)
            {

                //System.Threading.Thread.Sleep(100);
                var subdirs2 = info1.GetDirectories();
                foreach (DirectoryInfo info3 in subdirs2)
                {

                    try
                    {
                        if (!Directory.Exists(destRoot.FullName + info3.Name))
                        {
                            try
                            {
                                destRoot.CreateSubdirectory(info3.Name);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{e.Message} \n {e.StackTrace}");
                            }
                        }

                        Debug.WriteLine(info3.Name);

                        var files = info3.GetFiles();
                        foreach (var file3 in files)
                        {
                            try
                            {
                                string oldname = file3.FullName;
                                string newname = destRoot.FullName + info3.Name + @"\" + file3.Name;
                                File.Move(oldname, newname);

                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + file3.FullName);
                            }
                        }
                        System.Threading.Thread.Sleep(250);
                        //                Directory.Delete(info3.FullName, true);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + info3.FullName);
                    }
                }
            }
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
                using (StreamWriter wr = new StreamWriter(symbolFileInfo.FullName.Replace("symbols","badsymbols"), false))
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
