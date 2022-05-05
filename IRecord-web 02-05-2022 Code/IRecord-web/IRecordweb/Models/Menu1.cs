using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
    {
   public class Menu1
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Menu ID")]
        public int MenuID { get; set; }
        [Display(Name = "Menu Name")]
        public string Name { get; set; }
        [Display(Name = "Parent Menu ID")]
        public int ParentMenuID { get; set; }
        [Display(Name = "Url")]
        public string url { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }


        public List<Menu1> MenuList { get; set; }

        public List<Menu1> MenuTree(List<Menu1> menuList, int? ParentMenuID)
            {
            return menuList.Where(x => x.ParentMenuID == ParentMenuID).Select(
               x => new Menu1
                   {
                   MenuID = x.MenuID,
                   Name = x.Name,
                   ParentMenuID = x.ParentMenuID,
                   url = x.url,
                   MenuList = MenuTree(menuList, x.MenuID)

                   }).ToList();
            }

        }
    }
