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
using System.Globalization;
using System.IO;

namespace MarketData.YahooFinance
{
    /// <summary>
    /// TradeBar class for second and minute resolution data: 
    /// An OHLC implementation of the QuantConnect BaseData class with parameters for candles.
    /// </summary>
    public class TradeBar 
    {
        // scale factor used in QC equity/forex data files
        private const decimal _scaleFactor = 10000m;

        private MarketDataType _dataType = MarketDataType.Base;
        private DateTime _time = new DateTime();
        private string _symbol = "";
        private decimal _value = 0;
        private bool _isFillForward = false;

        /******************************************************** 
        * CLASS VARIABLES
        *********************************************************/
        /// <summary>
        /// Volume:
        /// </summary>
        public long Volume { get; set; }

        /// <summary>
        /// Opening price of the bar: Defined as the price at the start of the time period.
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// High price of the TradeBar during the time period.
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Low price of the TradeBar during the time period.
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// Closing price of the TradeBar. Defined as the price at Start Time + TimeSpan.
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// The closing time of this bar, computed via the Time and Period
        /// </summary>
        public DateTime EndTime
        {
            get { return Time + Period; }
            set { Period = value - Time; } 
        }

        /// <summary>
        /// The period of this trade bar, (second, minute, daily, ect...)
        /// </summary>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// True if this is a fill forward piece of data
        /// </summary>
        public bool IsFillForward
        {
            get { return _isFillForward; }
        }

        /// <summary>
        /// Current time marker of this data packet.
        /// </summary>
        /// <remarks>All data is timeseries based.</remarks>
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }


        /// <summary>
        /// String symbol representation for underlying Security
        /// </summary>
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
            }
        }

        /// <summary>
        /// Value representation of this data packet. All data requires a representative value for this moment in time.
        /// For streams of data this is the price now, for OHLC packets this is the closing price.
        /// </summary>
        public decimal Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// As this is a backtesting platform we'll provide an alias of value as price.
        /// </summary>
        public decimal Price
        {
            get
            {
                return Value;
            }
        }
        /// <summary>
        /// Market Data Type of this data - does it come in individual price packets or is it grouped into OHLC.
        /// </summary>
        /// <remarks>Data is classed into two categories - streams of instantaneous prices and groups of OHLC data.</remarks>
        public MarketDataType DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        public decimal ScaleFactor
        {
            get { return _scaleFactor; }
        }

        /******************************************************** 
        * CLASS CONSTRUCTORS
        *********************************************************/
        /// <summary>
        /// Default initializer to setup an empty tradebar.
        /// </summary>
        public TradeBar()
        {
            Symbol = "";
            Time = new DateTime();
            Value = 0;
            DataType = MarketDataType.TradeBar;
            Open = 0; 
            High = 0;
            Low = 0; 
            Close = 0;
            Volume = 0;
            Period = TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// Cloner constructor for implementing fill forward. 
        /// Return a new instance with the same values as this original.
        /// </summary>
        /// <param name="original">Original tradebar object we seek to clone</param>
        public TradeBar(TradeBar original) 
        {
            Time = new DateTime(original.Time.Ticks);
            Symbol = original.Symbol;
            Value = original.Close;
            Open = original.Open;
            High = original.High;
            Low = original.Low;
            Close = original.Close;
            Volume = original.Volume;
            Period = original.Period;
        }

        /// <summary>
        /// Initialize Trade Bar with OHLC Values:
        /// </summary>
        /// <param name="time">DateTime Timestamp of the bar</param>
        /// <param name="symbol">Market MarketType Symbol</param>
        /// <param name="open">Decimal Opening Price</param>
        /// <param name="high">Decimal High Price of this bar</param>
        /// <param name="low">Decimal Low Price of this bar</param>
        /// <param name="close">Decimal Close price of this bar</param>
        /// <param name="volume">Volume sum over day</param>
        /// <param name="period">The period of this bar, specify null for default of 1 minute</param>
        public TradeBar(DateTime time, string symbol, decimal open, decimal high, decimal low, decimal close, long volume, TimeSpan? period = null)
        {
            Time = time;
            Symbol = symbol;
            Value = close;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            Period = period ?? TimeSpan.FromMinutes(1);
        }

        /******************************************************** 
        * CLASS METHODS
        *********************************************************/
       
    } // End Trade Bar Class
}
