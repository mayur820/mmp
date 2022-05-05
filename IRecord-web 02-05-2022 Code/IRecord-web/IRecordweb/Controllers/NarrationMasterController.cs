using BAL;
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
    public class NarrationMasterController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: Narration
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetListofNarration()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_NARRATIONDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = MemberCode;
            // cmd.Parameters.Add("@AccountId", SqlDbType.Int).Value = FinancialYearCode;

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
        [HttpGet]
        public ActionResult SaveNarration()
            {          
            return View();
            }
        [HttpPost]
        public ActionResult SaveNarration(NARRATION _Narration)
            {
            if (ModelState.IsValid)
                {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();
                NARRATION objdata = new NARRATION();
                objdata.NarrationID = _Narration.NarrationID;
                objdata.Name = _Narration.Name;
                objdata.Active = _Narration.Active;
                objdata.CreatedBy = Convert.ToInt32(MemberCode.ToString());
                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/NARRATIONMASTER?DBAction=Insert&ID=0");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
               // Console.WriteLine(response.Content);
                DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                foreach (DataRow dr in data.Tables[0].Rows)
                    {
                    NARRATION item = new NARRATION();
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
                   
                //ViewBag.Message = "Records Save Sucessfully !!";
                }
            return RedirectToAction("Index");
            }
        [HttpGet]
        public ActionResult NarrationDetails(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/NARRATIONMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            NarrationModel.Rootobject obj = new NarrationModel.Rootobject();
            obj = JsonConvert.DeserializeObject<NarrationModel.Rootobject>(response.Content.ToString());
            NARRATION Narration = new NARRATION();
            foreach (NarrationModel.Datum dr in obj.Data)
                {
                Narration.NarrationID = Convert.ToInt32(dr.NarrationID.ToString());
                Narration.Code = dr.Code.ToString();
                Narration.Name = dr.Name.ToString();
                Narration.Active = Convert.ToBoolean(dr.Active.ToString());
                Narration.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["NarrationModel"] = Narration;
                }
            var data = ViewData["NarrationModel"];
            return View(data);
            }
        [HttpGet]
        public ActionResult NarrationDelete(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/NARRATIONMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            NarrationModel.Rootobject obj = new NarrationModel.Rootobject();
            obj = JsonConvert.DeserializeObject<NarrationModel.Rootobject>(response.Content.ToString());
            NARRATION Narration = new NARRATION();
            foreach (NarrationModel.Datum dr in obj.Data)
                {
                Narration.NarrationID = Convert.ToInt32(dr.NarrationID.ToString());
                Narration.Code = dr.Code.ToString();
                Narration.Name = dr.Name.ToString();
                Narration.Active = Convert.ToBoolean(dr.Active.ToString());
                Narration.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["NarrationModel"] = Narration;
                }
            var data = ViewData["NarrationModel"];
            return View(data);
            }
        [HttpPost]
        public ActionResult NarrationDelete(Narration narration, int id)
        {
            Narration objdata = new Narration();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/NARRATIONMASTER?DBAction=Delete&ID=" + id);
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
                NARRATION item = new NARRATION();
                item.Code = dr["Code"].ToString();
                item.Message = dr["Message"].ToString();
                if (item.Code == "200")
                {
                    TempData["Message"] = item.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = item.Message;
                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult NarrationUpdate(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/NARRATIONMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            NarrationModel.Rootobject obj = new NarrationModel.Rootobject();
            obj = JsonConvert.DeserializeObject<NarrationModel.Rootobject>(response.Content.ToString());
            NARRATION Narration = new NARRATION();
            foreach (NarrationModel.Datum dr in obj.Data)
                {
                Narration.NarrationID = Convert.ToInt32(dr.NarrationID.ToString());
                Narration.Code = dr.Code.ToString();
                Narration.Name = dr.Name.ToString();
                Narration.Active = Convert.ToBoolean(dr.Active.ToString());
                Narration.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["NarrationModel"] = Narration;
                }
            var data = ViewData["NarrationModel"];
            return View(data);
            }
        [HttpPost]
        public ActionResult NarrationUpdate(NARRATION _Narration, int id, string name)
            {
            if(!string.IsNullOrWhiteSpace(_Narration.Name))
                {
                var CreatedBy = Session["UserID"];
                NARRATION objdata = new NARRATION();
                objdata.NarrationID = _Narration.NarrationID;
                objdata.Name = _Narration.Name;
                objdata.Active = _Narration.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/NARRATIONMASTER?DBAction=Update&ID=" + id);
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
                    NARRATION item = new NARRATION();
                    item.Code = dr["Code"].ToString();
                    item.Message = dr["Message"].ToString();
                    if (item.Code == "200")
                    {
                        TempData["Message"] = item.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = item.Message;
                        return View();
                    }
                }
            }
            return RedirectToAction("Index");         
            }
        //public JsonResult IsUserExists(string Name)
        //   {

        //    var client = new RestClient(URL + "api/Master/NARRATIONMASTER?DBAction=View&ID=0");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Authorization", BasicAuth);
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddParameter("application/json", "{\r\n   \r\n   \"Name\" : \"TestNa\",\r\n   \"Active\": \"true\",\r\n   \"CreatedBy\" : \"173\"\r\n\r\n}", ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    Console.WriteLine(response.Content);
        //    return Json(!obj.GetAllNarration().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
        //   // return Json(!obj.GetAllNarration().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
        //    }

        }
}