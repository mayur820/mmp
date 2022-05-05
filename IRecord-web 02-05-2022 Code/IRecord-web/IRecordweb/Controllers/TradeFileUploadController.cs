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

namespace IRecordweb.Controllers
{
    public class TradeFileUploadController : Controller
    {
        // GET: TradeFileUpload
        DAL.TradeFileDAL obj = new DAL.TradeFileDAL();
        MType mtype = new MType();
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
       

        public ActionResult Index()
        {
            return View();
        }
        public List<MTYPE> BindInvenstmentType()
            {
            var ID = 1;
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MTYPEMASTER?DBAction=ViewById&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MTYPE> list = new List<MTYPE>();
            foreach (DataRow dr in data.Tables[0].Rows)
                {
                MTYPE item = new MTYPE();
                item.TypeId = Convert.ToInt32(dr["TypeId"].ToString());
                item.Code = dr["Code"].ToString();
                item.Name = dr["Name"].ToString();
                item.Active = Convert.ToBoolean(dr["Active"].ToString());
                list.Add(item);
                }
            return list;
            }
        public List<CONSULTANT> BindConsultantMaster()
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=BindConsultant&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<CONSULTANT> ConsultList = new List<CONSULTANT>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                CONSULTANT cobj = new CONSULTANT();
                cobj.ConsultantID = Convert.ToInt32(ds.Tables[0].Rows[i]["ConsultantID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                Session["ConsultantNM"] = cobj.Name;
                ConsultList.Add(cobj);
                }
            return ConsultList;
            }
        public List<DEMAT> BindDematMaster()
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=BindDemat&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);

            List<DEMAT> DematList = new List<DEMAT>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                DEMAT cobj = new DEMAT();
                cobj.DematID = Convert.ToInt32(ds.Tables[0].Rows[i]["DematID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                DematList.Add(cobj);
                }
            return DematList;
            }
        public List<ACCOUNT> BindBrokerData()
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=BindBroker&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> AccountList = new List<ACCOUNT>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                ACCOUNT cobj = new ACCOUNT();
                cobj.AccountId = Convert.ToInt32(ds.Tables[0].Rows[i]["AccountId"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                AccountList.Add(cobj);
                }
            return AccountList;
            }

        [HttpGet]
        public ActionResult SaveTradeFile()
            {
            ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.Consultant = new SelectList(BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            ViewBag.DematAC = new SelectList(BindDematMaster().ToList(), dataValueField: "DematID", dataTextField: "Name");
            ViewBag.Broker = new SelectList(BindBrokerData().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            return View();
            }
        [HttpPost]
        public ActionResult SaveTradeFile(TradeFiles _TradeFiles, HttpPostedFileBase FilePath ,string Import, string OK)
            {
            ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.Consultant = new SelectList(BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            ViewBag.DematAC = new SelectList(BindDematMaster().ToList(), dataValueField: "DematID", dataTextField: "Name");
            ViewBag.Broker = new SelectList(BindBrokerData().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            if (!string.IsNullOrEmpty(Import))
                {
                return View("");
                }
            else if (!string.IsNullOrEmpty(OK))
                {
                obj.SaveImport(FilePath);
                }
            return View();
            }

        public ActionResult OnboardingTrade()
            {
            return View();
            }


        }
}