
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRecordweb.Models;
using BAL;
using DAL;
using RestSharp;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Net;
using System.Data.SqlClient;
using System.Web.Security;

namespace IRecordweb.Controllers
{
    public class SubscriberMasterController : Controller
    {
        DAL.SubscriberDAL obj = new SubscriberDAL();
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: SubscriberMaster
        public ActionResult Index()
        {
            return View();
        }

        public List<ROLE> BindRole()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
           System.Security.Cryptography.X509Certificates.X509Chain chain,
           System.Net.Security.SslPolicyErrors sslPolicyErrors)
{
    return true; // **** Always accept
};
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/ROLEMASTER?DBAction=GetRole&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ROLE> list = new List<ROLE>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                ROLE item = new ROLE();
                item.RoleId = Convert.ToInt32(dr["RoleId"].ToString());
                item.Name = dr["Name"].ToString();

                list.Add(item);
            }
            return list;
        }
        [HttpGet]
        public ActionResult ShowData(Subscriber __subscriber)
        {
            // obj.GetSubscriberMaster(__subscriber);
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            FormsAuthentication.SignOut();
            ViewBag.RoleId = new SelectList(BindRole().ToList(), dataValueField: "RoleId", dataTextField: "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Register(SUBSCRIBER _subscriber)
        {
            ViewBag.RoleId = new SelectList(BindRole().ToList(), dataValueField: "RoleId", dataTextField: "Name");
            SUBSCRIBER _obj = new SUBSCRIBER();

            if (ModelState.IsValid)
            {

                _subscriber.Action = "Register";
                _obj = InsertSubscriberMaster(_subscriber);
                if (_obj.Code == "200")
                {
                    _subscriber.EmailID = _obj.EmailID;
                    Session["EmailID"] = _obj.EmailID;
                    _subscriber.EmailOTP = Session["EmailOTP"].ToString();
                    _subscriber.MobileOTP = Session["MobileOTP"].ToString();
                    Models.EmailOTP otpser = new Models.EmailOTP();
                    otpser.Mail(_subscriber);
                    otpser.SMS_MobileOTP(_subscriber.MobileNo, _subscriber.MobileOTP, _subscriber.SubscriberName);
                    //   otpser.UserNmPassMail(_subscriber);
                    ViewBag.Message = _obj.Message;
                    Session["SubscriberName"] = _subscriber.SubscriberName;
                    TempData["SubscriberName"] = Session["SubscriberName"];
                    Session["MobileNo"] = _subscriber.MobileNo;
                    TempData["MobileNo"] = Session["MobileNo"];
                    Session["LoginUserName"] = _obj.EmailID;
                    return RedirectToAction("RegisterOTP");

                }
                else
                {
                    ViewBag.Message = _obj.Message;
                    return View();
                }
            }
            else
            {
                return View();
            }

        }
        public SUBSCRIBER InsertSubscriberMaster(SUBSCRIBER _subscriber)
        {

            SUBSCRIBER objdata = new SUBSCRIBER();
            objdata.SubscriberName = _subscriber.SubscriberName;
            objdata.EmailID = _subscriber.EmailID;
            objdata.MobileNo = _subscriber.MobileNo;
            Session["MobileNo"] = objdata.MobileNo;
            if (Session["MobileNo"] != null)
            {
                TempData["MobileNo"] = Session["MobileNo"];
            }
            objdata.RoleId = _subscriber.RoleId;
            Session["RoleId"] = objdata.RoleId;
            objdata.EmailOTP = _subscriber.EmailOTP;
            objdata.MobileOTP = _subscriber.MobileOTP;

            objdata.MemberSubscription = _subscriber.MemberSubscription;
            if (objdata.MemberSubscription.ToString() != null)
            {
                Session["onMemberSubscription"] = objdata.MemberSubscription;
                TempData["onMemberSubscription"] = Session["onMemberSubscription"].ToString();
            }
            objdata.CreatePassword = _subscriber.CreatePassword;
            objdata.SubscriberID = _subscriber.SubscriberID;
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SUBSCRIBERMASTER?DBAction=" + _subscriber.Action + "&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SUBSCRIBER item = new SUBSCRIBER();
                objdata.Code = dr["Code"].ToString();
                Session["Code"] = objdata.Code;
                objdata.Message = dr["Message"].ToString();
                Session["Message"] = objdata.Message;
                if (objdata.Code == "200")
                {
                    objdata.SubscriberID = Convert.ToInt32(dr["SubscriberID"].ToString());
                    Session["SubscriberID"] = objdata.SubscriberID;
                    Session["UserID"] = objdata.SubscriberID;
                    objdata.EmailID = dr["EmailID"].ToString();
                    Session["EmailID"] = objdata.EmailID;
                    objdata.MobileNo = dr["MobileNo"].ToString();
                    Session["MobileNo"] = objdata.MobileNo;
                    if (objdata.EmailID != "")
                    {
                        Session["ONborUserName"] = objdata.EmailID;
                    }
                    objdata.EmailOTP = dr["EmailOTP"].ToString();
                    Session["EmailOTP"] = objdata.EmailOTP;
                    Session["MobileOTP"] = dr["MobileOtp"].ToString();
                    objdata.MobileOTP = dr["MobileOtp"].ToString();
                    objdata.CreatePassword = dr["Password"].ToString();
                    if (objdata.CreatePassword != "")
                    {
                        Session["ONborPassword"] = objdata.CreatePassword;
                    }
                   
                   
                    if (dr["MemberCount"].ToString() != "")
                    {
                        objdata.MemberCount = Convert.ToInt32(dr["MemberCount"].ToString());
                        Session["onMemberCount"] = objdata.MemberCount;
                        TempData["onMemberCount"] = Session["onMemberCount"].ToString();
                    }
                    ViewBag.Message = objdata.Message;

                }
                else
                {
                    ViewBag.Message = objdata.Message;

                }
            }
            return objdata;
        }
        [HttpGet]
        public ActionResult RegisterOTP()
        {
            return View();
        }
        public void Insertsub(string subid)
        {


            // string date = obj.FromDate;
            //return View();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[ALGEBRA].[SP_SUB_INS_Irecord_User_Rights_Master]";
            cmd.Parameters.Add("@Sub_id", SqlDbType.Int).Value = subid;

            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                string query = cmd.CommandText;
                foreach (SqlParameter p in cmd.Parameters)
                {
                    query += " " + p.ParameterName + "=" + p.Value.ToString() + "\n";
                }
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

        }
        [HttpPost]
        public ActionResult RegisterOTP(string MobileOTP, string EmailOTP, string VERIFY, string VERIFY1, string Resend, string Resend1)
        {

            SUBSCRIBER _subscriber = new SUBSCRIBER();
            _subscriber.MobileOTP = MobileOTP;
            _subscriber.EmailOTP = EmailOTP;
            if ((VERIFY == "VERIFIED") && (VERIFY1 == "VERIFIED"))
            {
                if (ModelState.IsValid)
                {
                    SUBSCRIBER _obj = new SUBSCRIBER();
                    // _subscriber.Action = "UserEntry";
                    _subscriber.Action = "RegisterOTPORUserEntry";
                    _subscriber.SubscriberID = Convert.ToInt32(Session["SubscriberID"].ToString());
                    Session["VID"] = _subscriber.SubscriberID;
                    _obj = InsertSubscriberMaster(_subscriber);
                    // Insertsub(Session["SubscriberID"].ToString());
                    // InsertUserEntryMaster();
                    if (Session["Message"] != null)
                    {
                        //   TempData["OTPMessage"] = Session["Message"];
                        if (TempData["OTPMessage"].ToString() == "OTP Verified !")
                        {
                            ViewBag.Message = Session["Message"];

                            VerifyDate();

                            if (Session["OTPVerifiedDate"] != null)
                            {
                                Session["Message"] = "OTP Verified Successfully";
                                Models.EmailOTP opt = new Models.EmailOTP();
                                string Email = Session["EmailID"].ToString();
                                string Mobile = Session["MobileNo"].ToString();
                                opt.UserNmPassMail(Email, Mobile);
                                return Json(Session["Message"].ToString(), JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                Session["Message"] = null;
                                return Json(Session["Message"].ToString(), JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    else
                    {
                        Session["Message"] = null;
                        return Json(Session["Message"].ToString(), JsonRequestBehavior.AllowGet);
                    }
                }

                else
                {
                    Session["Message"] = null;
                    return Json(Session["Message"].ToString(), JsonRequestBehavior.AllowGet);
                }

            }
            if (!string.IsNullOrEmpty(VERIFY1))
            {
                _subscriber.Action = "MatchEmailOTP";
                InsertOTPDT(_subscriber);
                if (Session["Message"] != null)
                {
                    TempData["OTPMessage"] = Session["Message"];
                    ViewBag.Message = Session["Message"];
                }
                return Json(Session["Message"].ToString(), JsonRequestBehavior.AllowGet);

            }


            if (!string.IsNullOrEmpty(VERIFY))
            {
                _subscriber.Action = "MatchMobileOTP";

                InsertOTPDT(_subscriber);
                if (Session["Message"] != null)
                {
                    TempData["OTPMessage"] = Session["Message"];
                    ViewBag.Message = Session["Message"];
                }

                return Json(Session["Message"].ToString(), JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(Resend))
            {
                _subscriber.Action = "CreateOTP";
                _subscriber.SubscriberID = Convert.ToInt32(Session["SubscriberID"].ToString());
                InsertSubscriberMaster(_subscriber);
                return View();

            }

            if (!string.IsNullOrEmpty(Resend1))
            {
                _subscriber.Action = "CreateOTP";
                _subscriber.SubscriberID = Convert.ToInt32(Session["SubscriberID"].ToString());
                InsertSubscriberMaster(_subscriber);
                return View();

            }
            else
            {
                return View();
            }



        }
        public void InsertOTPDT(SUBSCRIBER _subscriber)
        {

            var SubscriberID = Session["SubscriberID"];
            SUBSCRIBER objdata = new SUBSCRIBER();
            objdata.MobileOTP = _subscriber.MobileOTP;
            objdata.EmailOTP = _subscriber.EmailOTP;

            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SUBSCRIBERMASTER?DBAction=" + _subscriber.Action + "&ID=" + SubscriberID);
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
                SUBSCRIBER item = new SUBSCRIBER();
                objdata.Code = dr["Code"].ToString();
                Session["Code"] = item.Code;
                objdata.Message = dr["Message"].ToString();
                Session["Message"] = objdata.Message;
            }
        }
        public void VerifyDate()
        {
            var ID = Session["VID"]; // Session["SubscriberID"];
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SUBSCRIBERMASTER?DBAction=VerifyDate&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SUBSCRIBER item = new SUBSCRIBER();
                item.OTPVerifiedDate = Convert.ToDateTime(dr["OTPVerifiedDate"].ToString());
                Session["OTPVerifiedDate"] = item.OTPVerifiedDate;
            }
        }
        public void InsertUserEntryMaster()
        {
            var SubscriberID = Session["SubscriberID"];
            USER objdata = new USER();
            objdata.UserName = Session["EmailID"].ToString();
            objdata.Password = Session["MobileNo"].ToString();
            objdata.RoleId = Convert.ToInt32(Session["RoleId"].ToString());
            objdata.SubscriberID = Convert.ToInt32(SubscriberID.ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/USERMASTER?DBAction=UserEntry&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

        }

        [HttpGet]
        public ActionResult CreatePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreatePassword(SUBSCRIBER _subscriber)
        {
            if (ModelState.IsValid)
            {
                SUBSCRIBER _obj = new SUBSCRIBER();
                _subscriber.Action = "CreatePassword";
                _subscriber.SubscriberID = Convert.ToInt32(Session["UserID"].ToString());
                string CreatePassword = _subscriber.CreatePassword;
                string ConfirmPasssword = _subscriber.ConfirmPasssword;

                Session["LoginPassword"] = CreatePassword;
                if (CreatePassword == ConfirmPasssword)
                {
                    _obj = InsertSubscriberMaster(_subscriber);
                    ViewBag.Message = _obj.Message;
                    return RedirectToAction("RegisterSubmit");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult RegisterSubmit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterSubmit(SUBSCRIBER _subscriber)
        {

            if (ModelState.IsValid)
            {

                SUBSCRIBER _obj = new SUBSCRIBER();
                _subscriber.Action = "SubscriptionLimit";
                _subscriber.SubscriberID = Convert.ToInt32(Session["UserID"].ToString());
                _obj = InsertSubscriberMaster(_subscriber);
                ViewBag.Message = _obj.Message;


                //USER _User = new USER();
                //_User.UserName = Session["ONborUserName"].ToString();
                //_User.Password = Session["ONborPassword"].ToString();
                //LoginController ng = new LoginController();

                //ng.UserLogin(_User);


                //FormsAuthentication.SetAuthCookie(_User.UserName, false);

                //ViewBag.Message = "Login Successful !!";


                return Redirect("~/Family/OnboardingFamily");
            }

            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult SaveSubscriber()
        {
            ViewBag.RoleId = new SelectList(obj.BindRole().ToList(), dataValueField: "RoleId", dataTextField: "Name");
            return View();
        }
        [HttpPost]
        public ActionResult SaveSubscriber(Subscriber _subscriber)
        {
            try
            {
                ViewBag.RoleId = new SelectList(obj.BindRole().ToList(), dataValueField: "RoleId", dataTextField: "Name");
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    Subscriber _obj = new Subscriber();

                    _obj = obj.InsertSubscriberMaster(_subscriber);
                    ViewBag.Message = "Records Save Sucessfully !!";
                }
                return View();
            }
            catch (Exception ex)
            {

                return View(ex);
            }
        }

        public JsonResult IsUserExists(string SubscriberName)
        {
            return Json(!obj.SelectallSubscriber().Any(x => x.SubscriberName == SubscriberName), JsonRequestBehavior.AllowGet);

        }
        public JsonResult IsEmailExists(string EmailID)
        {
            return Json(!obj.SelectallSubscriber().Any(x => x.EmailID == EmailID), JsonRequestBehavior.AllowGet);

        }
        public JsonResult IsMobileExists(string MobileNo)
        {
            return Json(!obj.SelectallSubscriber().Any(x => x.MobileNo == MobileNo), JsonRequestBehavior.AllowGet);

        }
    }
}