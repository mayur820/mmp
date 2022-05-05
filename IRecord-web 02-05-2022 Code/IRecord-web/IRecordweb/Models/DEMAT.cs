using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    public class DEMAT
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Demat ID")]
        public int DematID { get; set; }
        public int DeletedBy { get; set; }
        public int MemberId { get; set; }
        public int FinancialYearMemberID { get; set; }
        //[Required(ErrorMessage = "Please Enter Demat Name")]
        //[Display(Name = "Demat A/C Name")]
        //  [Remote("IsUserExistsDemat", "DematMaster", ErrorMessage = "Demat Name Already Exist")]
        public string Name { get; set; }
        [Display(Name = "Demat Code")]
        public List<int> SelectedMultiDemate { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<DEMAT> ShowDemat { get; set; }

        }
    }