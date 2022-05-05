using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
{
    public class MEMBER
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Member ID")]
        public int MemberID { get; set; }
        [Display(Name = "Family Name")]
        public int FamilyID { get; set; }
        [Required(ErrorMessage = "Please Enter Member Name")]
        [Display(Name = "Member Name")]
        //  [Remote("IsUserExists", "MemberMaster", ErrorMessage = "Member Name Already Exist")]
        public string MemberName { get; set; }
        // [Required(ErrorMessage = "Please Enter Code")]
        [Display(Name = "Member Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please Enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please select gender")]
        public int Gender { get; set; }
        [Required(ErrorMessage = "Please Enter GST No")]
        [RegularExpression("([0-9]){2}([A-Z]){5}([0-9]){4}([A-Z]){1}([0-9]){1}([A-Z]){1}([0-9]){1}$", ErrorMessage = "Invalid GST Number")]
        [Display(Name = "GST No")]
        public string ServTax_No { get; set; }
        [Required(ErrorMessage = "Please Enter Aadhar Card No")]
        [Display(Name = "Aadhar Card No")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Invalid Aadhar Card Number")]
        public string AadharCardNo { get; set; }
        [Display(Name = "Upload Logo")]
        public HttpPostedFileBase ReportLogoPath { get; set; }
        public string ReportLogoPathData { get; set; }
        public string EmailOTP { get; set; }
        public string Status { get; set; }
        [Display(Name = "User Id")]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int SubscriberID { get; set; }
        [Required(ErrorMessage = "Please Enter PAN Card No")]
        [RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessage = "Invalid PAN Number")]
        public string PAN { get; set; }
        public int SubscriptionLimits { get; set; }
        public int MemberCount { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public string Client_Code { get; set; }
        
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FinancialYear { get; set; }
        public int FinancialYearID { get; set; }
        //public bool Save { get; set; }
        //public bool Search { get; set; }
        //public bool Delete { get; set; }
        public List<MEMBER> ShowMember { get; set; }
        public List<SelectListItem> items { get; set; }
    }
}
