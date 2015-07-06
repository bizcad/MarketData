using System;
using System.IO;
using NUnit.Framework;
using MarketData.GoogleFinance;

namespace MarketData.Test
{
    [TestFixture]
    public class SymbolDirectoryFactoryTests
    {
        const string TestInputfilename = @"\SPY_Companies.csv";
        private string _dir;

        [Test]
        public void CreatesASymbolDirectoryUnderASingleLetterDirectory()
        {
            GetDefaults();
            DirectoryInfo singleLettDirectoryInfo =
                new DirectoryInfo(_dir + @"\NYSEARCA\S\");
            SymbolDirectoryFactory.Create(singleLettDirectoryInfo, "stest");
            bool direxists = Directory.Exists(singleLettDirectoryInfo.FullName + @"\stest\");
            Assert.IsTrue(direxists);
        }


        private void GetDefaults()
        {
            _dir = Config.GetDefaultDownloadDirectory();
        }
    }
}
