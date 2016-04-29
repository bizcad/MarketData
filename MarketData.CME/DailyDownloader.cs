using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MarketData.CME
{
    public class DailyDownloader
    {
        private const string baseurl =
            @"http://www.cmegroup.com/trading/equity-index/us-index/e-mini-sandp500_quotes_globex_options.html?optionExpiration=M6#strikeRange=ATM";

        private const string downloadurl =
            @"http://www.cmegroup.com/CmeWS/exp/voiProductDetailsViewExport.ctl?media=xls&tradeDate=20160427&reportType=P&productId=133";
        private string selector = @"//table[@id='optionQuotesProductTable1']//tbody//tr";

        public string GlobexOptionsDownload()
        {

            string message = "Use the download page instead of screen scrape";
            Debug.WriteLine(message);
            return message;
            //StringBuilder sb = new StringBuilder();
            //string qc = "\",\"";
            //List<string> rowList = new List<string>();
            //var web = new HtmlWeb();
            //var document = web.Load(baseurl);
            //var ppage = document.DocumentNode;
            ////var decendants = ppage.SelectNodes("//div[@id='content']//table//tr//div");
            //var decendants = ppage.SelectNodes(selector);
            //foreach (var d in decendants)
            //{
            //    var cn = d.ChildNodes;
            //    int count = cn.Count;
            //    if (count > 0)
            //    {
            //        rowList = (from x in cn where x.Name == "td" select x.InnerHtml).ToList();
            //    }
            //    sb.Append("\"");
            //    for (int i = 0; i < rowList.Count - 1; i++)
            //    {
            //        sb.Append(rowList[i]);
            //        sb.Append(qc);
            //    }
            //    sb.Append(rowList[rowList.Count-1]);
            //    sb.AppendLine("\"");
            //    string csv = sb.ToString();
            //}
            //return "";

        }
    }
}
