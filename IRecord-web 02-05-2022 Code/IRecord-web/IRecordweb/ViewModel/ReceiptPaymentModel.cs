using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.ViewModel
{
    public class ReceiptPaymentModel
    {
        public string EntryID { get; set; }
        public DateTime Date { get; set; }
        public int Typeid { get; set; }
        public string CashBank { get; set; }

        public string Mode { get; set; }
        public String PaymentMode { get; set; }
        public string Cheque { get; set; }
        public string refernce { get; set; }
        public string cashbankaccount { get; set; }
        public string Account { get; set; }
        public string amount { get; set; }
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

    }
}