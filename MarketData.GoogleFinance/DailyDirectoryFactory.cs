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
using System.IO;

namespace MarketData.GoogleFinance
{
    /// <summary>
    /// Creates a FileInfo for a single letter folder based upon the first letter of the symbol
    /// </summary>
    public static class DailyDirectoryFactory
    {
        /// <summary>
        /// Checks to see if a folder with the "daily" directory exist, if not creates it.
        /// </summary>
        /// <param name="outputDirectoryInfo">DirectoryInfo - the info for the root directory</param>
        /// <returns></returns>
        public static DirectoryInfo Create(DirectoryInfo outputDirectoryInfo)
        {

            string dailyDirectory = outputDirectoryInfo.FullName;
            if (!dailyDirectory.EndsWith(@"\"))
                dailyDirectory += @"\";
            dailyDirectory += @"equity\usa\daily\";
            if (!Directory.Exists(dailyDirectory))
                Directory.CreateDirectory(dailyDirectory);

            return new DirectoryInfo(dailyDirectory);
        }
    }
}
