using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class UserModel
        {
        public class Rootobject
            {
            public Datum[] Data { get; set; }
            }

        public class Datum
            {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public DateTime LastLoginCMS { get; set; }
            public bool Active { get; set; }
            public DateTime PasswordChangeDateTime { get; set; }
            public int SubscriberID { get; set; }
            public object MemberSubscription { get; set; }
            public int MemberCount { get; set; }
            public int RoleId { get; set; }
            public string Name { get; set; }
            }

        }
    }