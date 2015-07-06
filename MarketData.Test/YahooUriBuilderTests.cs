using System;
using System.Diagnostics;
using MarketData.YahooFinance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    [TestClass]
    public class YahooUriBuilderTests
    {
        [TestMethod]
        public void GetsTheDefaultUriForASymbol()
        {
            var builder = new DownloadURIBuilder("SPY");
            string uri = builder.GetPricesUri();
            //Debug.WriteLine(uri);
            Assert.IsTrue(uri == "http://ichart.finance.yahoo.com/table.csv?s=SPY&a=11&b=31&c=1999&d=5&e=20&f=2015&g=d&ignore=.csv");
        }
        [TestMethod]
        public void GetsTheDatedUriForASymbol()
        {
            var builder = new DownloadURIBuilder("SPY");
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate - new TimeSpan(2, 0, 0,0);
            string uri = builder.GetPricesUrlForSpecificPeriod(startDate, endDate);
            //Debug.WriteLine(uri);
            Assert.IsTrue(uri == "http://ichart.finance.yahoo.com/table.csv?s=SPY&a=5&b=18&c=2015&d=5&e=20&f=2015&g=d&ignore=.csv");
        }
        [TestMethod]
        public void GetsWeeklyUriForASymbol()
        {
            var builder = new DownloadURIBuilder("SPY");
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate - new TimeSpan(0, 0, 0, 0);
            string uri = builder.GetPricesUrlForSpecificPeriod(startDate, endDate, "w");
            //Debug.WriteLine(uri);
            Assert.IsTrue(uri == "http://ichart.finance.yahoo.com/table.csv?s=SPY&a=5&b=8&c=2015&d=5&e=20&f=2015&g=w&ignore=.csv");
        }
    }
}
