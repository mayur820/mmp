using BAL;
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
    public class FamilyController : Controller
    {


        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: Family
        public ActionResult Index1()
        {
            return View();
        }
        public ActionResult Index()
        {
            FnUserRights Rights = new FnUserRights();
            string url = Request.Url.PathAndQuery;
            List<Menu> menulst = (List<Menu>)Session["SystemUserMenuDetail"];
            var menudt = menulst.Where(item => item.url == url.Remove(0, 1)).FirstOrDefault();
            
            Rights.SetUserRight(url);
            var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=ViewByUserId&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<FamilyModel> list = new List<FamilyModel>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                FamilyModel item = new FamilyModel();
                item.FamilyID = Convert.ToInt32(dr["FamilyID"].ToString());
                item.Code = dr["Code"].ToString();
                item.Name = dr["Name"].ToString();
                //  item.Active = Convert.ToBoolean(dr["Active"].ToString());
                //    item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                list.Add(item);
            }
            FamilyModel _family = new FamilyModel();
            _family.FamilyList = list;
            //String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            //SqlConnection con = new SqlConnection(strConnString);
            //SqlCommand cmd = new SqlCommand();

            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "usp_GetMenuAccess";
            //cmd.Parameters.Add("@MenuName", SqlDbType.NVarChar).Value = "Family Master";
            //cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = 0;
            //cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = 1;

            //cmd.Connection = con;
            //System.Data.DataTable DT = new System.Data.DataTable();
            //try
            //{
            //    con.Open();

            //    using (var da = new SqlDataAdapter(cmd))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        da.Fill(DT);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    con.Close();
            //    con.Dispose();
            //}
            //var add = false;
            //var edit = false;
            //var delete = false;
            //var view = false;
            //if (DT.Rows.Count > 0)
            //{
            //    add = Convert.ToBoolean(DT.Rows[0][3].ToString());
            //    edit = Convert.ToBoolean(DT.Rows[0][4].ToString());
            //    delete = Convert.ToBoolean(DT.Rows[0][5].ToString());
            //    view = Convert.ToBoolean(DT.Rows[0][6].ToString());
            //}
            ViewBag.Add = menudt.Add;
            ViewBag.Edit = menudt.Edit;
            ViewBag.Delete = menudt.Delete;
            ViewBag.View = menudt.View;
            return View(_family);
        }
        public JsonResult GetListofFamily()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_FAMILYDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedById;

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
        public ActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save(FamilyModel _family)
        {
            if (ModelState.IsValid)
            {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();

                var SubscriberID = CreatedById;
                var CreatedBy = CreatedById;
                FamilyModel objdata = new FamilyModel();
                objdata.FamilyID = _family.FamilyID;
                objdata.Name = _family.Name;
                objdata.Active = _family.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
                objdata.SubscriberID = Convert.ToInt32(SubscriberID.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=Insert&ID=0");
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
                    FamilyModel item = new FamilyModel();
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
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult FamilyDetails(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            FAMILY.Rootobject obj = new FAMILY.Rootobject();
            obj = JsonConvert.DeserializeObject<FAMILY.Rootobject>(response.Content.ToString());
            FamilyModel family = new FamilyModel();
            foreach (FAMILY.Datum dr in obj.Data)
            {
                family.FamilyID = Convert.ToInt32(dr.FamilyID.ToString());
                family.Code = dr.Code.ToString();
                family.Name = dr.Name.ToString();
                family.UserId = Convert.ToInt32(dr.UserId.ToString());
                family.Active = Convert.ToBoolean(dr.Active.ToString());
                family.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FamilyModel"] = family;
            }
            //  Console.WriteLine(response.Content);
            var data = ViewData["FamilyModel"];
            return View(data);
        }
        [HttpGet]
        public ActionResult DeleteFamily(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            FAMILY.Rootobject obj = new FAMILY.Rootobject();
            obj = JsonConvert.DeserializeObject<FAMILY.Rootobject>(response.Content.ToString());
            FamilyModel family = new FamilyModel();
            foreach (FAMILY.Datum dr in obj.Data)
            {
                family.FamilyID = Convert.ToInt32(dr.FamilyID.ToString());
                family.Code = dr.Code.ToString();
                family.Name = dr.Name.ToString();
                family.UserId = Convert.ToInt32(dr.UserId.ToString());
                family.Active = Convert.ToBoolean(dr.Active.ToString());
                family.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FamilyModel"] = family;
            }
            //  Console.WriteLine(response.Content);
            var data = ViewData["FamilyModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult DeleteFamily(FamilyModel _Family, int id)
        {
            FamilyModel objdata = new FamilyModel();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=Delete&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
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


            TempData["Message"] = "Records Deleted Successfully !!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditFamily(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            FAMILY.Rootobject obj = new FAMILY.Rootobject();
            obj = JsonConvert.DeserializeObject<FAMILY.Rootobject>(response.Content.ToString());
            FamilyModel family = new FamilyModel();
            foreach (FAMILY.Datum dr in obj.Data)
            {
                family.FamilyID = Convert.ToInt32(dr.FamilyID.ToString());
                family.Code = dr.Code.ToString();
                family.Name = dr.Name.ToString();
                family.UserId = Convert.ToInt32(dr.UserId.ToString());
                family.Active = Convert.ToBoolean(dr.Active.ToString());
                family.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["FamilyModel"] = family;
            }
            //  Console.WriteLine(response.Content);
            var data = ViewData["FamilyModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult EditFamily(FamilyModel _Family, int id)
        {
            if (!string.IsNullOrWhiteSpace(_Family.Name))
            {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();


                var SubscriberID = CreatedById;
                var CreatedBy = CreatedById;
                FamilyModel objdata = new FamilyModel();
                objdata.FamilyID = id;
                objdata.Name = _Family.Name;
                if (SubscriberID != null)
                {
                    objdata.UserId = Convert.ToInt32(SubscriberID.ToString());
                }
                else
                {
                    objdata.UserId = Convert.ToInt32(CreatedById.ToString());
                }
                objdata.Active = _Family.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=Update&ID=" + id);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", BasicAuth);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    FamilyModel item = new FamilyModel();
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
            }
            return RedirectToAction("Index");
        }
        public JsonResult IsUserExists(string Name)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<FamilyModel> FamilyList = new List<FamilyModel>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FamilyModel cobj = new FamilyModel();
                cobj.FamilyID = Convert.ToInt32(ds.Tables[0].Rows[i]["FamilyID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                FamilyList.Add(cobj);
            }
            return Json(!FamilyList.Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            //check if any of the UserName matches the MemberName specified in the Parameter using the ANY extension method.  
            ////return Json(!obj.BindAllFamily().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult OnboardingFamily()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OnboardingFamily(FamilyModel _family)
        {
            if (ModelState.IsValid)
            {
                var UserID = Session["UserID"].ToString();
                //var SubscriberID = Session["UserID"].ToString();
                int SubscriberID = GetUserID(Convert.ToInt32(UserID.ToString()));
                FamilyModel objdata = new FamilyModel();
                objdata.FamilyID = _family.FamilyID;
                objdata.Name = _family.Name;
                objdata.Active = _family.Active;
                objdata.CreatedBy = Convert.ToInt32(UserID.ToString());
                objdata.SubscriberID = Convert.ToInt32(SubscriberID.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=Insert&ID=0");
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
                    FamilyModel item = new FamilyModel();
                    item.Code = dr["Code"].ToString();
                    item.Message = dr["Message"].ToString();
                    if (item.Code == "200")
                    {
                        ViewBag.Message = item.Message;
                        return Redirect("~/MemberMaster/OnboardingMember");
                    }
                    else
                    {
                        ViewBag.Message = item.Message;
                        return View();
                    }
                }
            }
            return View();
        }

        public int GetUserID(int ID)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=GetUserID&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            SubscriberModel.Rootobject obj = new SubscriberModel.Rootobject();
            obj = JsonConvert.DeserializeObject<SubscriberModel.Rootobject>(response.Content.ToString());
            SUBSCRIBER sub = new SUBSCRIBER();
            int UserID = 0;
            foreach (SubscriberModel.Datum dr in obj.Data)
            {
                sub.SubscriberID = Convert.ToInt32(dr.SubscriberID.ToString());
                UserID = sub.SubscriberID;
            }
            return UserID;
        }
    }
}
