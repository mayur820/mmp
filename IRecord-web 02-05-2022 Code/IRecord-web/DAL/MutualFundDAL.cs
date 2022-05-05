using BAL;
using IrecordDAL;
using IRecordweb.Models;
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
    public class MutualFundDAL
        {
        //To Insert Mutual Fund Master data
        public void BindFundFamilyMaster(MutualFund _MutualFund)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 1;
            var Investment = _MutualFund.InvestmentOption.ToString().Split('-');
            var InvestmentOption = Investment[0];           
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@SchemeName", _MutualFund.NameOfScheme);
            Adapter.AddParam(pcol, "@SchemeCode", _MutualFund.SchemeCode);
            Adapter.AddParam(pcol, "@InvestmentCode", InvestmentOption);
            Adapter.AddParam(pcol, "@InvestmentType", 4);
            Adapter.AddParam(pcol, "@FundType", _MutualFund.Code);
            Adapter.AddParam(pcol, "@FMutualFundID", _MutualFund.FundFamilyName);
            Adapter.AddParam(pcol, "@Active", _MutualFund.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPMutualFundInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
          
            }
        //To Update Mutual Fund Master data
        public void UpdateMutualFundMaster(MutualFund _MutualFund, int MutualFundID)
            {
            DataTable dt = new DataTable();
            var ModifiedBy = HttpContext.Current.Session["UserID"];
            var Case = 2;
            var Investment = _MutualFund.InvestmentOption.ToString().Split('-');
            var InvestmentOption = Investment[0];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@SchemeName", _MutualFund.NameOfScheme);
            Adapter.AddParam(pcol, "@SchemeCode", _MutualFund.SchemeCode);
            Adapter.AddParam(pcol, "@InvestmentCode", InvestmentOption);
            Adapter.AddParam(pcol, "@InvestmentType", 4);
            Adapter.AddParam(pcol, "@FundType", _MutualFund.Code);
            Adapter.AddParam(pcol, "@FMutualFundID", _MutualFund.FundFamilyName);
            Adapter.AddParam(pcol, "@Active", _MutualFund.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", ModifiedBy);
            Adapter.AddParam(pcol, "@ScriptID", MutualFundID);
            Adapter.ExecutenNonQuery("USPMutualFundInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Insert Mutual Fund Master data
        public void DeleteMutualFundMaster(MutualFund _MutualFund , int MutualFundID)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 3;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@case", Case);
            Adapter.AddParam(pcol, "@ScriptID", MutualFundID);
            Adapter.ExecutenNonQuery("USPMutualFundInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        // To Display All Mutual Fund Master 
        public List<MutualFund> GetAllMutualFund()
            {
            List<MutualFund> scripdata = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 4;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPMutualFundInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            scripdata = new List<MutualFund>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MutualFund cobj = new MutualFund();
                cobj.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.NameOfScheme = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                cobj.SchemeCode = ds.Tables[0].Rows[i]["BSECode"].ToString();
                cobj.InvestmentOption = ds.Tables[0].Rows[i]["NSECode"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["FundType"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                scripdata.Add(cobj);
                }

            return scripdata;
            }


        // To Display Mutual Fund Master By CreatedBy 
        public List<MutualFund> GetMutualFund()
            {
            List<MutualFund> scripdata = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 5;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPMutualFundInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            scripdata = new List<MutualFund>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MutualFund cobj = new MutualFund();
                cobj.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.NameOfScheme = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                cobj.SchemeCode = ds.Tables[0].Rows[i]["BSECode"].ToString();
                cobj.InvestmentOption = ds.Tables[0].Rows[i]["NSECode"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["FundType"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                scripdata.Add(cobj);
                }

            return scripdata;
            }

        // To Display Mutual Fund Master 
        public List<MutualFund> GetMutualFundByID(int MutualFundID)
            {
            List<MutualFund> scripdata = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 6;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPMutualFundInsertUpdate", Adapter.Connection);
            com.Parameters.AddWithValue("@case", Case);
            com.Parameters.AddWithValue("@ScriptID", MutualFundID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            scripdata = new List<MutualFund>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MutualFund cobj = new MutualFund();
                cobj.FundFamilyName = ds.Tables[0].Rows[i]["MutualFundID"].ToString();
                cobj.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.NameOfScheme = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                cobj.SchemeCode = ds.Tables[0].Rows[i]["BSECode"].ToString();
                cobj.InvestmentOption = ds.Tables[0].Rows[i]["NSECode"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["FundType"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                scripdata.Add(cobj);
                }

            return scripdata;
            }

        // To Bind Fund Family Master DropdownList
        public List<FundFamily> BindFundFamilyMaster()
            {
            List<FundFamily> FundFamilyList = null;
            SqlCommand com = new SqlCommand("USPGetFundFamilyMaster", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            FundFamilyList = new List<FundFamily>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FundFamily _FundFamily = new FundFamily();
                _FundFamily.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundID"].ToString());
                _FundFamily.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                FundFamilyList.Add(_FundFamily);
                }
            return FundFamilyList;
            }
        }
    }
