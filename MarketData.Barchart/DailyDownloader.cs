/*
 * This class gets web pages from barchart.com for the e-mini and computes the pain strike for the contract
 * 
 * First it gets a list of contract pages from the drop down list in the header table
 * Then it gets the values in the main table and parses it into a csv list.  (It includes the footer table)
 * The list is then parsed into puts and calls
 * Then, for each stike price, the pain value is calculated supposing the contract expires as the strike price
 * The minimum pain strike, pain value, and header/footer information for each contract are summarized.
 */
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MarketData.Barchart.Models;
using MarketData.ToolBox.Utility;

namespace MarketData.Barchart
{
    public class DailyDownloader
    {
        #region "private fields"
        private const string Baseurl = @"http://www.barchart.com/commodityfutures/E-Mini_S&P_500_Futures/options/{option}?mode=d&view=";
        private string rowSelector = @"//table[@class='datatable_simple']";
        private string headerselector = @"//div[@id='divContent']//td//table";
        private string optionSelector = @"//div[@id='divContent']//td//table//td//option";
        private decimal contractMinpainstrike = 0;
        private double contractMinpain = double.MaxValue;
        private string dataDir = string.Empty;
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
        public Dictionary<string, string> Headers = new Dictionary<string, string>();
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
        public string StorageFolder = @"H:\PainStrike\";
        /// <summary>
        /// A CsvRowsList of possible contracts from the web site.
        /// </summary>
        public List<string> ContractList;
        /// <summary>
        /// The HtmlDocument for HtmlAgilityPack
        /// </summary>
        public HtmlDocument Document;

        private int counter = 0;
        #endregion
        #region "public methods"

        /// <summary>
        /// Gets the web page as a HtmlAgilityPack Document
        /// </summary>
        /// <param name="pageMode"></param>
        public void GetDocument(string pageMode)
        {
            var web = new HtmlWeb();
            string url = Baseurl.Replace("{option}", pageMode);
            Debug.WriteLine(url);
            Document = web.Load(url);
        }

        /// <summary>
        /// Gets a CsvRowsList of contracts from the dropdown CsvRowsList on the page
        /// </summary>
        /// <returns>A CsvRowsList of strings with the contract names</returns>
        /// <remarks>Uses http://www.barchart.com/commodityfutures/E-Mini_S&P_500_Futures/options/ES?mode=d&view=
        /// and scrapes the list from the dropdown in the table header.
        /// </remarks>
        public List<string> GetContractList()
        {
            ContractList = new List<string>();

            GetDocument("ES");
            var ppage = Document.DocumentNode;
            var optionNodes = ppage.SelectNodes(optionSelector);
            // get the selections
            foreach (var optionNode in optionNodes.Where(optionNode => optionNode.Attributes["value"].Value?.Length > 0))
            {
                ContractList.Add(optionNode.Attributes["value"].Value);
            }
            return ContractList;
        }

        /// <summary>
        /// Gets the header dictionary from the page.  This method is used for testing.
        /// It loads the page from the web and gets the DocumentNode before calling GetHeaderDictionary
        /// </summary>
        /// <param name="pageMode">the contract name</param>
        /// <returns>A dictionary of header names and values</returns>
        public Dictionary<string, string> GetHeaderDictionary(string pageMode)
        {
            Headers = new Dictionary<string, string>();
            GetDocument(pageMode);
            var ppage = Document.DocumentNode;
            return GetHeaderDictionary(pageMode, ppage.SelectNodes(headerselector));
        }

        private Dictionary<string, string> GetHeaderDictionary(string pageMode, HtmlNode ppage)
        {
            var tables = ppage.SelectNodes(headerselector);
            return GetHeaderDictionary(pageMode, tables);
        }

        private Dictionary<string, string> AddToHeadersDictionary(HtmlNode data)
        {
            var rows = data.ChildNodes.Where(n => !n.Name.Contains("#text") && n.ChildNodes.Count < 3);
            foreach (var row in rows)
            {
                if (row.HasChildNodes)
                {
                    var nottext = row.ChildNodes
                        .Where(n => !n.Name.Contains("#text")
                            && !n.Name.Contains("th")
                            && !n.InnerText.Contains("nbsp"));
                    foreach (var arr in nottext.Select(node => node.InnerText.Split(':')))
                    {
                        Headers.Add(arr[0], filterValue(arr[1]));
                    }
                }
            }
            return Headers;
        }


