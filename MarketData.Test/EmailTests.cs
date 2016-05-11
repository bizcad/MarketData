using System;
using MarketData.ToolBox.Utility;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketData.Test
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        public void WillSendEmail()
        {
            string message = string.Format(@"{0} There was a problem processing GoogleFinance. \n\n{1}", DateTime.Now.ToLongDateString(), "This is a test.");
            Notifications n = new Notifications();
            var ret = n.CustomEmail("nicholasstein@cox.net", "Email Test", message);
            Assert.IsTrue(ret);
        }
    }
}
