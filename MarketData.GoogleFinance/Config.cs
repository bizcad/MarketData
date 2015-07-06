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
using System.Collections.Specialized;
using System.Configuration;
using MarketData.ToolBox;

namespace MarketData.GoogleFinance
{
    /// <summary>
    /// Gets values from the app.config file
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Gets the default download directory from the config appSettings
        /// </summary>
        /// <returns>string - the directory path</returns>
        public static string GetDefaultDownloadDirectory()
        {
            string defaultOutputDirectory = null;
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            for (int i = 0; i < appSettings.Count; i++)
            {
                if (appSettings.GetKey(i) == "defaultOutputDirectoryPath")
                {
                    defaultOutputDirectory = appSettings[i];
                }
            }
            return defaultOutputDirectory;
        }
        /// <summary>
        /// Gets the beginning of the uri for GoogleFinance downloading
        /// </summary>
        /// <returns>string - the beginning of the uri</returns>
        public static string GetGoogleFinanceApiBeginning()
        {
            string uriBeginning = null;
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            for (int i = 0; i < appSettings.Count; i++)
            {
                if (appSettings.GetKey(i) == "GOOGLE_GET_PRICES_METHOD_URI_BEGINNING")
                {
                    uriBeginning = appSettings[i];
                }
            }
            return uriBeginning;
        }
        /// <summary>
        /// Gets the default download directory from the config appSettings
        /// </summary>
        /// <returns>string - the directory path</returns>
        public static string GetDefaultInputFile()
        {
            string defaultOutputDirectory = null;
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            for (int i = 0; i < appSettings.Count; i++)
            {
                if (appSettings.GetKey(i) == "defaultInputFilePath")
                {
                    defaultOutputDirectory = appSettings[i];
                }
            }
            return defaultOutputDirectory;
        }
        /// <summary>
        /// Gets the default resolution from the config appSettings
        /// </summary>
        /// <returns>int the enum of the resolution</returns>
        public static Enums.Resolution GetDefaultResolution()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            for (int i = 0; i < appSettings.Count; i++)
            {
                if (appSettings.GetKey(i) == "defaultResolution")
                {
                    var defaultRes = appSettings[i];
                    if (defaultRes.ToLower().Contains("min"))
                    {
                        return Enums.Resolution.minute;
                    }
                    else
                    {
                        return Enums.Resolution.eod;
                    }
                }
            }
            return Enums.Resolution.minute;
        }
    }
}
