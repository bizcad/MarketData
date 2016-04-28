using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Barchart.Models
{
    public class PainRow
    {
        public decimal currentPrice { get; set; }
        public decimal strike { get; set; }
        public decimal CallOpenInterest { get; set; }
        public decimal PutOpenInterest { get; set; }
        public decimal Put { get; set; }
        public decimal Call { get; set; }
        public decimal Ext { get; set; }
    }
}
