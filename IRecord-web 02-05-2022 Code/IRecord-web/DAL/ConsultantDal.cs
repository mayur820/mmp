using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using BAL;
using System.Data;
using System.Data.SqlClient;
using IrecordDAL;
using System.Net.Http;
using System.Web.SessionState;
using System.Web.Security;
using System.Net;
using System.IO;
using LinqToExcel;
using System.Data.OleDb;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Data.Common;
using IRecordweb.Models;
namespace DAL
{
 public class ConsultantDal
    {
        public List<Consultant> GetAllConsultantList()
        {
            List<Consultant> GetAllConsultantList = new List<Consultant>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllConsulatantList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Consultant cobj = new Consultant();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                GetAllConsultantList.Add(new Consultant()
                {
                    ConsultantID = Convert.ToInt32(ds.Tables[0].Rows[i]["ConsultantID"].ToString()),
                    Name = ds.Tables[0].Rows[i]["Name"].ToString(),
                    MobileNo = ds.Tables[0].Rows[i]["MobileNo"].ToString(),
                    Email = ds.Tables[0].Rows[i]["Email"].ToString(),
                    Code = ds.Tables[0].Rows[i]["Code"].ToString(),
                    Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString()),
                    CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString()),
                });
            }
            return GetAllConsultantList;
        }
        public void InsertConsultantMaster(Consultant _Consultant,int Flag)
        {
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.AddParam(pcol, "@Consultantid", _Consultant.ConsultantID);
            Adapter.AddParam(pcol, "@Name", _Consultant.Name);
            Adapter.AddParam(pcol, "@MobileNo", _Consultant.MobileNo);
            Adapter.AddParam(pcol, "@Email", _Consultant.Email);
            Adapter.AddParam(pcol, "@Active", _Consultant.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.AddParam(pcol, "@Flag", Flag);
            Adapter.ExecutenNonQuery("USPConsultantInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

        }
    }
}
