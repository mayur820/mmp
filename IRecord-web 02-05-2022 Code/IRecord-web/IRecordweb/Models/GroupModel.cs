using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class GroupModel
        {
        public class Rootobject
            {
            public Datum[] Data { get; set; }
            }

        public class Datum
            {
            public int GroupID { get; set; }
            public string Group_Code { get; set; }
            public string Group_Name { get; set; }
            public int UGroupID { get; set; }
            public bool Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public object ModifiedBy { get; set; }
            public object ModifiedDate { get; set; }
            public string UGroupName { get; set; }
            public List<GroupModel> GroupList { get; set; }
            }
        }
    }