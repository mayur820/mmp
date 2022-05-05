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
   public class MFCategoryDAL
        {
        //To Insert Mutual Fund Category Master data
        public void InsertMFundCatMaster(MutualFundCategory _MutualFundCategory)
            {
            DataTable dt = new DataTable();
            var Case = 1;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Case", Case);
            Adapter.AddParam(pcol, "@Name", _MutualFundCategory.Name);
            Adapter.AddParam(pcol, "@Active", _MutualFundCategory.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPMFundCatInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Insert Mutual Fund Category Master data
        public void UpdateMFundCatMaster(MutualFundCategory _MutualFundCategory, int MutualFundCategoryID)
            {
            DataTable dt = new DataTable();
            var Case = 2;
            var ModifiedBy = HttpContext.Current.Session["UserID"];           
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Case", Case);
            Adapter.AddParam(pcol, "@Name", _MutualFundCategory.Name);
            Adapter.AddParam(pcol, "@Active", _MutualFundCategory.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", ModifiedBy);
            Adapter.AddParam(pcol, "@MutualFundCategoryID", MutualFundCategoryID);
            Adapter.ExecutenNonQuery("USPMFundCatInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Insert Mutual Fund Category Master data
        public void DeleteMFundCatMaster(int MutualFundCategoryID)
            {
            DataTable dt = new DataTable();
            var Case = 3;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Case", Case);
            Adapter.AddParam(pcol, "@MutualFundCategoryID", MutualFundCategoryID);    
            Adapter.ExecutenNonQuery("USPMFundCatInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }

        // Get All Mutual Fund Category Master
        public List<MutualFundCategory> GetAllMFCatMaster()
            {
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 4;
            List<MutualFundCategory> MFCatList = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPMFundCatInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@Case", Case);      
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            MFCatList = new List<MutualFundCategory>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MutualFundCategory cobj = new MutualFundCategory();
                cobj.MutualFundCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundCategoryID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                MFCatList.Add(cobj);
                }
            return MFCatList;
            }

        // Get Mutual Fund Category Master by CreatedBy
        public List<MutualFundCategory> GetMFCatMaster()
            {
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 5;
            List<MutualFundCategory> MFCatList = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPMFundCatInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@Case", Case);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            MFCatList = new List<MutualFundCategory>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MutualFundCategory cobj = new MutualFundCategory();
                cobj.MutualFundCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundCategoryID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                MFCatList.Add(cobj);
                }
            return MFCatList;
            }
        // Get Mutual Fund Category Master by ID
        public List<MutualFundCategory> GetMFCatMasterByID(int MutualFundCategoryID)
            {
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 6;
            List<MutualFundCategory> MFCatList = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPMFundCatInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@Case", Case);
            com.Parameters.AddWithValue("@MutualFundCategoryID", MutualFundCategoryID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            MFCatList = new List<MutualFundCategory>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MutualFundCategory cobj = new MutualFundCategory();
                cobj.MutualFundCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundCategoryID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                MFCatList.Add(cobj);
                }
            return MFCatList;
            }
        }
    }
