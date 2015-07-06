﻿/*
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketData.GoogleFinance;
using MarketData.ToolBox;
using MarketData.ToolBox.Utility;

namespace MarketData.GoogleFinanceDownloader
{
    /// <summary>
    /// Entry point into the console app
    /// </summary>
    internal class Program
    {

        /// <summary>
        /// Entry point into the console app
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                Arguments commandLine = new Arguments(args);

                GetHelp(commandLine);
                // Defalult to minute unless the user specifies eod
                var resolution = GetResolution(commandLine);
                var runmode = GetRunmode(commandLine);
                var destinationDirectory = GetDestinationDirectory(commandLine);
                var defaultInputFile = GetDefaultInputFile(commandLine);
                DisplayInteractiveInstructions(runmode);

                GetOptionsInteractive(runmode, ref defaultInputFile, ref destinationDirectory, ref resolution);
                if (runmode == Enums.Runmode.Automatic)
                {
                    Console.WriteLine("1. Source file for ticker symbol and exchange list: \n" + defaultInputFile);
                    Console.WriteLine("2. Destination LEAN Data directory: \n" + destinationDirectory);
                    Console.WriteLine("3. Resolution: " + "minute");
                }

                //Validate the user input:
                Validate(defaultInputFile, destinationDirectory, resolution.ToString());

                //Remove the final slash to make the path building easier:
                defaultInputFile = StripFinalSlash(defaultInputFile);
                destinationDirectory = StripFinalSlash(destinationDirectory);
                string[] validatedArgs = new string[3];
                validatedArgs[0] = defaultInputFile;
                validatedArgs[1] = destinationDirectory;
                validatedArgs[2] = resolution.ToString();

                Console.WriteLine("Processing Files ...");
                Task.Run(async () => { await MainAsync(validatedArgs); }).Wait();

                // if auto, just run the thing and exit
                if (runmode == Enums.Runmode.Interactive)
                {
                    Console.WriteLine("Done. Press any key to exit.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }

        private static void GetOptionsInteractive(Enums.Runmode runmode, ref string defaultInputFile, ref string destinationDirectory,
            ref Enum resolution)
        {
            if (runmode == Enums.Runmode.Interactive)
            {
                Console.WriteLine("1. Source file for ticker symbol and exchange list: \n" + defaultInputFile);
                defaultInputFile = Console.ReadLine();
                if (defaultInputFile.Length == 0)
                    defaultInputFile = Config.GetDefaultInputFile();
                Console.WriteLine("2. Destination Output root directory: \n" + destinationDirectory);
                destinationDirectory = Console.ReadLine();
                if (destinationDirectory.Length == 0)
                    destinationDirectory = Config.GetDefaultDownloadDirectory();
                Console.WriteLine("3. Enter Resolution (minute/EOD): " + "minute");
                var res = Console.ReadLine();
                if (res.Length == 0)
                    resolution = Enums.Resolution.minute;
            }
        }

        private static void DisplayInteractiveInstructions(Enums.Runmode runmode)
        {
            if (runmode == Enums.Runmode.Interactive)
            {
                //Document the process:
                Console.WriteLine("QuantConnect.ToolBox: Google Finance Downloader: ");
                Console.WriteLine("==============================================");
                Console.WriteLine("The Downloader gets Minute data from GoogleFinance and saves it");
                Console.WriteLine(" into the LEAN Algorithmic Trading Engine Data Format.");
                Console.WriteLine("Parameters are optional: ");
                Console.WriteLine("   1> Source File is a list of symbols and exchanges as a .csv file.");
                Console.WriteLine(
                    "   2> Output Directory is the root where the output is placed; which could, for example, be located under Lean/Data");
                Console.WriteLine("   3> Resolution. (either minute or EOD the end of day)");
                Console.WriteLine();
                Console.WriteLine("NOTE: THIS PROGRAM WILL OVERWRITE ONLY TODAY'S FILES.");
                Console.WriteLine("Press any key to Continue or Escape to quit.");
                Console.WriteLine();
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
            }
        }

        private static string GetDefaultInputFile(Arguments commandLine)
        {
            var defaultInputFile = Config.GetDefaultInputFile();
            if (commandLine["c"] != null)
            {
                defaultInputFile = commandLine["c"];
            }
            if (commandLine["csv"] != null)
            {
                defaultInputFile = commandLine["csv"];
            }
            return defaultInputFile;
        }

        private static string GetDestinationDirectory(Arguments commandLine)
        {
            var destinationDirectory = Config.GetDefaultDownloadDirectory();
            if (commandLine["o"] != null)
            {
                destinationDirectory = commandLine["o"];
            }
            if (commandLine["out"] != null)
            {
                destinationDirectory = commandLine["out"];
            }
            return destinationDirectory;
        }

        private static void GetHelp(Arguments commandLine)
        {
            if (commandLine["h"] != null || commandLine["help"] != null)
            {
                Error(FormatCommandLineHelp());
            }
        }

        private static Enums.Runmode GetRunmode(Arguments commandLine)
        {
            var runmode = Enums.Runmode.Interactive; // default to interactive
            if (commandLine["a"] != null || commandLine["auto"] != null)
            {
                runmode = Enums.Runmode.Automatic;
            }
            if (commandLine["i"] != null || commandLine["interactive"] != null)
            {
                runmode = Enums.Runmode.Interactive;
            }
            return runmode;
        }

        private static Enum GetResolution(Arguments commandLine)
        {
            var resolution = Config.GetDefaultResolution();
            if ((commandLine["e"] != null || commandLine["eod"] != null) &&
                (commandLine["m"] != null || commandLine["minute"] != null))
            {
                Error("End of day and minute data are mutually exclusive.");
            }
            else
            {
                if (commandLine["e"] != null || commandLine["eod"] != null)
                {
                    resolution = Enums.Resolution.eod;
                }
            }
            return resolution;
        }

        /// <summary>
        /// The async portion of the app
        /// </summary>
        /// <param name="args">string[] - a validated copy of the command line args</param>
        /// <returns>nothing</returns>
        private async static Task MainAsync(string[] args)
        {
            if (args[2] == "minute")
            {
                try
                {
                    FileInfo tickerListInfo = new FileInfo(args[0]);
                    string directory = args[1];

                    MinuteDownloader minuteDownloader = new MinuteDownloader(tickerListInfo, directory)
                    {
                        FormatAsMilliseconds = true,
                        SplitDays = true,
                        ZipOutput = true
                    };
                    await minuteDownloader.DownloadDataFromListAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + e.StackTrace);
                }
            }
            if (args[2].ToLower() == "eod")
            {
                try
                {
                    FileInfo tickerListInfo = new FileInfo(args[0]);
                    string directory = args[1];

                    AllDataDownloader downloader = new AllDataDownloader(tickerListInfo, directory) { ZipOutput = true };
                    await downloader.DownloadDataFromListAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + e.StackTrace);
                }
            }


        }

        /// <summary>
        /// Application error: display error and then stop conversion
        /// </summary>
        /// <param name="error">Error string</param>
        private static void Error(string error)
        {
            Console.WriteLine(error);
            Console.ReadKey();
            Environment.Exit(0);
        }

        /// <summary>
        /// Get the count of the files to process
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <returns></returns>
        /// <remarks>not used</remarks>
        private static int GetCount(string sourceDirectory)
        {
            var count = 0;
            foreach (var directory in Directory.EnumerateDirectories(sourceDirectory))
            {
                count += Directory.EnumerateFiles(directory, "*.csv").Count();
            }
            return count;
        }

        /// <summary>
        /// Remove the final slash to make path building easier
        /// </summary>
        private static string StripFinalSlash(string directory)
        {
            return directory.Trim('/', '\\');
        }

        /// <summary>
        /// Get the date component of the file path.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime GetDate(string date)
        {
            var splits = date.Split('/', '\\');
            var dateString = splits[splits.Length - 1].Replace("allstocks_", "");
            return DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Extract the symbol from the path
        /// </summary>
        private static string GetSymbol(string filePath)
        {
            var splits = filePath.Split('/', '\\');
            var file = splits[splits.Length - 1];
            file = file.Trim('.', '/', '\\');
            file = file.Replace("table_", "");
            return file.Replace(".csv", "");
        }

        /// <summary>
        /// Validate the users input and throw error if not valid
        /// </summary>
        private static void Validate(string sourceSymbolListFile, string destinationDirectory, string resolution)
        {
            if (string.IsNullOrWhiteSpace(sourceSymbolListFile))
            {
                Error("Error: Please enter a valid source directory.");
            }
            if (string.IsNullOrWhiteSpace(destinationDirectory))
            {
                Error("Error: Please enter a valid destination directory.");
            }
            if (!File.Exists(sourceSymbolListFile))
            {
                Error("Error: Source file does not exist.");
            }
            if (!Directory.Exists(destinationDirectory))
            {
                Error("Error: Destination directory does not exist.");
            }
            if (resolution != "minute" && resolution != "eod")
            {
                Error("Error: Resolution specified is not supported. Please enter tick or eod");
            }
        }

        private static string FormatCommandLineHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("QuantConnect.GoogleFinanceDownloader Help");
            sb.AppendLine();
            sb.AppendLine("Valid Commands:");
            sb.AppendLine("-a or --auto Run the program using the parameters found in app.config");
            sb.AppendLine("-i or --interactive Prompt for program parameters");
            sb.AppendLine("-h or --help Display this help screen");
            sb.AppendLine("-c or --csv=\"{filepath}\" will read from the file at {filepath}");
            sb.AppendLine("-o or --out=\"{rootpath}\" will direct the output starting at {rootpath} ");
            sb.AppendLine("             For minute data directories for an exchange, ");
            sb.AppendLine("             a first letter of the symbol,");
            sb.AppendLine(@"             and ticker symbol.  For Example C:\My files\NYSEARCA\S\SPY\");
            sb.AppendLine(@"             For EOD data the directory would be like C:\My files\daily\");
            sb.AppendLine("-m or --minute sets the resolution to minutes and gets the last 15 days data");
            sb.AppendLine("-e or --eod sets the resolution to daily and gets all data since 1/3/2000");
            sb.AppendLine();
            sb.AppendLine("Valid parameters forms: {-,/,--}param{ ,=,:}((\",')value(\",'))");
            sb.AppendLine("A command starts with - or -- or /");
            sb.AppendLine(" followed by a command name or abbreviation");
            sb.AppendLine("If the command has a value, the value delimiter is = or : or a space");
            sb.AppendLine("If the value has spaces in it, such as with a filename, surround with quotes");
            sb.AppendLine("");
            sb.AppendLine(" Examples: -a --help /c:=\"C:\\My Files\\SPY.csv\" -e");
            sb.AppendLine("");
            sb.AppendLine("Press any key to exit.");

            return sb.ToString();
        }

    }
}