using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class FUNDCATEGORY
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mutual Fund Category ID")]
        public int MutualFundCategoryID { get; set; }
        [Required(ErrorMessage = "Please Enter Mutual Fund Category Name")]
        [Display(Name = "Mutual Fund Category Name")]
      //  [Remote("IsUserExists", "MutualFundCategory", ErrorMessage = "Fund Category Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Mutual Fund Category Code")]
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