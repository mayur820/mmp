using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class SECTOR
    {
        [Key]
        public int SectorID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int DeletedBy { get; set; }
    }
}