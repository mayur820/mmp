using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace BAL
    {
   public class ImportExcel
        {
        [Required(ErrorMessage = "Please select file")]
        [FileExtensions(Extensions = ".xls,.xlsx", ErrorMessage = "Only excel file")]
        public HttpPostedFileBase FilePath { get; set; }
        }
    }
