using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Company Id")]
        public int CompanyId { get; set; }

        //[Required(ErrorMessage = "Please Enter Country Id")]
        [Display(Name = "Country Id")]
        public int CountryId { get; set; }

        //[Required(ErrorMessage = "Please Enter Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please Enter Company Name")]
        [Display(Name = "Company Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please Enter Pincode")]
        public string Pincode { get; set; }
        [Required(ErrorMessage = "Please Enter Subscription Limits")]
        [Display(Name = "Subscription Limits")]
        public int SubscriptionLimits { get; set; }
        [Required(ErrorMessage = "Please Select Start Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Please Select End Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Please Check")]
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        }
    
}
