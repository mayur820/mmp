using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class OpeningStock
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockID { get; set; }
        [Display(Name = "Demat A/C")]
        public string DematAc { get; set; }
        [Display(Name = "Consultant")]
        public string Consultant { get; set; }
        [Display(Name = "Broker")]
        public string Broker { get; set; }
        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; }
        [Display(Name = "Holding Type")]
        public string HoldingType { get; set; }
        [Display(Name = "Select to Import")]
        public HttpPostedFileBase FileUpload { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        }
    }