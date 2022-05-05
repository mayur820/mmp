using BAL;
using DAL;
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
    public class InvestmentMasterController : Controller
        {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: InvestmentMaster
        public ActionResult Index()
            {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INVESTMENTMASTER?DBAction=ViewByUserId&ID="+ MemberCode);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<INVESTMENT> list = new List<INVESTMENT>();
            foreach (DataRow dr in data.Tables[0].Rows)
                {
                INVESTMENT item = new INVESTMENT();
                item.InvestmentID = Convert.ToInt32(dr["InvestmentID"].ToString());
                item.Code = dr["Code"].ToString();
                item.Name = dr["Name"].ToString();
                item.Active = Convert.ToBoolean(dr["Active"].ToString());
                item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                list.Add(item);
                }
            return View(list);


            }
        [HttpGet]
        public ActionResult SaveInvestment()
            {
            return View();
            }
        [HttpPost]
        public ActionResult SaveInvestment(INVESTMENT _Investment)
            {
            if (ModelState.IsValid)
                {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();
                INVESTMENT objdata = new INVESTMENT();
                objdata.InvestmentID = _Investment.InvestmentID;
                try
                {
                    objdata.FinancialYearMemberID = Convert.ToInt32(FinancialYearCode.ToString());

                }
                catch {
                    objdata.FinancialYearMemberID =-1;
                }
               
                objdata.Name = _Investment.Name;
                objdata.Active = _Investment.Active;
                objdata.CreatedBy = Convert.ToInt32(MemberCode.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/INVESTMENTMASTER?DBAction=Insert&ID=0");
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
                    INVESTMENT item = new INVESTMENT();
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
        public ActionResult InvestmentDetails(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INVESTMENTMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            InvestmentModel.Rootobject obj = new InvestmentModel.Rootobject();
            obj = JsonConvert.DeserializeObject<InvestmentModel.Rootobject>(response.Content.ToString());
            INVESTMENT Investment = new INVESTMENT();
            foreach (InvestmentModel.Datum dr in obj.Data)
                {
                Investment.InvestmentID = Convert.ToInt32(dr.InvestmentID.ToString());
                Investment.Code = dr.Code.ToString();
                Investment.Name = dr.Name.ToString();
                Investment.Active = Convert.ToBoolean(dr.Active.ToString());
                Investment.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["InvestmentModel"] = Investment;
                }
            var data = ViewData["InvestmentModel"];
            return View(data);
            }

        [HttpGet]
        public ActionResult InvestmentEdit(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INVESTMENTMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            InvestmentModel.Rootobject obj = new InvestmentModel.Rootobject();
            obj = JsonConvert.DeserializeObject<InvestmentModel.Rootobject>(response.Content.ToString());
            INVESTMENT Investment = new INVESTMENT();
            foreach (InvestmentModel.Datum dr in obj.Data)
                {
                Investment.InvestmentID = Convert.ToInt32(dr.InvestmentID.ToString());
                Investment.Code = dr.Code.ToString();
                Investment.Name = dr.Name.ToString();
                Investment.Active = Convert.ToBoolean(dr.Active.ToString());
                Investment.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["InvestmentModel"] = Investment;
                }
            var data = ViewData["InvestmentModel"];
            return View(data);
            }

        [HttpPost]
        public ActionResult InvestmentEdit(INVESTMENT investment, int id)
            {
            if(!string.IsNullOrWhiteSpace(investment.Name))
                {
                var CreatedBy = Session["UserID"];
                INVESTMENT objdata = new INVESTMENT();
                objdata.InvestmentID = id;
                objdata.Name = investment.Name;
                objdata.Active = investment.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/INVESTMENTMASTER?DBAction=Update&ID=" + id);
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
        public ActionResult InvestmentDelete(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INVESTMENTMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            InvestmentModel.Rootobject obj = new InvestmentModel.Rootobject();
            obj = JsonConvert.DeserializeObject<InvestmentModel.Rootobject>(response.Content.ToString());
            INVESTMENT Investment = new INVESTMENT();
            foreach (InvestmentModel.Datum dr in obj.Data)
                {
                Investment.InvestmentID = Convert.ToInt32(dr.InvestmentID.ToString());
                Investment.Code = dr.Code.ToString();
                Investment.Name = dr.Name.ToString();
                Investment.Active = Convert.ToBoolean(dr.Active.ToString());
                Investment.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["InvestmentModel"] = Investment;
                }
            var data = ViewData["InvestmentModel"];
            return View(data);
            }
        [HttpPost]
        public ActionResult InvestmentDelete(INVESTMENT investment, int id)
        {
            INVESTMENT objdata = new INVESTMENT();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INVESTMENTMASTER?DBAction=Delete&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return RedirectToAction("Index");
        }
        public JsonResult IsUserExists(string Name)
            {
            //check if any of the UserName matches the MemberName specified in the Parameter using the ANY extension method.  
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INVESTMENTMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);

            List<INVESTMENT> InvestmentList = new List<INVESTMENT>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                INVESTMENT cobj = new INVESTMENT();
                cobj.InvestmentID = Convert.ToInt32(ds.Tables[0].Rows[i]["InvestmentID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                InvestmentList.Add(cobj);
                }
            return Json(!InvestmentList.Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            //  return Json(!obj.GetAllInvestment().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);

            }

        }
    }