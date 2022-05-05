using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class USER
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please Enter User Name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsMobile { get; set; }
        [Required(ErrorMessage = "Please Select Date")]
        [Display(Name = "Last Login CMS")]
        public DateTime LastLoginCMS { get; set; }
        [Required(ErrorMessage = "Please Select Date")]
        [Display(Name = "Last Login Mobile")]
        public DateTime LastLoginMobile { get; set; }
        [Display(Name = "Password Change DateTime")]
        public DateTime PasswordChangeDateTime { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Subscriber ID")]
        public int SubscriberID { get; set; }
        [Display(Name = "Role Id")]
        public int RoleId { get; set; }
        [Display(Name = "PAN No.")]
        [RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessage = "Invalid PAN Number")]
        public string PAN { get; set; }
        public int SubscriptionLimits { get; set; }
        public int MemberCount { get; set; }
        public List<ROLE> RoleMasterList { get; set; }
        public bool SuperAdmin { get; set; }
        public string NormalUser { get; set; }
    }
    }