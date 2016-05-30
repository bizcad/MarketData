using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MarketData.ToolBox.Utility;

namespace MarketData.Barchart
{
    public class WebSolDownloader
    {
        #region "private fields"
        private List<string> headerList = new List<string>();

        private const string baseurl =
            @"http://newpl.websol.barchart.com/?module=futureDetail&region=US&symbol={option}&selected=futureOptions";
        //    @"http://www.barchart.com/commodityfutures/E-Mini_S&P_500_Futures/options/{option}?mode=d&view=";
        private string rowSelector = @"//table[@class='datatable_simple']";
        //#yui_3_5_1_1_1460835901563_162
        private string headerselector = @"//div[@class='bc_futureOptions_header']";
        //private string optionSelector = @"//div[@id='divContent']//td//table//td//option";
        private string symbolSelector = @"//select";
        #endregion
        #region "public properties"
        /// <summary>
        /// The heading row for the csv file as a comma delimited values
        /// </summary>
        public string CsvHeader = string.Empty;
        /// <summary>
        /// The table rows for the csv file as a list of comma delimited values
        /// </summary>
        public List<string> CsvRowsList = new List<string>();
        /// <summary>
        /// A dictionary of table heading name/value pairs
        /// </summary>
        public List<string> Headers = new List<string>();
        /// <summary>
        /// A dictionary of strike prices and open interest for puts
        /// </summary>
        public Dictionary<decimal, int> PutsDictionary = new Dictionary<decimal, int>();
        /// <summary>
        /// A dictionary of strike prices and open interest for calls
        /// </summary>
        public Dictionary<decimal, int> CallsDictionary = new Dictionary<decimal, int>();
        /// <summary>
        /// The base folder for writing files.
        /// </summary>
        public string StorageFile = @"H:\PainStrike\{pageMode}.csv";
        /// <summary>
        /// A CsvRowsList of possible contracts from the web site.
        /// </summary>
        public List<string> ContractList;
        /// <summary>
        /// The HtmlDocument for HtmlAgilityPack
        /// </summary>
        public HtmlDocument Document;
        #endregion
        #region "public methods"
        /// <summary>
        /// Gets the web page as a HtmlAgilityPack Document
        /// </summary>
        /// <param name="pageMode"></param>
        public void GetDocument(string pageMode)
        {
            var web = new HtmlWeb();
            string url = baseurl.Replace("{option}", pageMode);
            Document = web.Load(url);
        }
        /// <summary>
        /// Gets a CsvRowsList of contracts from the dropdown CsvRowsList on the page
        /// </summary>
        /// <returns>A CsvRowsList of strings with the contract names</returns>
        public List<string> GetContractList(string pageMode)
        {
            ContractList = new List<string>();
            GetDocument(pageMode);
            var ppage = Document.DocumentNode;
            var optionNodes = ppage.SelectNodes(symbolSelector);
            // get the selections
            //foreach (var optionNode in optionNodes.Where(optionNode => optionNode.Attributes["value"].Value?.Length > 0))
            foreach (var node in optionNodes[0].ChildNodes.Where(n => !n.Name.Contains("#text")))
            {
                ContractList.Add(node.Attributes["value"].Value);
            }
            return ContractList;
        }

