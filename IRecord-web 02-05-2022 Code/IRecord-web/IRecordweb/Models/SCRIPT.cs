using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class SCRIPT
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Script ID")]
        public Int64 ScriptID { get; set; }
        public Int64 MemberID { get; set; }
        public int DeletedBy { get; set; }
        [Required(ErrorMessage = "Please Enter Script Name")]
        [Display(Name = "Script Name")]
       // [Remote("IsUserExists", "ScriptMaster", ErrorMessage = "Script Name Already Exist")]
        public string ScriptName { get; set; }
        [Required(ErrorMessage = "Please Enter BSE Code")]
        [Display(Name = "BSE Code")]
        public string BSECode { get; set; }
        [Required(ErrorMessage = "Please Enter NSE Code")]
        [Display(Name = "NSE Code")]
        public string NSECode { get; set; }
        [Display(Name = "Industry ID")]
        public int IndustryID { get; set; }
        [Required(ErrorMessage = "Please Enter Group Name")]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        //[Required(ErrorMessage = "Please Select Investment Type")]
        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; }
        // public List<SelectListItem> data { get; set; }
        public bool IsMcx { get; set; }
        public bool IsCurrency { get; set; }
        public bool IsNcdx { get; set; }
        public bool IsFO { get; set; }
        public string FaceValue { get; set; }
        public string ISIN { get; set; }
        [Display(Name = "Mutual Fund ID")]
        public int MutualFundID { get; set; }
        public string MFName { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int TypeId { get; set; }
        [Display(Name = "Type Name")]
        public string Name { get; set; }
        [Display(Name = "Sector Name")]
        public int SectorID { get; set; }
        public string SectorName { get; set; }        // Change datatype  string to
        [NotMapped]
        public SelectList SectorID1 { get; set; }
        [Display(Name = "List Type")]
        public string ListType { get; set; }
        [NotMapped]
        public string[] scriptlist { get; set; }
        public List<Script> scripdata { get; set; }
        public List<SelectListItem> items { get; set; }
        public List<SelectListItem> Values { get; set; }
        }
    }