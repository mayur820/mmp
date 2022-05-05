using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class UserModels
        {
        public class Rootobject
            {
            public Datum[] Data { get; set; }
            }

        public class Datum
            {
            public int ID { get; set; }
            public int CITYID { get; set; }
            public int STATEID { get; set; }
            public string NAME { get; set; }
            public string ADDRESS { get; set; }
            }

        }
    }