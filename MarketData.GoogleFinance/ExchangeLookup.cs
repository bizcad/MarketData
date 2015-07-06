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
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;

namespace MarketData.GoogleFinance
{
    public class ExchangeLookup
    {
        private string _symbol;

        public ExchangeLookup() { }
        public ExchangeLookup(string symbol)
        {
            _symbol = symbol;
        }

        public async Task<string> GetExchange()
        {
            var builder = new DownloadURIBuilder("", _symbol);
            DateTime startdate = DateTime.Now.AddDays(-2);
            DateTime enddate = DateTime.Now.AddDays(-1);

            string uri = builder.GetGetPricesUrlForRecentData(startdate, enddate);
            WebClient wClient = new WebClient();
            NameValueCollection myQueryStringCollection = new NameValueCollection();
            myQueryStringCollection.Add("symbol", _symbol);
            wClient.QueryString = myQueryStringCollection;
            
            string result = await wClient.DownloadStringTaskAsync(uri);
            string exchange = result.Substring(11, result.IndexOf("\n", System.StringComparison.Ordinal) - 11);
            return exchange;
            
        }

        public string GetExchangeForSymbol(string symbol)
        {
            var builder = new DownloadURIBuilder("", symbol);
            DateTime startdate = DateTime.Now.AddDays(-2);
            DateTime enddate = DateTime.Now.AddDays(-1);

            string uri = builder.GetGetPricesUrlForRecentData(startdate, enddate);
            WebClient wClient = new WebClient();
            NameValueCollection myQueryStringCollection = new NameValueCollection();
            myQueryStringCollection.Add("symbol", _symbol);
            wClient.QueryString = myQueryStringCollection;
            
            string result = wClient.DownloadString(uri);
            string exchange = result.Substring(11, result.IndexOf("\n", System.StringComparison.Ordinal) - 11);
            return exchange;

        }

       

    }
}
