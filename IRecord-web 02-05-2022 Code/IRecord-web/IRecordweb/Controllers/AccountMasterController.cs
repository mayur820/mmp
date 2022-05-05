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
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class AccountMasterController : Controller
    {
        private readonly string URL = ConfigurationManager.AppSettings["ScreenURL"];
        private readonly string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: AccountMaster
        public ActionResult Index1()
        {
            return View();

        }
        public ActionResult Index()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            // var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=ViewByUserId&ID=" + MemberCode + ":" + FinancialYearCode)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> list = new List<ACCOUNT>();
            if (data != null)
            {
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    ACCOUNT item = new ACCOUNT
                    {
                        AccountId = Convert.ToInt32(dr["AccountId"].ToString()),
                        Name = dr["Name"].ToString(),
                        //item.AccountName = dr["AccountName"].ToString();
                        Address = dr["Address"].ToString(),
                        Emailid = dr["Emailid"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        Telephone = dr["Telephone"].ToString(),
                        AadharCardNo = dr["AadharCardNo"].ToString(),
                        PAN = dr["PAN"].ToString(),
                        GroupID = dr["GroupID"].ToString(),
                        GroupName = dr["GroupName"].ToString(),
                        //if (item.GroupID != 0)
                        //{
                        //    GroupName(item.GroupID);
                        //    if (Session["Group_Name"].ToString() != null)
                        //    {
                        //        item.GroupName = Session["Group_Name"].ToString();
                        //    }
                        //}
                        OpeningBalance = Convert.ToDouble(dr["OpeningBalance"].ToString()),
                        // item.OpeningCal = dr["OpeningCal"].ToString();
                        Contactperson = dr["Contactperson"].ToString(),
                        Active = Convert.ToBoolean(dr["Active"].ToString()),
                        CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString()),
                        //InternalEntry = dr["InternalEntry"].ToString()
                    };
                    list.Add(item);
                }
            }
            ACCOUNT _account = new ACCOUNT
            {
                ShowAccount = list
            };

            return View(_account);

        }
        public List<ACCOUNT> GroupList1()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            //  var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=GetGroupList&ID=" + MemberCode)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> list = new List<ACCOUNT>();
            if (data != null)
            {
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    ACCOUNT item = new ACCOUNT
                    {
                        GroupID = dr["GroupID"].ToString(),
                        GroupName = dr["Group_Name"].ToString()
                    };
                    Session["GroupName"] = item.GroupName.ToString();
                    //item.UGroupID = Convert.ToInt32(dr["UGroupID"].ToString());
                    Session["UGroupID"] = item.UGroupID;
                    list.Add(item);

                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<ACCOUNT> AllBrokerList(string GroupID)
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
            cmd.CommandText = "[DBO].[SP_DRPDATA]";

            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ViewAccountMasterdataData";
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;
            cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = GroupID;

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
            List<ACCOUNT> list = new List<ACCOUNT>();
            foreach (DataRow dr in DT.Rows)
            {
                ACCOUNT item = new ACCOUNT
                {
                    ID = Convert.ToInt32(dr["ID"].ToString()),
                    Name = dr["NAME"].ToString()
                };
                list.Add(item);

            }
            Session["ListOfAcMaster"] = list;

            return list;
        }

        public List<ACCOUNT> AllBrokerList1()
        {
            object ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=BindBrokerData&ID=0")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> list = new List<ACCOUNT>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                ACCOUNT item = new ACCOUNT
                {
                    ID = Convert.ToInt32(dr["ID"].ToString()),
                    Name = dr["NAME"].ToString()
                };
                list.Add(item);

            }
            return list;
        }
        public List<ACCOUNT> OnboardBrokerList()
        {
            object ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=OnboardBroker&ID=0")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> list = new List<ACCOUNT>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                ACCOUNT item = new ACCOUNT
                {
                    ID = Convert.ToInt32(dr["ID"].ToString()),
                    Name = dr["NAME"].ToString()
                };
                list.Add(item);

            }
            return list;
        }
        public List<ACCOUNT> GroupList()
        {
            object ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=GetGroupList&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> list = new List<ACCOUNT>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                ACCOUNT item = new ACCOUNT
                {
                    GroupID = dr["GroupID"].ToString(),
                    GroupName = dr["Group_Name"].ToString()
                };
                Session["GroupName"] = item.GroupName.ToString();
                item.UGroupID = Convert.ToInt32(dr["UGroupID"].ToString());
                Session["UGroupID"] = item.UGroupID;
                list.Add(item);

            }
            return list;
        }
        public List<ACCOUNT> AccountName(string ID)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=GetAccountName&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> list = new List<ACCOUNT>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                ACCOUNT item = new ACCOUNT
                {
                    AccountName = dr["NAME"].ToString()
                };
                Session["AccountName"] = item.AccountName;
                list.Add(item);
            }
            return list;
        }

        public List<ACCOUNT> BankName(string ID)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=GetBankName&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> list = new List<ACCOUNT>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                ACCOUNT item = new ACCOUNT
                {
                    AccountName = dr["Description"].ToString()
                };
                Session["BankName"] = item.AccountName;
                list.Add(item);
            }
            return list;
        }
        public List<GROUP> GroupName(string ID)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=GetGroupName&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<GROUP> list = new List<GROUP>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                GROUP item = new GROUP
                {
                    Group_Name = dr["Group_Name"].ToString()
                };
                Session["Group_Name"] = item.Group_Name;
                list.Add(item);
            }
            return list;
        }

        public List<GROUP> UGroupName()
        {
            int ID = Convert.ToInt32(Session["UGroupID"].ToString());
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=GetUGroupName&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<GROUP> list = new List<GROUP>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                GROUP item = new GROUP
                {
                    UGroupName = dr["GroupName"].ToString()
                };
                Session["UGroupName"] = item.UGroupName;
                list.Add(item);
            }
            return list;
        }
        public JsonResult GetFormatList(string GroupID)
        {
            List<ACCOUNT> brokerdata = AllBrokerList(GroupID.Replace("A", "").Replace("U", ""));
            return Json(brokerdata, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SaveAccountMaster(string VarAction)
        {
            ACCOUNT _Account = new ACCOUNT();
            // ViewBag.Name = new SelectList(AllBrokerList().ToList(), dataValueField: "ID", dataTextField: "Name");
            ViewBag.GroupID = new SelectList(GroupList1().ToList(), dataValueField: "GroupID", dataTextField: "GroupName");

            if (VarAction == "WithOutLayout")
            {
                TempData["WithOutLayout"] = "1";
                ViewBag.masterpage = "~/Views/Shared/_LayoutWithOutMenu.cshtml";
                return View("SaveAccountMaster", null);

            }
            else
            {
                ViewBag.masterpage = "~/Views/Shared/_Layout.cshtml";
                return View();
            }
        }
        public string SetDefaultString(object idata)
        {
            if (idata == null)
            {
                return "";
            }
            else
            {
                return idata.ToString();
            }
        }
        [HttpPost]
        public ActionResult SaveAccountMaster(ACCOUNT _Account)
        {
            //  ViewBag.Name = new SelectList(AllBrokerList().ToList(), dataValueField: "ID", dataTextField: "Name");
            //  ViewBag.GroupID = new SelectList(, dataValueField: "GroupID", dataTextField: "GroupName");

            if (ModelState.IsValid || ModelState.IsValid == false)
            {
                List<ACCOUNT> Groupitems = GroupList1().ToList();
                ACCOUNT Getgroupname = Groupitems.FirstOrDefault(x => x.GroupID == _Account.GroupID);
                List<ACCOUNT> listofac = Session["ListOfAcMaster"] as List<ACCOUNT>;

                if (_Account.GroupID.Contains("A"))
                {
                    _Account.SuperAdminID = 0;
                }
                else
                {
                    _Account.SuperAdminID = 0;
                }



                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                string CreatedById = Session["UserID"].ToString();
                ACCOUNT objdata = new ACCOUNT();
                if (_Account.GroupID.Contains("A"))
                {
                    ACCOUNT Accountname = listofac.FirstOrDefault(x => x.ID == Convert.ToInt32(_Account.Name));
                    objdata.Name = Accountname.Name.ToString();
                }
                else
                {
                    objdata.Name = _Account.txt_name.ToString();

                }

                //if (Convert.ToInt32(objdata.Name) < 5)
                //{
                //    AccountName(objdata.Name);
                //    objdata.Name = Session["AccountName"].ToString();
                //}
                //else
                //{
                //    BankName(objdata.Name);
                //    objdata.Name = Session["BankName"].ToString();
                //}

                objdata.Address = SetDefaultString(_Account.Address);
                objdata.Mobile = SetDefaultString(_Account.Mobile).ToString();
                objdata.MemberID = Convert.ToInt32(MemberCode.ToString());
                objdata.FinancialYearMemberID = Convert.ToInt32(FinancialYearCode.ToString());
                objdata.Emailid = SetDefaultString(_Account.Emailid).ToString();
                objdata.Telephone = SetDefaultString(_Account.Telephone).ToString();
                objdata.AadharCardNo = SetDefaultString(_Account.AadharCardNo).ToString();
                objdata.GSTIN = SetDefaultString(_Account.GSTIN).ToString();
                objdata.Contactperson = SetDefaultString(_Account.Contactperson).ToString();
                objdata.GroupName = Getgroupname.GroupName.ToString();
                objdata.SuperAdminID = Convert.ToInt32(_Account.Name);
                //UGroupName();
                //objdata.UGroupName = Session["UGroupName"].ToString();
                objdata.GroupID = _Account.GroupID.Replace("A", "").Replace("U", "").ToString();
                objdata.OpeningBalance = Convert.ToDouble(_Account.OpeningBalance.ToString());
                //  objdata.OpeningCal = _Account.OpeningCal.ToString();
                objdata.PAN = SetDefaultString(_Account.PAN).ToString();
                objdata.Active = _Account.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedById);
                string json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=Insert&ID=0")
                {
                    Timeout = -1
                };
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    ACCOUNT item = new ACCOUNT
                    {
                        Code = dr["Code"].ToString(),
                        Message = dr["Message"].ToString()
                    };
                    if (item.Code == "200")
                    {
                        ViewBag.Message = item.Message;
                        TempData["Message"] = item.Message;
                        ///////////New Case by ankit///
                        if (TempData["WithOutLayout"] != null)
                        {
                            return RedirectToAction("SaveAccountMaster", new { VarAction = "WithOutLayout" });
                        }
                        else
                        {
                            return RedirectToAction("Index");

                        }



                    }
                    else
                    {
                        ViewBag.Message = item.Message;
                        TempData["Message"] = item.Message;
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.Message = "Failed !!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detailaccount(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=ViewById&ID=" + id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            AccountModel.Rootobject obj = new AccountModel.Rootobject();
            obj = JsonConvert.DeserializeObject<AccountModel.Rootobject>(response.Content.ToString());
            ACCOUNT account = new ACCOUNT();
            foreach (AccountModel.Datum dr in obj.Data)
            {
                account.AccountId = Convert.ToInt32(dr.AccountId.ToString());
                account.Name = SetDefaultString(dr.Name);
                account.AccountName = SetDefaultString(dr.AccountName);
                account.Address = SetDefaultString(dr.Address);
                account.Mobile = SetDefaultString(dr.Mobile);
                account.Emailid = SetDefaultString(dr.Emailid);
                account.Contactperson = SetDefaultString(dr.Contactperson);
                account.Telephone = SetDefaultString(dr.Telephone);
                account.AadharCardNo = SetDefaultString(dr.AadharCardNo);
                account.GSTIN = SetDefaultString(dr.GSTIN);
                account.GroupID = dr.GroupID.ToString();
                if (account.GroupID != "")
                {
                    GroupName(account.GroupID);
                    if (Session["Group_Name"].ToString() != null)
                    {
                        account.GroupName = Session["Group_Name"].ToString();
                    }
                }
                account.OpeningBalance = Convert.ToDouble(dr.OpeningBalance.ToString());
                //  account.OpeningCal = dr.OpeningCal.ToString();
                account.PAN = SetDefaultString(dr.PAN);
                account.Active = Convert.ToBoolean(dr.Active.ToString());
                account.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["AccountModel"] = account;
            }

            object data = ViewData["AccountModel"];
            return View(data);

        }

        [HttpGet]
        public ActionResult EditAccount(int id)
        {
            // ViewBag.Name = new SelectList(AllBrokerList1().ToList(), dataValueField: "ID", dataTextField: "NAME");
            //ViewBag.AccountName = new SelectList(GroupList().ToList(), dataValueField: "GroupID", dataTextField: "GroupName");
            ViewBag.GroupID = new SelectList(GroupList1().ToList(), "GroupID", "GroupName", id.ToString());
            ViewBag.DefaultBrand = id.ToString();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=ViewById&ID=" + id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            AccountModel.Rootobject obj = new AccountModel.Rootobject();
            obj = JsonConvert.DeserializeObject<AccountModel.Rootobject>(response.Content.ToString());
            ACCOUNT account = new ACCOUNT();
            foreach (AccountModel.Datum dr in obj.Data)
            {
                account.AccountId = Convert.ToInt32(dr.AccountId.ToString());
                account.Name = SetDefaultString(dr.Name);
                //  account.AccountName = dr.AccountName.ToString();
                account.Address = SetDefaultString(dr.Address);
                account.Mobile = SetDefaultString(dr.Mobile);
                account.Emailid = SetDefaultString(dr.Emailid);
                account.GroupID = SetDefaultString(dr.GroupID);
                account.UGroupName = SetDefaultString(dr.UGroupName);
                account.GroupName = SetDefaultString(dr.GroupName);
                account.Contactperson = SetDefaultString(dr.Contactperson);
                account.Telephone = SetDefaultString(dr.Telephone);
                account.AadharCardNo = SetDefaultString(dr.AadharCardNo);
                account.GSTIN = SetDefaultString(dr.GSTIN);
                account.OpeningBalance = Convert.ToDouble(dr.OpeningBalance.ToString());
                // account.OpeningCal = dr.OpeningCal.ToString();
                account.PAN = SetDefaultString(dr.PAN);
                account.Active = Convert.ToBoolean(dr.Active.ToString());
                account.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["AccountModel"] = account;
            }

            object data = ViewData["AccountModel"];
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(ACCOUNT _Account, int id)
        {
            //  ModelState.Remove(_Account.GSTIN);
            if (ModelState.IsValid == true || ModelState.IsValid == false)
            {
                // ViewBag.Name = new SelectList(AllBrokerList1().ToList(), dataValueField: "ID", dataTextField: "NAME");
                //ViewBag.AccountName = new SelectList(GroupList().ToList(), dataValueField: "GroupID", dataTextField: "GroupName", id.ToString());
                //ViewBag.GroupID = new SelectList(GroupList1().ToList(), "GroupID", "GroupName", id.ToString());

                //ViewBag.DefaultBrand = id.ToString();
                object CreatedBy = Session["UserID"];
                ACCOUNT objdata = new ACCOUNT
                {
                    AccountId = 0,
                    Name = "".ToString()
                };
                //if (Convert.ToInt32(objdata.Name) < 5)
                //{
                //    AccountName(objdata.Name);
                //    objdata.Name = Session["AccountName"].ToString();
                //}
                //else
                //{
                //    BankName(objdata.Name);
                //    objdata.Name = Session["BankName"].ToString();
                //}
                objdata.Name = "";
                objdata.AccountName = "".ToString();
                objdata.Address = SetDefaultString(_Account.Address).ToString();
                objdata.Mobile = SetDefaultString(_Account.Mobile).ToString();
                objdata.Emailid = SetDefaultString(_Account.Emailid).ToString();
                objdata.Telephone = SetDefaultString(_Account.Telephone).ToString();
                //GroupList1();
                objdata.GroupName = "".ToString();
                //UGroupName();
                objdata.UGroupName = "".ToString();
                objdata.GroupID = "".ToString();
                objdata.Contactperson = SetDefaultString(_Account.Contactperson).ToString();
                //Session["Contactperson"] = objdata.Contactperson;
                objdata.AadharCardNo = SetDefaultString(_Account.AadharCardNo).ToString();
                objdata.GSTIN = SetDefaultString(_Account.GSTIN).ToString();
                objdata.OpeningBalance = Convert.ToDouble(_Account.OpeningBalance.ToString());
                //   objdata.OpeningCal = _Account.OpeningCal.ToString();
                objdata.PAN = SetDefaultString(_Account.PAN).ToString();
                objdata.Active = _Account.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                string json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=Update&ID=" + id)
                {
                    Timeout = -1
                };
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    ACCOUNT item = new ACCOUNT
                    {
                        Code = dr["Code"].ToString(),
                        Message = dr["Message"].ToString()
                    };
                    if (item.Code == "200")
                    {
                        ViewBag.Message = item.Message;
                        TempData["Message"] = item.Message;
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        TempData["Message"] = "Failed !!";
                        return View();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public JsonResult GetListofAccount()
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
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@AccountId", SqlDbType.Int).Value = FinancialYearCode;

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
        public ActionResult DeleteAccount(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=ViewById&ID=" + id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            AccountModel.Rootobject obj = new AccountModel.Rootobject();
            obj = JsonConvert.DeserializeObject<AccountModel.Rootobject>(response.Content.ToString());
            ACCOUNT account = new ACCOUNT();
            foreach (AccountModel.Datum dr in obj.Data)
            {
                account.Name = dr.Name.ToString();
                // var MemberId = (Session["MemberID"] != null ? Session["MemberID"].ToString() : "-1").ToString()
                account.Address = SetDefaultString(dr.Address);
                account.Mobile = SetDefaultString(dr.Mobile);
                account.Emailid = SetDefaultString(dr.Emailid);
                account.Telephone = SetDefaultString(dr.Telephone);
                account.GroupID = SetDefaultString(dr.GroupID);
                if (account.GroupID != "")
                {
                    GroupName(account.GroupID);
                    if (Session["Group_Name"].ToString() != null)
                    {
                        account.GroupName = Session["Group_Name"].ToString();
                    }
                }
                account.AadharCardNo = SetDefaultString(dr.AadharCardNo);
                account.GSTIN = SetDefaultString(dr.GSTIN);
                account.Contactperson = SetDefaultString(dr.Contactperson);
                account.OpeningBalance = Convert.ToDouble(dr.OpeningBalance.ToString());
                account.PAN = SetDefaultString(dr.PAN);
                account.Active = Convert.ToBoolean(dr.Active.ToString());
                account.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["AccountModel"] = account;


            }

            object data = ViewData["AccountModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult DeleteAccount(ACCOUNT _Account, int id)
        {
            ACCOUNT objdata = new ACCOUNT();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=Delete&ID=" + id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            string Message = response.Content;
            if (Message.IndexOf("Error|") == 0)
            {
                string[] M = Message.Split('|');
                if (M.Length > 0)
                {
                    ViewBag.Message = M[1].ToString();
                    TempData["Message"] = M[1].ToString();
                    return RedirectToAction("Index");
                }
            }
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                ACCOUNT item = new ACCOUNT
                {
                    Code = dr["Code"].ToString(),
                    Message = dr["Message"].ToString()
                };
                if (item.Code == "200")
                {
                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["Message"] = "Failed !!";
                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult FamliySearch(string Prefix)
        {
            object ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=ViewByUserId&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<FamilyModel> list = new List<FamilyModel>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                FamilyModel item = new FamilyModel
                {
                    FamilyID = Convert.ToInt32(dr["FamilyID"].ToString()),
                    Code = dr["Code"].ToString(),
                    Name = dr["Name"].ToString(),
                    Active = Convert.ToBoolean(dr["Active"].ToString()),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString())
                };

                list.Add(item);
            }
            var Name = (from N in list
                        where N.Name.StartsWith(Prefix)
                        select new { N.Name });
            return Json(Name, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsUserExistsAccount(string Name)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=View&ID=0")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ACCOUNT> AccountList = new List<ACCOUNT>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ACCOUNT cobj = new ACCOUNT
                {
                    AccountId = Convert.ToInt32(ds.Tables[0].Rows[i]["AccountId"].ToString()),
                    Name = ds.Tables[0].Rows[i]["Name"].ToString(),
                    Address = ds.Tables[0].Rows[i]["Address"].ToString(),
                    Emailid = ds.Tables[0].Rows[i]["Emailid"].ToString(),
                    AadharCardNo = ds.Tables[0].Rows[i]["AadharCardNo"].ToString(),
                    PAN = ds.Tables[0].Rows[i]["PAN"].ToString(),
                    Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString()),
                    CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString())
                };
                AccountList.Add(cobj);
            }

            return Json(!AccountList.Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
        }
    }
}