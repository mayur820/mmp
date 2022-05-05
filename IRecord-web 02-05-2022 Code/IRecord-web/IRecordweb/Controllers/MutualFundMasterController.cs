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
    public class MutualFundMasterController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: MutualFundMaster
        public ActionResult Index()
        {
            var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MUTUALFUNDMASTER?DBAction=ViewByUserId&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MUTUALFUND> list = new List<MUTUALFUND>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                MUTUALFUND item = new MUTUALFUND();
                item.MutualFundID = Convert.ToInt32(dr["ScriptID"].ToString());
                item.NameOfScheme = dr["ScriptName"].ToString();
                item.SchemeCode = dr["BSECode"].ToString();
                item.InvestmentOption = dr["NSECode"].ToString();
                item.FundFamilyName = dr["MutualFundID"].ToString();
                item.Code = dr["FundType"].ToString();
                item.Active = Convert.ToBoolean(dr["Active"].ToString());
                item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                list.Add(item);
            }
            return View(list);
        }
        public List<FUNDFAMILY> FundFamilyList()
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
                item.Name = dr["Name"].ToString();
                item.Active = Convert.ToBoolean(dr["Active"].ToString());
                item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                list.Add(item);
            }
            return list;
        }

        [HttpGet]
        public ActionResult SaveMutualFund()
        {
            ViewBag.FundFamilyName = new SelectList(FundFamilyList().ToList(), dataValueField: "MutualFundID", dataTextField: "Name");
            return View();
        }
        [HttpPost]
        public ActionResult SaveMutualFund(MUTUALFUND _MutualFund)
        {
            ViewBag.FundFamilyName = new SelectList(FundFamilyList().ToList(), dataValueField: "MutualFundID", dataTextField: "Name");

            if (ModelState.IsValid)
            {
                var CreatedBy = Session["UserID"];
                var Investment = _MutualFund.InvestmentOption.ToString().Split('-');
                var InvestmentOption = Investment[0];
                MUTUALFUND objdata = new MUTUALFUND();
                objdata.NameOfScheme = _MutualFund.NameOfScheme;
                objdata.SchemeCode = _MutualFund.SchemeCode;
                objdata.InvestmentOption = InvestmentOption;
                objdata.InvestmentType = "4";
                objdata.Code = _MutualFund.Code;
                objdata.FundFamilyName = _MutualFund.FundFamilyName;
                objdata.FMutualFundID = _MutualFund.FMutualFundID;
                objdata.Active = _MutualFund.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/MUTUALFUNDMASTER?DBAction=Insert&ID=0");
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
                    MUTUALFUND item = new MUTUALFUND();
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
        public ActionResult MutualFundDetails(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MUTUALFUNDMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            MutualFundModel.Rootobject obj = new MutualFundModel.Rootobject();
            obj = JsonConvert.DeserializeObject<MutualFundModel.Rootobject>(response.Content.ToString());
            MUTUALFUND MFund = new MUTUALFUND();
            foreach (MutualFundModel.Datum dr in obj.Data)
            {
                MFund.MutualFundID = Convert.ToInt32(dr.ScriptID.ToString());
                MFund.SchemeCode = dr.BSECode.ToString();
                MFund.InvestmentOption = dr.NSECode.ToString();
                MFund.NameOfScheme = dr.ScriptName.ToString();
                if (dr.FundType != null)
                {
                    MFund.Code = dr.FundType.ToString();
                }
                MFund.FundFamilyName = dr.MutualFundID.ToString();
                MFund.Active = Convert.ToBoolean(dr.Active.ToString());
                MFund.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["MutualFundModel"] = MFund;
            }
            var data = ViewData["MutualFundModel"];
            return View(data);
        }
        [HttpGet]
        public ActionResult MutualFundDelete(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MUTUALFUNDMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            MutualFundModel.Rootobject obj = new MutualFundModel.Rootobject();
            obj = JsonConvert.DeserializeObject<MutualFundModel.Rootobject>(response.Content.ToString());
            MUTUALFUND MFund = new MUTUALFUND();
            foreach (MutualFundModel.Datum dr in obj.Data)
            {
                MFund.MutualFundID = Convert.ToInt32(dr.ScriptID.ToString());
                MFund.SchemeCode = dr.BSECode.ToString();
                MFund.InvestmentOption = dr.NSECode.ToString();
                MFund.NameOfScheme = dr.ScriptName.ToString();
                if (dr.FundType != null)
                {
                    MFund.Code = dr.FundType.ToString();
                }
                MFund.FundFamilyName = dr.MutualFundID.ToString();
                MFund.Active = Convert.ToBoolean(dr.Active.ToString());
                MFund.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["MutualFundModel"] = MFund;
            }
            var data = ViewData["MutualFundModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult MutualFundDelete(MUTUALFUND _MFund, int id)
        {
            MUTUALFUND objdata = new MUTUALFUND();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MUTUALFUNDMASTER?DBAction=Delete&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult MutualFundUpdate(int id)
        {
            ViewBag.FundFamilyName = new SelectList(FundFamilyList().ToList(), dataValueField: "MutualFundID", dataTextField: "Name");
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MUTUALFUNDMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            MutualFundModel.Rootobject obj = new MutualFundModel.Rootobject();
            obj = JsonConvert.DeserializeObject<MutualFundModel.Rootobject>(response.Content.ToString());
            MUTUALFUND MFund = new MUTUALFUND();
            foreach (MutualFundModel.Datum dr in obj.Data)
            {
                MFund.MutualFundID = Convert.ToInt32(dr.ScriptID.ToString());
                MFund.SchemeCode = dr.BSECode.ToString();
                MFund.InvestmentOption = dr.NSECode.ToString();
                MFund.NameOfScheme = dr.ScriptName.ToString();
                if (dr.FundType != null)
                {
                    MFund.Code = dr.FundType.ToString();
                }
                MFund.FundFamilyName = dr.MutualFundID.ToString();
                MFund.Active = Convert.ToBoolean(dr.Active.ToString());
                MFund.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["MutualFundModel"] = MFund;
            }
            var data = ViewData["MutualFundModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult MutualFundUpdate(MUTUALFUND _MutualFund, int id)
        {
            ViewBag.FundFamilyName = new SelectList(FundFamilyList().ToList(), dataValueField: "MutualFundID", dataTextField: "Name");
            if (!string.IsNullOrWhiteSpace(_MutualFund.NameOfScheme))
            {
                var CreatedBy = Session["UserID"];
                var Investment = _MutualFund.InvestmentOption.ToString().Split('-');
                var InvestmentOption = Investment[0];
                MUTUALFUND objdata = new MUTUALFUND();
                objdata.MutualFundID = id;
                objdata.NameOfScheme = _MutualFund.NameOfScheme;
                objdata.SchemeCode = _MutualFund.SchemeCode;
                objdata.InvestmentOption = InvestmentOption;
                objdata.InvestmentType = "4";
                objdata.Code = _MutualFund.Code;
                objdata.FundFamilyName = _MutualFund.FundFamilyName;
                objdata.FMutualFundID = _MutualFund.FMutualFundID;
                objdata.Active = _MutualFund.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/MUTUALFUNDMASTER?DBAction=Update&ID=" + id);
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

        public JsonResult IsUserExists(string NameOfScheme)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MUTUALFUNDMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MUTUALFUND> MFList = new List<MUTUALFUND>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                MUTUALFUND cobj = new MUTUALFUND();
                cobj.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.NameOfScheme = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                cobj.SchemeCode = ds.Tables[0].Rows[i]["BSECode"].ToString();
                cobj.InvestmentOption = ds.Tables[0].Rows[i]["NSECode"].ToString();
                cobj.FundFamilyName = ds.Tables[0].Rows[i]["MutualFundID"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["FundType"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                MFList.Add(cobj);
            }
            return Json(!MFList.Any(x => x.NameOfScheme == NameOfScheme), JsonRequestBehavior.AllowGet);
            //return Json(!obj.GetAllMutualFund().Any(x => x.NameOfScheme == NameOfScheme), JsonRequestBehavior.AllowGet);
        }


    }
}