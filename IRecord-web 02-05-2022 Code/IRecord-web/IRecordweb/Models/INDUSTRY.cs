using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class INDUSTRY
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Industry ID")]
        public int IndustryID { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [Display(Name = "Industry Name")]
      //  [Remote("IsUserExists", "IndustryMaster", ErrorMessage = "Industry Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Industry Code")]
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