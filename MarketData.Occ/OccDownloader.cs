using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using MarketData.Occ.Models;

namespace MarketData.Occ
{
    public class OccDownloader
    {
        private const string ListedProductsDownloadUrl =
            @"http://www.theocc.com/webapps/daily-delo-download?reportDate={ReportDate}&format=txt";
        /// <summary>
        /// The HtmlDocument for HtmlAgilityPack
        /// </summary>
        public HtmlDocument Document;
        /// <summary>
        /// The base folder for writing files.
        /// </summary>
        public string StorageFolder = @"H:\PainStrike\Occ\";

        //private string rowSelector = @"//table[@class='tablesorter']";
        private string tableHeadSelector = @"//table[@class='tablesorter']//thead//tr//th";
        private string tableRowSelector = @"//table[@class='tablesorter']//tbody//tr";

        private string tableHeading;
        private List<string> tableList;

        public void GetDocument()
        {
            Document = new HtmlDocument();
            Document.Load(@"H:\PainStrike\SPXPM.htm");
            //var web = new HtmlWeb();
            //string url = Baseurl.Replace("{option}", pageMode);
            //Debug.WriteLine(url);
            //Document = web.Load(url);


        }

        

        public List<ListedOption> GetContractList()
        {
            string contents = string.Empty;
            var contractList = new List<ListedOption>();
            string uri = ListedProductsDownloadUrl.Replace("{ReportDate}", "20160511");
            uri = @"http://www.optionseducation.org/quotes.html?quote=SPX";
            WebClient wClient = new WebClient();
            wClient.DownloadDataCompleted += wClient_DownloadDataCompleted;
            Stream stream = wClient.OpenRead(uri);
            StreamReader sr = new StreamReader(stream);
            {
                while (!sr.EndOfStream)
                {
                    var readLine = sr.ReadLine();
                    if (!string.IsNullOrEmpty(readLine))
                    {
                        var line = readLine;
                        //ListedOption o = new ListedOption
                        //{
                        //    OptionSymbol = line[0],
                        //    UnderlyingSymbol = line[1],
                        //    SymbolName = line[2],
                        //    OnnProductType = line[3],
                        //    PostionLimit = line[4],
                        //    Exchanges = line[5]
                        //};
                        //contractList.Add(o);
                    }
                }
                sr.Close();
            }

            return contractList;

        }

        private void wClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public List<string> GetTableFromHtml()
        {
            tableList = new List<string>();
            GetDocument();
            var ppage = Document.DocumentNode;
            GetTableHeading(ppage);
            tableList.Add(tableHeading);
            GetTableCsvList(ppage);
            SaveRowList("Occ", tableList);
            
            return tableList;
        }

        private void GetTableCsvList(HtmlNode ppage)
        {
            HtmlNodeCollection tableNodes = ppage.SelectNodes(tableRowSelector);
            foreach (var node in tableNodes)
            {
                tableList.Add(CsvFromTableRowNode(node));
            }
        }

        private void GetTableHeading(HtmlNode ppage)
        {
            HtmlNodeCollection tableHeadNodes = ppage.SelectNodes(tableHeadSelector);
            StringBuilder sb = new StringBuilder();
            foreach (var node in tableHeadNodes)
            {
                sb.Append("\"");
                sb.Append(node.InnerText);
                sb.Append("\"");
                sb.Append(",");
            }
            tableHeading = sb.ToString();
            tableHeading = tableHeading.Substring(0, tableHeading.Length - 1);
            
        }

        public List<OccRecord> CsvToObjectList(List<string> tableList)
        {
            List<OccRecord> list = new List<OccRecord>();
            for (int i = 1; i < tableList.Count; i++)
            {
                string[] arr = tableList[i].Split(',');
                OccRecord record = new OccRecord();
                record.Id = 0;
                record.Symbol = QuoteFilter(arr[0]);
                record.ContractDate = DateTime.Parse(arr[2].Replace("\"",string.Empty) + " " + arr[3] + ", " + arr[1]);
                record.Strike = decimal.Parse(arr[4] + "." + arr[5]);
                record.CP = QuoteFilter(arr[6]);
                record.CallOpenInterest = decimal.Parse(arr[7]);
                record.PutOpenInterest = decimal.Parse(arr[8]);
                record.PositonLimit = decimal.Parse(arr[9]);
                list.Add(record);
            }

            return list;
        }

        private string QuoteFilter(string s)
        {
            return s.Replace("\"", string.Empty);
        }
        private static string CsvFromTableRowNode(HtmlNode node)
        {
            var sb = new StringBuilder();
            var tds = node.ChildNodes;
            foreach (var td in tds.Where(n => n.Name == "td"))
            {
                string s = td.InnerText.Trim();
                string s1 = Regex.Replace(s, @"[\t\n\r,]", string.Empty).Replace("&nbsp", string.Empty);
                //int val = 0;
                //var x = int.TryParse(s1, out val);
                //if (!x)
                //    sb.Append("\"");
                sb.Append(s1);
                //if (!x)
                //    sb.Append("\"");
                sb.Append(",");
            }

            string tr = sb.ToString();
            tr = tr.Substring(0, tr.Length - 1);
            return tr;
        }

        #region "    Save files"
        /// <summary>
        /// Saves the list of rows from the web page table to a csv file
        /// </summary>
        /// <param name="pageMode">the contract abbreviation</param>
        private void SaveRowList(string pageMode, List<string> CsvRowsList)
        {
            string filename = GetRowListFilename(pageMode);
            using (StreamWriter sw = new StreamWriter(filename, false))
            {

                foreach (var item in CsvRowsList)
                {
                    sw.WriteLine(item);
                }
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// Gets the name of the csv file generated from the web page table
        /// </summary>
        /// <param name="pageMode">The contract name abbreviation</param>
        /// <returns>The full path to the folder</returns>
        /// <remarks>
        /// For example: H:\PainStrike\Data20160421\ESK16.csv
        /// </remarks>
        public string GetRowListFilename(string pageMode)
        {
            StringBuilder sb = new StringBuilder(GetDataFolder());
            sb.Append(pageMode);
            sb.Append("_");
            sb.Append("TableRows");
            sb.Append("_");
            AddDateToFilename(sb);
            sb.Append(".csv");
            return sb.ToString();
        }

        private void AddDateToFilename(StringBuilder sb)
        {
            sb.Append(DateTime.Now.Year);
            if (DateTime.Now.Month < 10)
                sb.Append("0");
            sb.Append(DateTime.Now.Month);
            if (DateTime.Now.Day < 10)
                sb.Append("0");
            sb.Append(DateTime.Now.Day);
        }

        private string GetDataFolder()
        {
            GetStorageFolder();

            StringBuilder sb = new StringBuilder();
            DateTime dt = DateTime.Now;

            sb.Append(StorageFolder);
            sb.Append(@"Data");
            sb.Append(dt.Year);
            if (dt.Month < 10)
                sb.Append("0");
            sb.Append(dt.Month);
            if (dt.Day < 10)
                sb.Append("0");
            sb.Append(dt.Day);
            sb.Append(@"\");

            DirectoryInfo di = new DirectoryInfo(sb.ToString());
            if (!di.Exists)
            {
                Directory.CreateDirectory(di.FullName);
            }
            return di.FullName;
        }
        /// <summary>
        /// Assures that the directory exists for the StorageFolder property
        /// </summary>
        private void GetStorageFolder()
        {
            DirectoryInfo di = new DirectoryInfo(StorageFolder);
            if (!di.Exists)
            {
                Directory.CreateDirectory(di.FullName);
            }
        }
        #endregion 
    }
}
