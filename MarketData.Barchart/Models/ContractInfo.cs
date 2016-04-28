using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Barchart.Models
{
    public class ContractInfo
    {
        /// <summary>
        /// The primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The option from the dropdown on the web page.  Equates to the Contract Name
        /// </summary>
        public string PageMode { get; set; }
        /// <summary>
        /// The option Expiration date
        /// </summary>
        public DateTime OptionsExpirationDate { get; set; }
        /// <summary>
        /// the number of days until the option expires.  OptionExpirationDate - Now
        /// </summary>
        public decimal  DaysToExpiration  { get; set; }
        /// <summary>
        /// The closing price of the underlying at 4:00 Eastern on the DailyOptionDate
        /// </summary>
        public decimal ClosingPrice { get; set; }
        /// <summary>
        /// The DailyOption string from the web page
        /// </summary>
        public string DailyOption { get; set; }
        /// <summary>
        /// The Daily Option translated to a datetime
        /// </summary>
        public DateTime DailyOptionDate { get; set; }
        /// <summary>
        /// The total of the call premium column on the web page
        /// </summary>
        public decimal CallPremiumTotal { get; set; }
        /// <summary>
        /// The total of the put premium column on the web page
        /// </summary>
        public decimal PutPremiumTotal { get; set; }
        /// <summary>
        /// The ratio of the call premium total to the put premium total on the web page
        /// </summary>
        public decimal CallPutPremiumRatio { get; set; }
        /// <summary>
        /// Total Call open interest for the contract
        /// </summary>
        public long CallOpenInterestTotal { get; set; }
        /// <summary>
        /// Total Put open interest for the contract
        /// </summary>
        public long PutOpenInterestTotal { get; set; }
        /// <summary>
        /// The ratio of call/put open interest
        /// </summary>
        public decimal CallPutOpenInterestRatio { get; set; }
        /// <summary>
        /// The strike price for the minimum pain
        /// </summary>
        public decimal MinimumPainStrike { get; set; }
        /// <summary>
        /// The amount of the minimum pain at the MinimumPainStrike price
        /// </summary>
        public decimal MinimumPainAmount { get; set; }

    }
}
