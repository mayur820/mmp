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
    public class Demat
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Demat ID")]
        public int DematID { get; set; }
        public int MemberId { get; set; }
        public int FinancialYearMemberID { get; set; }
        [Required(ErrorMessage = "Please Enter Demat Name")]
        [Display(Name ="Demat A/C Name")]
        [Remote("IsUserExistsDemat", "DematMaster", ErrorMessage = "Demat Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Demat Code")]
        public string Code { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<Demat> ShowDemat { get; set; }
    }
    }
