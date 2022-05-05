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
    public class Subscriber
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Subscriber Id")]
        [NotMapped]
        public int SubscriberID { get; set; }
       // [Required(ErrorMessage = "Please Enter Subscriber Name")]
       // [Remote("IsUserExists", "SubscriberMaster", ErrorMessage = "Subscriber Name Already Exist")]
        [Display(Name = "Subscriber Name")]
        public string SubscriberName { get; set; }
        [Remote("IsEmailExists", "SubscriberMaster", ErrorMessage = "EmailID Already Exist")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [Display(Name = "Email ID")]
       //  [Required(ErrorMessage = "Please Enter EmailID")]
        public string EmailID { get; set; }
        [Remote("IsMobileExists", "SubscriberMaster", ErrorMessage = "Mobile No Already Exist")]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Mobile Number should be 10 digits")]
       //  [Required(ErrorMessage = "Please Enter Mobile No")]
        public string MobileNo { get; set; }
       // [Required(ErrorMessage = "Please Enter Mobile OTP")]
        public int OTPID { get; set; }
        [Display(Name = "Mobile OTP")]
        [MaxLength(6)]
        public string MobileOTP { get; set; }
       // [Required(ErrorMessage = "Please Enter Email OTP")]
        [Display(Name = "Email OTP")]
        [MaxLength(6)]
        public string EmailOTP { get; set; }
        [Display(Name = " ")]
      //  [Required(ErrorMessage = "Please Enter Role")]
        public int RoleId { get; set; }
      //  [Required(ErrorMessage = "Please Enter Subscription Limits")]
        [Display(Name = "Select No. Of InvestingMembers In Your Family:")]
        public int MemberSubscription { get; set; }

        [Display(Name = "Create Password")]
        [DataType(DataType.Password)]
        public string CreatePassword { get; set; }

        [Display(Name = "Confirm Passsword")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("CreatePassword", ErrorMessage = "The password and confirmation password does not match.")]
        public string ConfirmPasssword { get; set; }
        public DateTime OTPVerifiedDate { get; set; }
        //[DataType(DataType.Date)]
        // [Required(ErrorMessage = "Please Check")]
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Code { get; set; }
        public int Case { get; set; }
        public string Message { get; set; }
       
        }
    
}
