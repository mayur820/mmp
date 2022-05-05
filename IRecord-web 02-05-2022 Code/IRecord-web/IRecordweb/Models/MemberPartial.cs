using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IRecordweb.Models
    {
    [MetadataType(typeof(UserMetaData))]
    public class MemberPartial
        {
        }
    //New line 
    class UserMetaData
        {
        [Remote("IsUserExists", "MemberMaster", ErrorMessage = "User Name already in use")]
        public string Name { get; set; }
        }
    }