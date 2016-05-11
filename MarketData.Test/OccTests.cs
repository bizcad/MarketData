using System;
using HtmlAgilityPack;
using MarketData.Occ;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    [TestClass]
    public class OccTests
    {
        [TestMethod]
        public void GetsAndSavesTableFromHtml()
        {
            OccDownloader dl = new OccDownloader();
            var ret = dl.GetTableFromHtml();
            Assert.IsNotNull(ret);
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod]
        public void ConvertsCsvToObjectList()
        {
            OccDownloader dl = new OccDownloader();
            var csv = dl.GetTableFromHtml();
            var ret = dl.CsvToObjectList(csv);
            Assert.IsNotNull(ret);
            Assert.IsTrue(ret.Count > 0);
        }
    }
}