        /// <summary>
        /// Gets the heading information from the page
        /// </summary>
        /// <param name="pageMode">the contract name</param>
        /// <param name="tables">the tables in the heading of the table</param>
        /// <returns>A dictionary of header names and values</returns>
        public Dictionary<string, string> GetHeaderDictionary(string pageMode, HtmlNodeCollection tables)
        {
            Headers = new Dictionary<string, string>();

            var key = string.Empty;
            var val = string.Empty;
            string[] arr = null;

            // Get the first 4 headers
            foreach (var node in tables[0].ChildNodes.Where(n => !n.Name.Contains("#text")))
            {
                arr = node.InnerText.Split(':');
                Headers.Add(arr[0], filterValue(arr[1]));
            }

            // Only get the header for Daily Options
            // it is the second header table and the 6th child node
            arr = tables[1].ChildNodes[5].InnerText.Split(':');
            Headers.Add(arr[0], filterValue(arr[1]));

            return Headers;

        }

        /// <summary>
        /// Gets a CsvRowsList of rows from the data table on the page
        /// </summary>
        /// <param name="pageMode">the contract name</param>
        /// <returns>A list of csv rows</returns>
        /// 
        /// <remarks>
        /// The headers are in two separate tables before the main table. The footers are a part of the main table.
        /// This routine gets the first of the two header tables.  The second is the select element and option list.
        /// </remarks>
        public List<string> GetRowList(string pageMode)
        {
            GetDocument(pageMode);

            if (Document != null)
            {
                var ppage = Document.DocumentNode;
                GetHeaderDictionary(pageMode, ppage);   // Get the headers at the top of the list

                var data = ppage.SelectSingleNode(rowSelector);
                // Get the headers at the bottom of the list.
                AddToHeadersDictionary(data);

                // Make sure the CsvRowList is new and blank
                CsvRowsList = new List<string>();

                // Add the rows to the list
                foreach (var row in data.ChildNodes.Where(n => !n.Name.Contains("#text")))
                {
                    ParseTableRowToCsv(row, ref CsvRowsList);
                }
            }

            SaveRowList(pageMode);
            SaveHeaderList(pageMode);

            return CsvRowsList;
        }



        private void ParseTableRowToCsv(HtmlNode row, ref List<string> rowList)
        {
            StringBuilder sb = new StringBuilder();
            counter++;
            var txt = row.ChildNodes.Where(n => !n.Name.Contains("#text")).Select(c => c.InnerText);
            if (txt.Contains("Strike") && rowList.Count < 1)
            {
                foreach (var s in row.ChildNodes.Where(n => !n.Name.Contains("#text")).Select(c => c.InnerText))
                {
                    sb.Append("\"");
                    sb.Append(s);
                    sb.Append("\"");
                    sb.Append(",");
                }
            }
            else
            {
                var list = row.ChildNodes.Where(n => !n.Name.Contains("#text")).Select(c => c.InnerText).ToList();
                if (list[0].Contains("Strike"))
                    return;

                if (list.Count() < 3)
                    return;

                foreach (var s in row.ChildNodes.Where(n => !n.Name.Contains("#text")).Select(c => c.InnerText))
                {
                    string s1 = Regex.Replace(s, @"[^0-9\.+-PC]*[;&,s]", "")
                        .Replace("unch", "0");
                    sb.Append("\"");
                    sb.Append(s1);
                    sb.Append("\"");
                    sb.Append(",");
                }
            }

            rowList.Add(sb.ToString());
        }

        private string ExtractHeadingFromNode(HtmlNode node)
        {
            StringBuilder sbHeader = new StringBuilder();
            foreach (string s in from th in node.ChildNodes where !th.Name.Contains("#text") select th.InnerText)
            {
                sbHeader.Append("\"");
                sbHeader.Append(s);
                sbHeader.Append("\"");
                sbHeader.Append(",");
            }
            return sbHeader.ToString();
        }

        /// <summary>
        /// Gets puts and calls open interest lists from a file
        /// </summary>
        /// <param name="pageMode">The contract name</param>
        /// <returns></returns>
        public Dictionary<decimal, int> GetOpenInterestLists(string pageMode)
        {
            string filename = GetRowListFilename(pageMode);
            using (StreamReader sr = new StreamReader(filename))
            {
                return GetOpenInterestLists(sr, pageMode);
            }
        }
        /// <summary>
        /// Gets two parallel dictionaries of open interest, one for calls and one for puts
        /// with the strike prices as keys and the contracts open interest as the value.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="pageMode">The contract name</param>
        /// <returns></returns>
        public Dictionary<decimal, int> GetOpenInterestLists(StreamReader sr, string pageMode)
        {
            while (!sr.EndOfStream)
            {
                ParseRowListStream(sr);
            }
            return PutsDictionary;
        }

        private void ParseRowListStream(StreamReader sr)
        {
            string line = sr.ReadLine();
            if (CsvHeader != null && CsvHeader.Length == 0)
            {
                CsvHeader = line;
            }
            else
            {
                ParsePutsAndCallsFromRowListLine(line);
            }
        }


