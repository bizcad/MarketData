using System;
using MarketData.GoogleFinance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dir = Config.GetDefaultDownloadDirectory();
            Assert.IsTrue(dir.Length > 0);
        }
    }
}
