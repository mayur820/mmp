using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class ImportTradefileContract_INDEX_Models
        {
        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string ToDate { get; set; }
        [Display(Name = "Contract Note")]
        public string ContractNoteId { get; set; }
        public string ContractNoteName { get; set; }
        [Display(Name = "Demat A/C")]
        public string Demat_Ac_Id { get; set; }
        public string Demat_Ac_Name { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string FilesString { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string isTradeFile { get; set; }
        public string Fileextenion { get; set; }
        public string ContentType { get; set; }
        public HttpPostedFileBase FileUpload { get; set; }
        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; }
        public string Consultant { get; set; }
        public string ConsultantName { get; set; }
        public string Broker { get; set; }
        public int BrokerId { get; set; }
        public string HoldingType { get; set; }
        public int MemberID { get; set; }
        public int FinancialYearID { get; set; }
        public int CreatedBy { get; set; }
        public string Script { get; set; }

        //Datatble fields //
        public int ScriptID { get; set; }

        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string BillNo { get; set; }
        public DateTime TransDate { get; set; }
        public string TransType { get; set; }
        public string AccountType { get; set; }
        public int Quantity { get; set; }
        public double GrossRate { get; set; }
        public double BrokerageAmt { get; set; }
        public double  NetAmount { get; set; }
        public double NetRate { get;  set; }
        public string  isIntraDay { get; set; }
        public double BrokerageperUnit { get; set; }
        public string BookType { get; set; }

        //End //
        }
    }