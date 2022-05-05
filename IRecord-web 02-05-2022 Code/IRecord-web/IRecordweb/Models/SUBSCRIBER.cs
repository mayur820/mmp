using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class SUBSCRIBER
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
       // [Remote("IsEmailExists", "SubscriberMaster", ErrorMessage = "EmailID Already Exist")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [Display(Name = "Email ID")]
        public string EmailID { get; set; }
      //  [Remote("IsMobileExists", "SubscriberMaster", ErrorMessage = "Mobile No Already Exist")]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Mobile Number should be 10 digits")]
        public string MobileNo { get; set; }
        public int MemberCount { get; set; }
        public int OTPID { get; set; }
        public int UserID { get; set; }
        [Display(Name = "Mobile OTP")]
        [MaxLength(6)]
        public string MobileOTP { get; set; }
        [Display(Name = "Email OTP")]
        [MaxLength(6)]
        public string EmailOTP { get; set; }
        [Display(Name = " ")]
        public int RoleId { get; set; }
        [Range(0, 5, ErrorMessage = "5 License Free On User")]
        [Display(Name = "Select No. Of InvestingMembers In Your Family:")]
        public int MemberSubscription { get; set; }
        [Display(Name = "Create Password")]
       // [StringLength(255, ErrorMessage = "Password should be 8 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
       // [PasswordPropertyText]
        [MinLength(8)]
        [DisplayName("New Password")]
       // [RegularExpression("^(.{0,7}|[^0-9]*|[^A-Z]*|[^a-z]*|[a-zA-Z0-9]*)$", ErrorMessage = "Password should be 8 characters")]
        public string CreatePassword { get; set; }
        [Display(Name = "Confirm Passsword")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("CreatePassword", ErrorMessage = "The password and confirmation password does not match.")]
        public string ConfirmPasssword { get; set; }
        public DateTime OTPVerifiedDate { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Code { get; set; }
        public int Case { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }

        }
    }