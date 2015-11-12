using System;
using MarketData.GoogleFinanceDownloader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    [TestClass]
    public class FileCopierTests
    {
        [TestMethod]
        public void TestFileCopier()
        {
            FileCopier fc = new FileCopier();
            Assert.IsTrue(fc.CopyFiles() > 0);

        }

    }
}
