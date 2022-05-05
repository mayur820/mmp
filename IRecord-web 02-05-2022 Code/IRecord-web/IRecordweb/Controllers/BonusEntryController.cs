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
    public class BonusEntryController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: BonusEntry
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SaveBonus()
        {
            ViewBag.ScriptList = new SelectList(ScriptList().ToList(), dataValueField: "ScriptCode", dataTextField: "Script");
            return View();
        }

        [HttpPost]
        public ActionResult SaveBonus(int id)
        {
            ViewBag.ScriptList = new SelectList(ScriptList().ToList(), dataValueField: "ScriptCode", dataTextField: "Script");
            return View();
        }
        public List<BONUSENTRY> ScriptList()
        {
            BONUSENTRY objdata = new BONUSENTRY();
            var ID = Session["UserID"];
            var client = new RestClient(URL + "api/Master/BONUSENTRY?DBAction=ScriptList&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<BONUSENTRY> list = new List<BONUSENTRY>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                BONUSENTRY item = new BONUSENTRY();
                item.ScriptCode = Convert.ToInt32(dr["Script Code"].ToString());
                item.Script = dr["Script Name"].ToString();
                item.OldISIN = dr["ISIN"].ToString();
                list.Add(item);
            }
            return list;
        }

        public JsonResult ScriptISIN(string OldISIN)
        {
            BONUSENTRY objdata = new BONUSENTRY();
            var ID = Session["UserID"];
            var client = new RestClient(URL + "api/Master/BONUSENTRY?DBAction=ScriptList&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<BONUSENTRY> list = new List<BONUSENTRY>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                BONUSENTRY item = new BONUSENTRY();
                item.ScriptCode = Convert.ToInt32(dr["Script Code"].ToString());
                item.Script = dr["Script Name"].ToString();
                item.OldISIN = dr["ISIN"].ToString();
                list.Add(item);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAllScript()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BonusEntry_Uitily]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewScript";

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
        public JsonResult getAlldata()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BonusEntry_Uitily]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEW";
            

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
        [HttpPost]
        public JsonResult SubmitData(string JsonData)
        {
            DataTable dt_JsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
          
           SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BonusEntryMaster]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@ScriptId", SqlDbType.Int).Value = dt_JsonData.Rows[0]["ScriptId"].ToString().Split(':')[1];
            cmd.Parameters.Add("@RecordDate", SqlDbType.Date).Value = Convert.ToDateTime(dt_JsonData.Rows[0]["RecordDate"].ToString()).ToString("yyyy/MM/dd");
            cmd.Parameters.Add("@ExistingISIN", SqlDbType.NVarChar).Value = dt_JsonData.Rows[0]["ExistingISIN"].ToString();
            cmd.Parameters.Add("@NewISIN", SqlDbType.NVarChar).Value = dt_JsonData.Rows[0]["NewISIN"].ToString();
            cmd.Parameters.Add("@PerQty", SqlDbType.Int).Value = dt_JsonData.Rows[0]["BonusQtyPer"].ToString();
            cmd.Parameters.Add("@BonusQty", SqlDbType.Int).Value = dt_JsonData.Rows[0]["Share"].ToString(); 
            cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = Session["UserID"].ToString();
         
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
            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public JsonResult getISIN(string Id)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BonusEntry_Uitily]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewISIN";
            cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id.Split(':')[1];
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
            string ISIN = "";
            if (DT.Rows.Count > 0)
            {
                ISIN = DT.Rows[0]["ISIN"].ToString();
            }
            return Json(ISIN, JsonRequestBehavior.AllowGet);

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