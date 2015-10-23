/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MarketData.GoogleFinance
{
    /// <summary>
    /// Builds a List of Symbols from a csv file downloaded from the NASDAQ page   
    /// </summary>
    public class SymbolListBuilder
    {
        private Dictionary<string, string> symbolList = new Dictionary<string, string>();
        private List<string> linelist = new List<string>();
        #region public
        /// <summary>
        /// Entry point into the class. It builds a list from a csv file 
        /// </summary>
        /// <param name="symbolFileInfo">FileInfo - the symbol file</param>
        /// <returns>List<string>A list of symbols</string></returns>
        public Dictionary<string, string> BuildListFromFile(FileInfo symbolFileInfo)
        {
            if (!ReadSymbolFileIntoDictionary(symbolFileInfo))
            {
                // only make a backup of the old csv file if there was a blank exchange column (second column)
                SaveBackupCopyOfSymbolFile(symbolFileInfo);
            }

            // Saving a sorted copy of the symbol list is cheap, only once per run
            //  and has the advantage of allowing the user to add symbols to the 
            //  bottom of the file and having them moved to their sorted position;
            //  with the exchange in the second column
            SaveSortedSymbolList(symbolFileInfo);
            return symbolList;
        }
        public string BuildSymbolsStringFromFile(FileInfo symbolFileInfo)
        {
            string s = string.Empty;
            if (!ReadSymbolFileIntoDictionary(symbolFileInfo))
            {
                // only make a backup of the old csv file if there was a blank exchange column (second column)
                SaveBackupCopyOfSymbolFile(symbolFileInfo);
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in symbolList.Keys)
            {
                sb.Append(item);
                sb.Append(",");
            }
            s = sb.ToString();
            s = s.Remove(s.Length - 1);
            using (var sr = new StreamWriter(symbolFileInfo.FullName.Replace(".csv", ".txt")))
            {

                sr.Write(s);
                sr.Flush();

            }
            return s;

        }
        #endregion
        #region private
        /// <summary>
        /// Reads the ticker symbol/exchange symbol info from a csv file
        /// </summary>
        /// <param name="symbolFileInfo">FileInfo - Points to the symbol file</param>
        /// <returns>bool - true if the exchanges were in the file, false if any blank second column was found</returns>
        private bool ReadSymbolFileIntoDictionary(FileInfo symbolFileInfo)
        {
            bool foundNoBlankExchangeColumn = true;
            using (StreamReader sr = new StreamReader(symbolFileInfo.FullName))
            {
                try
                {
                    //string buffer = sr.ReadLine();
                    //if (buffer != null && buffer.Split(',')[0].ToLower().Contains("symbol"))
                    //    buffer = sr.ReadLine();

                    while(!sr.EndOfStream)
                    {
                        string buffer = sr.ReadLine();
                        
                        if (!buffer.Contains(","))
                        {
                            buffer += ",";
                        }

                        string[] columns = buffer.Split(',');
                        string symbol = columns[0];
                        {
                            if (columns.Length == 1)
                            {
                                System.Diagnostics.Debug.WriteLine("here");
                            }
                            if (columns[1].Length == 0)
                            {
                                foundNoBlankExchangeColumn = false;
                                ExchangeLookup lookup = new ExchangeLookup(symbol);
                                columns[1] = lookup.GetExchangeForSymbol(symbol);
                            }
                            symbol = symbol.Replace("\"", "");
                            if (symbol.Contains(@"^"))
                            {
                                continue;
                            }
                                
                            // Skip duplicate symbols
                            if (!symbolList.ContainsKey(symbol))
                            {
                                symbolList.Add(symbol, columns[1]);
                                linelist.Add(ColumnJoiner.JoinColumns(columns));
                            }
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return foundNoBlankExchangeColumn;
        }
        /// <summary>
        /// Sorts and saves the symbol list to the original file
        /// </summary>
        /// <param name="symbolFileInfo">FileInfo - Points to the symbol file</param>
        private void SaveSortedSymbolList(FileInfo symbolFileInfo)
        {
            // Save a sorted copy of the line list of symbols and names
            using (StreamWriter wr = new StreamWriter(symbolFileInfo.FullName, false))
            {
                foreach (string line in linelist.OrderBy(l => l.Substring(0, l.IndexOf(",", System.StringComparison.Ordinal))))
                {
                    wr.WriteLine(line);
                }
                wr.Flush();
            }
        }
        /// <summary>
        /// Saves a backup copy of the symbol file if there were any blank exchanges
        /// </summary>
        /// <param name="symbolFileInfo">FileInfo - Points to the symbol file</param>
        private static void SaveBackupCopyOfSymbolFile(FileInfo symbolFileInfo)
        {
            string newfilename = symbolFileInfo.FullName.Replace(".csv", "-old.csv");
            symbolFileInfo.CopyTo(newfilename, true);
        }
        #endregion
    }
}
