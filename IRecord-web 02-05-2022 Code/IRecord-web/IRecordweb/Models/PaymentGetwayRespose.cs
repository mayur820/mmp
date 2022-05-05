using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class PaymentGetwayRespose
    {
        public class Rootobject
        {
            public bool accept_partial { get; set; }
            public int amount { get; set; }
            public int amount_paid { get; set; }
            public string callback_method { get; set; }
            public string callback_url { get; set; }
            public int cancelled_at { get; set; }
            public int created_at { get; set; }
            public string currency { get; set; }
            public Customer customer { get; set; }
            public string description { get; set; }
            public int expire_by { get; set; }
            public int expired_at { get; set; }
            public int first_min_partial_amount { get; set; }
            public string id { get; set; }
            public Notes notes { get; set; }
            public Notify notify { get; set; }
            public object payments { get; set; }
            public string reference_id { get; set; }
            public bool reminder_enable { get; set; }
            public object[] reminders { get; set; }
            public string short_url { get; set; }
            public string status { get; set; }
            public int updated_at { get; set; }
            public bool upi_link { get; set; }
            public string user_id { get; set; }
        }

        public class Customer
        {
            public string contact { get; set; }
            public string email { get; set; }
            public string name { get; set; }
        }

        public class Notes
        {
            public string policy_name { get; set; }
        }

        public class Notify
        {
            public bool email { get; set; }
            public bool sms { get; set; }
        }

    }
}