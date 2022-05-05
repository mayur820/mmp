using IRecordweb.Models;
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
    public class BrokerwiseFormatMapController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BrokerMappingList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FnUpdateBrokerFormatMap(string JsonData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;
            var MemberId = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearMemberID = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            BROKERBILLFORMATMAP objdata = new BROKERBILLFORMATMAP();
            int counter = 0;

            for (counter = 0; counter < dtJsonData.Rows.Count; counter++)
            {
                objdata.BrokerID = Convert.ToInt32(dtJsonData.Rows[counter]["BrokerID"].ToString());
                // objdata.FileUpload = dtJsonData.Rows[counter]["SampleFilePath"].ToString();
                objdata.Sr_No = Convert.ToInt32(dtJsonData.Rows[counter]["Sr_No"].ToString());

                String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_BROKERBILL_FORMAT_MAP]";
                cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "UPDATE";
                // cmd.Parameters.Add("@SampleFilePath", SqlDbType.Int).Value = objdata.FileUpload;
                cmd.Parameters.Add("@BrokerID", SqlDbType.Int).Value = objdata.BrokerID;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = objdata.Sr_No;

                cmd.Connection = con;
                System.Data.DataTable DT = new System.Data.DataTable();
                try
                {
                    con.Open();

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
        public JsonResult GetListofBrokerFormat()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKERBILL_FORMAT_MAP]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_BROKERBILL_FORMATLIST";

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();

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
        public JsonResult GetAllAccountList()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKERBILL_FORMAT_MAP]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEW_BROKELIST";

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();

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
                    dict[col.ColumnName] = (row[col]);
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(list);
        }
    }
    }