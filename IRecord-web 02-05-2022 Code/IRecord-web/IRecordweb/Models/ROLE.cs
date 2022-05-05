using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class ROLE
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Role ID")]
        public int RoleId { get; set; }
        [Display(Name = "Parent Role Id")]
        public int ParentRoleId { get; set; }
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public List<ROLE> RoleMasterList { get; set; }

        }
    }