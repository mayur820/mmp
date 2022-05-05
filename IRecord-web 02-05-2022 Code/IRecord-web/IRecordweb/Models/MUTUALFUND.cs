using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class MUTUALFUND
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mutual Fund ID")]
        public int MutualFundID { get; set; }
        [Display(Name = "Mutual Fund Code")]
        public string MCode { get; set; }
        [Display(Name = "Fund Family")]
        public string FundFamilyName { get; set; }
        [Display(Name = "Name Of Scheme")]
        [Required(ErrorMessage = "Please Enter Mutual Fund Name")]
        // [Remote("IsUserExists", "MutualFundMaster", ErrorMessage = "Mutual Fund Name Already Exist")]
        public string NameOfScheme { get; set; }
        [Display(Name = "Scheme Code")]
        [Required(ErrorMessage = "Please Enter Scheme Code")]
        public string SchemeCode { get; set; }
        [Display(Name = "Investment Option")]
        public string InvestmentOption { get; set; }
        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; }
        [Display(Name = "Code")]
        public string Code { get; set; }
        public int FMutualFundID { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DeletedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        [NotMapped]
        public List<SelectListItem> items { get; set; }           /*To get selected dropdown value */

        }
    }