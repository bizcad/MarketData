using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MarketData.Oic
{
    public class OicDownloader
    {
        public string Baseurl = @"http://www.optionseducation.org/quotes.html?quote=SPX";
        public HtmlDocument Document;

        /// <summary>
        /// Gets the web page as a HtmlAgilityPack Document
        /// </summary>
        public void GetDocument()
        {
            var web = new HtmlWeb();
            string url = Baseurl;
            Debug.WriteLine(url);
            Document = web.Load(url);
            using (StreamWriter sw = new StreamWriter(@"H:\page.html"))
            {
                sw.Write(Document.DocumentNode.InnerHtml);
                sw.Flush();
                sw.Close();
            }

        }

        public async Task<object> GetSpx()
        {
            
            string uri = @"http://www.optionseducation.org/quotes.html?quote=SPX";
            //HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            //HtmlDocument document = web.Load(uri);
            //HtmlNode ppage = document.DocumentNode;
            //HtmlNodeCollection tables = ppage.SelectNodes("//table");
            //return tables;

            using (HttpClient client = new HttpClient())
            {
                var x = await client.GetAsync(uri);
                //using (HttpResponseMessage response = await client.GetAsync(uri))
                //{


                //    ICollection<string> contentEncoldingCollection = response.Content.Headers.ContentEncoding;
                //    if (contentEncoldingCollection.Contains("gzip"))
                //    {
                //        using (StreamContent content = (StreamContent) response.Content)
                //        {
                //            byte[] result = content.ReadAsByteArrayAsync().Result;
                //            //byte[] decompressed = Decompress(result);
                //            //pagetext = Encoding.ASCII.GetString(decompressed);
                //            //offset = pagetext.IndexOf("ka ka-magnet", offset + 1,
                //            //    StringComparison.Ordinal);
                //        }
                //    }
                //}


                return null;

            }

        }
    }
}
