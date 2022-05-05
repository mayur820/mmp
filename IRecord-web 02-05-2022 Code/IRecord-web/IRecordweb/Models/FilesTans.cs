using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class FilesTans
        {
        public string pdfcode { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }
        public string fileextension { get; set; }
        public string pdfbytes { get; set; }
        ////
        public string member_code { get; set; }
        public string year_code { get; set; }
        //     public string FormatNo { get; set; }
        public string Brokercode { get; set; }
        public string invstyp { get; set; }
        public string invstyptext { get; set; }
        public string ConsultantCode { get; set; }
        public string Consultant { get; set; }
        public string HoldingTypeCode { get; set; }
        public string HoldingType { get; set; }
        public string Password { get; set; }
        }
    }