        /// <summary>
        /// Computes the Pain value for each strike in the Calls and Puts Dictionaries given the current market price
        /// </summary>
        /// <param name="currentPrice">the current market price of the underlying</param>
        /// <returns>The pain value for the current market price</returns>
        public decimal ComputePainForPrice(decimal currentPrice, string pageMode)
        {
            if (CallsDictionary.Count == 0 || PutsDictionary.Count == 0)
            {
                throw new InvalidDataException("Calls and Puts dictionaries are not initialized");
            }

            List<PainRow> rows = PutsDictionary.Keys.Select(strike => new PainRow
            {
                currentPrice = currentPrice,
                strike = strike,
                CallOpenInterest = CallsDictionary[strike],
                PutOpenInterest = PutsDictionary[strike],
                Call = ComputeCallValue(strike, currentPrice, true),
                Put = ComputeCallValue(strike, currentPrice, false),
            }).ToList();
            foreach (var row in rows)
            {
                row.Ext = row.Call + row.Put;
            }

            var serializedPainRows = CsvSerializer.Serialize(",", rows, true);

            SaveSerializedPainRows(currentPrice, pageMode, serializedPainRows);
            decimal calls = rows.Sum(r => r.Call);
            decimal puts = rows.Sum(r => r.Put);
            decimal ret1 = rows.Sum(r => r.Ext);
            // for testing.  the sum of the rows should equal the 
            //decimal calls = CallsDictionary.Keys.Sum(key => ComputeCallValue(key, currentPrice, true));
            //decimal puts = PutsDictionary.Keys.Sum(key => ComputeCallValue(key, currentPrice, false));
            decimal ret = calls + puts;
            if (ret1 != ret)
                throw new InvalidProgramException("The crosscheck of the pain rows failed.");
            return ret;
        }

        /// <summary>
        /// Computes the 
        /// </summary>
        /// <param name="strike">the strik price when iterating through the strike</param>
        /// <param name="currentPrice">the current price when iterating through the strikes</param>
        /// <param name="call">boolean - true if the option is a call, false otherwise</param>
        /// <returns>The open interest * the difference between the strike and current price</returns>
        public decimal ComputeCallValue(decimal strike, decimal currentPrice, bool call)
        {
            decimal ret = 0;
            int openInterest = 0;
            if (call)
            {
                if (currentPrice > strike)
                {
                    openInterest = CallsDictionary[strike];
                    if (openInterest > 0)
                        ret = (currentPrice - strike) * System.Convert.ToDecimal(openInterest);

                }
            }
            else
            {
                if (currentPrice < strike)
                {
                    openInterest = PutsDictionary[strike];
                    if (openInterest > 0)
                        ret = (strike - currentPrice) * System.Convert.ToDecimal(openInterest);
                }
            }
            return ret;
        }
        /// <summary>
        /// Computes the minimum Pain Strike for a particular contract
        /// </summary>
        /// <param name="pageMode">The contract.  This parameter is retreived from the pageMode dropdown
        /// list on the web page in .</param>
        /// <returns></returns>
        public string ComputeMinimumPain(string pageMode)
        {
            var rowList = GetRowList(pageMode);
            GetOpenInterestListsFromRowList(rowList);

            Dictionary<decimal, double> dic = new Dictionary<decimal, double>();
            contractMinpainstrike = 0;
            contractMinpain = double.MaxValue;
            foreach (decimal key in PutsDictionary.Keys)
            {

                double painvalue = (double)ComputePainForPrice(key, pageMode);
                dic.Add(key, painvalue);
                if (painvalue < contractMinpain)
                {
                    contractMinpain = painvalue;
                    contractMinpainstrike = key;
                }
            }
            var serializedPainValues = CsvSerializer.Serialize(",", dic, true);
            SavePainValues(pageMode, serializedPainValues);
            Debug.WriteLine("{0} {1} {2}", pageMode, contractMinpainstrike, contractMinpain);
            string ret = $"{pageMode},{contractMinpainstrike},{contractMinpain}";
            return ret;
        }

