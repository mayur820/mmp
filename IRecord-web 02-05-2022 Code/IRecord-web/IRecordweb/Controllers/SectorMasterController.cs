using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class SectorMasterController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: SectorMaster
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetListofSector()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_SECTORDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedById;
            // cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;

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
        public ActionResult SaveSector()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveSector(SECTOR _sector)
        {
            if (ModelState.IsValid)
            {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();
                SECTOR objdata = new SECTOR();
                objdata.SectorID = _sector.SectorID;
                objdata.Name = _sector.Name;
                objdata.Active = _sector.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/SECTORMASTER?DBAction=Insert&ID=0");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    SECTOR item = new SECTOR();
                    item.Code = dr["Code"].ToString();
                    item.Message = dr["Message"].ToString();
                    if (item.Code == "200")
                    {
                        ViewBag.Message = item.Message;
                        TempData["Message"] = item.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = item.Message;
                        TempData["Message"] = item.Message;
                        return View();
                    }
                }
            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult SectorDetails(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SECTORMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            SectorModel.Rootobject obj = new SectorModel.Rootobject();
            obj = JsonConvert.DeserializeObject<SectorModel.Rootobject>(response.Content.ToString());
            SECTOR sector = new SECTOR();
            foreach (SectorModel.Datum dr in obj.Data)
            {
                sector.SectorID = Convert.ToInt32(dr.SectorID.ToString());
                // sector.Code = dr.Code.ToString();
                sector.Name = dr.Name.ToString();
                sector.Active = Convert.ToBoolean(dr.Active.ToString());
                sector.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["SectorModel"] = sector;
            }
            var data = ViewData["SectorModel"];
            return View(data);

        }
        [HttpGet]
        public ActionResult UpdateSector(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SECTORMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            SectorModel.Rootobject obj = new SectorModel.Rootobject();
            obj = JsonConvert.DeserializeObject<SectorModel.Rootobject>(response.Content.ToString());
            SECTOR sector = new SECTOR();
            foreach (SectorModel.Datum dr in obj.Data)
            {
                sector.SectorID = Convert.ToInt32(dr.SectorID.ToString());
                // sector.Code = dr.Code.ToString();
                sector.Name = dr.Name.ToString();
                sector.Active = Convert.ToBoolean(dr.Active.ToString());
                sector.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["SectorModel"] = sector;
            }

            var data = ViewData["SectorModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult UpdateSector(SECTOR sector, int id)
        {
            if (!string.IsNullOrWhiteSpace(sector.Name))
            {
                var CreatedBy = Session["UserID"];
                SECTOR objdata = new SECTOR();
                objdata.SectorID = id;
                objdata.Name = sector.Name;
                objdata.Active = sector.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/SECTORMASTER?DBAction=Update&ID=" + id);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content); DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    SECTOR item = new SECTOR();
                    item.Code = dr["Code"].ToString();
                    item.Message = dr["Message"].ToString();
                    if (item.Code == "200")
                    {
                        ViewBag.Message = item.Message;
                        TempData["Message"] = item.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = item.Message;
                        TempData["Message"] = item.Message;
                        return View();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteSector(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SECTORMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            SectorModel.Rootobject obj = new SectorModel.Rootobject();
            obj = JsonConvert.DeserializeObject<SectorModel.Rootobject>(response.Content.ToString());
            SECTOR sector = new SECTOR();
            foreach (SectorModel.Datum dr in obj.Data)
            {
                sector.SectorID = Convert.ToInt32(dr.SectorID.ToString());
                sector.Name = dr.Name.ToString();
                sector.Active = Convert.ToBoolean(dr.Active.ToString());
                sector.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["SectorModel"] = sector;
            }

            var data = ViewData["SectorModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult DeleteSector(SECTOR sector, int id)
        {
            SECTOR objdata = new SECTOR();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SECTORMASTER?DBAction=Delete&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content); DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SECTOR item = new SECTOR();
                item.Code = dr["Code"].ToString();
                item.Message = dr["Message"].ToString();
                if (item.Code == "200")
                {
                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

    }
}