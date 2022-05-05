using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class BankStatementDataTrans
        {
        public class Rootobject
            {
            public string Bankcode { get; set; }
            public string fileName { get; set; }
            public string contentType { get; set; }
            public string fileextension { get; set; }
            public string pdfbytes { get; set; }
            [Display(Name = "From Date")]
            public string StartDate { get; set; }
            [Display(Name = "To Date")]
            public string EndDate { get; set; }
            }
        }
    }