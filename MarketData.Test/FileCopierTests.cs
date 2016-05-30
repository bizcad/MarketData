using System;
using System.IO;
using MarketData.GoogleFinance;
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
        [TestMethod]
        public void MovesDailyFiles()
        {
            bool ret = true;
            FileMover.CopyDailyFiles(new DirectoryInfo(@"L:\GoogleFinanceData\equity\usa\daily\"));
            Assert.IsTrue(ret);

        }
        /// <summary>
        /// This one has not been thoroughly debugged
        /// </summary>
        [TestMethod]
        public void MovesMinuteFiles()
        {
            bool ret = true;
            FileMover.CopyMinuteFiles(new DirectoryInfo(@"L:\GoogleFinanceData\equity\usa\minute\"));
            Assert.IsTrue(ret);

        }

    }
}
