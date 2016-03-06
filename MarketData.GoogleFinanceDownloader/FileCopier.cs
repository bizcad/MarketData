using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketData.GoogleFinance;

namespace MarketData.GoogleFinanceDownloader
{
    public class FileCopier
    {
        const string symbolfile = @"I:\Dropbox\JJ\symbols.txt";
        public List<string> Symbols = new List<string>();

        public int CopyFiles()
        {
            int filesCopied = 0;

            SymbolsFromFile();
            foreach (string symbol in Symbols)
            {
                CopyMinute(symbol, out filesCopied);
                Console.WriteLine("Copied {0} Minute Files for Symbol {1}", filesCopied, symbol);
                CopyDaily(symbol.ToLower());

            }
            return filesCopied;
        }

        private void CopyDaily(string symbol)
        {

            string sourcefolder = @"H:\GoogleFinanceData\equity\usa\daily\";
            string destfolder = @"I:\Dropbox\JJ\data\equity\usa\daily\";
            FileInfo destfile = new FileInfo(destfolder + symbol + ".zip");
            FileInfo f = new FileInfo(sourcefolder + symbol + ".zip");
            if (f.Exists)
            {
                try
                {
                    File.Copy(f.FullName, destfile.FullName, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Console.WriteLine("Copied 1 daily file {0}", destfile.Name);
            }
            else
            {
                Console.WriteLine("File Not Copied {0}", f.Name);
                using (var sw = new StreamWriter(AssemblyLocator.ExecutingDirectory() + "CopyDailyFailed.txt", true))
                {
                    sw.WriteLine("File Not Copied {0}", f.Name);
                    sw.Flush();
                    sw.Close();
                }
            }
        }




        private static void CopyMinute(string symbol, out int filesCopied)
        {
            filesCopied = 0;
            string sourcefolder = @"H:\GoogleFinanceData\equity\usa\minute\" + symbol;
            string destfolder = @"I:\Dropbox\JJ\data\equity\usa\minute\" + symbol;
            var di = new DirectoryInfo(sourcefolder);
            if (di.Exists)
            {
                FileInfo[] sourceFileInfos = di.GetFiles();
                if (!Directory.Exists(destfolder))
                    Directory.CreateDirectory(destfolder);
                FileInfo[] destFileInfos = new DirectoryInfo(destfolder).GetFiles();
                foreach (FileInfo f in sourceFileInfos)
                {
                    string destfile = destfolder + @"\" + f.Name;
                    if (!File.Exists(destfile))
                    {
                        try
                        {
                            File.Copy(f.FullName, destfile);
                        }
                        catch (Exception e)
                        {
                            using (var sw = new StreamWriter(AssemblyLocator.ExecutingDirectory() + "CopyMinuteFailed.txt", true))
                            {
                                sw.WriteLine("File Not Copied {0}\n{1}\n{2}", f.Name, e.Message, e.StackTrace);
                                sw.Flush();
                                sw.Close();
                            }
                        }
                        filesCopied++;
                    }
                }
            }
        }

        private void SymbolsFromFile()
        {
            #region "Read Symbols from File"

            /**********************************************
                THIS SECTION IS FOR READING SYMBOLS FROM A FILE
            ************************************************/
            using (StreamReader sr = new StreamReader(symbolfile))
            {
                string[] symbols = { };
                var readLine = sr.ReadLine();
                while (readLine != null)
                {
                    symbols = readLine.Split(',');
                    Symbols.Add(symbols[0]);
                    readLine = sr.ReadLine();
                }
                sr.Close();
            }
            #endregion
        }



    }
}
