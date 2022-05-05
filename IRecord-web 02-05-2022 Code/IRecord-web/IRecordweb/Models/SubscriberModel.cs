using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class SubscriberModel
        {
        public class Rootobject
            {
            public Datum[] Data { get; set; }
            }

        public class Datum
            {
            public int SubscriberID { get; set; }
            public string SubscriberName { get; set; }
            public string EmailID { get; set; }
            public string MobileNo { get; set; }
            public int UserID { get; set; }
            public string MobileOTP { get; set; }
            public string EmailOTP { get; set; }
            public int RoleId { get; set; }
            public int? MemberSubscription { get; set; }
            public bool Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public object ModifiedBy { get; set; }
            public object ModifiedDate { get; set; }
            }

        }
    }