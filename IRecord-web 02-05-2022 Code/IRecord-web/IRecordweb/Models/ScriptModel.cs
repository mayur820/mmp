using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class ScriptModel
        {
        public class Rootobject
            {
            public Datum[] Data { get; set; }
            }

        public class Datum
            {
            
                 public int SectorID { get; set; }
            public int ScriptID { get; set; }
            public string ScriptName { get; set; }
            public string BSECode { get; set; }
            public string NSECode { get; set; }
            public object IndustryID { get; set; }
            public object GroupName { get; set; }
            public string InvestmentType { get; set; }
            public object IsMcx { get; set; }
            public object IsCurrency { get; set; }
            public object IsNcdx { get; set; }
            public object IsFO { get; set; }
            public object FaceValue { get; set; }
            public object ISIN { get; set; }
            public object SectorName { get; set; }
            public object ListType { get; set; }
            public object FundType { get; set; }
            public int MutualFundID { get; set; }
            public bool Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public object ModifiedBy { get; set; }
            public object ModifiedDate { get; set; }
            }

        }
    }