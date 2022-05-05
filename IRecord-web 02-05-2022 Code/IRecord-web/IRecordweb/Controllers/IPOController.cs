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
    public class IPOController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: IPO
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult SaveIPOData()
        {
            ViewBag.Consultant = new SelectList(BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            ViewBag.Broker = new SelectList(BindBrokerDataList().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            ViewBag.Demat = new SelectList(BindDematMaster(), dataValueField: "DematID", dataTextField: "Name");
            ViewBag.ScriptID = new SelectList(BindScriptMaster(), dataValueField: "ScriptID", dataTextField: "Name");
            ViewBag.Bank = new SelectList(BindBankDataList().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            //
           
            return View();
        }
        [HttpPost]
        public ActionResult SaveIPOData(IPO _IPO)
        {
            ViewBag.Consultant = new SelectList(BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            ViewBag.Broker = new SelectList(BindBrokerDataList().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            ViewBag.Demat = new SelectList(BindDematMaster(), dataValueField: "DematID", dataTextField: "Name");
            ViewBag.ScriptID = new SelectList(BindScriptMaster(), dataValueField: "ScriptID", dataTextField: "Name");
            ViewBag.Bank = new SelectList(BindBankDataList().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            IPO item = new IPO();
            var MemberID = (Session["MemberID"] != null ? Session["MemberID"].ToString() : "-1");
            var FinincialYearID = (Session["FinincialYearID"] != null ? Session["FinincialYearID"].ToString() : "-1");
            var CreatedBy = Session["UserID"];
            string bktyp = "BK";
            int TransNo = GetTransNo(bktyp);
            item.TransactionId = TransNo;
            item.ApplicationNo = _IPO.ApplicationNo;
            item.TransDate = _IPO.TransDate;
            item.MemberId = MemberID.ToString();
            item.FinancialYrId = FinincialYearID.ToString();
            item.Broker = _IPO.Broker;
            item.ScriptID = _IPO.ScriptID;
            item.Qty = _IPO.Qty;
            item.Rate = _IPO.Rate;
            item.Amount = _IPO.Amount;
            item.AllotedDate = _IPO.AllotedDate;
            item.AllotedQty = _IPO.AllotedQty;
            item.AllotedRate = _IPO.AllotedRate;
            item.AllotedAmt = _IPO.AllotedAmt;
            item.Bank = _IPO.Bank;
            item.Consultant = _IPO.Consultant;
            item.Demat = _IPO.Demat;
            item.BalQty = _IPO.AllotedQty;
            item.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
            var json = new JavaScriptSerializer().Serialize(item);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/IPOData?DBAction=Insert&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            SaveAcTrans(_IPO, TransNo);
            ViewBag.Message = "Record Saved Successfully!";
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            List<IPO> list = new List<IPO>();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                string query = "Select Top 1 Trans_no,Application_no,Qty,Rate,Amount from [IPO] Order by Trans_no Desc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            IPO item1 = new IPO();
                            item1.ApplicationNo = Convert.ToString(sdr["Application_no"]);
                            item1.TransactionId = Convert.ToInt32(sdr["Trans_no"]);
                            item1.Qty = Convert.ToInt32(sdr["Qty"]);
                            item1.Rate = Convert.ToInt64(sdr["Rate"]);
                            item1.Amount = Convert.ToInt32(sdr["Amount"]);
                            list.Add(item1);
                        }
                    }
                    con.Close();
                }
            }
            IPO _ipo = new IPO();
            _ipo.Showipo = list;
            return View(_ipo);
        }

        public void SaveAcTrans(IPO _IPO, int TransNo)
        {
            IPO item = new IPO();
            var MemberID = (Session["MemberID"] != null ? Session["MemberID"].ToString() : "-1");
            var FinincialYearID = (Session["FinincialYearID"] != null ? Session["FinincialYearID"].ToString() : "-1");
            var CreatedBy = Session["UserID"];
            string bktyp = "BK";
            //  int TransNo = GetTransNo(bktyp);
            item.TransactionId = TransNo;
            item.ApplicationNo = _IPO.ApplicationNo;
            item.TransDate = _IPO.TransDate;
            item.MemberId = MemberID.ToString();
            item.FinancialYrId = FinincialYearID.ToString();
            item.Broker = _IPO.Broker;
            item.ScriptID = _IPO.ScriptID;
            item.Qty = _IPO.Qty;
            item.Rate = _IPO.Rate;
            item.Amount = _IPO.Amount;
            item.AllotedDate = _IPO.AllotedDate;
            item.AllotedQty = _IPO.AllotedQty;
            item.AllotedRate = _IPO.AllotedRate;
            item.AllotedAmt = _IPO.AllotedAmt;
            item.Bank = _IPO.Bank;
            item.Consultant = _IPO.Consultant;
            item.Demat = _IPO.Demat;
            item.BalQty = _IPO.AllotedQty;
            item.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
            var json = new JavaScriptSerializer().Serialize(item);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/IPOData?DBAction=InsertAcTransEntry&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public int GetTransNo(string BookType)
        {
            var CreatedBy = Session["UserID"];
            int TransactionId = 0;
            IPO item = new IPO();
            item.BookType = BookType;
            item.Date = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
            item.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
            var json = new JavaScriptSerializer().Serialize(item);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var client = new RestClient(URL + "api/Master/IPODATA?DBAction=InsertTransNo&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                IPO item1 = new IPO();
                item1.TransactionId = Convert.ToInt32(dr["Trans_No"].ToString());
                TransactionId = item1.TransactionId;

            }
            return TransactionId;
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
                // Session["ConsultantNM"] = cobj.Name;
                ConsultList.Add(cobj);
            }
            return ConsultList;
        }
        public List<ACCOUNT> BindBrokerDataList()

        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=BindBrokerList&ID=0");
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
                // cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                AccountList.Add(cobj);
            }
            return AccountList;
        }

        public List<ACCOUNT> BindBankDataList()

        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=BindBankList&ID=0");
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
                // cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                AccountList.Add(cobj);
            }
            return AccountList;
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
                // cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                AccountList.Add(cobj);
            }
            return AccountList;
        }
        public List<ACCOUNTBROKER> BindBrokerData1()
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=BindBrokerData&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNTBROKER> AccountList = new List<ACCOUNTBROKER>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ACCOUNTBROKER cobj = new ACCOUNTBROKER();
                cobj.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                // cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                AccountList.Add(cobj);
            }
            return AccountList;
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
                Session["DematName"] = cobj.Name;
                DematList.Add(cobj);
            }
            return DematList;
        }
        public List<SCRIPT> BindScriptMaster()
        {
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);

            List<SCRIPT> ScriptList = new List<SCRIPT>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                SCRIPT cobj = new SCRIPT();
                cobj.ScriptID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                Session["ScriptName"] = cobj.Name;
                ScriptList.Add(cobj);
            }
            return ScriptList;
        }
    }
}