        /// <summary>
        /// Gets the heading information from the page
        /// </summary>
        /// <param name="pageMode"></param>
        /// <returns></returns>
        public List<string> GetHeaderDictionary(string pageMode)
        {
            Headers = new List<string>();
            GetDocument(pageMode);
            var ppage = Document.DocumentNode;
            var header = ppage.SelectNodes(headerselector);
            var key = string.Empty;
            var val = string.Empty;
            

            // Get the first 4 headers
            foreach (var node in header[0].ChildNodes.Where(n => !n.Name.Contains("#text")))
            {
                if (!node.InnerText.Contains("Select Month"))
                {
                    foreach (var att in node.Attributes)
                    {
                        if (att.Name.Contains("class"))
                        {
                            string inner = Regex.Replace(node.InnerText, @"\t*\n*", "").Replace("&nbsp;","");
                            if (inner.Length > 0)
                            {
                                var array = inner.Split(' ');
                                foreach (string s in array)
                                {
                                    Headers.Add(filterValue(s));
                                }
                            }
                            
                        }
                    }
                    //arr = node.InnerText.Split(':');
                    //Headers.Add(arr[0], filterValue(arr[1]));
                }
            }

            // Only get the header for Daily Options
            // it is the second header table and the 6th child node
            //arr = header[1].ChildNodes[5].InnerText.Split(':');
            //Headers.Add(arr[0], filterValue(arr[1]));

            return Headers;

        }
        /// <summary>
        /// Gets a CsvRowsList of rows from the data table on the page
        /// </summary>
        /// <param name="pageMode">the contract name</param>
        /// <returns></returns>
        public List<string> GetRowList(string pageMode)
        {
            GetDocument(pageMode);

            if (Document != null)
            {
                var ppage = Document.DocumentNode;
                var tables = ppage.SelectNodes(headerselector);
                var data = ppage.SelectSingleNode(rowSelector);

                HtmlNode headerRowNode = data.ChildNodes.FirstOrDefault(n => !n.Name.Contains("#text"));
                ExtractHeadingFromNode(headerRowNode);

                foreach (var row in data.ChildNodes.Where(n => !n.Name.Contains("#text")))
                {
                    ParseTableRowToCsv(row);
                }
            }
            StringBuilder sbHeader = new StringBuilder();
            string filename = StorageFile.Replace("{pageMode}", pageMode);
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var item in CsvRowsList)
                {
                    sw.WriteLine(item);
                }
                sw.Flush();
                sw.Close();
            }

            return CsvRowsList;
        }

        private void ParseTableRowToCsv(HtmlNode row)
        {
            StringBuilder sb = new StringBuilder();
            //List<string> rowNodeList = row.ChildNodes.Where(n => !n.Name.Contains("#text")).Select(col => col.InnerText).ToList();
            foreach (string s1 in row.ChildNodes
                .Where(n => !n.Name.Contains("#text"))
                .Select(col => col.InnerText)
                .ToList()
                .Select(s => Regex.Replace(s, @"[^0-9\.+-PC]*[;&,s]", "")
                .Replace("unch", "0")))
            {
                sb.Append("\"");
                sb.Append(s1);
                sb.Append("\"");
                sb.Append(",");
            }
            CsvRowsList.Add(sb.ToString());
        }

        private void ExtractHeadingFromNode(HtmlNode node)
        {
            StringBuilder sbHeader = new StringBuilder();
            foreach (string s in from th in node.ChildNodes where !th.Name.Contains("#text") select th.InnerText)
            {
                sbHeader.Append("\"");
                sbHeader.Append(s);
                sbHeader.Append("\"");
                sbHeader.Append(",");
            }
            CsvHeader = sbHeader.ToString();
        }

        /// <summary>
        /// Gets puts and calls open interest lists from a file
        /// </summary>
        /// <param name="pageMode">The contract name</param>
        /// <returns></returns>
        public Dictionary<decimal, int> GetOpenInterestLists(string pageMode)
        {
            string filename = StorageFile.Replace("{pageMode}", pageMode);
            using (StreamReader sr = new StreamReader(filename))
            {
                return GetOpenInterestLists(sr, pageMode);
            }
        }
        /// <summary>
        /// Gets two di
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="pageMode"></param>
        /// <returns></returns>
        public Dictionary<decimal, int> GetOpenInterestLists(StreamReader sr, string pageMode)
        {
            while (!sr.EndOfStream)
            {
                ParseLine(sr);
            }
            return PutsDictionary;
        }

        private void ParseLine(StreamReader sr)
        {
            string line = sr.ReadLine();
            if (CsvHeader != null && CsvHeader.Length == 0)
            {
                CsvHeader = line;
            }
            else
            {
                string[] arr = LineSplitter.SplitFilterCommasInQuotedStrings(line);

                decimal strike;
                int openInterest;
                if (arr[0].EndsWith("P"))
                {
                    strike = decimal.Parse(arr[0].Replace("P", string.Empty));
                    openInterest = int.Parse(arr[7]);
                    PutsDictionary.Add(strike, openInterest);
                }
                else
                {
                    if (arr[0].EndsWith("C"))
                    {
                        strike = decimal.Parse(arr[0].Replace("C", string.Empty));
                        openInterest = int.Parse(arr[7]);
                        CallsDictionary.Add(strike, openInterest);
                    }
                    else
                    {
                        string message = line;
                    }
                }
            }
        }

        #endregion
        #region "private methods"
        private string filterValue(string val)
        {
            return (val.Replace("$", string.Empty).Replace(",", string.Empty));
        }
        #endregion 
    }
}
