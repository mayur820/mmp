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
   public class IndustryDAL
        {
        //To Insert Industry Master data
        public void InsertIndustyMaster(Industry _Industry)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Name", _Industry.Name);
            Adapter.AddParam(pcol, "@Active", _Industry.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPIndusrtyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Get Industry Master data
        public List<Industry> GetIndustry()
            {
            List<Industry> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetIndustry", Adapter.Connection);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Industry>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Industry cobj = new Industry();
                cobj.IndustryID = Convert.ToInt32(ds.Tables[0].Rows[i]["IndustryID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }
            return IList;
            }

        //To Get Industry Master data
        public List<Industry> GetAllIndustry()
            {
            List<Industry> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllIndustry", Adapter.Connection);
            //com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Industry>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Industry cobj = new Industry();
                cobj.IndustryID = Convert.ToInt32(ds.Tables[0].Rows[i]["IndustryID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }
            return IList;
            }


        //To Get Industry Master data By ID
        public List<Industry> GetIndustryById(int IndustryID)
            {
            List<Industry> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetIndustryByID", Adapter.Connection);
            com.Parameters.AddWithValue("@IndustryID", IndustryID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Industry>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Industry cobj = new Industry();
                cobj.IndustryID = Convert.ToInt32(ds.Tables[0].Rows[i]["IndustryID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }
            return IList;
            }

        //To Delete Industry Master data by its User
        public void DeleteIndustryMaster(int IndustryID)
            {
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@IndustryID", IndustryID);
            Adapter.ExecutenNonQuery("USPDeleteIndustryByID", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Update Industry Master data by its User
        public void UpdateIndustryMaster(Industry industry, int IndustryId)
            {
            DataTable dt = new DataTable();
            var ModifiedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Name", industry.Name);
            Adapter.AddParam(pcol, "@Active", industry.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", ModifiedBy);
            Adapter.AddParam(pcol, "@IndustryID", IndustryId);
            Adapter.ExecutenNonQuery("USPUpdateIndustryByID", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        

        }
    }
