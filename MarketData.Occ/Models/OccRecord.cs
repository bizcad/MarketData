using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Occ.Models
{
    public class OccRecord
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public DateTime ContractDate { get; set; }
        public decimal Strike { get; set; }
        public string CP { get; set; }
        public decimal CallOpenInterest { get; set; }
        public decimal PutOpenInterest { get; set; }
        public decimal PositonLimit { get; set; }
    }
}
