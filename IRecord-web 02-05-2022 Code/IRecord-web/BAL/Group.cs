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
    public class Group
    {
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
        public int DeletedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int UGroupID { get; set; }
        [Required(ErrorMessage = "Please Select Under Group Name")]
        [Display(Name = "Under Group")]
        public string UGroupName { get; set; }
        public List<Group> ShowGroup { get; set; }
    }
}
