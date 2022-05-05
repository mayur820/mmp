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
    //[Authorize]
    public class MutualFundCategoryController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: MutualFundCategory
        public ActionResult Index()
        {
            var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDCATEGORYMASTER?DBAction=ViewByUserId&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<FUNDCATEGORY> list = new List<FUNDCATEGORY>();
            foreach (DataRow dr in data.Tables[0].Rows)
                {
                FUNDCATEGORY item = new FUNDCATEGORY();
                item.MutualFundCategoryID = Convert.ToInt32(dr["MutualFundCategoryID"].ToString());
                item.Code = dr["Code"].ToString();
                item.Name = dr["Name"].ToString();
                item.Active = Convert.ToBoolean(dr["Active"].ToString());
                item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                list.Add(item);
                }
            return View(list);
            }
        [HttpGet]
        public ActionResult SaveMutualFundCat()
            {    
            return View();
            }
        [HttpPost]
        public ActionResult SaveMutualFundCat(FUNDCATEGORY _FundCategory)
            {
            if (ModelState.IsValid)
                {
                var CreatedBy = Session["UserID"];
                FUNDCATEGORY objdata = new FUNDCATEGORY();
                objdata.MutualFundCategoryID = _FundCategory.MutualFundCategoryID;
                objdata.Name = _FundCategory.Name;
                objdata.Active = _FundCategory.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/FUNDCATEGORYMASTER?DBAction=Insert&ID=0");
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
                    FUNDCATEGORY item = new FUNDCATEGORY();
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
        public ActionResult MutualFundCatDetails(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDCATEGORYMASTER?DBAction=ViewById&ID="+ id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            FundCategoryModel.Rootobject obj = new FundCategoryModel.Rootobject();
            obj = JsonConvert.DeserializeObject<FundCategoryModel.Rootobject>(response.Content.ToString());
            FUNDCATEGORY FundCategory = new FUNDCATEGORY();
            foreach (FundCategoryModel.Datum dr in obj.Data)
                {
                FundCategory.MutualFundCategoryID = Convert.ToInt32(dr.MutualFundCategoryID.ToString());
                FundCategory.Code = dr.Code.ToString();
                FundCategory.Name = dr.Name.ToString();
                FundCategory.Active = Convert.ToBoolean(dr.Active.ToString());
                FundCategory.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FundCatModel"] = FundCategory;
                }
            var data = ViewData["FundCatModel"];
            return View(data);
            }
        [HttpGet]
        public ActionResult MutualFundCatUpdate(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDCATEGORYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            FundCategoryModel.Rootobject obj = new FundCategoryModel.Rootobject();
            obj = JsonConvert.DeserializeObject<FundCategoryModel.Rootobject>(response.Content.ToString());
            FUNDCATEGORY FundCategory = new FUNDCATEGORY();
            foreach (FundCategoryModel.Datum dr in obj.Data)
                {
                FundCategory.MutualFundCategoryID = Convert.ToInt32(dr.MutualFundCategoryID.ToString());
                FundCategory.Code = dr.Code.ToString();
                FundCategory.Name = dr.Name.ToString();
                FundCategory.Active = Convert.ToBoolean(dr.Active.ToString());
                FundCategory.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FundCatModel"] = FundCategory;
                }
            var data = ViewData["FundCatModel"];
            return View(data);
            }
        [HttpPost]
        public ActionResult MutualFundCatUpdate(FUNDCATEGORY MFCat,  int id)
            {
            if(!string.IsNullOrWhiteSpace(MFCat.Name))
                {
                var CreatedBy = Session["UserID"];
                FUNDCATEGORY objdata = new FUNDCATEGORY();
                objdata.MutualFundCategoryID = id;
                objdata.Name = MFCat.Name;
                objdata.Active = MFCat.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/FUNDCATEGORYMASTER?DBAction=Update&ID="+ id);
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
        public ActionResult MutualFundCatDelete(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDCATEGORYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            FundCategoryModel.Rootobject obj = new FundCategoryModel.Rootobject();
            obj = JsonConvert.DeserializeObject<FundCategoryModel.Rootobject>(response.Content.ToString());
            FUNDCATEGORY FundCategory = new FUNDCATEGORY();
            foreach (FundCategoryModel.Datum dr in obj.Data)
                {
                FundCategory.MutualFundCategoryID = Convert.ToInt32(dr.MutualFundCategoryID.ToString());
                FundCategory.Code = dr.Code.ToString();
                FundCategory.Name = dr.Name.ToString();
                FundCategory.Active = Convert.ToBoolean(dr.Active.ToString());
                FundCategory.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FundCatModel"] = FundCategory;
                }
            var data = ViewData["FundCatModel"];
            return View(data);
            }
        [HttpPost]
        public ActionResult MutualFundCatDelete(FUNDCATEGORY MFCat, int id)
        {
            FUNDCATEGORY objdata = new FUNDCATEGORY();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FUNDCATEGORYMASTER?DBAction=Delete&ID=" + id);
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
            var client = new RestClient(URL + "api/Master/FUNDCATEGORYMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<FUNDCATEGORY> FundCatList = new List<FUNDCATEGORY>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FUNDCATEGORY cobj = new FUNDCATEGORY();
                cobj.MutualFundCategoryID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundCategoryID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                FundCatList.Add(cobj);
                }
            return Json(!FundCatList.Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            //  return Json(!obj.GetAllMFCatMaster().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            }
        }
}