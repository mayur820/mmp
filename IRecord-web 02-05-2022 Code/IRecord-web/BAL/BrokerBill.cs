using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BAL
    {
  public class BrokerBill
        {
        [Key]
        public int BillID { get; set; }
        [Display(Name = "Member")]
        public string Member { get; set; }
        public string Broker { get; set; }
        [Display(Name = "Settlement No")]
        public int SettlementNo { get; set; }
        [Display(Name = "Bill No")]
        public int BillNo { get; set; }
       // [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        public string Demat { get; set; }
        public string Consultant { get; set; }
        [Display(Name = "Holding Type")]
        public string HoldingType { get; set; }
        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; }
        [Display(Name = "Import Bill")]
        public string  ImportBill { get; set; }
        [Display(Name = "Broker Format")]
        public string BrokerFormat { get; set; }
        [Display(Name = "File Path")]
        public HttpPostedFileBase FilePath { get; set; }
        [Display(Name = "PDF Password")]
        public string PdfPassword { get; set; }
        public string Type { get; set; }
        public string ScriptName { get; set; }
        public bool IntraDay { get; set; }
        public int Qty { get; set; }
        public float GrossRate { get; set; }
        public float GrossAmt { get; set; }
        public float BrokeragePerUnit { get; set; }
        public float BrokerageAmt { get; set; }
        public float NetRate { get; set; }
        public float NetAmt { get; set; }
        public bool SLBM { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int BrokerBillFormatID { get; set; }
        public string BrokerFormatName { get; set; }
        public bool isDefault { get; set; }
        public List<BrokerBill> Accountlist { get; set; }
        public List<BrokerBill> BrokerBillList { get; set; }

        }
    }
