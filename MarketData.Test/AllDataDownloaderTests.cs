using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarketData.GoogleFinance;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace MarketData.Test
{
    [TestFixture]
    public class AllDataDownloaderTests
    {

        [Test]
        public async Task WritesAStreamAsync()
        {
            const string testoutputfilename = "test";
            string dir =  Config.GetDefaultDownloadDirectory();
            var files = new DirectoryInfo(dir).GetFiles().Where(n => n.Name == testoutputfilename + ".zip");
            var fileInfos = files as IList<FileInfo> ?? files.ToList();
            if (fileInfos.Any())
            {
                foreach (var file in fileInfos)
                {
                    File.Delete(file.FullName);
                }
            }
            FileInfo spyfile = new FileInfo(dir + @"\SPY_Companies.csv");

            AllDataDownloader dl = new AllDataDownloader(spyfile, null)
            {
                ZipOutput = true
            };
            await dl.WriteStreamAsync(DateTime.Now, "Test Data", dir, testoutputfilename);

            files = new DirectoryInfo(dir).GetFiles().Where(n => n.Name == testoutputfilename + ".zip");

            Assert.IsTrue(files.Any());
        }
        /// <summary>
        /// Tests getting the SPY list from a csv file in the Executing OutputDirectory and writing a {ExecutingDirectory}/daily/spy.csv file
        /// </summary>
        /// <returns>Task - not used</returns>
        [Test]
        public async Task GetsCsvSpyFromGoogleFinance()
        {
            const string testInputfilename = @"\SPY_Companies.csv";
            string dir = Config.GetDefaultDownloadDirectory();
            FileInfo spyfile = new FileInfo(dir + testInputfilename);
            AllDataDownloader dl = new AllDataDownloader(spyfile, null)
            {
                ZipOutput = false,
                OutputDirectory = dir
            };
            await dl.DownloadDataFromListAsync();
            DirectoryInfo di = new DirectoryInfo(dir + @"\daily\");
            var files = di.GetFiles().Where(n => n.Name.Contains("csv"));
            int count = files.Count();
            Assert.IsTrue(count > 0);

        }
        /// <summary>
        /// Tests getting the SPY list from a csv file in the Executing OutputDirectory and writing a {ExecutingDirectory}/daily/spy.zip file
        /// </summary>
        /// <returns>Task - not used</returns>
        [Test]
        public async Task GetsAndZipsSpyFromGoogleFinance()
        {
            const string testInputfilename = @"\SPY_Companies.csv";
            string dir = Config.GetDefaultDownloadDirectory();
            FileInfo spyfile = new FileInfo(dir + testInputfilename);
            DirectoryInfo di = new DirectoryInfo(dir + @"\daily\");

            AllDataDownloader dl = new AllDataDownloader(spyfile, null)
            {
                ZipOutput = true,
                OutputDirectory = dir
            };
            await dl.DownloadDataFromListAsync();
            var files = di.GetFiles().Where(n => n.Name.Contains("zip"));
            int count = files.Count();
            Assert.IsTrue(count > 0);

        }
        /// <summary>
        /// There was a bug in the formatData when AAMC had a price higher than $1000  1.0024E7
        /// This test assures that it was fixed by converting to decimal, then to int64.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ProcessesAamcWithPriceOver1000Dollars()
        {
            const string testInputfilename = @"\AAMC_Companies.csv";
            string dir = Config.GetDefaultDownloadDirectory();
            FileInfo spyfile = new FileInfo(dir + testInputfilename);
            DirectoryInfo di = new DirectoryInfo(dir + @"\daily\");

            AllDataDownloader dl = new AllDataDownloader(spyfile, null)
            {
                ZipOutput = true,
                OutputDirectory = dir
            };
            await dl.DownloadDataFromListAsync();
            var files = di.GetFiles().Where(n => n.Name.Contains("zip"));
            int count = files.Count();
            Assert.IsTrue(count > 0);

        }
        /// <summary>
        /// There was a bug in that intel had a volume too large for an Int32. 
        /// "2320384000" imagine that.
        /// This test makes sure it was fixed by converting to int64.  
        /// </summary>
        /// <returns>nothing</returns>
        [Test]
        public async Task CanProcessIntcWithVolumeGTInt32MaxValue()
        {
            const string testInputfilename = @"\INTC_Companies.csv";
            string dir = Config.GetDefaultDownloadDirectory();
            FileInfo spyfile = new FileInfo(dir + testInputfilename);
            DirectoryInfo di = new DirectoryInfo(dir + @"\daily\");

            AllDataDownloader dl = new AllDataDownloader(spyfile, null)
            {
                ZipOutput = true,
                OutputDirectory = dir
            };
            await dl.DownloadDataFromListAsync();
            var files = di.GetFiles().Where(n => n.Name.Contains("zip"));
            int count = files.Count();
            Assert.IsTrue(count > 0);

        }
        [Test]
        public async Task DataForAamrq()
        {
            FileInfo aamqrFileInfo = new FileInfo(@"I:\Dropbox\JJ\badsymbols.csv");
            AllDataDownloader dl = new AllDataDownloader(aamqrFileInfo.FullName, null)
            {
                ZipOutput = true,
                OutputDirectory = aamqrFileInfo.DirectoryName + @"\data\"
            };
            dl.SymbolList = aamqrFileInfo.FullName;
            //
            // Puts it in I:\Dropbox\JJ\equity\daily instead of I:\Dropbox\JJ\data\equity\daily 
            //
            await dl.DownloadDataFromListAsync();
            var files = new DirectoryInfo(dl.OutputDirectory).GetFiles("aamrq.*", SearchOption.AllDirectories);
            int count = files.Count();
            Assert.IsTrue(count > 0);
        }
        
    }
}