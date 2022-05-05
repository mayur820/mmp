using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL;
using IRecordweb.Models;
using System.Net;
using RestSharp;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using IRecordweb.ViewModel;

namespace IRecordweb.Controllers
{
    public class ReceiptPaymentEntryController : Controller
    {
        DAL.ReceiptPaymentDAL obj = new DAL.ReceiptPaymentDAL();
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: ReceiptPaymentEntry
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SaveReceiptPaymentEntry()
        {
            // ViewBag.TypeList = new SelectList(TypeList().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            //ViewBag.ModeList = new SelectList(ModeList().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            //ViewBag.Narration = new SelectList(obj.GetNarrationList().ToList(), dataValueField: "Code", dataTextField: "Name");
            // ViewBag.Account = new SelectList(GetAllAccountList().ToList(), dataValueField: "Code", dataTextField: "Name");
            return View();
        }
        [HttpPost]
        public ActionResult SaveReceiptPaymentEntry(RECEIPTPAYMENTENTRY _Receipt, string Save)
        {

            // ViewBag.TypeList = new SelectList(TypeList().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.ModeList = new SelectList(ModeList().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.Narration = new SelectList(obj.GetNarrationList().ToList(), dataValueField: "Code", dataTextField: "Name");
            // ViewBag.Account = new SelectList(GetAllAccountList().ToList(), dataValueField: "Code", dataTextField: "Name");

            RECEIPTPAYMENTENTRY Receipt = new RECEIPTPAYMENTENTRY();
            List<RECEIPTPAYMENTENTRY> list = new List<RECEIPTPAYMENTENTRY>();

            if (Session["ListofView"] != null)
            {
                Receipt = (Session["ListofView"] as RECEIPTPAYMENTENTRY);
                list = Receipt.ReceiptList;
            }
            if (ModelState.IsValid)
            {
                //var CreatedBy = Session["UserID"];

                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberId = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearMemberID = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();
                //var MemberId = 1;//Session["MemberID"];
                //var FinancialYearMemberID = 1;//Session["FinancialYearMemberID"];

                RECEIPTPAYMENTENTRY objdata = new RECEIPTPAYMENTENTRY();
                string cheqrefer = "";
                if (!string.IsNullOrEmpty(_Receipt.Cheque) || _Receipt.Cheque != null)
                {
                    cheqrefer = _Receipt.Cheque;
                }
                else if (!string.IsNullOrEmpty(_Receipt.refernce) || _Receipt.refernce != null)
                {
                    cheqrefer = _Receipt.refernce;
                }

                objdata.MemberID = Convert.ToInt32(MemberId.ToString());
                objdata.FinancialYearMemberID = Convert.ToInt32(FinancialYearMemberID.ToString());
                objdata.Date = _Receipt.Date;
                objdata.Mode = _Receipt.Mode;
                objdata.cashbankaccount = _Receipt.cashbankaccount;
                objdata.Typeid = _Receipt.Typeid;
                objdata.CashBank = _Receipt.CashBank;
                objdata.Cheque = cheqrefer;
                objdata.PaymentMode = _Receipt.PaymentMode;
                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                if (objdata.Mode == "0")
                {
                    objdata.Mode = "R";
                }
                else
                {
                    objdata.Mode = "P";
                }
                if (objdata.CashBank == "20")
                {
                    objdata.CashBank = "B";
                }
                else
                {
                    objdata.CashBank = "C";
                }
                if (objdata.CashBank != null && objdata.PaymentMode != null && objdata.Mode != null && objdata.cashbankaccount != null && objdata.Cheque != null)
                {
                    Session["Mode"] = objdata.Mode;
                    Session["Type"] = objdata.CashBank;
                    Session["PaymentMode"] = objdata.PaymentMode;
                    Session["cashbankaccount"] = objdata.cashbankaccount;
                    Session["Cheque"] = objdata.Cheque;
                }

                if (objdata.Mode == null || objdata.CashBank == null || objdata.PaymentMode == null || objdata.cashbankaccount == null || objdata.Cheque == null)
                {
                    // objdata.Mode = Session["Mode"].ToString();
                    objdata.Cheque = Session["Cheque"].ToString();
                    objdata.cashbankaccount = Session["cashbankaccount"].ToString();
                    objdata.PaymentMode = Session["PaymentMode"].ToString();
                }
                objdata.EntryID = Guid.NewGuid().ToString();

                objdata.Account = _Receipt.Account;
                string CashBank = objdata.cashbankaccount;
                string Acc = objdata.Account;
                objdata.CashBankName = GetAccountName(CashBank);
                objdata.AccountName = GetAccountName(Acc);
                objdata.amount = _Receipt.amount;
                objdata.Narration = _Receipt.Narration;
                objdata.BrokerType = "Bk";
                objdata.VoucherNo = GetGlobalTransactionNumber(objdata.BrokerType);
                objdata.SrNo = 1;
                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                list.Add(objdata);

                if (!string.IsNullOrEmpty(Save))
                {

                    var json = new JavaScriptSerializer().Serialize(objdata);
                    System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var client = new RestClient(URL + "api/Master/RECEIPTPAYMENTENTRYDATA?DBAction=Insert&ID=0");
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
                        RECEIPTPAYMENTENTRY item = new RECEIPTPAYMENTENTRY();
                        item.Code = dr["Code"].ToString();
                        item.Message = dr["Message"].ToString();
                        if (item.Code == "200")
                        {
                            ViewBag.Message = item.Message;
                            return View();
                        }
                        else
                        {
                            ViewBag.Message = item.Message;
                            return View();
                        }
                    }

                    objdata.SrNo = objdata.SrNo++;
                }

            }
            Receipt.ReceiptList = list;
            Session["ListofView"] = Receipt;
            TempData["ListofView"] = Session["ListofView"];
            return View(Receipt);
        }

