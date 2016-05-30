using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSolDownloader = MarketData.Barchart.WebSolDownloader;

namespace MarketData.Test
{
    [TestClass]
    public class WebsolTests
    {
        string pageMode = "ESU16";

        [TestMethod]
        public void GetsDocument()
        {
            WebSolDownloader dl = new WebSolDownloader();
            dl.GetDocument(pageMode);
            Assert.IsNotNull(dl.Document);
        }
        [TestMethod]
        public void GetsContractsList()
        {
            WebSolDownloader dl = new WebSolDownloader();
            var ret = dl.GetContractList(pageMode);
            foreach (var item in ret)
            {
                Debug.WriteLine($@"{item}");
            }
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod]
        public void GetsHeader()
        {
            WebSolDownloader dl = new WebSolDownloader();
            var ret = dl.GetHeaderDictionary(pageMode);
            foreach (var item in ret)
            {
                Debug.WriteLine(item);
            }
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod]
        public void GetsRowList()
        {
            WebSolDownloader dl = new WebSolDownloader();
            var ret = dl.GetRowList(pageMode);
            Assert.IsTrue(ret.Count > 0);
        }
        [TestMethod]
        public void GetsOpenInterestList()
        {
            WebSolDownloader dl = new WebSolDownloader();
            var ret = dl.GetOpenInterestLists(pageMode);
            Assert.IsTrue(ret.Count > 0);
            Assert.IsTrue(dl.CallsDictionary.Count > 0);
            Assert.IsTrue(dl.PutsDictionary.Count > 0);
        }
        [TestMethod]
        public void GetsOpenInterestListFromStream()
        {
            WebSolDownloader dl = new WebSolDownloader();
            string filename = dl.StorageFile.Replace("{pageMode}", pageMode);
            using (StreamReader sr = new StreamReader(filename))
            {
                var ret = dl.GetOpenInterestLists(sr, pageMode);
            }

            Assert.IsTrue(dl.CallsDictionary.Count > 0);
            Assert.IsTrue(dl.PutsDictionary.Count > 0);
        }

    }
}