        /// <summary>
        /// Computes all available minimum Pain Strikes
        /// </summary>
        /// <returns></returns>
        public List<string> ComputeAllMinimumPains()
        {
            List<string> minimumPainList = new List<string>();

            var contractlist = GetContractList();
            foreach (string s in contractlist)
            {
                PutsDictionary = new Dictionary<decimal, int>();
                CallsDictionary = new Dictionary<decimal, int>();

                minimumPainList.Add(ComputeMinimumPain(s));
            }

            SaveMinimumPainList(minimumPainList);

            return minimumPainList;
        }
        #endregion
        #region "private methods"
        /// <summary>
        /// Parses a line from the RowList into either the Puts Dictionary or the Calls Dictionary.
        /// Other lines (header and footer) are ingnored.
        /// </summary>
        /// <param name="line">The line from the list</param>
        private void ParsePutsAndCallsFromRowListLine(string line)
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
                    // just discard non put or call lines
                    string message = line;
                }
            }
        }

        /// <summary>
        /// Separates the list of web page table rows into a Calls Dictionary and a Puts Dictionary
        /// </summary>
        /// <param name="rowList"></param>
        private void GetOpenInterestListsFromRowList(List<string> rowList)
        {
            foreach (string line in rowList)
            {
                if (CsvHeader != null && CsvHeader.Length == 0)
                {
                    CsvHeader = line;
                }
                else
                {
                    ParsePutsAndCallsFromRowListLine(line);
                }
            }
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
        /// <summary>
        /// Gets the "DataYYYYMMDD" folder and makes sure it exists.
        /// </summary>
        /// <returns>The folder Fullname</returns>
        private string GetDataFolder()
        {
            GetStorageFolder();

            StringBuilder sb = new StringBuilder();

            var datestring = Headers["Daily Options"].Trim() + " " + DateTime.Now.Year;
            DateTime dt = DateTime.Now;
            if (!DateTime.TryParse(datestring, out dt))
            {
                throw new InvalidDataException("Invalid date time: " + datestring);
            }

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
                dataDir = di.FullName;
            }
            return sb.ToString();
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
            sb.Append(".csv");
            return sb.ToString();
        }
        /// <summary>
        /// Gets the \\StorageFolder\DataFolder\PainStrikes\PainStrike_{Contract}_{currentAssumedMarketPrice}.csv folder
        /// </summary>
        /// <param name="currentPrice">The assumed market price</param>
        /// <param name="pageMode">The contract abbreviation</param>
        /// <returns></returns>
        private string GetCurrentPriceFilename(decimal currentPrice, string pageMode)
        {
            StringBuilder sb = new StringBuilder(GetDataFolder());

            sb.Append(@"PainStrikes\");
            DirectoryInfo di = new DirectoryInfo(sb.ToString());
            if (!di.Exists)
            {
                Directory.CreateDirectory(di.FullName);
                dataDir = di.FullName;
            }
            sb.Append(@"PainStrike");
            sb.Append("_");
            sb.Append(pageMode);
            sb.Append("_");
            sb.Append(Convert.ToInt64(currentPrice).ToString());
            sb.Append(".csv");

            string filename = sb.ToString();

            return filename;
        }
        #region "    Save files"
        /// <summary>
        /// Saves the list of rows from the web page table to a csv file
        /// </summary>
        /// <param name="pageMode">the contract abbreviation</param>
        private void SaveRowList(string pageMode)
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

        private void SaveHeaderList(string pageMode)
        {
            string filename = GetHeaderListFilename(pageMode);
            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                foreach (var key in Headers.Keys)
                {
                    sw.WriteLine("{0},{1}", key, Headers[key]);
                }
            }
        }

        private string GetHeaderListFilename(string pageMode)
        {
            StringBuilder sb = new StringBuilder(GetDataFolder());
            sb.Append(pageMode);
            sb.Append("Header.csv");
            return sb.ToString();
        }

        private void SaveSerializedPainRows(decimal currentPrice, string pageMode, IEnumerable<string> serializedPainRows)
        {
            var filename = GetCurrentPriceFilename(currentPrice, pageMode);

            using (var sw = new StreamWriter(filename, false))
            {
                foreach (var y in serializedPainRows)
                {
                    sw.WriteLine(y);
                }
                sw.Flush();
                sw.Close();
            }
        }

        private void SavePainValues(string pageMode, IEnumerable<string> serializedPainValues)
        {
            string filename = GetDataFolder() + $@"\PainByStrike{pageMode}.csv";
            using (var sw = new StreamWriter(filename, false))
            {
                foreach (var y in serializedPainValues)
                {
                    sw.WriteLine(y);
                }
                sw.Flush();
                sw.Close();
            }
        }
        private void SaveMinimumPainList(List<string> csvList)
        {
            using (var sr = new StreamWriter(GetDataFolder() + @"\MinPainStrikes.csv", false))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                sb.Append("Symbol");
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"");
                sb.Append("Strike");
                sb.Append("\"");
                sb.Append(",");
                sb.Append("\"");
                sb.Append("Pain Amount");
                sb.Append("\"");
                sr.WriteLine(sb.ToString());

                foreach (string s in csvList)
                {
                    sr.WriteLine(s);
                }
                sr.Flush();
                sr.Close();
            }
        }
        #endregion
        private string filterValue(string val)
        {
            return (val.Replace("$", string.Empty).Replace(",", string.Empty));
        }
        #endregion

    }

}

