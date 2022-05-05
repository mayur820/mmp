using BAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class IType
        {
        [Key]
        public int TypeId { get; set; }
        public int[] TypeIDs { get; set; }
        public List<SelectListItem> Scripts  { get; set; }
        [Display(Name = "Parent Type ID")]
        public int ParentTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SequenceNo { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<MType> MTypeList { get; set; }

        }
    }