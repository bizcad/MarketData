using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Occ.Models
{
    public class ListedOption
    {
        public string OptionSymbol { get; set; }
        public string UnderlyingSymbol { get; set; }
        public string SymbolName { get; set; }
        public string Exchanges { get; set; }
        public string PostionLimit { get; set; }
        public string OnnProductType { get; set; }

    }
}
