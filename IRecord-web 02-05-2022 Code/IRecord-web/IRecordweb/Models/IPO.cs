using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class IPO
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Consultant")]
        public string Consultant { get; set; }
        [Display(Name = "Demat A/C")]
        public string Demat { get; set; }
        [Display(Name = "Broker")]
        public string Broker { get; set; }
        public string ScriptID { get; set; }
        public float TAmount { get; set; }
        public int TransactionId { get; set; }

        public string ApplicationNo { get; set; }
        public DateTime TransDate { get; set; }
        public string MemberId { get; set; }
        public string FinancialYrId { get; set; }
        public string BrokerId { get; set; }
        //  public string ScriptId { get; set; }
        public int Qty { get; set; }
        public float Rate { get; set; }
        public float Amount { get; set; }
        public DateTime AllotedDate { get; set; }
        public int AllotedQty { get; set; }
        public float AllotedRate { get; set; }
        public float AllotedAmt { get; set; }
        public string Bank { get; set; }
        public string ConsultantId { get; set; }
        public string DematId { get; set; }
        public int BalQty { get; set; }
        public string BookType { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; }
        public List<IPO> Showipo { get; set; }

    }
}