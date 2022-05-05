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
    public class FinancialYearTransController : Controller
    {
        DAL.Master obj = new DAL.Master();
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: FinancialYearTrans
        public ActionResult Index()
        {
            var ID = Session["UserID"];
            //FinancialYear _FinYear = new FinancialYear();
            //_FinYear.ShowFinYear =  obj.GetFinYrByID();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FINANCIALYEARMASTER?DBAction=ViewByUserId&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<FINANCIALYEAR> list = new List<FINANCIALYEAR>();
            if (data.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    FINANCIALYEAR item = new FINANCIALYEAR();
                    item.FinancialYearID = Convert.ToInt32(dr["FinancialYearID"].ToString());
                    item.Code = dr["Code"].ToString();
                    item.FromDate = Convert.ToDateTime(dr["FromDate"].ToString());
                    item.ToDate = Convert.ToDateTime(dr["ToDate"].ToString());
                    item.Active = Convert.ToBoolean(dr["Active"].ToString());
                    item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                    list.Add(item);
                }
            }
            FINANCIALYEAR _FinYr = new FINANCIALYEAR();
            _FinYr.ShowFinYear = list;
            return View(_FinYr);
        }
        [HttpGet]
        public ActionResult SaveFinYear()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveFinYear(FINANCIALYEAR _FinYear)
        {
            var CreatedBy = Session["UserID"];
            FINANCIALYEAR objdata = new FINANCIALYEAR();
            objdata.FromDate = _FinYear.FromDate;
            objdata.ToDate = _FinYear.ToDate;
            objdata.Active = _FinYear.Active;
            objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FINANCIALYEARMASTER?DBAction=Insert&ID=0");
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
                FINANCIALYEAR item = new FINANCIALYEAR();
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

        public void FinancialYrByID(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FINANCIALYEARMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            FinancialYearModel.Rootobject obj = new FinancialYearModel.Rootobject();
            obj = JsonConvert.DeserializeObject<FinancialYearModel.Rootobject>(response.Content.ToString());
            FINANCIALYEAR FinYrr = new FINANCIALYEAR();
            foreach (FinancialYearModel.Datum dr in obj.Data)
            {
                FinYrr.FinancialYearID = Convert.ToInt32(dr.FinancialYearID.ToString());
                FinYrr.Code = dr.Code.ToString();
                FinYrr.FromDate = Convert.ToDateTime(dr.FromDate.ToString());
                FinYrr.ToDate = Convert.ToDateTime(dr.ToDate.ToString());
                FinYrr.Active = Convert.ToBoolean(dr.Active.ToString());
                FinYrr.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FinYearModel"] = FinYrr;
            }
        }
        [HttpGet]
        public ActionResult ViewFinYear(int id)
        {
            FinancialYrByID(id);
            var data = ViewData["FinYearModel"];
            return View(data);
        }
        [HttpGet]
        public ActionResult EditFinYear(int id)
        {
            FinancialYrByID(id);
            var data = ViewData["FinYearModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult EditFinYear(FINANCIALYEAR _FinYear, int id)
        {
            var CreatedBy = Session["UserID"];
            FINANCIALYEAR objdata = new FINANCIALYEAR();
            objdata.FromDate = _FinYear.FromDate;
            objdata.ToDate = _FinYear.ToDate;
            objdata.Active = _FinYear.Active;
            objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FINANCIALYEARMASTER?DBAction=Update&ID=" + id);
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
                FINANCIALYEAR item = new FINANCIALYEAR();
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
        [HttpGet]
        public ActionResult DeleteFinYear(int id)
        {
            FinancialYrByID(id);
            var data = ViewData["FinYearModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult DeleteFinYear(FINANCIALYEAR _FinYear, int id)
        {
            FINANCIALYEAR objdata = new FINANCIALYEAR();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FINANCIALYEARMASTER?DBAction=Delete&ID=" + id);
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
                FINANCIALYEAR item = new FINANCIALYEAR();
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