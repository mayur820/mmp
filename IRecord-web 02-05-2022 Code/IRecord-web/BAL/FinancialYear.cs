using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
    {
    public class FinancialYear
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Financial Year ID")]
        public int FinancialYearID { get; set; }
        [Display(Name = "Financial Year Code")]
        public string Code { get; set; }
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]     
        public DateTime ToDate { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<FinancialYear> ShowFinYear { get; set; }

        }
    }
