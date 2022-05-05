using System.Collections.Generic;

namespace IRecordweb.Models
{
    public class BonusEntryViewModel
    {
        public string MemberId { get; set; }
        public string Member { get; set; }
        public string DematAC { get; set; }
        public string OldStockQty { get; set; }
        public string NewScriptCAQty { get; set; }
        public List<BonusEntryDetils> ListofDetils { get; set; }
        public bool IsShow { get; set; }

    }
    public class BonusEntryDetils
    {
        public string Consultant { get; set; }
        public string Broker { get; set; }
        public string InvestmentType { get; set; }
        public string CAQty { get; set; }
        public double AllocateQty { get; set; }


    }
}