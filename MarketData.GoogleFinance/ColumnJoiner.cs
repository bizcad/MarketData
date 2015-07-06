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
using System.Text;

namespace MarketData.GoogleFinance
{
    /// <summary>
    /// Joins a string array of columns into a comma delimited line.  No quotes
    /// </summary>
    public static class ColumnJoiner
    {
        /// <summary>
        /// Joins array members into a comma delimeted line with no quotes
        /// </summary>
        /// <param name="columns">string[] - the column array to join</param>
        /// <returns>string - the joined line</returns>
        public static string JoinColumns(string[] columns)
        {
            StringBuilder sb = new StringBuilder();
            // Concantenate all but the last column with the comma separator
            for (int i = 0; i < columns.Length-1; i++)
            {
                sb.Append(columns[i]);
                sb.Append(@",");
            }
            // Cat the last column 
            sb.Append(columns[columns.Length - 1]);
            return sb.ToString();
        }

    }
}
