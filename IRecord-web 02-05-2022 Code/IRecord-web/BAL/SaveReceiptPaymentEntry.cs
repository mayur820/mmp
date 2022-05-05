using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BAL
{
    public class SaveReceiptPaymentEntry
    {
        public DateTime Date { get; set; }
        public int typeid { get; set; }
        [Display(Name = "Type")]
        public string CashBank { get; set; }
        public string Mode { get; set; }
        [Display(Name = "Payment Mode")]
        public String PaymentMode { get; set; }
        [Display(Name = "Cheque Number")]
        public string Cheque { get; set; }
        [Display(Name = "Refrence Number")]
        public string refernce { get; set; }
        [Display(Name ="Cash/Bank Account")]
        public string cashbankaccount { get; set; }
        [Display(Name ="Amount Account")]
        public string AmountAccount { get; set; }
        public string amount { get; set; }
        public string []narration { get; set; }
        //public List<SelectListItem> narration { get; set; }
        public int code { get; set; }
        public string Name { get; set; }
        public  int TransactionId { get; set; }
    
    }
}
