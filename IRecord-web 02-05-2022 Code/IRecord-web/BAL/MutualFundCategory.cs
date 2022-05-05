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
    public class MutualFundCategory
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mutual Fund Category ID")]
        public int MutualFundCategoryID { get; set; }
        [Required(ErrorMessage = "Please Enter Mutual Fund Category Name")]
        [Display(Name = "Mutual Fund Category Name")]
        [Remote("IsUserExists", "MutualFundCategory", ErrorMessage = "Fund Category Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Mutual Fund Category Code")]
        public string Code { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        }
    }