        [HttpPost]
        public JsonResult FnSaveReceiptEntry(string JsonData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;
            var MemberId = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearMemberID = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            RECEIPTPAYMENTENTRY objdata = new RECEIPTPAYMENTENTRY();
            string cheqrefer = "";
            int SrNo = 1;
            int counter = 0;
            //objdata.BrokerType = "Bk";
            int VoucherNo = GetGlobalTransactionNumber(objdata.BrokerType);
            objdata.VoucherNo = VoucherNo;
            for (counter = 0; counter < dtJsonData.Rows.Count; counter++)
            {
                objdata.BrokerType = dtJsonData.Rows[counter]["Book_Type"].ToString();
                if (!string.IsNullOrEmpty(dtJsonData.Rows[counter]["txt_Cheque"].ToString()) || dtJsonData.Rows[counter]["txt_Cheque"].ToString() != null)
                {
                    cheqrefer = dtJsonData.Rows[counter]["txt_Cheque"].ToString();
                }
                else if (!string.IsNullOrEmpty(dtJsonData.Rows[counter]["txt_reference"].ToString()) || dtJsonData.Rows[counter]["txt_reference"].ToString() != null)
                {
                    cheqrefer = dtJsonData.Rows[counter]["txt_reference"].ToString();
                }
                objdata.Date = Convert.ToDateTime(dtJsonData.Rows[counter]["txtDate"].ToString());
                objdata.MemberID = Convert.ToInt32(MemberId.ToString());
                objdata.FinancialYearMemberID = Convert.ToInt32(FinancialYearMemberID.ToString());
                objdata.Mode = dtJsonData.Rows[counter]["Mode"].ToString();
                objdata.cashbankaccount = dtJsonData.Rows[counter]["ddlCashBankAccount"].ToString();
                objdata.CashBank = dtJsonData.Rows[counter]["ddlCashbanklist"].ToString();
                objdata.Cheque = cheqrefer.ToString();
                objdata.PaymentMode = dtJsonData.Rows[counter]["ddlPaymentMode"].ToString();
                objdata.EntryID = Guid.NewGuid().ToString();
                objdata.Account = dtJsonData.Rows[counter]["ddlAccount"].ToString();
                objdata.amount = dtJsonData.Rows[counter]["txt_amount"].ToString();
                objdata.Narration = dtJsonData.Rows[counter]["txt_Narration"].ToString();

                objdata.SrNo = SrNo;


                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                if (objdata.Mode == "0")
                {
                    objdata.Mode = "R";
                }
                else
                {
                    objdata.Mode = "P";
                }
                if (objdata.CashBank == "20")
                {
                    objdata.CashBank = "B";
                }
                else
                {
                    objdata.CashBank = "C";
                }
                if (objdata.CashBank != null && objdata.PaymentMode != null && objdata.Mode != null && objdata.cashbankaccount != null && objdata.Cheque != null)
                {
                    Session["Mode"] = objdata.Mode;
                    Session["Type"] = objdata.CashBank;
                    Session["PaymentMode"] = objdata.PaymentMode;
                    Session["cashbankaccount"] = objdata.cashbankaccount;
                    Session["Cheque"] = objdata.Cheque;
                }

                if (objdata.Mode == null || objdata.CashBank == null || objdata.PaymentMode == null || objdata.cashbankaccount == null || objdata.Cheque == null)
                {
                    objdata.Mode = Session["Mode"].ToString();
                    objdata.Cheque = Session["Cheque"].ToString();
                    objdata.cashbankaccount = Session["cashbankaccount"].ToString();
                    objdata.PaymentMode = Session["PaymentMode"].ToString();
                }
                var json = new JavaScriptSerializer().Serialize(objdata);

                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/RECEIPTPAYMENTENTRYDATA?DBAction=Insert&ID=0");
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
                    RECEIPTPAYMENTENTRY item = new RECEIPTPAYMENTENTRY();
                    item.Code = dr["Code"].ToString();
                    item.Message = dr["Message"].ToString();
                    if (item.Code == "200")
                    {
                        ViewBag.Message = item.Message;
                    }
                    else
                    {
                        ViewBag.Message = item.Message;
                    }
                    SrNo++;
                }

            }




