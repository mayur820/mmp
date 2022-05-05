using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class OperatorMaster_Index_Models
    {
        public int ID { get; set; }
        public string OperatorName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int ManagerId { get; set; }
        public int Sucriberid { get; set; }
        public int Ismanager { get; set; }
        public string Createddate { get; set; }
        public int Createdby { get; set; }
        public string Password { get; set; }


    }
}