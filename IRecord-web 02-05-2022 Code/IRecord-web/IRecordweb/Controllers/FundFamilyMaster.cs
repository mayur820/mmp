using BAL;
using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class FundFamilyMasterController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: MutualFundMaster
        public ActionResult Index()
        {
            var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDFAMILYMASTER?DBAction=ViewByUserId&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<FUNDFAMILY> list = new List<FUNDFAMILY>();
            foreach (DataRow dr in data.Tables[0].Rows)
                {
                FUNDFAMILY item = new FUNDFAMILY();
                item.MutualFundID = Convert.ToInt32(dr["MutualFundID"].ToString());
                item.Code = dr["Code"].ToString();
                item.Name = dr["Name"].ToString();
                item.Active = Convert.ToBoolean(dr["Active"].ToString());
                item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                list.Add(item);
                }
            return View(list);
            }
        [HttpGet]
        public ActionResult SaveFundFamily()
            {        
            return View();
            }
        [HttpPost]
        public ActionResult SaveFundFamily(FUNDFAMILY _FundFamily)
            {
            if (ModelState.IsValid)
                {
                var CreatedBy = Session["UserID"];
                FUNDFAMILY objdata = new FUNDFAMILY();
                objdata.MutualFundID = _FundFamily.MutualFundID;
                objdata.Name = _FundFamily.Name;
                objdata.Active = _FundFamily.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/FUNDFAMILYMASTER?DBAction=Insert&ID=0");
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
                    FUNDFAMILY item = new FUNDFAMILY();
                    item.Code = dr["Code"].ToString();
                    item.Message = dr["Message"].ToString();
                    if (item.Code == "200")
                        {
                        ViewBag.Message = item.Message;
                        return RedirectToAction("Index");
                        }
                    else
                        {
                        ViewBag.Message = item.Message;
                        return View();
                        }
                    }
                }
            return RedirectToAction("Index");
            }
        [HttpGet]
        public ActionResult FundFamilyDetails(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDFAMILYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            FundFamilyModel.Rootobject obj = new FundFamilyModel.Rootobject();
            obj = JsonConvert.DeserializeObject<FundFamilyModel.Rootobject>(response.Content.ToString());
            FUNDFAMILY Fundfamily = new FUNDFAMILY();
            foreach (FundFamilyModel.Datum dr in obj.Data)
                {
                Fundfamily.MutualFundID = Convert.ToInt32(dr.MutualFundID.ToString());
                Fundfamily.Code = dr.Code.ToString();
                Fundfamily.Name = dr.Name.ToString();
                Fundfamily.Active = Convert.ToBoolean(dr.Active.ToString());
                Fundfamily.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FundFamilyModel"] = Fundfamily;
                }
            var data = ViewData["FundFamilyModel"];
            return View(data);
            }
        [HttpGet]
        public ActionResult UpdateFundFamily(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDFAMILYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            FundFamilyModel.Rootobject obj = new FundFamilyModel.Rootobject();
            obj = JsonConvert.DeserializeObject<FundFamilyModel.Rootobject>(response.Content.ToString());
            FUNDFAMILY Fundfamily = new FUNDFAMILY();
            foreach (FundFamilyModel.Datum dr in obj.Data)
                {
                Fundfamily.MutualFundID = Convert.ToInt32(dr.MutualFundID.ToString());
                Fundfamily.Code = dr.Code.ToString();
                Fundfamily.Name = dr.Name.ToString();
                Fundfamily.Active = Convert.ToBoolean(dr.Active.ToString());
                Fundfamily.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FundFamilyModel"] = Fundfamily;
                }
            var data = ViewData["FundFamilyModel"];
            return View(data);
            }
        [HttpPost]
        public ActionResult UpdateFundFamily(FUNDFAMILY fFamily, int id)
            {
            if (!string.IsNullOrWhiteSpace(fFamily.Name))
                {
                var CreatedBy = Session["UserID"];
                FUNDFAMILY objdata = new FUNDFAMILY();
                objdata.MutualFundID = id;
                objdata.Name = fFamily.Name;
                objdata.Active = fFamily.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/FUNDFAMILYMASTER?DBAction=Update&ID=" + id);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                }
            return RedirectToAction("Index");
            }
        [HttpGet]
        public ActionResult DeleteFundFamily(int id)
            {
            var client = new RestClient(URL + "api/Master/FUNDFAMILYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            FundFamilyModel.Rootobject obj = new FundFamilyModel.Rootobject();
            obj = JsonConvert.DeserializeObject<FundFamilyModel.Rootobject>(response.Content.ToString());
            FUNDFAMILY Fundfamily = new FUNDFAMILY();
            foreach (FundFamilyModel.Datum dr in obj.Data)
                {
                Fundfamily.MutualFundID = Convert.ToInt32(dr.MutualFundID.ToString());
                Fundfamily.Code = dr.Code.ToString();
                Fundfamily.Name = dr.Name.ToString();
                Fundfamily.Active = Convert.ToBoolean(dr.Active.ToString());
                Fundfamily.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FundFamilyModel"] = Fundfamily;
                }
            var data = ViewData["FundFamilyModel"];
            return View(data);
            }

        [HttpPost]
        public ActionResult DeleteFundFamily(FUNDFAMILY fFamily, int id)
        {
            FUNDFAMILY objdata = new FUNDFAMILY();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDFAMILYMASTER?DBAction=Delete&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return RedirectToAction("Index");
        }

        public JsonResult IsUserExists(string Name)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDFAMILYMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);

            List<FUNDFAMILY> FFamilyList = new List<FUNDFAMILY>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FUNDFAMILY cobj = new FUNDFAMILY();
                cobj.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                FFamilyList.Add(cobj);
                }
            return Json(!FFamilyList.Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            //return Json(!obj.GetAllFundFamilyMaster().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            }
        }
}