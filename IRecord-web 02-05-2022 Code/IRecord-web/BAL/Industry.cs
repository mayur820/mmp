using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BAL
    {
    public class Industry
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Industry ID")]
        public int IndustryID { get; set; }
       
        [Required(ErrorMessage = "Please Enter Name")]
        [Display(Name = "Industry Name")]
        [Remote("IsUserExists", "IndustryMaster", ErrorMessage = "Industry Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Industry Code")]
        public string Code { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        }
    }
