using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class MainMemberMasterModel
    {
        public int MemberID { get; set; }
        public int FamilyID { get; set; }
        public string MemberName { get; set; }
        public string Code { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string Address_3 { get; set; }
        public int Gender { get; set; }
        public string ServTax_No { get; set; }
        public string AadharCardNo { get; set; }
        public string ReportLogoPath { get; set; }
        public int UserId { get; set; }
        public string PAN { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public object ModifiedBy { get; set; }
        public object ModifiedDate { get; set; }
        public string SubscriberID { get; set; }
        public string OperatorID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Client_Code { get; set; }
        
    }
}