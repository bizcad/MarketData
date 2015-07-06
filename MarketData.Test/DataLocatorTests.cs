using System.IO;
using NUnit.Framework;
using MarketData.GoogleFinance;

namespace MarketData.Test
{
    [TestFixture]
    public class DataLocatorTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetsTestCompaniesFile()
        {
            const string testInputfilename = @"\SPY_Companies.csv";
            string dir = Config.GetDefaultDownloadDirectory();
            FileInfo spyfile = new FileInfo(dir + testInputfilename);
            Assert.IsTrue(File.Exists(spyfile.FullName));
        }
    }
}
 