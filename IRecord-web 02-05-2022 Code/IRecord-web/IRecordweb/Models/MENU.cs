using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class MENU
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuID { get; set; }
        [Display(Name = "Menu Name")]
        public string Name { get; set; }
        [Display(Name = "Parent Menu ID")]
        public int? ParentMenuID { get; set; }
        [Display(Name = "Url")]
        public string url { get; set; }
        public int UserId { get; set; }
        public int UserMenuID { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }
        //End
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<MENU> MenuList { get; set; }

        }
    }