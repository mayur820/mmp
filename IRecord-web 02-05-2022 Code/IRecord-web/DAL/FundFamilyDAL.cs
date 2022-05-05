using BAL;
using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
    {
   public class FundFamilyDAL
        {

        //To Insert Fund Family Master data
        public void InsertFundFamilyMaster(FundFamily _FundFamily)
            {
            DataTable dt = new DataTable();
            var Case = 1; 
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@Name", _FundFamily.Name);
            Adapter.AddParam(pcol, "@Active", _FundFamily.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPFundFamilyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }

        //To Insert Fund Family Master data
        public void UpdateFundFamilyMaster(FundFamily _FundFamily, int MutualFundID)
            {
            DataTable dt = new DataTable();
            var Case = 2;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@Name", _FundFamily.Name);
            Adapter.AddParam(pcol, "@Active", _FundFamily.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.AddParam(pcol, "@MutualFundID", MutualFundID);
            Adapter.ExecutenNonQuery("USPFundFamilyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }

        //To Delete Fund Family 
        public void DeleteFFamilyByID(int MutualFundID)
            {

            DataTable dt = new DataTable();
            var Case = 3;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@MutualFundID", MutualFundID);
            Adapter.ExecutenNonQuery("USPFundFamilyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }

        //To Get Fund Family Master data by CreatedBy
        public List<FundFamily> GetAllFundFamilyMaster()
            {
            List<FundFamily> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 4;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPFundFamilyInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<FundFamily>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FundFamily cobj = new FundFamily();
                cobj.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }

            return IList;
            }

        //To Get Fund Family Master data by CreatedBy
        public List<FundFamily> GetFundFamilyMaster()
            {
            List<FundFamily> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 5;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPFundFamilyInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<FundFamily>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FundFamily cobj = new FundFamily();
                cobj.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundID"].ToString());               
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }

            return IList;
            }

        //To Get Fund Family Master data by CreatedBy
        public List<FundFamily> GetFundFamilyByID(int MutualFundID)
            {
            List<FundFamily> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 6;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPFundFamilyInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.Parameters.AddWithValue("@MutualFundID", MutualFundID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<FundFamily>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FundFamily cobj = new FundFamily();
                cobj.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }

            return IList;
            }
        }
    }
