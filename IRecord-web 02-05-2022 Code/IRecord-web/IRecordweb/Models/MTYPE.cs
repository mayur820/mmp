﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class MTYPE
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Type ID")]
        public int TypeId { get; set; }
        public int[] TypeIDs { get; set; }
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
        //  public List<SelectListItem> MyProperty { get; set; }
        public List<MTYPE> MTypeList { get; set; }
        [NotMapped]
        public SelectList SectorID { get; set; }

        }
    }