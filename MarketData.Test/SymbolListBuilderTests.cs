using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using MarketData.GoogleFinance;

namespace MarketData.Test   
{
    [TestFixture]
    public class SymbolListBuilderTests
    {
        const string TestInputfilename = @"\SandP_Companies.csv";
        private string _dir;
        private FileInfo _spyfile;

        [Test]
        public void BuildsAListFromFile()
        {
            GetDefaults();
            SymbolListBuilder builder = new SymbolListBuilder();
            FileInfo symbolFileInfo = new FileInfo(_dir + @"\SandP_Companies.csv");
            Dictionary<string, string> symbolDictionary = builder.BuildListFromFile(symbolFileInfo);

            Assert.IsNotNull(symbolDictionary);
            Assert.IsNotNull(symbolFileInfo);
            Assert.IsTrue(symbolDictionary.ContainsKey("MMM"));
        }

        [Test]
        public void BuildsSymbolsStringFromFile()
        {
            GetDefaults();
            string filepath = _dir + @"SandPRemoved.csv";
            SymbolListBuilder builder = new SymbolListBuilder();
            FileInfo symbolFileInfo = new FileInfo(filepath);
            var result = builder.BuildSymbolsStringFromFile(symbolFileInfo);
            Assert.IsTrue(result.Length > 0);
        }
        private void GetDefaults()
        {
            _dir = Config.GetDefaultDownloadDirectory();
            _spyfile = new FileInfo(_dir + TestInputfilename);
             
        }
    }
}
