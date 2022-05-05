using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class BrokerBillFormatMasterController : Controller
    {
        // GET: BrokerBillFormatMaster
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ViewBrokerFormatedata()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_M_BROKER_MAPPING]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW";

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

        public JsonResult Get_Validate_SrNo(string SrNo)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_M_BROKER_MAPPING]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VALIDATE_SR_NO";
            cmd.Parameters.Add("@Sr_No", SqlDbType.NVarChar).Value = SrNo.Trim();
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

            if (DT.Rows.Count > 0)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }
        public void WriteByteArrayToPdf(string inPDFByteArrayStream, string path)
        {
            byte[] data = Convert.FromBase64String(inPDFByteArrayStream);
            //if (Directory.Exists(path))s
            //{

            using (FileStream Writer = new System.IO.FileStream(path, FileMode.Create, FileAccess.Write))
            {

                Writer.Write(data, 0, data.Length);
            }
            //}
            //else
            //{
            //    throw new System.Exception("PDF Shared Location not found");
            //}

        }
        [HttpPost]
        public JsonResult FnBrokerFormatMasterSaveEntry(string JsonData, string files)
        {

            string filename = "";
            if (files != "")
            {
                string uploadfilename = files.Split('@')[0].ToString();
                string filedata = files.Split('@')[1].ToString().Split(',')[1].ToString();
                filename = "/CN_SampleFile/" + Guid.NewGuid().ToString() + uploadfilename;
                WriteByteArrayToPdf(filedata, System.Web.Hosting.HostingEnvironment.MapPath(filename));

            }
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            // if(dtJsonData.Rows[0]["chk_IsTradeFile"].i)
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_M_BROKER_MAPPING]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = dtJsonData.Rows[0]["txt_Sr_No"].ToString();
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["txt_Description"].ToString();
            cmd.Parameters.Add("@FileType", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["ddlFileType"].ToString();
            cmd.Parameters.Add("@FileExtension", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["ddl_File_Extension_Type"].ToString();
            cmd.Parameters.Add("@InvestmentType", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["ddlInvestmentType"].ToString();
            cmd.Parameters.Add("@Display_in_List", SqlDbType.NVarChar).Value = (dtJsonData.Columns.Contains("chk_DisplayInList") ? (dtJsonData.Rows[0]["chk_DisplayInList"].ToString().ToLower() == "true" ? "Y" : "N") : "N");
            cmd.Parameters.Add("@isTradefile", SqlDbType.NVarChar).Value = (dtJsonData.Columns.Contains("chk_IsTradeFile") ? (dtJsonData.Rows[0]["chk_IsTradeFile"].ToString().ToLower() == "true" ? "Y" : "N") : "N");
            cmd.Parameters.Add("@CombinedFormat", SqlDbType.NVarChar).Value = (dtJsonData.Columns.Contains("chk_CombinedFile") ? (dtJsonData.Rows[0]["chk_CombinedFile"].ToString().ToLower() == "true" ? "Y" : "N") : "N");
            cmd.Parameters.Add("@SampleFilePath", SqlDbType.NVarChar).Value = filename.ToString();
            cmd.Parameters.Add("@BrokerID", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["ddlBroker"].ToString();

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

        [HttpPost]
        public JsonResult FnBrokerFormatMasterUpdateEntry(string JsonData, string files)
        {
            string filename = "";
            if (files != "")
            {
                string uploadfilename = files.Split('@')[0].ToString();
                string filedata = files.Split('@')[1].ToString().Split(',')[1].ToString();
                filename = "/CN_SampleFile/" + Guid.NewGuid().ToString() + uploadfilename;
                WriteByteArrayToPdf(filedata, System.Web.Hosting.HostingEnvironment.MapPath(filename));

            }
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_M_BROKER_MAPPING]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATE";
            cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = dtJsonData.Rows[0]["txt_Sr_No"].ToString();
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["txt_Description"].ToString();
            cmd.Parameters.Add("@FileType", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["ddlFileType"].ToString();
            cmd.Parameters.Add("@FileExtension", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["ddl_File_Extension_Type"].ToString();
            cmd.Parameters.Add("@InvestmentType", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["ddlInvestmentType"].ToString();
            cmd.Parameters.Add("@Display_in_List", SqlDbType.NVarChar).Value = (dtJsonData.Columns.Contains("chk_DisplayInList") ? (dtJsonData.Rows[0]["chk_DisplayInList"].ToString().ToLower() == "true" ? "Y" : "N") : "N");
            cmd.Parameters.Add("@isTradefile", SqlDbType.NVarChar).Value = (dtJsonData.Columns.Contains("chk_IsTradeFile") ? (dtJsonData.Rows[0]["chk_IsTradeFile"].ToString().ToLower() == "true" ? "Y" : "N") : "N");
            cmd.Parameters.Add("@CombinedFormat", SqlDbType.NVarChar).Value = (dtJsonData.Columns.Contains("chk_CombinedFile") ? (dtJsonData.Rows[0]["chk_CombinedFile"].ToString().ToLower() == "true" ? "Y" : "N") : "N");
            cmd.Parameters.Add("@SampleFilePath", SqlDbType.NVarChar).Value = filename.ToString();
            cmd.Parameters.Add("@BrokerID", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["ddlBroker"].ToString();


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

        public JsonResult BrokerFormatDeleteConfig(string ID)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_M_BROKER_MAPPING]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DELETE";
            cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = ID;


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