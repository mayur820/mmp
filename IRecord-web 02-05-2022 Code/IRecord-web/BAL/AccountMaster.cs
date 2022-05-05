using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BAL
{
    public class AccountMaster

    {
        [Key]
        public int Code { get; set; }
        [Required(ErrorMessage = "Please Enter Account Name")]
        [Display(Name = "Account Name")]
        [Remote("IsUserExistsAccount", "AccountMaster", ErrorMessage = "Account Name Already Exist")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int MemberID { get; set; }
        public int FinancialYearMemberID { get; set; }
        [Required(ErrorMessage = "Please Select Group Name")]
        [Display(Name = "Group Name")]
        public int GroupID { get; set; }
        [Required(ErrorMessage = "Please Enter Opening Balance")]
        [Display(Name = "Opening Balance")]
        public double OpeningBalance { get; set; }
        [Display(Name = "Debit/Credit")]
        public string OpeningCal { get; set; }
        [Display(Name = "Contact Person")]
        public string Contactperson { get; set; }
        // [Display(Name = "Broker code")]
        // public int Brokercode { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Email Id")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string Emailid { get; set; }
        //[Display(Name = "Email 2")]
        //public string Email2 { get; set; }
        [Display(Name = "Mobile NO")]
        public string Mobile { get; set; }
        // [Display(Name = "Mobile 2")]
        //public string Mobile2 { get; set; }
        [Display(Name = "Telephone No")]
        public string Telephone { get; set; }
        [Display(Name = "Aadhar No")]
        public string AadharCardNo { get; set; }
        [Display(Name = "GSTIN")]
        [RegularExpression("^([0-9]){2}([A-Z]){5}([0-9]){4}([A-Z]){1}([0-9]){1}([A-Z]){1}([0-9]){1}$", ErrorMessage = "Invalid GST Number")]
        public string GSTIN { get; set; }
        [Required(ErrorMessage = "Please Enter Pan No")]
        [RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessage = "Invalid PAN Number")]
        [Display(Name = "Pan No")]
        public string PAN { get; set; }
        public string GroupName { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<AccountMaster> ShowAccount { get; set; }
        public int Accountid { get; set; }
        public int InternalEntry { get; set; }
    }
}
