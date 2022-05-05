using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class BONUSENTRY
    {
        [Key]
        public int ID { get; set; }
        public string Script { get; set; }
        [Display(Name = "Record Date")]
        public string RecordDate { get; set; }
        public int ScriptCode { get; set; }
        public int MemberID { get; set; }
        public int FinancialID { get; set; }
        [Display(Name = "Exitsing ISIN")]
        public string OldISIN { get; set; }
        [Display(Name = "New ISIN")]
        public string NewISIN { get; set; }
        [Display(Name = "Bonus QTY Per")]
        public double Bonusqtyper { get; set; }
        public double Share { get; set; }
        [Display(Name = "Sr.No")]
        public int srno { get; set; }
        [Display(Name = "Member Name")]
        public string MemberName { get; set; }
        [Display(Name = "Holding Type")]
        public string HoldingType { get; set; }
        [Display(Name = "Demat Name")]
        public string DematName { get; set; }
        [Display(Name = "Broker Name")]
        public string brokerName { get; set; }
        public string Consultant { get; set; }
        [Display(Name = "Holding Qty")]
        public double holdingQty { get; set; }
        [Display(Name = "Received Qty")]
        public double ReceivedQty { get; set; }
        [Display(Name = "Total Qty")]
        public double TotalQty { get; set; }
        public string Ac_Code { get; set; }

    }
}