using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class FUNDFAMILY
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mutual Fund ID")]
        public int MutualFundID { get; set; }
        [Required(ErrorMessage = "Please Enter Fund Family Name")]
        [Display(Name = "Fund Family Name")]
       // [Remote("IsUserExists", "FundFamilyMaster", ErrorMessage = "Fund Family Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Fund Family Code")]
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