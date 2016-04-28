using System;
using MarketData.Volatility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    [TestClass]
    public class VolatilityTests
    {
        [TestMethod]
        public void GetsPage()
        {
            Volatility.VolatilityCalculator calc = new VolatilityCalculator();
            string ret = calc.GetPage();
            Assert.IsTrue(ret.Length > 0);
        }
    }
}
