using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class BankStatement_Index_Model
        {
        public int ID { get; set; }
        public string Name { get; set; }
        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string ToDate { get; set; }
        [Display(Name = "Bank")]
        public string BankName { get; set; }
        [Display(Name = "Browse")]
        public HttpPostedFileBase FileUpload { get; set; }
        public string BankNameId { get; set; }
        [Display(Name = "Bank Config")]
        public string BankConfig { get; set; }
        public string BankConfigId { get; set; }
        public string Password { get; set; }
        public string FilesString { get; set; }
        //[Display(Name = "Browse")]
        public string FileName { get; set; }
        public string Fileextenion { get; set; }
        public string ContentType { get; set; }
        public int CreatedBy { get; set; }
        public int MemberID { get; set; }
        public int FinancialYearMemberID { get; set; }
        public List<BankStatement_Index_Model> BankFormatList { get; set; }

        //-------Datatable data -----//
        public DateTime Date { get; set; }
        public string Accounts { get; set; }
        public string Accounts_Color { get; set; }
        public string Cheque { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string Narration { get; set; }
        public string ACCode { get; set; }

        //------------End Here ----//
        }
    }