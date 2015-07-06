using NUnit.Framework;
using MarketData.GoogleFinance;

namespace MarketData.Test
{

    [TestFixture]
    public class AssemblyLocatorTest
    {
        [Test]
        public void GetsTheFolderForRunningAssemblyDebug()
        {
            // You need to replace this string with your local project path.
            string exeDir = "c:\\users\\nick\\appdata\\local\\temp\\".ToLower();
            string executingDirectory = AssemblyLocator.ExecutingDirectory().ToLower();
            Assert.IsTrue(executingDirectory.Contains(exeDir));
        }
    }
}
