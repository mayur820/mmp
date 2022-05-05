using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace IRecordweb.Models
    {
    public class TradeFilesTrans
        {
        public class Rootobject
            {
          
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Filecode { get; set; }
            public string fileName { get; set; }
            public string ContentType { get; set; }
            public string fileextension { get; set; }
            public string pdfbytes { get; set; }
            public string isTradeFile { get; set; }
            public string Consultant { get; set; }
            public string ConsultantName { get; set; }
            public string Broker { get; set; }
            public string ContractNoteName { get; set; }
            public string Demat_Ac_Id { get; set; }
            public string Demat_Ac_Name { get; set; }
            public string HoldingType { get; set; }
            public string contentType { get; set; }
            public string BrokerName { get; set; }
        }


        }
    }