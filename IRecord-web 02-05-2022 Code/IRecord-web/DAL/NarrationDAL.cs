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
  public class NarrationDAL
        {
        //To Insert Narration Master data
        public void InsertNarrationMaster(Narration _Narration)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 1;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@Name", _Narration.Name);
            Adapter.AddParam(pcol, "@Active", _Narration.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPNarrationInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        //To Update Narration Master data
        public void UpdateNarrationMaster(Narration _Narration, int NarrationID)
            {
            DataTable dt = new DataTable();
            var ModifiedBy = HttpContext.Current.Session["UserID"];
            var Case = 2;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@Name", _Narration.Name);
            Adapter.AddParam(pcol, "@Active", _Narration.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", ModifiedBy);
            Adapter.AddParam(pcol, "@NarrationID", NarrationID);
            Adapter.ExecutenNonQuery("USPNarrationInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Update Narration Master data
        public void DeleteNarrationMaster(int NarrationID)
            {
            DataTable dt = new DataTable();          
            var Case = 3;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@NarrationID", NarrationID);
            Adapter.ExecutenNonQuery("USPNarrationInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        //To Get All Narration Master data 
        public List<Narration> GetAllNarration()
            {
            List<Narration> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 4;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPNarrationInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Narration>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Narration cobj = new Narration();
                cobj.NarrationID = Convert.ToInt32(ds.Tables[0].Rows[i]["NarrationID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }

            return IList;
            }

        //To Get Narration Master data By CreatedBy 
        public List<Narration> GetNarrationByCB()
            {
            List<Narration> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 5;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPNarrationInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Narration>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Narration cobj = new Narration();
                cobj.NarrationID = Convert.ToInt32(ds.Tables[0].Rows[i]["NarrationID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }

            return IList;
            }

        //To Get Narration Master data By ID
        public List<Narration> GetNarrationByID(int NarrationID)
            {
            List<Narration> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 6;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPNarrationInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.Parameters.AddWithValue("@NarrationID", NarrationID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Narration>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Narration cobj = new Narration();
                cobj.NarrationID = Convert.ToInt32(ds.Tables[0].Rows[i]["NarrationID"].ToString());
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
