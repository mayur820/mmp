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
    public class Narration
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Narration ID")]
        public int NarrationID { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        [Display(Name = "Narration Name")]
        [Remote("IsUserExists", "NarrationMaster", ErrorMessage = "Narration Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Narration Code")]
        public string Code { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int DeletedBy { get; set; }
    }
    }
