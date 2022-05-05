using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class SCRIPTDATA
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Script ID")]
        public int ScriptID { get; set; }
        [Required(ErrorMessage = "Please Enter Script Name")]
        [Display(Name = "Trans Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Script Name")]
        public string ScriptName { get; set; }
        [Required(ErrorMessage = "Please Enter BSE Code")]
        [Display(Name = "BSE Code")]
        public string BSECode { get; set; }
        [Required(ErrorMessage = "Please Enter NSE Code")]
        [Display(Name = "NSE Code")]
        public string NSECode { get; set; }
        [Display(Name = "Industry ID")]
        public int IndustryID { get; set; }      
        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; }
        [Display(Name = "Exchange")]
        public string Exchange { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        }
    }