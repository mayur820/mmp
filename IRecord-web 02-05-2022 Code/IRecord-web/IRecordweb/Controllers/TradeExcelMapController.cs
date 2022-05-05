using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
    {
    public class TradeExcelMapController : Controller
        {
        string PdfConfig = ConfigurationManager.AppSettings["PdfConfig"];
        // GET: TradeExcelMap
        public ActionResult Index()
            {
            return View();
            }
        public JsonResult GetAllBILLS(string Invtype)
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_TRADEFILE";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = Invtype;
            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
                {
                con.Open();
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
        public JsonResult GetGetFormate(string Invtype, string BrokerBill_Format_Id)
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_TRADE_EXCEL_UTITLY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = Invtype;
            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
                {
                con.Open();
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
            DT.Columns.Add("InvestmentType", typeof(string));
            DT.Columns.Add("BrokerBill_Format_Id", typeof(string));
            foreach (DataRow dr in DT.Rows)
                {
                dr["InvestmentType"] = Invtype;
                dr["BrokerBill_Format_Id"] = BrokerBill_Format_Id;

                }
            DT.AcceptChanges();
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
            JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return serializer.Serialize(list);
            }
        public JsonResult GETALLEXCELCOLNM()
            {
            return Json(DataTableToJSON(Session["DT_EXCEL_COLUMN"] as DataTable), JsonRequestBehavior.AllowGet);
            }

        public JsonResult GetGetAllMappingFormate()
            {

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_EXCEL_MAPPING";

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
                {
                con.Open();
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
        public JsonResult DELETEFORMATE(string ID)
            {

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "DELETE_EXCEL_MAPPING";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = ID;

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
                {
                con.Open();
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

        public void GetAllColumInDataTable(DataTable dt)
            {
            DataTable dtclm = new DataTable();
            dtclm.Columns.Add("ID", typeof(string));
            dtclm.Columns.Add("NAME", typeof(string));
            string[] columnNames = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

            foreach (string str in columnNames)
                {
                DataRow DR = dtclm.NewRow();
                DR["ID"] = str;
                DR["NAME"] = str;
                dtclm.Rows.Add(DR);


                }
            Session["DT_EXCEL_COLUMN"] = dtclm;
            }

        [HttpPost]
        public void Upload()
            {


            foreach (string key in Request.Files)
                {
                HttpPostedFileBase FilePath = Request.Files[key];
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(FilePath.InputStream))
                    {
                    bytes = br.ReadBytes(FilePath.ContentLength);
                    }
                string _FileName = Path.GetFileName(FilePath.FileName);
                string fileextenion = Path.GetExtension(FilePath.FileName);
                string ContentType = FilePath.ContentType.ToString();
                Session["FilesString"] = Convert.ToBase64String(bytes);
                Session["FileName"] = _FileName;
                Session["Fileextenion"] = fileextenion;
                Session["ContentType"] = ContentType;

                TradeFilesTrans.Rootobject objtrans = new TradeFilesTrans.Rootobject();
                //objtrans.Filecode = data.ContractNoteId;
                objtrans.fileName = _FileName;
                objtrans.contentType = ContentType;
                objtrans.fileextension = fileextenion;
                objtrans.pdfbytes = Convert.ToBase64String(bytes);
                // objtrans.StartDate = data.FromDate;
                // objtrans.EndDate = data.ToDate;

                ServicePointManager.ServerCertificateValidationCallback = new
     RemoteCertificateValidationCallback
     (
        delegate { return true; }
     );
                var client = new RestClient(PdfConfig + "api/PdfProcess/GetdDataExcelUtility");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
                request.AddHeader("Content-Type", "application/json");
                var body = Newtonsoft.Json.JsonConvert.SerializeObject(objtrans);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(response.Content);
                GetAllColumInDataTable(myDataSet.Tables[0]);
                Session["ExcelData"] = myDataSet.Tables[0];
                }

            //return Content("Success");
            }
        [HttpPost]
        public void Save(string JsonData)
            {

            DataTable dsdata = JsonConvert.DeserializeObject<DataTable>(JsonData);
            foreach (DataRow dr in dsdata.Rows)
                {
                String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_M_EXCEL_MAPPING]";
                cmd.Parameters.Add("@InvestmentType", SqlDbType.NVarChar).Value = dr["InvestmentType"].ToString();
                cmd.Parameters.Add("@BrokerBill_Format_Id", SqlDbType.NVarChar).Value = dr["BrokerBill_Format_Id"].ToString();
                cmd.Parameters.Add("@dbcolname", SqlDbType.NVarChar).Value = dr["NAME"].ToString();
                cmd.Parameters.Add("@excelcolname", SqlDbType.NVarChar).Value = dr["EXCELNAME"].ToString();
                cmd.Connection = con;
                System.Data.DataTable DT = new System.Data.DataTable();
                try
                    {
                    con.Open();
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



            //return Content("Success");
            }
        }
    }