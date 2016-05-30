using System;
using System.Threading.Tasks;
using MarketData.Oic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    [TestClass]
    public class OicDownloaderTests
    {
        [TestMethod]
        public void GetsDocument()
        {
            OicDownloader dl = new OicDownloader();
            dl.GetDocument();
            Assert.IsNotNull(dl.Document);
        }
        [TestMethod]
        public async void GetsTables()
        {
            OicDownloader dl = new OicDownloader();
            var ret = await dl.GetSpx();
            
            Assert.IsNotNull(ret);
            

        }
    }
}
