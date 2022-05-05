using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class BrokerageSetting
    {
        
        public double Buy { get; set; }
        public double Sell { get; set; }
        public double MaxInAccount { get; set; }
        public double MinInAccount { get; set; }
        public double PreOrderTrade { get; set; }
    }
}