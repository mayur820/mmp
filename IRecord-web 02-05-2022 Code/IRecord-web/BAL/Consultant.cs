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
    public class Consultant
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Consultant ID")]
        public int ConsultantID { get; set; }
        [Required(ErrorMessage = "Please Select Member ID")]
        [Display(Name = "Member ID")]
        public int MemberID { get; set; }
        [Display(Name = "Consultant Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        [Display(Name = "Consultant Name")]
        [Remote("IsUserExistsConsultant", "ConsultantMaster", ErrorMessage = "Consultant Name Already Exist")]
        public string Name { get; set; }
       // [Required(ErrorMessage = "Please Enter PAN No")]
        public string PAN { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile No")]
        [Display(Name = "Mobile No")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please Enter EmailID")]
        [Display(Name = "Email ID")]
        [RegularExpression(@"[a - z0 - 9._ % +-] +@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public List<Consultant> ConsultantList { get; set; }
       

        }
    }
