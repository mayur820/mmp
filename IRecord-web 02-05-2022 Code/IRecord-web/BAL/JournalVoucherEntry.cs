using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BAL
{
    public class JournalVoucherEntry
    {
        [Key]
        public int TransactionId { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Debit Group")]
        public string Debit { get; set; }
        [Display(Name = "Credit Group")]
        public string Credit { get; set; }
        [Display(Name = "Amount")]
        public double Amount { get; set; }
        [Display(Name = "Narration")]
        public string Narration { get; set; }
        public int Accouncode { get; set; }
        public string AccountName { get; set; }
        public int Undercode { get; set; }
        public string GroupName { get; set; }
        [Display(Name = "Debit Account")]
        public string DebitAccount { get; set; }
        [Display(Name = "Credit Account")]
        public string CreditAccount { get; set; }
        public int ChequeNo { get; set; }
       
    }
}
