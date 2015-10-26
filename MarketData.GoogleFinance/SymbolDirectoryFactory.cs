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
using System.IO;

namespace MarketData.GoogleFinance
{
    /// <summary>
    /// Factory to create a DirectoryInfo for a symbol in the singleLetterDirectory
    /// </summary>
    public static class SymbolDirectoryFactory
    {
        /// <summary>
        /// Checks to see if a folder with the first letter of the symbol name exists
        /// If not it creates it.  The downloadede data will be saved to the symbol folder under 
        /// the first letter folder
        /// </summary>
        /// <param name="singleLetterDirectoryInfo">DirectoryInfo - the info for a created directory for the exchange</param>
        /// <param name="symbol">string - the ticker symbol</param>
        /// <returns></returns>
        public static DirectoryInfo Create(DirectoryInfo singleLetterDirectoryInfo, string symbol)
        {
            if (symbol.Length == 0)
                throw new Exception("SymbolDirectoryFactory: You must supply a symbol");

            // start with the single letter directory
            string symboldirectory = singleLetterDirectoryInfo.FullName;
            if (!symboldirectory.EndsWith(@"\"))
                symboldirectory += @"\";
            if (!symboldirectory.EndsWith("minute"))
            {
                symboldirectory += "minute";
            }
            if (!symboldirectory.EndsWith(@"\"))
                symboldirectory += @"\";

            symboldirectory += symbol;

            if (!Directory.Exists(symboldirectory))
                Directory.CreateDirectory(symboldirectory);

            return new DirectoryInfo(symboldirectory);
        }
    }
}
