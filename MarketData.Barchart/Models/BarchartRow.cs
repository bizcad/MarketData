using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Barchart.Models
{
    public class BarchartRow
    {
        public decimal Strike { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Change { get; set; }
        public decimal Volume { get; set; }
        public decimal OpenInterest { get; set; }
        public decimal Delta { get; set; }
        public decimal Premium { get; set; }
    }
}
