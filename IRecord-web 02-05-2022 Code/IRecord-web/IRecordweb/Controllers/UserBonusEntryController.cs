using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class UserBonusEntryController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: UserBonusEntry
        public ActionResult Index()
        {
            return View();
        }




        public JsonResult getAllScript()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_CADetails]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ViewForBonusEntry";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@Var2", SqlDbType.Int).Value = FinancialYearCode;
            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(DT);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
        public JsonResult getAlldata(string ScpritId, string Date)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Client_BonusEntry_Uitily]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEWBY_Head";


            //cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Session["UserID"].ToString(); ;
            //cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = ScpritId.Split(':')[1];
            //cmd.Parameters.Add("@Var2", SqlDbType.NVarChar).Value = Convert.ToDateTime(Date).ToString("yyyy/MM/dd");

            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(DT);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            List<BonusEntryViewModel> objdatas = new List<BonusEntryViewModel>();
            foreach(DataRow dr in DT.Rows)
            {
                BonusEntryViewModel objdata = new BonusEntryViewModel();
                objdata.MemberId = dr["MemberId"].ToString();
                objdata.Member = dr["Member"].ToString();
                objdata.DematAC = dr["DematAC"].ToString();
                objdata.OldStockQty = dr["OldStockQty"].ToString();
                objdata.NewScriptCAQty = dr["NewScriptCAQty"].ToString();
                List<BonusEntryDetils> ListofDetils = new List<BonusEntryDetils>();
                DataTable dt = getdetilsdata();
                foreach(DataRow dr1 in dt.Rows)
                {
                    BonusEntryDetils obj1 = new BonusEntryDetils();

                    obj1.Consultant = dr1["Consultant"].ToString();
                    obj1.Broker = dr1["Broker"].ToString();
                    obj1.InvestmentType = dr1["InvestmentType"].ToString();
                    obj1.CAQty = dr1["CAQty"].ToString();
                    obj1.AllocateQty = Convert.ToDouble(dr1["AllocateQty"].ToString());
                    ListofDetils.Add(obj1);






                }
                objdata.ListofDetils = ListofDetils;
                objdatas.Add(objdata);
            }
            var json = new JavaScriptSerializer().Serialize(objdatas);

            return Json(objdatas, JsonRequestBehavior.AllowGet);

        }
        public DataTable getdetilsdata()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Client_BonusEntry_Uitily]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEWBY_details";


            //cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Session["UserID"].ToString(); ;
            //cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = ScpritId.Split(':')[1];
            //cmd.Parameters.Add("@Var2", SqlDbType.NVarChar).Value = Convert.ToDateTime(Date).ToString("yyyy/MM/dd");

            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(DT);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return DT;
        }
        [HttpPost]
        public JsonResult SubmitData(string JsonData,string JsonScriptHeader)
        {
            DataTable dt_JsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dt_JsonScript = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonScriptHeader);
            foreach (DataRow dr in dt_JsonData.Rows)
            {
                String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_Bonus_Rights_Div]";
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@Entry_type", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Script_code", SqlDbType.VarChar).Value = dt_JsonScript.Rows[0]["ScriptId"].ToString().Split(':')[1];
                cmd.Parameters.Add("@Declared_Date", SqlDbType.VarChar).Value = Convert.ToDateTime(dt_JsonScript.Rows[0]["RecordDate"].ToString()).ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@Record_Date", SqlDbType.VarChar).Value = Convert.ToDateTime(dt_JsonScript.Rows[0]["RecordDate"].ToString()).ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@Bonus_Rights_qty", SqlDbType.Float).Value = dt_JsonScript.Rows[0]["BonusQtyPer"].ToString();
                cmd.Parameters.Add("@PerShare", SqlDbType.Float).Value = dt_JsonScript.Rows[0]["Share"].ToString();
                cmd.Parameters.Add("@PerShare_Rate", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@Div_Rate", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@Div_Type", SqlDbType.VarChar).Value = 0;
                cmd.Parameters.Add("@FromMonth", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FromYear", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@ToMonth", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@ToYear", SqlDbType.SmallInt).Value = 0;
                cmd.Parameters.Add("@Member_Code", SqlDbType.VarChar).Value = dr["MemberId"].ToString();
                cmd.Parameters.Add("@YearCode", SqlDbType.VarChar).Value = (Session["Dt_FinancialYear"] as DataTable).Rows[0]["FinancialYearUserId"].ToString();
                cmd.Parameters.Add("@Additional_Qty", SqlDbType.Int).Value = 0;
                if (con.State == ConnectionState.Open) con.Close();
                if (con.State == ConnectionState.Closed) con.Open();
                DataTable DT = new DataTable();
                try
                {
                    //con.Open();
                    /////Only Use Case Of Mapping
                    //string query = cmd.CommandText;
                    //foreach (SqlParameter p in cmd.Parameters)
                    //{
                    //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                    //}
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(DT);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public JsonResult getFullDeatils(string Id)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Client_BonusEntry_Uitily]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewFullDeatils";
            cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id.Split(':')[2];
            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(DT);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
        public static object DataTableToJSON(System.Data.DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(list);
        }

        //public JsonResult GetGlobalCodes(string Code, string CodeType)
        //{
        //    return Json;
        //}
        //public List<BONUSENTRY> GetGlobalCodes(string Code, string CodeType)
        //{
        //    List<BONUSENTRY> list = new List<BONUSENTRY>();
        //    return list;
        //}
        //public void CalScriptWiseQty()
        //{
        //    string shareinvstmentacc, sharetradingac;
        //    shareinvstmentacc = GetGlobalCodes("Share_Inv_EQ_AC", "Account Code");
        //    sharetradingac = GetGlobalCodes("Share_Trad_AC", "Account Code");
        //}
    }
}