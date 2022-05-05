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
   public class ScriptMasterDAL
        {
        // To Display Script Data  By ID
        public List<Script> GetAllScriptData()
            {
            List<Script> scripdata = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetScript", Adapter.Connection);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            scripdata = new List<Script>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Script cobj = new Script();
                cobj.ScriptID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.ScriptName = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                cobj.BSECode = ds.Tables[0].Rows[i]["BSECode"].ToString();
                cobj.NSECode = ds.Tables[0].Rows[i]["NSECode"].ToString();
                cobj.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                cobj.FaceValue = ds.Tables[0].Rows[i]["FaceValue"].ToString();
                cobj.ISIN = ds.Tables[0].Rows[i]["ISIN"].ToString();
                cobj.InvestmentType = ds.Tables[0].Rows[i]["InvestmentType"].ToString();
                cobj.SectorName = ds.Tables[0].Rows[i]["SectorName"].ToString();
                HttpContext.Current.Session["SectorName"] = cobj.SectorName;
                string SectorNM = HttpContext.Current.Session["SectorName"].ToString();
               // GetSectorName(SectorNM);
               if(HttpContext.Current.Session["SectorNM"] !=null)
                    {
                    cobj.SectorName = HttpContext.Current.Session["SectorNM"].ToString();
                    }
                cobj.ListType = ds.Tables[0].Rows[i]["ListType"].ToString();
                //cobj.PAN = ds.Tables[0].Rows[i]["PAN"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                scripdata.Add(cobj);
                }

            return scripdata;
            }

        // To Display Script Data  By ID
        public List<Script> GetAllScriptMaster()
            {
            List<Script> scripdata = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetScriptMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            scripdata = new List<Script>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Script cobj = new Script();
                cobj.ScriptID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.ScriptName = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                cobj.BSECode = ds.Tables[0].Rows[i]["BSECode"].ToString();
                cobj.NSECode = ds.Tables[0].Rows[i]["NSECode"].ToString();
                cobj.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                cobj.FaceValue = ds.Tables[0].Rows[i]["FaceValue"].ToString();
                cobj.ISIN = ds.Tables[0].Rows[i]["ISIN"].ToString();
                cobj.InvestmentType = ds.Tables[0].Rows[i]["InvestmentType"].ToString();
                cobj.SectorName = ds.Tables[0].Rows[i]["SectorName"].ToString();
                HttpContext.Current.Session["SectorName"] = cobj.SectorName;
                string SectorNM = HttpContext.Current.Session["SectorName"].ToString();
                GetSectorName(SectorNM);
                if (HttpContext.Current.Session["SectorNM"] != null)
                    {
                    cobj.SectorName = HttpContext.Current.Session["SectorNM"].ToString();
                    }
                cobj.ListType = ds.Tables[0].Rows[i]["ListType"].ToString();
                //cobj.PAN = ds.Tables[0].Rows[i]["PAN"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                scripdata.Add(cobj);
                }

            return scripdata;
            }

        //End 

        public List<Sector> GetSectorName(string SectorName)
            {
            List<Sector> SectorList = new List<Sector>();

            var SectorID = HttpContext.Current.Session["SectorName"];
            SqlCommand com = new SqlCommand("USPGetSectorNM", Adapter.Connection);
            com.Parameters.AddWithValue("@SectorID", SectorID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Sector sector = new Sector();


                if (ds.Tables[0].Rows.Count <= 1)
                    {
                    sector.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    HttpContext.Current.Session["SectorNM"] = sector.Name;
                    }
                SectorList.Add(sector);
                }

            return SectorList;
            }

        // To Bind Investment Type DropdownList
        public List<MType> BindInvenstmentType(MType _MType)
            {
            List<MType> MTypeList = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetMTypeMaster", Adapter.Connection);
            com.Parameters.AddWithValue("@ParentTypeId", 1);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            MTypeList = new List<MType>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MType mobj = new MType();
                mobj.TypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString());
                mobj.ParentTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["ParentTypeId"].ToString());
                mobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                mobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                mobj.Description = ds.Tables[0].Rows[i]["Description"].ToString();             
                mobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());            
                MTypeList.Add(mobj);
                }

            return MTypeList;

            }

        public List<Sector> GetSector()
            {
            List<Sector> SectorList = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetSectorData", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            SectorList = new List<Sector>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Sector cobj = new Sector();
                cobj.SectorID = Convert.ToInt32(ds.Tables[0].Rows[i]["SectorID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                SectorList.Add(cobj);
                }
            return SectorList;
            }
        //To Insert Script Master data
        public Int64 InsertScriptMaster(Script _Script, MType mtype)
            {
            DataTable dt = new DataTable();
         
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@ScriptName", _Script.ScriptName);
            Adapter.AddParam(pcol, "@BSECode", _Script.BSECode);
            Adapter.AddParam(pcol, "@NSECode", _Script.NSECode);
            Adapter.AddParam(pcol, "@IndustryID", _Script.IndustryID);
            Adapter.AddParam(pcol, "@GroupName", _Script.GroupName);
            Adapter.AddParam(pcol, "@InvestmentType", _Script.scriptlist[0]); //_Script.InvestmentType
            Adapter.AddParam(pcol, "@IsMcx", _Script.IsMcx);
            Adapter.AddParam(pcol, "@IsCurrency", _Script.IsCurrency);
            Adapter.AddParam(pcol, "@IsNcdx", _Script.IsNcdx);
            Adapter.AddParam(pcol, "@IsFO", _Script.IsFO);
            Adapter.AddParam(pcol, "@FaceValue", _Script.FaceValue);
            Adapter.AddParam(pcol, "@ISIN", _Script.ISIN);
            Adapter.AddParam(pcol, "@MutualFundID", _Script.MutualFundID);
            Adapter.AddParam(pcol, "@SectorName", _Script.SectorName);
            Adapter.AddParam(pcol, "@ListType", _Script.ListType);
            Adapter.AddParam(pcol, "@Active", _Script.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Int64 ScriptID = Convert.ToInt64(Adapter.ExecuteScalar("USPScriptInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol)));
            HttpContext.Current.Session["ScriptID"] = ScriptID;
           
            return ScriptID;
            }
        //To Insert Script --> Investment Type Dropdown data
        public void InsertscriptInvestment(Script _Script, MType mtype)
            {
            List<Script> scriptlist = new List<Script>();

            //return MtypeList;
            mtype.MTypeList = BindInvenstmentType(mtype);
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            // SqlParameterCollection pcol = new SqlCommand().Parameters;
            if (_Script.scriptlist != null)
                {
                /* foreach (var item in mtype.MTypeList) */
                //    { 
                for (int i = 0; i < _Script.scriptlist.Count(); i++)
                    {
                    //if (_Script.scriptlist[i] == "result-selected")
                    //    {

                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@InvestmentTypeId", _Script.scriptlist[i]);     //item.typeId.tostring()  mtype.MTypeList[i].TypeId
                    Adapter.AddParam(pcol, "@ScriptID", _Script.ScriptID);
                    Adapter.AddParam(pcol, "@Active", _Script.Active);
                    Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
                    Adapter.ExecutenNonQuery("USPScriptInvestmentInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                    //}
                    }
                }

            }

        //To Get Script Master over ScriptId for View, Delete, Update
        public List<Script> GetScriptByID(int ScriptID)
            {

            List<Script> list = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetScriptByID", Adapter.Connection);
            com.Parameters.AddWithValue("@ScriptID", ScriptID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            list = new List<Script>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Script cobj = new Script();
                cobj.ScriptID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.ScriptName = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                cobj.BSECode = ds.Tables[0].Rows[i]["BSECode"].ToString();
                cobj.NSECode = ds.Tables[0].Rows[i]["NSECode"].ToString();
                cobj.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                cobj.FaceValue = ds.Tables[0].Rows[i]["FaceValue"].ToString();
                cobj.ISIN = ds.Tables[0].Rows[i]["ISIN"].ToString();
                cobj.InvestmentType = ds.Tables[0].Rows[i]["InvestmentType"].ToString();
                cobj.SectorName = ds.Tables[0].Rows[i]["SectorName"].ToString();
                HttpContext.Current.Session["SectorName"] = cobj.SectorName;
                string SectorNM = HttpContext.Current.Session["SectorName"].ToString();
                // GetSectorName(SectorNM);
                //cobj.SectorName = HttpContext.Current.Session["SectorNM"].ToString();
                cobj.ListType = ds.Tables[0].Rows[i]["ListType"].ToString();
                //cobj.PAN = ds.Tables[0].Rows[i]["PAN"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.IsMcx = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsMcx"].ToString());
                cobj.IsCurrency = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsCurrency"].ToString());
                cobj.IsNcdx = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsNcdx"].ToString());
                cobj.IsFO = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsF&O"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                list.Add(cobj);

                }
            return list;
            }

        //To Edit Script Master data
        public void UpdateScriptMaster(Script _Script, int ScriptID, MType mtype)
            {
            DataTable dt = new DataTable();
            var ModifiedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@ScriptName", _Script.ScriptName);
            Adapter.AddParam(pcol, "@BSECode", _Script.BSECode);
            Adapter.AddParam(pcol, "@NSECode", _Script.NSECode);
            Adapter.AddParam(pcol, "@IndustryID", _Script.IndustryID);
            Adapter.AddParam(pcol, "@GroupName", _Script.GroupName);
            Adapter.AddParam(pcol, "@InvestmentType", _Script.scriptlist[0].ToString()); //_Script.InvestmentType
            Adapter.AddParam(pcol, "@IsMcx", _Script.IsMcx);
            Adapter.AddParam(pcol, "@IsCurrency", _Script.IsCurrency);
            Adapter.AddParam(pcol, "@IsNcdx", _Script.IsNcdx);
            Adapter.AddParam(pcol, "@IsFO", _Script.IsFO);
            Adapter.AddParam(pcol, "@FaceValue", _Script.FaceValue);
            Adapter.AddParam(pcol, "@ISIN", _Script.ISIN);
            Adapter.AddParam(pcol, "@MutualFundID", _Script.MutualFundID);
            Adapter.AddParam(pcol, "@SectorName", _Script.SectorName.ToString());
            Adapter.AddParam(pcol, "@ListType", _Script.ListType);
            Adapter.AddParam(pcol, "@Active", _Script.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", ModifiedBy);
            Adapter.AddParam(pcol, "@ScriptID", ScriptID);
            Adapter.ExecutenNonQuery("USPUpdateScriptByID", CommandType.StoredProcedure, Adapter.param(pcol));
            // return ScriptId;
            }
        //To Update Script --> Investment Type Dropdown data
        public void UpdatescriptInvestment(Script _Script, MType mtype, int scriptid)
            {
            List<Script> scriptlist = new List<Script>();


            mtype.MTypeList = BindInvenstmentType(mtype);
            DataTable dt = new DataTable();
            var ModifiedBy = HttpContext.Current.Session["UserID"];

            if (_Script.scriptlist != null)
                {
                for (int i = 0; i < _Script.scriptlist.Count(); i++)
                    {
                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@InvestmentTypeId", _Script.scriptlist[i]);     //item.typeId.tostring()  mtype.MTypeList[i].TypeId
                    Adapter.AddParam(pcol, "@ScriptID", scriptid);
                    Adapter.AddParam(pcol, "@Active", _Script.Active);
                    Adapter.AddParam(pcol, "@CreatedBy", ModifiedBy);
                    Adapter.ExecutenNonQuery("USPScriptInvestmentInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                    }
                }

            }
        //To Delete Script 
        public void DeleteScriptByID(int ScriptID)
            {

            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@ScriptID", ScriptID);
            Adapter.ExecutenNonQuery("USPDeleteScriptByID", CommandType.StoredProcedure, Adapter.param(pcol));
            }
        //To Get Financial Year over FinancialYearID for View, Delete, Update
        public List<Script> GetScriptInvestment(int ScriptID)
            {

            List<Script> list = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetScriptInvestment", Adapter.Connection);
            com.Parameters.AddWithValue("@ScriptID", ScriptID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            list = new List<Script>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Script cobj = new Script();

                cobj.ScriptID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.TypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString());
                cobj.InvestmentType = ds.Tables[0].Rows[i]["Name"].ToString();
                list.Add(cobj);

                }
            return list;
            }
        }
    }
