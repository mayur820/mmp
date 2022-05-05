using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
    {
   public class ScriptInvestment
        {
        [Key]
        public int ID { get; set; }
        public int[] InvestmentId { get; set; }
        public int[] ScriptID { get; set; }
        public int MyProperty { get; set; }
        }
    }
