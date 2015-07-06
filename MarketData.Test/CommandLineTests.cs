using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketData.GoogleFinance;
using MarketData.ToolBox.Utility;

namespace MarketData.Test
{
    [TestClass]
    public class CommandLineTests
    {
        [TestMethod]
        public void ParsesAutomatic()
        {
            string[] args = new string[1];
            args[0] = "-a";
            Arguments arguments = new Arguments(args);
            Assert.IsNotNull(arguments["a"]);
        }
        [TestMethod]
        public void ParsesInteractive()
        {
            string[] args = new string[1];
            args[0] = "-i";
            Arguments arguments = new Arguments(args);
            Assert.IsNotNull(arguments["i"]);
        }
        [TestMethod]
        public void ParsesMinute()
        {
            string[] args = new string[1];
            args[0] = "-m";
            Arguments arguments = new Arguments(args);
            Assert.IsNotNull(arguments["m"]);
        }
        [TestMethod]
        public void ParsesEod()
        {
            string[] args = new string[1];
            args[0] = "-e";
            Arguments arguments = new Arguments(args);
            Assert.IsNotNull(arguments["e"]);
        }
        [TestMethod]
        public void ParsesFilename()
        {
            string defaultCsv = Config.GetDefaultInputFile();
            string[] args = new string[1];
            args[0] = "-c=\"" + defaultCsv + "\"";
            Arguments arguments = new Arguments(args);
            //System.Diagnostics.Debug.WriteLine(arguments["c"]);
            Assert.IsNotNull(arguments["c"]);
            Assert.AreEqual(defaultCsv,arguments["c"]);
        }



    }
}
