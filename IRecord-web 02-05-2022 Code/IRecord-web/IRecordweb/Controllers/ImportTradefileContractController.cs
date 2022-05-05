using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace IRecordweb.Controllers
{
    public class ImportTradefileContractController : Controller
    {

        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        string PdfConfig = ConfigurationManager.AppSettings["PdfConfig"];
        // GET: ImportTradefileContract

        public JsonResult Get_Equity(string BrokerBill_Name_ID)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_TRADE_FILES_EQTY_FORMATE";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = BrokerBill_Name_ID;
            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
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
        public object DataTableToJSON(System.Data.DataTable table)
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
        public JsonResult Get_FNO(string BrokerBill_Name_ID)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_TRADE_FILES_F_N_O_FORMATE";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = BrokerBill_Name_ID;
            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
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

        public JsonResult Get_Info()
        {
            ImportTradefileContract_INDEX_Models objdata = Session["Session_Info"] as ImportTradefileContract_INDEX_Models;
            objdata.FileUpload = null;
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(objdata), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Index(ImportTradefileContract_INDEX_Models import)
        {//1
            ImportTradefileContract_INDEX_Models import1 = new ImportTradefileContract_INDEX_Models();
          //  ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.Consultant = new SelectList(BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            ViewBag.Broker = new SelectList(BindBrokerData1().ToList(), dataValueField: "ID", dataTextField: "Name");
            //  ViewBag.ContractNoteId = new SelectList(BindBrokerFormat1().ToList(), dataValueField: "Sr_No", dataTextField: "Name");
            ViewBag.Demat_Ac_Id = new SelectList(BindDematMaster(), dataValueField: "DematID", dataTextField: "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Index(ImportTradefileContract_INDEX_Models import, HttpPostedFileBase FileUpload)
        {//1
            ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.Consultant = new SelectList(BindConsultantMaster1().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            var List = BindBrokerData1().ToList();
            ViewBag.Broker = new SelectList(List, dataValueField: "ID", dataTextField: "Name");
            //  ViewBag.ContractNoteId = new SelectList(BindBrokerFormat1().ToList(), dataValueField: "Sr_No", dataTextField: "Name");
            ViewBag.Demat_Ac_Id = new SelectList(BindDematMaster(), dataValueField: "DematID", dataTextField: "Name");

            var brokerNameitems = List.Where(x => x.ID == Convert.ToInt32(import.Broker)).SingleOrDefault();
            ViewImportTradefile(import, FileUpload, brokerNameitems.Name);
            return RedirectToAction("ViewImportTradefileContract");
        }

        public List<ImportTradefileContract_INDEX_Models> ViewImportTradefile(ImportTradefileContract_INDEX_Models objdata, HttpPostedFileBase Fileupload,string BrokerName)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(Fileupload.InputStream))
            {
                bytes = br.ReadBytes(Fileupload.ContentLength);
            }
            string _FileName = Path.GetFileName(Fileupload.FileName);
            string fileextenion = Path.GetExtension(Fileupload.FileName);
            string ContentType = Fileupload.ContentType.ToString();
            ImportTradefileContract_INDEX_Models data = new ImportTradefileContract_INDEX_Models();
            //   data = Session["data"] as ImportTradefileContract_INDEX_Models;
            TradeFilesTrans.Rootobject objtrans = new TradeFilesTrans.Rootobject();
            objtrans.Filecode = objdata.ContractNoteId;
            objtrans.fileName = _FileName;
            objtrans.ContentType = ContentType;
            objtrans.fileextension = fileextenion;
            objtrans.pdfbytes = Convert.ToBase64String(bytes);
            objtrans.StartDate = objdata.FromDate;
            Session["StartDate"] = objtrans.StartDate;
            objtrans.EndDate = objdata.ToDate;
            Session["EndDate"] = objtrans.EndDate;
            objtrans.Consultant = objdata.Consultant;
            Session["Consultant"] = objtrans.Consultant.ToString();
            objdata.ConsultantName = GetConsultantName(Convert.ToInt32(objtrans.Consultant.ToString()));
            Session["ConsultantNM"] = objdata.ConsultantName;
            objtrans.Consultant = Session["ConsultantNM"].ToString();
            objtrans.ContractNoteName = Session["BrokerConfig"].ToString();
            Session["ContractNoteName"] = objtrans.ContractNoteName;
            objtrans.isTradeFile = Session["isTradeFile"].ToString();
            objtrans.Broker = objdata.Broker;
            objtrans.BrokerName = BrokerName;
            
            objtrans.Demat_Ac_Id = objdata.Demat_Ac_Id;
            objtrans.Demat_Ac_Name = GetDematName(Convert.ToInt32(objtrans.Demat_Ac_Id.ToString()));
            Session["DematName"] = objtrans.Demat_Ac_Name.ToString();
            objtrans.HoldingType = objdata.HoldingType;
            Session["HoldingType"] = objtrans.HoldingType;

            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(PdfConfig + "api/PdfProcess/Tradeconfig");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ASP.NET_SessionId=d34sb145avmh4dnowe1ms4xp");
            var body = Newtonsoft.Json.JsonConvert.SerializeObject(objtrans);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            List<ImportTradefileContract_INDEX_Models> list = new List<ImportTradefileContract_INDEX_Models>();
            DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(response.Content);
            Session["Dt_Header"] = myDataSet.Tables[0];
            Session["Dt_Details"] = myDataSet.Tables[1];
            Session["Dt_Expanses"] = myDataSet.Tables[2];
            Session["Session_Info"] = objdata;
            return list;
        }

        public string GetConsultantName(int ID)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=GetConsultantNM&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            string Name = "";
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                CONSULTANT item = new CONSULTANT();
                item.Name = dr["Name"].ToString();
                Name = item.Name;
            }
            return Name;
        }

        public int GetSubscriberID(int ID)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=GetSubscriberID&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            string Name = "";
            int Id = 0;
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                USER item = new USER();
                item.SubscriberID = Convert.ToInt32(dr["SubscriberID"].ToString());
                Id = item.SubscriberID;
            }
            return Id;
        }

        public string GetDematName(int ID)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=GetDematNM&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            string Name = "";
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                DEMAT item = new DEMAT();
                item.Name = dr["Name"].ToString();
                Name = item.Name;
            }
            return Name;
        }
        public List<BankStatement_Index_Model> GetBankFormat()
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/BANKFORMATMASTER?DBAction=ViewBank&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<BankStatement_Index_Model> list = new List<BankStatement_Index_Model>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                BankStatement_Index_Model item = new BankStatement_Index_Model();
                item.ID = Convert.ToInt32(dr["ID"].ToString());
                item.Name = dr["NAME"].ToString();

                list.Add(item);
            }
            return list;
        }
        public List<BankStatement_Index_Model> GetBankFormatConfig()
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/BANKFORMATMASTER?DBAction=ViewConfig&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<BankStatement_Index_Model> list = new List<BankStatement_Index_Model>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                BankStatement_Index_Model item = new BankStatement_Index_Model();
                item.ID = Convert.ToInt32(dr["ID"].ToString());
                item.Name = dr["NAME"].ToString();
                Session["ContractNote"] = item.Name;

                list.Add(item);
            }
            return list;
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
        public List<CONSULTANT> BindConsultantMaster1()
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
        public List<CONSULTANT> BindConsultantMaster()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //int ID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = GetSubscriberID(ID);
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_CONSULTANTDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ONBOARDCONSULTANT";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = MemberCode;

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
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
            List<CONSULTANT> ConsultList = new List<CONSULTANT>();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                CONSULTANT cobj = new CONSULTANT();
                cobj.ConsultantID = Convert.ToInt32(DT.Rows[i]["ID"].ToString());
                cobj.Name = DT.Rows[i]["NAME"].ToString();
                // Session["ConsultantNM"] = cobj.Name;
                ConsultList.Add(cobj);
            }
            return ConsultList;
        }

        public List<ACCOUNT> BindBrokerData()
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=BindBrokerData&ID=0");
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

        public List<ACCOUNTBROKER> BindBrokerData2()
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
        public List<ACCOUNTBROKER> BindBrokerData1()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_ACCOUNTDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ONBOARDBROKER";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@AccountId", SqlDbType.Int).Value = FinancialYearCode;

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
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
            List<ACCOUNTBROKER> AccountList = new List<ACCOUNTBROKER>();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                ACCOUNTBROKER cobj = new ACCOUNTBROKER();
                cobj.ID = Convert.ToInt32(DT.Rows[i]["ID"].ToString());
                cobj.Name = DT.Rows[i]["NAME"].ToString();
                // cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                AccountList.Add(cobj);
            }
            return AccountList;
        }

        public List<ImportTradefileContract_INDEX_Models> BindBrokerFormat(int ID)
        {

            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/BROKERFORMATMASTER?DBAction=BrokerBillList&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ImportTradefileContract_INDEX_Models> BrokerFormatList = new List<ImportTradefileContract_INDEX_Models>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ImportTradefileContract_INDEX_Models cobj = new ImportTradefileContract_INDEX_Models();
                cobj.BrokerId = Convert.ToInt32(ds.Tables[0].Rows[i]["Sr_No"].ToString());
                cobj.Broker = ds.Tables[0].Rows[i]["Name"].ToString();
                Session["BrokerConfig"] = cobj.Broker;
                cobj.FileType = ds.Tables[0].Rows[i]["FileType"].ToString();
                cobj.isTradeFile = ds.Tables[0].Rows[i]["isTradefile"].ToString();
                Session["isTradeFile"] = cobj.isTradeFile;
                BrokerFormatList.Add(cobj);
            }
            return BrokerFormatList;
        }

        public List<ImportTradefileContract_INDEX_Models> BindBrokerFormat1(int ID)
        {

            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/BROKERFORMATMASTER?DBAction=BrokerBillConfigList&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ImportTradefileContract_INDEX_Models> BrokerFormatList = new List<ImportTradefileContract_INDEX_Models>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ImportTradefileContract_INDEX_Models cobj = new ImportTradefileContract_INDEX_Models();
                cobj.BrokerId = Convert.ToInt32(ds.Tables[0].Rows[i]["Sr_No"].ToString());
                cobj.Broker = ds.Tables[0].Rows[i]["Name"].ToString();
                Session["BrokerConfig"] = cobj.Broker;
                Session["BrokerConfig"] = cobj.Broker;
                cobj.FileType = ds.Tables[0].Rows[i]["FileType"].ToString();
                cobj.isTradeFile = ds.Tables[0].Rows[i]["isTradefile"].ToString();
                Session["isTradeFile"] = cobj.isTradeFile;
                BrokerFormatList.Add(cobj);
            }
            return BrokerFormatList;
        }

        //public List<BankStatement_Index_Model> Viewbankdata(BankStatement_Index_Model objdata, HttpPostedFileBase Fileupload)
        //{
        //    byte[] bytes;
        //    using (BinaryReader br = new BinaryReader(Fileupload.InputStream))
        //    {
        //        bytes = br.ReadBytes(Fileupload.ContentLength);
        //    }
        //    string _FileName = Path.GetFileName(Fileupload.FileName);
        //    string fileextenion = Path.GetExtension(Fileupload.FileName);
        //    string ContentType = Fileupload.ContentType.ToString();
        //    BankStatementDataTrans.Rootobject datatrn = new BankStatementDataTrans.Rootobject();
        //    datatrn.Bankcode = objdata.BankNameId;
        //    datatrn.fileName = _FileName;
        //    datatrn.contentType = ContentType;
        //    datatrn.fileextension = fileextenion;
        //    datatrn.pdfbytes = Convert.ToBase64String(bytes);
        //    datatrn.StartDate = objdata.FromDate;
        //    datatrn.EndDate = objdata.ToDate;
        //    System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        //    var client = new RestClient("http://localhost:64918/api/PdfProcess/BankFormat");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Authorization", BasicAuth);
        //    request.AddHeader("Content-Type", "application/json");
        //    var body = Newtonsoft.Json.JsonConvert.SerializeObject(datatrn);
        //    request.AddParameter("application/json", body, ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    Console.WriteLine(response.Content);

        //    DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(response.Content);
        //    Session["datadetails"] = myDataSet.Tables[0];
        //    TempData["Datadetails"] = Session["datadetails"];
        //    //Session["Dt_Details"] = myDataSet.Tables[1];
        //    //Session["Dt_Expanses"] = myDataSet.Tables[2];
        //    List<BankStatement_Index_Model> list = new List<BankStatement_Index_Model>();
        //    foreach (DataRow dr in myDataSet.Tables[0].Rows)
        //    {
        //        BankStatement_Index_Model item = new BankStatement_Index_Model();
        //        item.Date = Convert.ToDateTime(dr["Date"].ToString());
        //        Session["Date"] = item.Accounts;
        //        item.Accounts = dr["Accounts"].ToString();
        //        Session["Accounts"] = item.Accounts;
        //        item.Accounts_Color = dr["Accounts_color"].ToString();
        //        Session["Accounts_Color"] = item.Accounts_Color;
        //        item.Cheque = dr["Cheque"].ToString();
        //        Session["Cheque"] = item.Cheque;
        //        item.Debit = dr["Debit"].ToString();
        //        Session["Debit"] = item.Debit;
        //        item.Credit = dr["Credit"].ToString();
        //        Session["Credit"] = item.Credit;
        //        item.Narration = dr["Narration"].ToString();
        //        Session["Narration"] = item.Narration;
        //        item.ACCode = dr["ACcode"].ToString();
        //        Session["ACcode"] = item.ACCode;
        //        list.Add(item);
        //    }
        //    BankStatement_Index_Model item1 = new BankStatement_Index_Model();
        //    item1.BankFormatList = list;
        //    var dtdata = item1.BankFormatList;
        //    TempData["datadt"] = dtdata;
        //    Session["Finaldata"] = dtdata;
        //    return list;

        //}

        public List<DEMAT> BindDematMaster1()
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
        public List<DEMAT> BindDematMaster()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //int ID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = GetSubscriberID(ID);
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_DEMATDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ONBOARDDEMAT";

            cmd.Parameters.Add("@Memberid", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = FinancialYearCode;

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
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

            List<DEMAT> DematList = new List<DEMAT>();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DEMAT cobj = new DEMAT();
                cobj.DematID = Convert.ToInt32(DT.Rows[i]["ID"].ToString());
                cobj.Name = DT.Rows[i]["NAME"].ToString();
                Session["DematName"] = cobj.Name;
                DematList.Add(cobj);
            }
                return DematList;
        }

        [HttpGet]
        public ActionResult ViewImportTradefileContract()
        {//2
            DataTable dt = new DataTable();
            dt = Session["Dt_Details"] as DataTable;
            TempData["FinalData"] = dt;
            return View(dt);
        }


        [HttpPost]
        public ActionResult ViewImportTradefileContract(string Save, string Add)
        {
            if (!string.IsNullOrEmpty(Save))
            {
                Savedata();
                ViewBag.Message = "Record Saved Successfully !";
                ImportTradefileContract_INDEX_Models import1 = new ImportTradefileContract_INDEX_Models();
                ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
                ViewBag.Consultant = new SelectList(BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
                ViewBag.Broker = new SelectList(BindBrokerData1().ToList(), dataValueField: "ID", dataTextField: "Name");
                //  ViewBag.ContractNoteId = new SelectList(BindBrokerFormat1().ToList(), dataValueField: "Sr_No", dataTextField: "Name");
                ViewBag.Demat_Ac_Id = new SelectList(BindDematMaster(), dataValueField: "DematID", dataTextField: "Name");
                return View("Index");
            }
            if (!string.IsNullOrEmpty(Add))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

        public void Savedata()
        {
            DataTable dtSessionTable = Session["Dt_Details"] as DataTable;
            List<ImportTradefileContract_INDEX_Models> list = new List<ImportTradefileContract_INDEX_Models>();
            list = Session["Dt_Details"] as List<ImportTradefileContract_INDEX_Models>;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            int TransactionId = GetTransNo();
            foreach (DataRow dr in dtSessionTable.Rows)
            {
                var CreatedBy = Session["UserID"];
                ImportTradefileContract_INDEX_Models item = new ImportTradefileContract_INDEX_Models();
                var MemberId = MemberCode;//Session["MemberID"];
                var FinancialYearID = FinancialYearCode;// Session["FinincialYearID"];
                item.MemberID = Convert.ToInt32(MemberId.ToString());
                item.FinancialYearID = Convert.ToInt32(FinancialYearID.ToString());
                item.BillNo = dr["bill_no"].ToString();
                item.TransDate = Convert.ToDateTime(dr["date"].ToString());
                item.TransType = dr["type"].ToString();
                item.AccountType = dr["HoldingTypecode"].ToString();

                //  item.Consultant = dr["consultant"].ToString();
                item.Consultant = Session["Consultant"].ToString();
                item.Quantity = Convert.ToInt32(dr["qty"].ToString());
                item.GrossRate = Convert.ToDouble(dr["gross_rate"].ToString());
                item.NetRate = Convert.ToDouble(dr["net_rate"].ToString());
                item.NetAmount = Convert.ToDouble(dr["NetAmount"].ToString());
                item.BrokerageAmt = Convert.ToDouble(dr["brok_amt"].ToString());
                item.BrokerageperUnit = Convert.ToDouble(dr["brok_rate"].ToString());
                item.isIntraDay = dr["IntraDay"].ToString();
                item.Script = dr["script_name"].ToString();
                item.ScriptID = GetScriptID(item.Script);
                item.BookType = "BB";
                if (item.isIntraDay == "true")
                    item.isIntraDay = "1";
                else
                    item.isIntraDay = "0";
                string Transtypedemat = "";
                string bktyp = "BB";

                if (item.TransType == "Bought")
                    Transtypedemat = "I";
                else
                    Transtypedemat = "O";
                //  item.TransType = Transtypedemat;
                item.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
                item.TransactionId = TransactionId;
                item.InvestmentType = "2";

                var json = new JavaScriptSerializer().Serialize(item);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/TRADEFILEMASTER?DBAction=Insert&ID=0");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
        }

        public int GetScriptID(string ScriptName)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=GetScriptID&ID=" + ScriptName);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            int ScriptID = 0;
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SCRIPT item = new SCRIPT();
                item.ScriptID = Convert.ToInt32(dr["ScriptID"].ToString());
                ScriptID = Convert.ToInt32(item.ScriptID.ToString());
            }
            return ScriptID;

        }
        public int GetTransNo()
        {
            var CreatedBy = Session["UserID"];
            int TransactionId = 0;
            ImportTradefileContract_INDEX_Models item = new ImportTradefileContract_INDEX_Models();
            item.BookType = "BB";
            item.Date = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
            item.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
            var json = new JavaScriptSerializer().Serialize(item);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var client = new RestClient(URL + "api/Master/TRADEFILEMASTER?DBAction=InsertTransNo&ID=0");
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
                ImportTradefileContract_INDEX_Models item1 = new ImportTradefileContract_INDEX_Models();
                item1.TransactionId = Convert.ToInt32(dr["Trans_No"].ToString());
                TransactionId = item1.TransactionId;

            }
            return TransactionId;
        }
        [HttpGet]
        public ActionResult ImportTradefileExpense()
        {//3
            return View();
        }

        public JsonResult GetBrokerFormatList(int TypeId, ImportTradefileContract_INDEX_Models import)
        {
            List<ImportTradefileContract_INDEX_Models> brokerdata = BindBrokerFormat(TypeId);
            return Json(brokerdata, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetBrokerFormatList1(int ID, ImportTradefileContract_INDEX_Models import)
        {
            List<ImportTradefileContract_INDEX_Models> brokerdata = BindBrokerFormat1(ID);
            return Json(brokerdata, JsonRequestBehavior.AllowGet);

        }
        public ActionResult OnboardingTrade()
        {
            ViewBag.Broker = new SelectList(BindBrokerData1().ToList(), dataValueField: "ID", dataTextField: "Name");
            ViewBag.Demat_Ac_Id = new SelectList(BindDematMaster(), dataValueField: "DematID", dataTextField: "Name");

            return View();
        }
        [HttpPost]
        public ActionResult OnboardingTrade(ImportTradefileContract_INDEX_Models import, HttpPostedFileBase Fileupload, string UPLOAD, string VIEW, string APPROVE, string SKIP)
        {
            ViewBag.Broker = new SelectList(BindBrokerData1().ToList(), dataValueField: "ID", dataTextField: "Name");
            ViewBag.Demat_Ac_Id = new SelectList(BindDematMaster(), dataValueField: "DematID", dataTextField: "Name");
            if ((!string.IsNullOrEmpty(UPLOAD)) && (Fileupload != null))
            {
                Session["FileUpload"] = Fileupload;
                Session["ContractNote"] = import.ContractNoteId;
                Session["DematID"] = import.Demat_Ac_Id;
                Session["Broker"] = import.Broker;
                ViewBag.Message = "File Uploaded Successfully!";
            }

            if (!string.IsNullOrEmpty(VIEW))
            {
                Fileupload = Session["FileUpload"] as HttpPostedFileBase;

                OnViewImportTradefile(import, Fileupload);
            }
            if (!string.IsNullOrEmpty(SKIP))
            {
                USER _User = new USER();
                _User.UserName = Session["ONborUserName"].ToString();
                _User.Password = Session["ONborPassword"].ToString();
                LoginController ng = new LoginController();

                ng.UserLogin(_User);


                FormsAuthentication.SetAuthCookie(_User.UserName, false);
                return Redirect("~/Home/Index");
            }
            if (!string.IsNullOrEmpty(APPROVE))
            {
                USER _User = new USER();
                _User.UserName = Session["ONborUserName"].ToString();
                _User.Password = Session["ONborPassword"].ToString();
                LoginController ng = new LoginController();

                ng.UserLogin(_User);


                FormsAuthentication.SetAuthCookie(_User.UserName, false);

                Fileupload = Session["FileUpload"] as HttpPostedFileBase;

                OnViewImportTradefile(import, Fileupload);

                Savedata();



                ViewBag.Message = "Login Successful !!";

                ViewBag.Message = "Record Saved Successfully !";
                return Redirect("~/ScriptMaster/ScriptMapping");
            }
            // return Redirect("~/Home/Index");
            return View();
        }

        public List<ImportTradefileContract_INDEX_Models> OnViewImportTradefile(ImportTradefileContract_INDEX_Models objdata, HttpPostedFileBase Fileupload)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(Fileupload.InputStream))
            {
                bytes = br.ReadBytes(Fileupload.ContentLength);
            }
            string _FileName = Path.GetFileName(Fileupload.FileName);
            string fileextenion = Path.GetExtension(Fileupload.FileName);
            string ContentType = Fileupload.ContentType.ToString();
            ImportTradefileContract_INDEX_Models data = new ImportTradefileContract_INDEX_Models();
            //   data = Session["data"] as ImportTradefileContract_INDEX_Models;
            TradeFilesTrans.Rootobject objtrans = new TradeFilesTrans.Rootobject();
            objtrans.Filecode = Session["ContractNote"].ToString();
            objtrans.fileName = _FileName;
            objtrans.ContentType = ContentType;
            objtrans.fileextension = fileextenion;
            objtrans.pdfbytes = Convert.ToBase64String(bytes);
            objtrans.ContractNoteName = Session["BrokerConfig"].ToString();
            Session["ContractNoteName"] = objtrans.ContractNoteName;
            objtrans.isTradeFile = Session["isTradeFile"].ToString();
            objtrans.Broker = Session["Broker"].ToString();
            objtrans.Demat_Ac_Id = Session["DematID"].ToString();
            objtrans.Demat_Ac_Name = GetDematName(Convert.ToInt32(objtrans.Demat_Ac_Id.ToString()));
            Session["DematName"] = objtrans.Demat_Ac_Name.ToString();

            objtrans.StartDate = objdata.FromDate;
            objtrans.EndDate = objdata.ToDate;
            objtrans.Consultant = "";
            Session["Consultant"] = "";
            //objdata.ConsultantName = GetConsultantName(Convert.ToInt32(objtrans.Consultant.ToString()));
            Session["ConsultantNM"] = "";
            objtrans.Consultant = Session["ConsultantNM"].ToString();
            objtrans.ContractNoteName = Session["BrokerConfig"].ToString();
            Session["ContractNoteName"] = objtrans.ContractNoteName;
            objtrans.isTradeFile = Session["isTradeFile"].ToString();
            objtrans.Broker = objdata.Broker;

            objtrans.HoldingType = "";
            Session["HoldingType"] = objtrans.HoldingType;

            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(PdfConfig + "api/PdfProcess/Tradeconfig");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ASP.NET_SessionId=d34sb145avmh4dnowe1ms4xp");
            var body = Newtonsoft.Json.JsonConvert.SerializeObject(objtrans);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            List<ImportTradefileContract_INDEX_Models> list = new List<ImportTradefileContract_INDEX_Models>();
            DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(response.Content);
            Session["Dt_Header"] = myDataSet.Tables[0];
            Session["Dt_Details"] = myDataSet.Tables[1];
            Session["Dt_Expanses"] = myDataSet.Tables[2];
            return list;
        }



    }
}