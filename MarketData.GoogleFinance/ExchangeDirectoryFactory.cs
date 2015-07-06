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
    /// Factory to assure existance of the Exchange directory
    /// </summary>
    public static class ExchangeDirectoryFactory
    {
        /// <summary>
        /// Assures creation of a directory for the Exchange
        /// </summary>
        /// <param name="exchangeName">string - the name of the exchange</param>
        /// <param name="rootDirectory">optional string - the folder in which to create the directory</param>
        /// <returns></returns>
        public static DirectoryInfo Create(string exchangeName, string rootDirectory = "")
        {
            string exchangedir;
            var directory = rootDirectory ?? Config.GetDefaultDownloadDirectory();
            if (directory.Length == 0)
                directory = Config.GetDefaultDownloadDirectory();
            if (!directory.EndsWith(@"\"))
                directory += @"\";

            if (exchangeName.Length == 0)
                exchangedir = directory;
            else
            {
                exchangedir = directory + exchangeName;
            }

            if (!Directory.Exists(exchangedir))
                Directory.CreateDirectory(exchangedir);

            return new DirectoryInfo(exchangedir);
        }
    }
}
