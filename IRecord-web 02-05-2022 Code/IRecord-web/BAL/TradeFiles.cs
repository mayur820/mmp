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
    public class TradeFiles
        {
        public int ID { get; set; }
        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; }
        public string Broker { get; set; }
        [NotMapped]
        public HttpPostedFileBase FilePath { get; set; }
        public string Password { get; set; }
        public string Consultant { get; set; }
        public string DematAC { get; set; }
        public string HoldingType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }
        public string FormatToImport { get; set; }
        }
    }
