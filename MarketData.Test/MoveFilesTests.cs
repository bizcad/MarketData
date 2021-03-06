﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketData.GoogleFinance;
using MarketData.GoogleFinanceDownloader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    /// <summary>
    /// Summary description for MoveFilesTests
    /// </summary>
    [TestClass]
    public class MoveFilesTests
    {
        public MoveFilesTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void MovesFiles()
        {
            FileMover.CopyDailyFiles(new DirectoryInfo(@"H:\GoogleFinanceData\NYSE\"));
            Assert.IsTrue(File.Exists(@"H:\GoogleFinanceData\equity\usa\minute\ATT\20150519_trade.zip"));
        }
        [TestMethod]
        public void RenamesInteriorFileInZipFile()
        {
            FileMover.RenameInteriorFiles(new DirectoryInfo(@"H:\GoogleFinanceData\equity\usa\minute\"));
            Assert.IsTrue(true);
        }
        /// <summary>
        /// This method reads JJs symbols.txt and verifies that zip files exist for all entries.
        /// In the past, aamrq was a discontinued symbol and caused it to fail.
        /// Look in AllDataDownloaderTests for a test that adds it.
        /// </summary>
        [TestMethod]
        public void TestJjNames()
        {
            FileInfo jjlistpath = new FileInfo(@"I:\Dropbox\JJ\symbols.txt");
            var ret = FileMover.CheckJJList(jjlistpath);
            Assert.IsTrue(ret);

        }

        [TestMethod]
        public void SortsJjSymbols()
        {
            FileCopier fc = new FileCopier();
            
            fc.SymbolsFromFile();
            var s = fc.Symbols;

            Assert.IsTrue(s.Count > 0);
            Assert.IsTrue(s.Contains("ABEV"));

        }
    }
}
