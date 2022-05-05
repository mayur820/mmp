using BAL;
using IRecordweb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BAL                                 /* IRecordweb.Models*/
    {
    public class ScriptUploadDownload
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScriptID { get; set; }
        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; }
        public string Exchange { get; set; }
        // [Required]
        [NotMapped]
        public HttpPostedFileBase FilePath { get; set; }
        //Extra Column
        [Required(ErrorMessage = "Please Enter Script Name")]
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
        [Required(ErrorMessage = "Please Enter Group Name")]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        public bool IsMcx { get; set; }
        public bool IsCurrency { get; set; }
        public bool IsNcdx { get; set; }
        public bool IsFO { get; set; }
        public string FaceValue { get; set; }
        public string ISIN { get; set; }
        //Extra Column End Here
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SecurityCode { get; set; }

        public List<Script> scripdata { get; set; }
        public List<ScriptUploadDownload> scripdatalist { get; set; }
        }
    }
