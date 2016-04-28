using System;
using MarketData.CME;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    [TestClass]
    public class CMETests
    {
        [TestMethod]
        public void DownloadsGlobexPage()
        {
            CME.DailyDownloader dl = new DailyDownloader();
            var ret = dl.GlobexOptionsDownload();
            Assert.IsTrue(ret.Length > 0);
        }
    }
}
