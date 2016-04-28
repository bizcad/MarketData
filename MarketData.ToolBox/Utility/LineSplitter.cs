using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.ToolBox.Utility
{
    public static class LineSplitter
    {
        public static string[] SplitFilterCommasInQuotedStrings(string line)
        {
            return SimpleSplitOnCommas(FilterCommasInFields(line));
        }
        public static string[] SimpleSplitOnCommas(string line)
        {
            return line.Split(',');
        }
        public static string[] SplitLine(string line)
        {
            string filtered = FilterCommasInFields(line);
            return filtered.Split(',');
        }

        private static string FilterCommasInFields(string line)
        {
            string dq = "\"";
            while (line.Contains(dq))
            {
                int start = line.IndexOf(dq);
                int end = line.IndexOf(dq, start + 1);
                if (start < 0 || end < 0)
                    return string.Empty;
                string partline = line.Substring(start, end - start + 1);
                string newpart = partline.Replace(",", string.Empty).Replace(dq, string.Empty);
                line = line.Replace(partline, newpart);
            }
            return line;
        }

    }
}
