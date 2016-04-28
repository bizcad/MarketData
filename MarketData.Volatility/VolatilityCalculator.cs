using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MarketData.ToolBox.Utility;

namespace MarketData.Volatility
{
    public class VolatilityCalculator
    {
        /// <summary>
        /// The HtmlDocument for HtmlAgilityPack
        /// </summary>
        public HtmlDocument Document;

        private string baseurl =
            @"https://eztv.ag/";

        #region "public methods"
        /// <summary>
        /// Gets the web page as a HtmlAgilityPack Document
        /// </summary>
        public void GetDocument()
        {
            var web = new HtmlWeb();
            string url = baseurl;
            Document = web.Load(url);
        }

        public string GetPage()
        {

            WebClient wClient = new WebClient();
            string ret = wClient.DownloadString(baseurl);
            //SearchString1 = &SearchString = 1568 & search = Search
            return ret;
        }
        #endregion
    }
}
