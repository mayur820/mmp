using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class mobileotpModel
    {

        public class Rootobject
        {
            public Listsms listsms { get; set; }
            public string password { get; set; }
            public string user { get; set; }
        }

        public class Listsms
        {
            public string sms { get; set; }
            public string mobiles { get; set; }
            public string senderid { get; set; }
            public string clientSMSID { get; set; }
            public string accountusagetypeid { get; set; }
            public string entityid { get; set; }
            public string tempid { get; set; }
        }

    }

}