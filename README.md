# MarketData

Gets market data from free sources.  The initial commit only uses GoogleFinance but I hope to add Yahoo and others.  

Documentation as pdf and docx are in the Documentation folder.  They may be a little behind the current version.  

#Installation

Clone the project into your working Visual Studio\Projects folder.  Mark either MarketData.GoogleFinance.Ui or MarketData.GoogleFinanceDownloader as your start up project.  If you have VS set up appropriately, the first time you compile VS will get the necessary dependencies from NuGet and add them to your projects.  The solution is in debug mode for your compiling pleasure.  Feel free to recompile in release for better performance.

If you have trouble, feel free to add an issue to the repo.

#Some usage tips

The programs to run are in the bin\Debug directory for either the cli or the UI.

The work is all done in the MarketData.GoogleFinance dll with some help from MarketData.ToolBox for zipping, enums and command line arguments.

The UI is documented, albeit a little out of date, in the Documentation folder.  

If you want End Of Day data, that is the default.  It will download into a sub-folder with the name of the symbol you specify. 

If you want minute data click the Minute Data button.  You will get the last 15 days worth of data, which is a GoogleFinance limitation.  The default Exchange is NYSEARCA and the default symbol is SPY. If you do not know the Exchange, leave it blank and the program will ask GoogleFinance for it.

Click the download data button and take a look at your data.  If you are happy, click the Save to File button.  

-If you want the raw GoogleFinance data click the check box.  

-If you want Google's funky Unix ticks + offset date translated into a more readable (US format) date time, un-check the Raw Data checkbox

-If you want the file zipped, click the check box.

-If you do minute data and want it split into daily files, click the check box.

-If you want the date translated into milliseconds since midnight, click the Date Time checkbox and it will then say Milliseconds.

Ticker Symbols can be loaded from a csv file.  Only the first two colums matter, the rest can be what every you want.  The first column is the symbol, and the second is the Exchange symbol.  If the second column is blank, the program will fill it in for you by asking GoogleFinance and make a backup copy of your original file.  The program will also sort your csv file into symbol sequence.  The test folder has several examples.

The command line interface defaults to asking you questions about the source file for the list of symbols you wish to download, the directory you wish to download to, and the resolution (eod or minute).  The default values are in the app.config file which you can change to suit your environment.  After you answer the question, it will just run. A long list can take a while so check your hard disk light for some activity.  If you are running in debug mode System.Diagnostics.Debug gets a message of the current symbol being retreived.

If you run the cli with an argument of -a it will automatically run with the values from app.config.  Set up a shortcut with the -a arguement and you can run it nightly with one click.



