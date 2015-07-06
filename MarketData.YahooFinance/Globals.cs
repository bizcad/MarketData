using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.YahooFinance
{
        /// <summary>
        /// Shortcut date format strings
        /// </summary>
        public static class DateFormat
        {
            /// Year-Month-Date 6 Character Date Representation
            public const string SixCharacter = "yyMMdd";
            /// YYYY-MM-DD Eight Character Date Representation
            public const string EightCharacter = "yyyyMMdd";
            /// Daily and hourly time format
            public const string TwelveCharacter = "yyyyMMdd HH:mm";
            /// JSON Format Date Representation
            public static string JsonFormat = "yyyy-MM-ddThh:mm:ss";
            /// MySQL Format Date Representation
            public const string DB = "yyyy-MM-dd HH:mm:ss";
            /// QuantConnect UX Date Representation
            public const string UI = "yyyy-MM-dd HH:mm:ss";
            /// EXT Web Date Representation
            public const string EXT = "yyyy-MM-dd HH:mm:ss";
            /// en-US format
            public const string US = "M/d/yyyy h:mm:ss tt";
            /// Date format of QC forex data
            public const string Forex = "yyyyMMdd HH:mm:ss.ffff";
        }
        /// <summary>
        /// Market data style: is the market data a summary (OHLC style) bar, or is it a time-price value.
        /// </summary>
        public enum MarketDataType
        {
            /// Base market data type
            Base,
            /// TradeBar market data type (OHLC summary bar)
            TradeBar,
            /// Tick market data type (price-time pair)
            Tick,
            /// Data associated with an instrument
            Auxiliary
        }
}
