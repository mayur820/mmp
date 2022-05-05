using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class Family
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Family ID")]
        public int FamilyID { get; set; }
        [Display(Name = "Family Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please enter family name")]
        [Display(Name = "Family Name")]
        [Remote("IsUserExists", "Family", ErrorMessage = "Family Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "User ID")]
        public int UserId { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public List<Family> FamilyList { get; set; }

        }
    }