            return Json("1", JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult FnUpdateReceiptEntry(string JsonData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;
            var MemberId = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearMemberID = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            RECEIPTPAYMENTENTRY objdata = new RECEIPTPAYMENTENTRY();
            string cheqrefer = "";
            int SrNo = 1;
            int counter = 0;
          
            //int VoucherNo = GetGlobalTransactionNumber(objdata.BrokerType);
            //objdata.VoucherNo = VoucherNo;
            for (counter = 0; counter < dtJsonData.Rows.Count; counter++)
            {
                objdata.BrokerType = dtJsonData.Rows[counter]["Book_Type"].ToString();
                if (!string.IsNullOrEmpty(dtJsonData.Rows[counter]["txt_Cheque"].ToString()) || dtJsonData.Rows[counter]["txt_Cheque"].ToString() != null)
                {
                    cheqrefer = dtJsonData.Rows[counter]["txt_Cheque"].ToString();
                }
                else if (!string.IsNullOrEmpty(dtJsonData.Rows[counter]["txt_reference"].ToString()) || dtJsonData.Rows[counter]["txt_reference"].ToString() != null)
                {
                    cheqrefer = dtJsonData.Rows[counter]["txt_reference"].ToString();
                }
                objdata.Date = Convert.ToDateTime(dtJsonData.Rows[counter]["txtDate"].ToString());
                objdata.MemberID = Convert.ToInt32(MemberId.ToString());
                objdata.FinancialYearMemberID = Convert.ToInt32(FinancialYearMemberID.ToString());
                objdata.Mode = dtJsonData.Rows[counter]["Mode"].ToString();
                objdata.cashbankaccount = dtJsonData.Rows[counter]["ddlCashBankAccount"].ToString();
                objdata.CashBank = dtJsonData.Rows[counter]["ddlCashbanklist"].ToString();
                objdata.Cheque = cheqrefer.ToString();
                objdata.PaymentMode = dtJsonData.Rows[counter]["ddlPaymentMode"].ToString();
                objdata.EntryID = Guid.NewGuid().ToString();
                objdata.Account = dtJsonData.Rows[counter]["ddlAccount"].ToString();
                objdata.amount = dtJsonData.Rows[counter]["txt_amount"].ToString();
                objdata.Narration = dtJsonData.Rows[counter]["txt_Narration"].ToString();

                objdata.SrNo = SrNo;


                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                if (objdata.Mode == "0")
                {
                    objdata.Mode = "R";
                }
                else
                {
                    objdata.Mode = "P";
                }
                if (objdata.CashBank == "20")
                {
                    objdata.CashBank = "B";
                }
                else
                {
                    objdata.CashBank = "C";
                }
                if (objdata.CashBank != null && objdata.PaymentMode != null && objdata.Mode != null && objdata.cashbankaccount != null && objdata.Cheque != null)
                {
                    Session["Mode"] = objdata.Mode;
                    Session["Type"] = objdata.CashBank;
                    Session["PaymentMode"] = objdata.PaymentMode;
                    Session["cashbankaccount"] = objdata.cashbankaccount;
                    Session["Cheque"] = objdata.Cheque;
                }

                if (objdata.Mode == null || objdata.CashBank == null || objdata.PaymentMode == null || objdata.cashbankaccount == null || objdata.Cheque == null)
                {
                    objdata.Mode = Session["Mode"].ToString();
                    objdata.Cheque = Session["Cheque"].ToString();
                    objdata.cashbankaccount = Session["cashbankaccount"].ToString();
                    objdata.PaymentMode = Session["PaymentMode"].ToString();
                }
                var json = new JavaScriptSerializer().Serialize(objdata);

                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/RECEIPTPAYMENTENTRYDATA?DBAction=Update&ID="+ dtJsonData.Rows[counter]["ID"].ToString());
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                //DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                //foreach (DataRow dr in data.Tables[0].Rows)
                //{
                //    RECEIPTPAYMENTENTRY item = new RECEIPTPAYMENTENTRY();
                //    item.Code = dr["Code"].ToString();
                //    item.Message = dr["Message"].ToString();
                //    if (item.Code == "200")
                //    {
                //        ViewBag.Message = item.Message;
                //    }
                //    else
                //    {
                //        ViewBag.Message = item.Message;
                //    }
                //    SrNo++;
                //}

            }




            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteHeadPart(string Trans_no)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_RECEIPTPAYMENTENTRYDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "DELETE";
            cmd.Parameters.Add("@Transno", SqlDbType.BigInt).Value = Trans_no;
            cmd.Parameters.Add("@DELETED_BY", SqlDbType.Int).Value = CreatedById;

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
            return Json("1", JsonRequestBehavior.AllowGet);

        }



        public JsonResult GetNarrationList()   //string Name
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_RECEIPTPAYMENTENTRYDATA";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "GETNARRATION";
            // cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;

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

        public JsonResult GetListofReceiptPay()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_RECEIPTPAYMENTENTRYDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWRECEIPTLIST";
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;

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
        public ActionResult ReceiptPaymentList()
        {
            return View();
        }

        public ActionResult ViewReceiptPay(string TrnsNo)
        {



            #region get header data from database
            DataTable dtheaderdb = GetDBhead(TrnsNo);
            DataTable dtDeatilsdb = GetDeatilsdb(TrnsNo);
            Session["dtheaderdb"] = dtheaderdb;
            Session["dtDeatilsdb"] = dtDeatilsdb;
            #endregion



            return View("SaveReceiptPaymentEntry");
        }
        public JsonResult Get_HeadLevel_Info()
        {
            DataTable dthead = Session["dtheaderdb"] as DataTable;
            return Json(DataTableToJSON(dthead), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_DeatilsLevel_Info()
        {
            DataTable DTDetails = Session["dtDeatilsdb"] as DataTable;
            return Json(DataTableToJSON(DTDetails), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit(string JsonData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;
            var MemberId = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearMemberID = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            RECEIPTPAYMENTENTRY objdata = new RECEIPTPAYMENTENTRY();
            string cheqrefer = "";
            int SrNo = 1;
            int counter = 0;
            objdata.BrokerType = "Bk";
            int VoucherNo = GetGlobalTransactionNumber(objdata.BrokerType);
            objdata.VoucherNo = VoucherNo;
            for (counter = 0; counter < dtJsonData.Rows.Count; counter++)
            {
                if (!string.IsNullOrEmpty(dtJsonData.Rows[counter]["txt_Cheque"].ToString()) || dtJsonData.Rows[counter]["txt_Cheque"].ToString() != null)
                {
                    cheqrefer = dtJsonData.Rows[counter]["txt_Cheque"].ToString();
                }
                else if (!string.IsNullOrEmpty(dtJsonData.Rows[counter]["txt_reference"].ToString()) || dtJsonData.Rows[counter]["txt_reference"].ToString() != null)
                {
                    cheqrefer = dtJsonData.Rows[counter]["txt_reference"].ToString();
                }
                objdata.Date = Convert.ToDateTime(dtJsonData.Rows[counter]["txtDate"].ToString());
                objdata.MemberID = Convert.ToInt32(MemberId.ToString());
                objdata.FinancialYearMemberID = Convert.ToInt32(FinancialYearMemberID.ToString());
                objdata.Mode = dtJsonData.Rows[counter]["Mode"].ToString();
                objdata.cashbankaccount = dtJsonData.Rows[counter]["ddlCashBankAccount"].ToString();
                objdata.CashBank = dtJsonData.Rows[counter]["ddlCashbanklist"].ToString();
                objdata.Cheque = cheqrefer.ToString();
                objdata.PaymentMode = dtJsonData.Rows[counter]["ddlPaymentMode"].ToString();
                objdata.EntryID = Guid.NewGuid().ToString();
                objdata.Account = dtJsonData.Rows[counter]["ddlAccount"].ToString();
                objdata.amount = dtJsonData.Rows[counter]["txt_amount"].ToString();
                objdata.Narration = dtJsonData.Rows[counter]["txt_Narration"].ToString();

                objdata.SrNo = SrNo;


                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                if (objdata.Mode == "0")
                {
                    objdata.Mode = "R";
                }
                else
                {
                    objdata.Mode = "P";
                }
                if (objdata.CashBank == "20")
                {
                    objdata.CashBank = "B";
                }
                else
                {
                    objdata.CashBank = "C";
                }
                if (objdata.CashBank != null && objdata.PaymentMode != null && objdata.Mode != null && objdata.cashbankaccount != null && objdata.Cheque != null)
                {
                    Session["Mode"] = objdata.Mode;
                    Session["Type"] = objdata.CashBank;
                    Session["PaymentMode"] = objdata.PaymentMode;
                    Session["cashbankaccount"] = objdata.cashbankaccount;
                    Session["Cheque"] = objdata.Cheque;
                }

                if (objdata.Mode == null || objdata.CashBank == null || objdata.PaymentMode == null || objdata.cashbankaccount == null || objdata.Cheque == null)
                {
                    objdata.Mode = Session["Mode"].ToString();
                    objdata.Cheque = Session["Cheque"].ToString();
                    objdata.cashbankaccount = Session["cashbankaccount"].ToString();
                    objdata.PaymentMode = Session["PaymentMode"].ToString();
                }
                var json = new JavaScriptSerializer().Serialize(objdata);

                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/RECEIPTPAYMENTENTRYDATA?DBAction=Insert&ID=0");
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
                    RECEIPTPAYMENTENTRY item = new RECEIPTPAYMENTENTRY();
                    item.Code = dr["Code"].ToString();
                    item.Message = dr["Message"].ToString();
                    if (item.Code == "200")
                    {
                        ViewBag.Message = item.Message;
                    }
                    else
                    {
                        ViewBag.Message = item.Message;
                    }
                    SrNo++;
                }

            }




            return Json("1", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Update(string ID)
        {
            RECEIPTPAYMENTENTRY dtt = new RECEIPTPAYMENTENTRY();
            dtt = Session["ListofView"] as RECEIPTPAYMENTENTRY;
            var data = (dtt.ReceiptList.Where(s => s.EntryID == ID).SingleOrDefault());
            return View("SaveReceiptPaymentEntry", data);
        }
        [HttpPost]
        public ActionResult Update()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete(string ID)
        {
            RECEIPTPAYMENTENTRY dtt = new RECEIPTPAYMENTENTRY();
            dtt = TempData["ListofView"] as RECEIPTPAYMENTENTRY;
            var data = (dtt.ReceiptList.Where(s => s.EntryID == ID).SingleOrDefault());
            return View("SaveReceiptPaymentEntry", data);
        }
        [HttpPost]
        public ActionResult Delete()
        {
            return View();
        }

        public List<MTYPE> TypeList()
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MTYPEMASTER?DBAction=GetReceiptPayList&ID=0");
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
                item.Name = dr["Name"].ToString();
                list.Add(item);
            }
            return list;
        }

        public List<MTYPE> ModeList()
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MTYPEMASTER?DBAction=GetReceiptPayList&ID=0");
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
                item.Name = dr["Name"].ToString();
                list.Add(item);
            }
            return list;
        }

        public List<ACCOUNT> GetAllAccountList()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberId = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearMemberID = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            ACCOUNT objdata = new ACCOUNT();
            objdata.MemberID = Convert.ToInt32(dtfin.Rows[0]["MemberId"].ToString());
            objdata.FinancialYearMemberID = Convert.ToInt32(dtfin.Rows[0]["FinancialYearUserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MTYPEMASTER?DBAction=ViewLedger&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> AccountList = new List<ACCOUNT>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ACCOUNT cobj = new ACCOUNT();
                cobj.Code = ds.Tables[0].Rows[i]["ID"].ToString();
                cobj.Name = ds.Tables[0].Rows[i]["NAME"].ToString();
                AccountList.Add(cobj);
            }
            return AccountList;
        }
        public JsonResult GetAllAccountList1()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_MTYPEDATA";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEWLEDGER";
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;


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
        public JsonResult TypeList1()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_MTYPEDATA";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "RECEIPTPAYLIST";

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
        public List<ACCOUNT> GetAccountByCashBank(string Name)
        {
            string Action = "";
            if (Name == "15")
            {
                Action = "ViewPRCashEntry";
            }
            else
            {
                Action = "ViewPRBankEntry";
            }
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberId = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearMemberID = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            ACCOUNT objdata = new ACCOUNT();
            objdata.MemberID = Convert.ToInt32(dtfin.Rows[0]["MemberId"].ToString());
            objdata.FinancialYearMemberID = Convert.ToInt32(dtfin.Rows[0]["FinancialYearUserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MTYPEMASTER?DBAction=" + Action + "&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> AccountList = new List<ACCOUNT>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ACCOUNT cobj = new ACCOUNT();
                cobj.Code = ds.Tables[0].Rows[i]["ID"].ToString();
                cobj.Name = ds.Tables[0].Rows[i]["NAME"].ToString();
                // Session["AName"] = cobj.Name.ToString();
                AccountList.Add(cobj);
            }
            return AccountList;
        }
        public List<MTYPE> GetPaymodeList(string Name)
        {
            string Action = "";
            if (Name == "15")
            {
                Action = "GetCashList";
            }
            else
            {
                Action = "GetPayModeList";
            }
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MTYPEMASTER?DBAction=" + Action + "&ID=0");
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
                item.Name = dr["Name"].ToString();
                list.Add(item);
            }
            return list;
        }
        public JsonResult GetAccountList(string Name)
        {
            //List<ACCOUNT> Accountdata = GetAccountByCashBank(Name).ToList();
            //return Json(Accountdata, JsonRequestBehavior.AllowGet);
            string Action = "";
            if (Name == "15")
            {
                Action = "VIEWRPCASHENTRY";
            }
            else
            {
                Action = "VIEWRPBANKENTRY";
            }
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_MTYPEDATA";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = Action;
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;

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
        public JsonResult GetPaymode(string Name)
        {
            //List<MTYPE> Accountdata = GetPaymodeList(Name).ToList();
            //return Json(Accountdata, JsonRequestBehavior.AllowGet);
            string Action = "";
            if (Name == "15")
            {
                Action = "CASHMODE";
            }
            else
            {
                Action = "PAYMENTMODE";
            }
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_MTYPEDATA";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = Action;
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;

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
        public DataTable GetDBhead(string Trans_no)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_RECEIPTPAYMENTENTRYDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWHEAD";
            cmd.Parameters.Add("@TransNo", SqlDbType.BigInt).Value = Trans_no;

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
            return DT;



        }
        public DataTable GetDeatilsdb(string Trans_no)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_RECEIPTPAYMENTENTRYDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWDETAILS";
            cmd.Parameters.Add("@TransNo", SqlDbType.BigInt).Value = Trans_no;

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
            return DT;



        }
        public string GetAccountName(string ID)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=ViewAccountName&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            //  request.AddParameter("application/json", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            // List<ACCOUNT> AccountList = new List<ACCOUNT>();
            string AcName = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ACCOUNT cobj = new ACCOUNT();
                // cobj.Code = ds.Tables[0].Rows[i]["ID"].ToString();
                cobj.Name = ds.Tables[0].Rows[i]["NAME"].ToString();
                AcName = cobj.Name;
                /// AccountList.Add(cobj);
            }
            return AcName;
        }
        public int GetGlobalTransactionNumber(string bktyp)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            con.Open();
            string dttod = DateTime.Today.ToString("yyyy-MM-dd");
            SqlCommand cmd43 = new SqlCommand("Insert into Global_no(Book_typ, Date, MemberID, FinancialYearMemberID, CreatedBy, CreatedDate) values('" + bktyp + "',getdate()," + MemberCode + "," + FinancialYearCode + "," + CreatedById + ",getdate());SELECT SCOPE_IDENTITY();", con);
            var vchno = cmd43.ExecuteScalar();
            return Convert.ToInt32(vchno);
        }

    }
}