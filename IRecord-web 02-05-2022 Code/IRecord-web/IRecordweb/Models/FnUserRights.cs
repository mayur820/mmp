using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class FnUserRights
    {
        public void SetUserRight(string PageUrl)
        {
            try { 
            DataTable dt = (HttpContext.Current.Session["ViewMenuRightsForUser"] as DataTable);
            DataRow[] dr = (HttpContext.Current.Session["ViewMenuRightsForUser"] as DataTable).Select("url='" + PageUrl.Remove(0, 1) + "'");
          //  DataTable dt = (HttpContext.Current.Session["ViewMenuRightsForUser"] as DataTable).Select().CopyToDataTable();
            if (dr.Length != 0)
            {

              
                    HttpContext.Current.Session["Add_Rights"] = dr[0]["Add"].ToString();
                    HttpContext.Current.Session["Edit_Rights"] = dr[0]["Edit"].ToString();
                    HttpContext.Current.Session["Delete_Rights"] = dr[0]["Delete"].ToString();
                    HttpContext.Current.Session["View_Rights"] = dr[0]["View"].ToString();

            }
            else
            {//no Rights
                HttpContext.Current.Session["Add_Rights"] = "0";
                HttpContext.Current.Session["Edit_Rights"] = "0";
                HttpContext.Current.Session["Delete_Rights"] = "0";
                HttpContext.Current.Session["View_Rights"] = "0";
            }
            }
            catch {
                HttpContext.Current.Session["Add_Rights"] = "0";
                HttpContext.Current.Session["Edit_Rights"] = "0";
                HttpContext.Current.Session["Delete_Rights"] = "0";
                HttpContext.Current.Session["View_Rights"] = "0";

            }

        }
    }
}