using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRecordweb.Models;
using IrecordDAL;
using DAL;
using System.Web.SessionState;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Configuration;
using RestSharp;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

namespace IRecordweb.Controllers
{
    public class LoginController : Controller
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {


            FormsAuthentication.SignOut();
            Session.Abandon();
            return View();
        }
        [HttpGet]
        public ActionResult MemberLogin()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {

            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {

            return View();
        }
        [HttpPost]
        public JsonResult SendOtpFunction(string username, string WantTo)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_FORGOT_PASSWORD]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewUserByUserName";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = username;

            cmd.Connection = con;

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
            IRecordweb.Models.EmailOTP obj = new IRecordweb.Models.EmailOTP();
            if (DT.Rows.Count > 0)
            {
                Random random = new Random();
                string otp = random.Next(1000, 100000000).ToString().Substring(0, 4);
                Session["PasswordResetOtp"] = otp;
                if (WantTo == "1")
                {//To Mail

                    obj.SendmailWithEmail(DT.Rows[0]["EmailID"].ToString(), otp);
                    Session["ResetPassword_ID"] = DT.Rows[0]["ID"].ToString();
                    Session["ResetPassword_STATUS"] = DT.Rows[0]["STATUS"].ToString();
                    return Json(DT.Rows[0]["DIS_EmailID"].ToString(), JsonRequestBehavior.AllowGet);
                }
                else
                {

                    if (DT.Rows[0]["MobileNo"].ToString() != "")
                    {
                        obj.SMS_MobileOTP(DT.Rows[0]["MobileNo"].ToString(), otp, DT.Rows[0]["NAME"].ToString());
                        Session["ResetPassword_ID"] = DT.Rows[0]["ID"].ToString();
                        Session["ResetPassword_STATUS"] = DT.Rows[0]["STATUS"].ToString();
                    }
                    return Json(DT.Rows[0]["DIS_MobileNo"].ToString(), JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public ActionResult ResetPassword(SUBSCRIBER _subscriber)
        {
            if (ModelState.IsValid)
            {
                SUBSCRIBER _obj = new SUBSCRIBER();
                //_subscriber.Action = "CreatePassword";
                //_subscriber.SubscriberID = Convert.ToInt32(Session["SubscriberID"].ToString());
                string CreatePassword = _subscriber.CreatePassword;
                string ConfirmPasssword = _subscriber.ConfirmPasssword;
                if (CreatePassword == ConfirmPasssword)
                {
                    SetUpdatePassword(CreatePassword);
                    Response.Redirect("/Login/Login");
                    return View();
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
        public void SetUpdatePassword(string Password)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_FORGOT_PASSWORD]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UpdatePassword";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = Session["ResetPassword_STATUS"];
            cmd.Parameters.Add("@Var2", SqlDbType.NVarChar).Value = Session["ResetPassword_ID"].ToString();
            cmd.Parameters.Add("@Var3", SqlDbType.NVarChar).Value = Password;

            cmd.Connection = con;

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
        }
        [HttpPost]
        public JsonResult OtpVerifly(string Otp)
        {
            if (Session["PasswordResetOtp"].ToString() == Otp)
            {
                Session["PasswordResetOtpStatus"] = "1";
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            else
            {
                Session["PasswordResetOtpStatus"] = "0";
                return Json("0", JsonRequestBehavior.AllowGet);
            }




        }
        [HttpGet]
        public ActionResult AcSwitch()
        {

            return View();
        }
        [HttpGet]
        public ActionResult FinancialYear()
        {

            return View();
        }
        public JsonResult GetAllYear()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_FinincialYear_UTITLIY]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewFinincialMaster";

            cmd.Connection = con;

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

            //return Json("SingleUser", JsonRequestBehavior.AllowGet);
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);



        }
        public JsonResult GetAllYearWithMember(string MemberID)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_USERLOGINDATA_NEW]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewFinYear";
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = MemberID.ToString().Split(':')[1];
            cmd.Connection = con;

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

            //return Json("SingleUser", JsonRequestBehavior.AllowGet);
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);



        }
        public JsonResult GetYearByUser(string MemberID)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_FinincialYear_UTITLIY]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewbyUser";
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = MemberID.ToString().Split(':')[1];

            cmd.Connection = con;

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

            //return Json("SingleUser", JsonRequestBehavior.AllowGet);
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);



        }
        public void LoginWhenError(string UserName, string Password)
        {

            USER _User = new USER();
            _User.UserName = UserName;
            _User.Password = Password;
            UserLogin(_User);
        }
        public JsonResult GetAllMember()
        {

            LoginWhenError(Session["LoginUserName"].ToString(), Session["LoginPassword"].ToString());

            string Action = "";
            string UserId = "";
            if (Session["Login_Flags"].ToString() == "1")
            {//sub
                Action = "ViewSubscriberWiseAllMember";
                UserId = Session["SUBSC_OPR_ID"].ToString();
            }
            if (Session["Login_Flags"].ToString() == "2")
            {//Opr
                Action = "ViewOpratorsWiseAllMember";
                UserId = Session["SUBSC_OPR_ID"].ToString();
            }
            if (Session["Login_Flags"].ToString() == "3")
            {//Memb

                Action = "ViewMemberWiseLogin";
                UserId = Session["UserID"].ToString();

            }

            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_USERLOGINDATA_NEW]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = Action;
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserId;

            cmd.Connection = con;

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

            //return Json("SingleUser", JsonRequestBehavior.AllowGet);
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);



        }
        [HttpPost]
        public JsonResult InsertFinancialYear(string JsonData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_FinincialYear_UTITLIY]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Insert";
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["MemberId"].ToString().Split(':')[1];
            cmd.Parameters.Add("@FinincialYearId", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["FinincialYearId"].ToString().Split(':')[1];

            cmd.Connection = con;

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

            //return Json("SingleUser", JsonRequestBehavior.AllowGet);
            return Json("1", JsonRequestBehavior.AllowGet);



        }
        public JsonResult GetUserInfo(string Legerid)
        {
            DataTable DT = Session["DT_UserID_Info"] as DataTable;
            if (DT.Rows.Count == 1)
            {

                return Json("SingleUser", JsonRequestBehavior.AllowGet);
            }
            else
            {

                //return Json("SingleUser", JsonRequestBehavior.AllowGet);
                return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public JsonResult GetUserSingleInfo(string JsonData)
        {
            Session["Act_User_ID"] = null;
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            clsLogin objelogin = new clsLogin();
            objelogin.SingleUserLogin(dtJsonData);
            Session["_UserChange"] = "1";

            return Json("1", JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult SetFinancialYearUser(string JsonData)
        {

            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            Session["Dt_FinancialYear"] = dtJsonData;


            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public static object DataTableToJSON(System.Data.DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(list);
        }

        [HttpPost]
        // [Authorize]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(USER _User)
        {
            if (_User.SuperAdmin == false)
            {
                if (ModelState.IsValid)
                {
                    UserLogin(_User);
                    TempData["UserName"] = Session["UserNm"];
                    if (isValid(_User.UserName, _User.Password))
                    {
                        if (isCheckValidLicense(Session["UserID"].ToString()))
                        {
                            FormsAuthentication.SetAuthCookie(_User.UserName, false);
                            Session["LoginUserName"] = _User.UserName;
                            Session["LoginPassword"] = _User.Password;
                            ViewBag.Message = "Login Successful !!";

                            if (Session["OnboardingPhases"].ToString() == "1")
                            {
                                return Redirect("~/SubscriberMaster/CreatePassword");
                            }
                            if (Session["OnboardingPhases"].ToString() == "2")
                            {
                                return Redirect("~/SubscriberMaster/RegisterSubmit");
                            }
                            if (Session["OnboardingPhases"].ToString() == "3")
                            {
                                return Redirect("~/Family/OnboardingFamily");
                            }
                            if (Session["OnboardingPhases"].ToString() == "4")
                            {
                                return Redirect("~/MemberMaster/OnboardingMember");
                            }
                            if (Session["OnboardingPhases"].ToString() == "5")
                            {
                                return Redirect("~/Login/MemberLogin");
                            }
                            if (Session["OnboardingPhases"].ToString() == "0" || Session["OnboardingPhases"].ToString() == "5")
                            {
                                return Redirect("~/Login/MemberLogin");
                            }

                            //  return Redirect("~/Login/MemberLogin");
                        }
                        else
                        {
                            ViewBag.Message = "Your Free License Is Expired !!";
                        }


                    }
                    else
                    {
                        ViewBag.Message = "Login Failed !!";
                    }

                }
                else
                {
                    ViewBag.Message = "Login Failed !!";
                }
            }
            else
            {
                Int64 userid = SystemUserLogin(_User);
                if(userid == 0)
                {
                    ViewBag.Message = "Login Failed !!";
                }
                else
                {
                    Session["UserID"] = userid;
                    SystemMenuList(userid);
                    Session["Role_Type"] = "System";
                    Session["SUBSC_OPR_NAME"] = "";
                    return Redirect("~/Dashboard/Index");
                }
            }
            return View();
        }


        public void UserLogin(USER _user)
        {
            clsLogin objelogin = new clsLogin();
            objelogin.UserLoginNew(_user);
            TempData["UserNm"] = Session["UserNm"];
            TempData["MemberSubscription"] = Session["MemberSubscription"];
            TempData["MemberCount"] = Session["MemberCount"];
        }
        public Int64 SystemUserLogin(USER _user)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            Int64 userid = 0;
            //long roleid = Convert.ToInt64(RoleID);
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                string query = "select ID from Emp_Mast where Username = '" + _user.UserName + "' AND Password='" + _user.Password + "'";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            userid = Convert.ToInt64(sdr["ID"]);
                        }
                    }
                    con.Close();
                }
            }
            return userid;
        }
        public IList<Menu> SystemMenuList(Int64 UserId)
        {
            List<Menu> MenuList = new List<Menu>();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[usp_GetSystemUserMenu]";
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt64(UserId);
            cmd.Connection = con;
            con.Open();
            DataSet ds = new DataSet();
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
            }
            Menu cobj = new Menu();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                MenuList.Add(new Menu()
                {
                    MenuID = Convert.ToInt32(ds.Tables[0].Rows[i]["MenuID"].ToString()),
                    UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"].ToString()),
                    RoleID = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleID"].ToString()),
                    url = ds.Tables[0].Rows[i]["Url"].ToString(),
                    Add = Convert.ToBoolean(ds.Tables[0].Rows[i]["AddAccess"].ToString()),
                    Edit = Convert.ToBoolean(ds.Tables[0].Rows[i]["EditAccess"].ToString()),
                    Delete = Convert.ToBoolean(ds.Tables[0].Rows[i]["DeleteAccess"].ToString()),
                    View = Convert.ToBoolean(ds.Tables[0].Rows[i]["ViewAccess"].ToString()),
                });
            }
            Session["SystemUserMenuDetail"] = MenuList;
            return MenuList;
        }
        public List<Menu> MenuList(string UserId)
        {
            List<Menu> MenuList = new List<Menu>();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[Sp_GetUserMenuAccess]";
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = UserId;
            cmd.Connection = con;
            con.Open();
            DataSet ds = new DataSet();
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(ds);
            }
            Menu cobj = new Menu();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                MenuList.Add(new Menu()
                {
                    MenuID = Convert.ToInt32(ds.Tables[0].Rows[i]["MenuID"].ToString()),
                    UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString()),
                    Add = Convert.ToBoolean(ds.Tables[0].Rows[i]["Add"].ToString()),
                    Edit = Convert.ToBoolean(ds.Tables[0].Rows[i]["Edit"].ToString()),
                    Delete = Convert.ToBoolean(ds.Tables[0].Rows[i]["Delete"].ToString()),
                    View = Convert.ToBoolean(ds.Tables[0].Rows[i]["View"].ToString()),
                });
            }
            return MenuList;

        }
        public bool isCheckValidLicense(string UserId)
        {

            bool isValid = false;
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[Sp_User_Check_LicenseExpiry]";
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = UserId;

            cmd.Connection = con;

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
            if (DT.Rows.Count > 0)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }


            return isValid;
        }

        private bool isValid(string UserName, string Password)
        {
            bool isValid = false;
            var user = Session["UserID"];
            var pass = Session["UserName"];
            if (user != null && pass != null)
            {
                isValid = true;
            }
            return isValid;
        }



        public ActionResult LoginDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return View(Session["UserName"]);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {

            // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            try
            {
                FormsAuthentication.SignOut();
            }
            catch (Exception ex)
            {


            }

            Session.Abandon();
            // return RedirectToAction("Index", "Home");
            return RedirectToAction("Login");
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult ExtendSession()
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            var data = new { IsSuccess = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
