using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRecordweb.Models
    {
   public class Menu
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
        //new Columns Added 
        public int UserId { get; set; }
        public int UserMenuID { get; set; }
       // [Display(Name = "Menu ID")] 
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
        public List<Menu> MenuList { get; set; }

       public Int64? RoleID { get; set; }

        //public List<Menu> MenuTree(List<Menu> menuList, int? ParentMenuID)
        //    {
        //    return menuList.Where(x => x.ParentMenuID == ParentMenuID).Select(
        //       x => new Menu
        //           {
        //           MenuID = x.MenuID,
        //           Name = x.Name,
        //           ParentMenuID = x.ParentMenuID,
        //           url = x.url,
        //           MenuList = MenuTree(menuList, x.MenuID)

        //           }).ToList();
        //    }

        }

    //public class MenuDisplay
    //    {
       
    //    public int MenuID { get; set; }
       
    //    public string Name { get; set; }
      
      
    //    public int UserId { get; set; }
    //    [Key]
    //    public int UserMenuID { get; set; }
    //    // [Display(Name = "Menu ID")] 
    //    public bool Add { get; set; }
    //    public bool Edit { get; set; }
    //    public bool Delete { get; set; }
    //    public bool View { get; set; }
    //    public List<MenuDisplay> MenuList1 { get; set; }
    //    //End
    //    }
    }
