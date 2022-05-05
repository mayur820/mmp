using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class GROUP
        {
        public int MemberID { get; set; }
        public int FinancialYearMemberID { get; set; }
        [Key]
        [Display(Name = "Group ID")]
        public int Group_ID { get; set; }
        public string Group_Code { get; set; }
        [Required(ErrorMessage = "Please Enter Group Name")]
        [Display(Name = "Group Name")]
        [Remote("IsUserExistsGroup", "GroupMaster", ErrorMessage = "Group Name Already Exist")]
        public string Group_Name { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int UGroupID { get; set; }
        [Required(ErrorMessage = "Please Select Under Group Name")]
        [Display(Name = "Under Group")]
        public string UGroupName { get; set; }
        public List<GROUP> ShowGroup { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        [NotMapped]
        public List<SelectListItem> items { get; set; }

        }
    }