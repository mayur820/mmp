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
   public class DematDal
    {
        public List<Demat> GetAllDematList()
        {
            List<Demat> GetAllDematList = new List<Demat>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllDematList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Demat cobj = new Demat();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                GetAllDematList.Add(new Demat()
                {
                    DematID = Convert.ToInt32(ds.Tables[0].Rows[i]["DematID"].ToString()),
                    Name = ds.Tables[0].Rows[i]["Name"].ToString(),
                    Code = ds.Tables[0].Rows[i]["Code"].ToString(),
                    Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString()),
                    CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString()),
                });
            }
            return GetAllDematList;
        }
        public void InsertDematMaster(Demat _Demat,int Flag)
        {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinincialYearID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Memberid", _Demat.MemberId);
            Adapter.AddParam(pcol, "@FinancialYearMemberID", _Demat.FinancialYearMemberID);
            Adapter.AddParam(pcol, "@Name", _Demat.Name);
            Adapter.AddParam(pcol, "@Active", _Demat.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.AddParam(pcol, "@Flag", Flag);
            Adapter.AddParam(pcol, "@DCode", _Demat.DematID);
            Adapter.ExecutenNonQuery("USPDematInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
        }
    }
}
