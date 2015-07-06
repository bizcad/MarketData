using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarketData.GoogleFinance;
using NUnit.Framework;

namespace MarketData.Test
{
    [TestFixture]
    public class MinuteDownloaderTests
    {
        const string TestInputfilename = @"\SPY_Companies.csv";
        private string _dir;
        private FileInfo _spyfile;
        [Test]
        public async Task GetsSpyCsvFromGoogleFinance()
        {
            GetDefaults();
            DirectoryInfo di = new DirectoryInfo(_spyfile.DirectoryName + @"\NYSEARCA\S\SPY\");
            if (di.Exists)
                foreach (var file in di.EnumerateFiles())
                {
                    file.Delete();
                }

            MinuteDownloader dl = new MinuteDownloader(_spyfile, null)
            {
                FormatAsMilliseconds = true,
                SplitDays = true,
                ZipOutput = false,
                OutputDirectory = _dir
            };
            await dl.DownloadDataFromListAsync();
            var files = di.GetFiles().Where(n => n.Name.Contains("csv"));
            int count = files.Count();
            Assert.IsTrue(count > 0);

        }

        private void GetDefaults()
        {
            _dir = Config.GetDefaultDownloadDirectory();
            _spyfile = new FileInfo(_dir + TestInputfilename);
        }

        /// <summary>
        /// Gets data and zips it
        /// </summary>
        /// <returns>nothing</returns>
        /// <remarks>
        /// When you run all the tests, this one will only get one zip file, because
        /// the MinuteDownloader only downloads the file for the last day. After the CSV
        /// files have been written.  The routine is written so as to not request more 
        /// data than necessary, so it counts the number of days since the last file was written.
        /// 
        /// This means that the zip all files test below will also run well because it has
        /// some csv files to work on.
        /// </remarks>
        [Test]
        public async Task GetsSpyZipFromGoogleFinance()
        {
            GetDefaults();

            MinuteDownloader dl = new MinuteDownloader(_spyfile, null)
            {
                FormatAsMilliseconds = true,
                SplitDays = true,
                ZipOutput = true,
                OutputDirectory = _dir
            };
            await dl.DownloadDataFromListAsync();
            DirectoryInfo di = new DirectoryInfo(_dir + @"\NYSEARCA\S\SPY\");
            var files = di.GetFiles().Where(n => n.Name.Contains("zip"));
            int count = files.Count();
            Assert.IsTrue(count == 1);

        }

        /// <summary>
        /// Zip all csv files in the root download directory and recursively all subdirectories.
        /// The directory structure is:
        /// root
        ///  - Exchange symbol
        ///    - First letter of the symbol
        ///      - Symbol
        ///        Data files yyyymmdd_trade.zip
        ///        - yyyymmdd_{symbol}_Trade_Minute.csv
        /// 
        /// Along the way the routine also deletes any empty directories.
        /// </summary>
        /// <remarks>
        /// ************** Important ******************
        /// The MinuteDownloader.FindAndZipCsvFiles is not in the UI.  It is found only in
        /// the class.  This test is the way to run that routine.  The reason the routine
        /// was necessary was because the older dev versions of this program only saved to 
        /// csv files and I did not want to lose the older data.
        /// 
        /// QuantConnect can read csv files, but the zips are about 4:1 more space efficient.
        /// 
        /// This test method only needs to be run once.  Fee free to change the basedir to NYSE
        /// 
        /// </remarks>
        [Test]
        public void ZipsAllCsvFiles()
        {
            GetDefaults();
            var basedir = new DirectoryInfo(_dir + @"\NYSEARCA\");

            MinuteDownloader dl = new MinuteDownloader();
            dl.FormatAsMilliseconds = true;
            dl.SplitDays = true;
            dl.ZipOutput = true;
            dl.FindAndZipCsvFiles(basedir);

            // For test purposes look for the SPY directory, the most commonly used symbol
            DirectoryInfo di = new DirectoryInfo(basedir + @"\S\SPY\");

            // count the files and make sure there are at least 2 zip files
            var files = di.GetFiles().Where(n => n.Name.Contains("zip"));
            int count = files.Count();
            Assert.IsTrue(count > 2);
        }
    }
}
