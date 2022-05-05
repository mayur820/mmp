using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BAL
    {
   public class UploadBhavCopy
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DailyRateID { get; set; }
        [Display(Name ="Date")]
        public DateTime TransDate { get; set; }
        public string ScriptCode { get; set; }
        public DateTime ExpDate { get; set; }
        [Display(Name = "Exchange")]
        public string Exchange  { get; set; }
        [Display(Name = "Manual Upload")]
        public HttpPostedFileBase UploadFile { get; set; }
        public  float StrikePrice { get; set; }
        public  string OptionType { get; set; }
        public float BSERate { get; set; }
        public float  NSERate { get; set; }
        public float PrevBSE { get; set; }
        public float PrevNSE { get; set; }
        public float NSEFORate { get; set; }
        [Display(Name = "Investment Type")]
        public int InvestmentType { get; set; }
        public float BSEHighRate { get; set; }
        public string ScriptName { get; set; }
        public string InstrumentName { get; set; }
        public string date { get; set; }

        }

    public class UploadBhavCopyMCX
        {
        public string InstrumentName { get; set; }
        public string date { get; set; }
        }
    }
