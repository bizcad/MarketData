using System;
using System.Text;

namespace MarketData.YahooFinance
{
    public class DownloadURIBuilder
    {
        #region Class fields
        /// <summary>
        /// Name of the exchange where the ticker is defined.
        /// If it's null or empty, Google assumes it must be
        /// an American exchange.
        /// </summary>
        //private string Exchange;

        /// <summary>
        /// Name of the ticker.
        /// </summary>
        private readonly string TickerName;
        #endregion

        #region Initialization
        /// <summary>
        /// The exchange and the ticker are defined here.
        /// </summary>        
        public DownloadURIBuilder(string tickerName)
        {
            if (String.IsNullOrEmpty(tickerName))
                throw new ArgumentException("Can't be null or empty.", "exchange");

            TickerName = tickerName;
        }
        #endregion

        #region "public api"
        /// <summary>
        /// Returns a URI to invoke the getPrices method.
        /// </summary>
        /// <param name="interval">string - the historical quote interval "d", "w" or "m"</param>
        /// <remarks>
        ///    "http://ichart.yahoo.com/table.csv?s=GOOG&a=0&b=1&c=2000&d=0&e=31&f=2010&g=w&ignore=.csv"
        ///     http://real-chart.finance.yahoo.com/table.csv?s=SPY&a=05&b=17&c=2015&d=5&e=18&f=2015&g=d&ignore=.csv
        /// </remarks>
        public string GetPricesUri(string interval = "d")
        {
            return GetPricesUrlForSpecificPeriod(new DateTime(1999, 12, 31), DateTime.Now);
        }
        /// <summary>
        /// Gets a specific period of quotes from yahoo finance, and optionally an interval of week or month
        /// </summary>
        /// <param name="startDate">DateTime - the starting date</param>
        /// <param name="endDate">DateTime - the ending date; defaults to Now</param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public string GetPricesUrlForSpecificPeriod(DateTime startDate, DateTime endDate, string interval = "d")
        {
            string s = TickerName;
            int a = startDate.Month - 1;
            int b = startDate.Day;
            int c = startDate.Year;
            int d = endDate.Month - 1;
            int e = endDate.Day;
            int f = endDate.Year;


            StringBuilder sb = new StringBuilder();

            sb.Append(Config.GetYahooFinanceApiBeginning());
            sb.Append(string.Format("s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g={7}&ignore=.csv",
                s, a, b, c, d, e, f, interval));

            return sb.ToString();
        }
        #endregion
        #region "private methods"

        #endregion
    }
}
