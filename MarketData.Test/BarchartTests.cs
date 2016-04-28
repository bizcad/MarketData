using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DailyDownloader = MarketData.Barchart.DailyDownloader;

namespace MarketData.Test
{
    [TestClass]
    public class BarchartTests
    {
        string pageMode = "ESZ16";  // December 2016

        [TestMethod]
        public void GetsDocument()
        {
            DailyDownloader dl = new DailyDownloader();
            dl.GetDocument(pageMode);
            Assert.IsNotNull(dl.Document);
        }

        [TestMethod]
        public void GetsHeader()
        {
            DailyDownloader dl = new DailyDownloader();
            var ret = dl.GetHeaderDictionary(pageMode);
            foreach (var item in ret)
            {
                Debug.WriteLine(@"{0} - {1}", item.Key,item.Value);
            }
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod]
        public void GetsContractsList()
        {
            DailyDownloader dl = new DailyDownloader();
            var ret = dl.GetContractList();
            Assert.IsTrue(ret.Count > 0);
        }
        [TestMethod]
        public void GetsRowList()
        {
            DailyDownloader dl = new DailyDownloader();
            var ret = dl.GetRowList(pageMode);
            Assert.IsTrue(ret.Count > 0);
        }
        [TestMethod]
        public void GetsOpenInterestList()
        {
            DailyDownloader dl = new DailyDownloader();
            var ret = dl.GetOpenInterestLists(pageMode);
            Assert.IsTrue(ret.Count > 0);
            Assert.IsTrue(dl.CallsDictionary.Count > 0);
            Assert.IsTrue(dl.PutsDictionary.Count > 0);
        }
        [TestMethod]
        public void GetsOpenInterestListFromStream()
        {
            DailyDownloader dl = new DailyDownloader();
            dl.GetHeaderDictionary(pageMode);
            string filename = dl.GetRowListFilename(pageMode);

            using (StreamReader sr = new StreamReader(filename))
            {
                var ret = dl.GetOpenInterestLists(sr, pageMode);
            }
            
            Assert.IsTrue(dl.CallsDictionary.Count > 0);
            Assert.IsTrue(dl.PutsDictionary.Count > 0);
        }
        #region "Tests Calls and Puts In and out of money"
        [TestMethod]
        public void GetsOpenInterest_InMoneyCall()
        {
            DailyDownloader dl = new DailyDownloader();
            string filename = dl.StorageFolder.Replace("{pageMode}", pageMode);
            using (StreamReader sr = new StreamReader(filename))
            {
                var ret = dl.GetOpenInterestLists(sr, pageMode);
            }
            var value = dl.ComputeCallValue(2075, 2080, true);
            Assert.IsTrue(value > 0);
            Assert.IsTrue(value == 3585);
        }
        [TestMethod]
        public void GetsOpenInterest_OutMoneyCall()
        {
            DailyDownloader dl = new DailyDownloader();
            string filename = dl.StorageFolder.Replace("{pageMode}", pageMode);
            using (StreamReader sr = new StreamReader(filename))
            {
                var ret = dl.GetOpenInterestLists(sr, pageMode);
            }
            var value = dl.ComputeCallValue(2080, 2075, true);
            Assert.IsTrue(value == 0);
        }
        [TestMethod]
        public void GetsOpenInterest_OutMoneyPut()
        {
            DailyDownloader dl = new DailyDownloader();
            string filename = dl.StorageFolder.Replace("{pageMode}", pageMode);
            using (StreamReader sr = new StreamReader(filename))
            {
                var ret = dl.GetOpenInterestLists(sr, pageMode);
            }
            var value = dl.ComputeCallValue(2075, 2080, false);
            
            Assert.IsTrue(value == 0);
        }
        [TestMethod]
        public void GetsOpenInterest_InMoneyPut()
        {
            DailyDownloader dl = new DailyDownloader();
            string filename = dl.StorageFolder.Replace("{pageMode}", pageMode);
            using (StreamReader sr = new StreamReader(filename))
            {
                var ret = dl.GetOpenInterestLists(sr, pageMode);
            }
            var value = dl.ComputeCallValue(2075, 2070, false);
            Assert.IsTrue(value == 2365);
        }
        #endregion
        [TestMethod]
        public void PainForCurrentValue()
        {
            DailyDownloader dl = new DailyDownloader();
            string filename = dl.StorageFolder.Replace("{pageMode}", pageMode);
            using (StreamReader sr = new StreamReader(filename))
            {
                var ret = dl.GetOpenInterestLists(sr, pageMode);
            }
            var value = dl.ComputePainForPrice(2080, pageMode);
            Assert.IsTrue(value > 0);
        }
        [TestMethod]
        public void ComputesMinPain()
        {
            DailyDownloader dl = new DailyDownloader();
            var value = dl.ComputeMinimumPain(pageMode);
            Assert.IsTrue(value.Length > 0);
        }
        [TestMethod]
        public void ComputesAllMinPain()
        {
            DailyDownloader dl = new DailyDownloader();
            var list = dl.ComputeAllMinimumPains();
            Assert.IsTrue(list.Count > 0);
        }
    }
}
