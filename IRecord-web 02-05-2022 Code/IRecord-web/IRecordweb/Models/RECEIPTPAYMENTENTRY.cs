using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class RECEIPTPAYMENTENTRY
    {
        public string EntryID { get; set; }
        public DateTime Date { get; set; }
        public int Typeid { get; set; }
        [Display(Name = "Type")]
        public string CashBank { get; set; }

        [Display(Name = "Entry Mode")]
        public string Mode { get; set; }
        [Display(Name = "Payment Mode")]
        public String PaymentMode { get; set; }
        [Display(Name = "Cheque Number")]
        public string Cheque { get; set; }
        [Display(Name = "Refrence Number")]
        public string refernce { get; set; }
        [Display(Name = "Cash/Bank Ledger")]
        public string cashbankaccount { get; set; }
        [Display(Name = "Account")]
        public string Account { get; set; }
        [Display(Name = "Amount")]
        public string amount { get; set; }
        [Display(Name = "Narration")]
        public string Narration { get; set; }
        public string[] narration1 { get; set; }
        //public List<SelectListItem> narration { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TransactionId { get; set; }
        public int VoucherNo { get; set; }
        public int SrNo { get; set; }
        public int CreatedBy { get; set; }
        public int MemberID { get; set; }
        public int FinancialYearMemberID { get; set; }
        public string BrokerType { get; set; }
        public string Message { get; set; }
        public string AccountName { get; set; }
        public string CashBankName { get; set; }
        public List<RECEIPTPAYMENTENTRY> ReceiptList { get; set; }

    }
}
