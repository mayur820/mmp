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
    public class IndustryMasterController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: IndustryMaster
        public ActionResult Index()
        {
            //DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            //var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            //var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            //var CreatedById = Session["UserID"].ToString();
            //var ID = Session["UserID"];
            //System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=ViewByUserId&ID=" + CreatedById);
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", BasicAuth);
            //request.AddHeader("Content-Type", "application/json");
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            //List<INDUSTRY> list = new List<INDUSTRY>();
            //foreach (DataRow dr in data.Tables[0].Rows)
            //    {
            //    INDUSTRY item = new INDUSTRY();
            //    item.IndustryID = Convert.ToInt32(dr["IndustryID"].ToString());
            //    item.Code = dr["Code"].ToString();
            //    item.Name = dr["Name"].ToString();
            //    item.Active = Convert.ToBoolean(dr["Active"].ToString());
            //    item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

            //    list.Add(item);
            //    }
            return View();
        }

        public JsonResult GetListofIndustry()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_INDUSRTYDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedById;
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
        public ActionResult SaveIndustry()
            {   
            return View();
            }
        [HttpPost]
        public ActionResult SaveIndustry(INDUSTRY _Industry)
            {
            if (ModelState.IsValid)
                {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();
                INDUSTRY objdata = new INDUSTRY();
                objdata.IndustryID = _Industry.IndustryID;
                objdata.Name = _Industry.Name;
                objdata.Active = _Industry.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=Insert&ID=0");
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
                    INDUSTRY item = new INDUSTRY();
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
        public ActionResult IndustryDetails(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            IndustryModel.Rootobject obj = new IndustryModel.Rootobject();
            obj = JsonConvert.DeserializeObject<IndustryModel.Rootobject>(response.Content.ToString());
            INDUSTRY industry = new INDUSTRY();
            foreach (IndustryModel.Datum dr in obj.Data)
                {
                industry.IndustryID = Convert.ToInt32(dr.IndustryID.ToString());
                industry.Code = dr.Code.ToString();
                industry.Name = dr.Name.ToString();
                industry.Active = Convert.ToBoolean(dr.Active.ToString());
                industry.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["IndustryModel"] = industry;
                }
            var data = ViewData["IndustryModel"];
            return View(data);

            }
        [HttpGet]
        public ActionResult UpdateIndustry(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            IndustryModel.Rootobject obj = new IndustryModel.Rootobject();
            obj = JsonConvert.DeserializeObject<IndustryModel.Rootobject>(response.Content.ToString());
            INDUSTRY industry = new INDUSTRY();
            foreach (IndustryModel.Datum dr in obj.Data)
                {
                industry.IndustryID = Convert.ToInt32(dr.IndustryID.ToString());
                industry.Code = dr.Code.ToString();
                industry.Name = dr.Name.ToString();
                industry.Active = Convert.ToBoolean(dr.Active.ToString());
                industry.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["IndustryModel"] = industry;
                }

            var data = ViewData["IndustryModel"];
            return View(data);
            }
        [HttpPost]
        public ActionResult UpdateIndustry(INDUSTRY industry, int id)
            {
            if (!string.IsNullOrWhiteSpace(industry.Name))
                {
                var CreatedBy = Session["UserID"];
                INDUSTRY objdata = new INDUSTRY();
                objdata.IndustryID = id;
                objdata.Name = industry.Name;
                objdata.Active = industry.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=Update&ID=" + id);
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
                    INDUSTRY item = new INDUSTRY();
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
        public ActionResult DeleteIndustry(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            IndustryModel.Rootobject obj = new IndustryModel.Rootobject();
            obj = JsonConvert.DeserializeObject<IndustryModel.Rootobject>(response.Content.ToString());
            INDUSTRY industry = new INDUSTRY();
            foreach (IndustryModel.Datum dr in obj.Data)
                {
                industry.IndustryID = Convert.ToInt32(dr.IndustryID.ToString());
                industry.Code = dr.Code.ToString();
                industry.Name = dr.Name.ToString();
                industry.Active = Convert.ToBoolean(dr.Active.ToString());
                industry.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["IndustryModel"] = industry;
                }

            var data = ViewData["IndustryModel"];
            return View(data);
            }
        [HttpPost]
        public ActionResult DeleteIndustry(Industry industry, int id)
        {
            INDUSTRY objdata = new INDUSTRY();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var json = new JavaScriptSerializer().Serialize(objdata);
            var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=Delete&ID=" + id);
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
                INDUSTRY item = new INDUSTRY();
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

        public JsonResult IsUserExists(string Name)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<INDUSTRY> IndustryList = new List<INDUSTRY>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                INDUSTRY cobj = new INDUSTRY();
                cobj.IndustryID = Convert.ToInt32(ds.Tables[0].Rows[i]["IndustryID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IndustryList.Add(cobj);
                }
            return Json(!IndustryList.Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
           // return Json(!obj.GetAllIndustry().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            }
        }
}