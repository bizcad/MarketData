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
using System.Text;

namespace MarketData.GoogleFinance
{
    public static class FilenameDateFormatter
    {
        /// <summary>
        /// Formats a date time for use in file names
        /// </summary>
        /// <param name="dt">DateTime - the date to format</param>
        /// <returns>string - a date formatted as yyyymmdd</returns>
        public static string FormatDt(DateTime dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(dt.Year);
            if (dt.Month < 10)
                sb.Append("0");
            sb.Append(dt.Month);
            if (dt.Day < 10)
                sb.Append("0");
            sb.Append(dt.Day);
            return sb.ToString();
        }
    }
}
