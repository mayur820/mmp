using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class AccountModel
        {
        public class Rootobject
            {
            public Datum[] Data { get; set; }
            }

        public class Datum
            {
            public string AccountName { get; set; }
            public int AccountId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public object Description { get; set; }
            public int MemberID { get; set; }
            public int FinancialYearMemberID { get; set; }
            public int GroupID { get; set; }
            public int UGroupID { get; set; }
            public string UGroupName { get; set; }
            public string GroupName { get; set; }
            public float OpeningBalance { get; set; }
            public float OpeningCal { get; set; }
            public object Contactperson { get; set; }
            public object Address { get; set; }
            public object Mobile { get; set; }
            public object Telephone { get; set; }
            public object Emailid { get; set; }
            public string PAN { get; set; }
            public object GSTIN { get; set; }
            public object AadharCardNo { get; set; }
            public bool Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public object ModifiedBy { get; set; }
            public object ModifiedDate { get; set; }
            }

        }
    }