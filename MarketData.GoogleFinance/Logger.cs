using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketData.ToolBox.Utility;

namespace MarketData.GoogleFinance
{
    public class Logger
    {
        public string Filepath { get; set; }

        public Logger()
        {
            Filepath = AssemblyLocator.ExecutingDirectory() + "Log.txt";
            if (File.Exists(Filepath))
                File.Delete(Filepath);
        }
        public Logger(string filename)
        {
            Filepath = AssemblyLocator.ExecutingDirectory() + filename;
            if (File.Exists(Filepath))
                File.Delete(Filepath);
        }

        public void Log(string message)
        {
            using (var sw = new StreamWriter(Filepath, true))
            {
                sw.WriteLine(message);
                sw.Flush();
                sw.Close();
            }
            
        }
    }
}
