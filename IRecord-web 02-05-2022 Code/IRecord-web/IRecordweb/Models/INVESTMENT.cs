using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class INVESTMENT
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Investment ID")]
        public int InvestmentID { get; set; }
        [Display(Name = "Financial Year Member ID")]
        public int FinancialYearMemberID { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        [Display(Name = "Investment Name")]
       // [Remote("IsUserExists", "InvestmentMaster", ErrorMessage = "Investment Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Investment Code")]
        public string Code { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int DeletedBy { get; set; }
    }
    }