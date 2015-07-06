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
using System.Reflection;

namespace MarketData.GoogleFinance
{
    /// <summary>
    /// Locates the directory where the application is executing
    /// </summary>
    public static class AssemblyLocator
    {
        /// <summary>
        /// Returns the folder where the application is executing including the following backslash
        /// </summary>
        /// <returns>string - the name of the directory where the application is executing</returns>
        public static string ExecutingDirectory()
        {
            var assem = Assembly.GetExecutingAssembly();
            FileInfo info = new FileInfo(assem.Location);
            string directory = info.Directory.FullName;
            if (!directory.EndsWith(@"\"))
            {
                directory += @"\";
            }
            return directory;
        }
    }
}
