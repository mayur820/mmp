using BAL;
using DAL;
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
    public class ConsultantMasterController : Controller
    {
       
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: ConsultantMaster
        public ActionResult Index()
        {
            //DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            //var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            //var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            //var CreatedById = Session["UserID"].ToString();
            //System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=ViewByUserId&ID=" + MemberCode);
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            //request.AddHeader("Content-Type", "application/json");
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            //List<CONSULTANT> list = new List<CONSULTANT>();
            //foreach (DataRow dr in data.Tables[0].Rows)
            //    {
            //    CONSULTANT item = new CONSULTANT();
            //    item.ConsultantID = Convert.ToInt32(dr["ConsultantID"].ToString());
            //    item.Code = dr["Code"].ToString();
            //    item.Name = dr["Name"].ToString();
            //    item.MobileNo = dr["MobileNo"].ToString();
            //    item.Email = dr["Email"].ToString();
            //    item.Active = Convert.ToBoolean(dr["Active"].ToString());
            //    item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

            //    list.Add(item);
            //    }
            //CONSULTANT _consult = new CONSULTANT();
            //_consult.ConsultantList = list;
            return View();

            }
        public JsonResult GetListofConsultant()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_CONSULTANTDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = MemberCode;
            // cmd.Parameters.Add("@AccountId", SqlDbType.Int).Value = FinancialYearCode;

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
        public ActionResult SaveConsultant()
            {          
            return View();
            }
        [HttpPost]
        public ActionResult SaveConsultant(CONSULTANT _Consultant)
            {
            if (ModelState.IsValid)
                {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();
                CONSULTANT objdata = new CONSULTANT();
                objdata.ConsultantID = _Consultant.ConsultantID;
                objdata.MemberID = Convert.ToInt32(MemberCode.ToString());
                objdata.Name = _Consultant.Name;
                objdata.MobileNo = _Consultant.MobileNo;
                objdata.Email = _Consultant.Email;
                objdata.Active = _Consultant.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                objdata.Password=_Consultant.Password;
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=Insert&ID=0");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString() , ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                foreach (DataRow dr in data.Tables[0].Rows)
                    {
                    CONSULTANT item = new CONSULTANT();
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
               // ViewBag.Message = "Records Save Sucessfully !!";
                }
            return RedirectToAction("Index");
            }
        [HttpGet]
        public ActionResult DetailsConsultant(int id)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient( URL + "api/Master/CONSULTANTMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            ConsultantModel.Rootobject obj = new ConsultantModel.Rootobject();
            obj = JsonConvert.DeserializeObject<ConsultantModel.Rootobject>(response.Content.ToString());
            CONSULTANT consultant = new CONSULTANT();
            foreach (ConsultantModel.Datum dr in obj.Data)
                {
                consultant.ConsultantID = Convert.ToInt32(dr.ConsultantID.ToString());
                consultant.Code = dr.Code.ToString();
                consultant.Name = dr.Name.ToString();
                consultant.MobileNo = dr.MobileNo.ToString();
                consultant.Email = dr.Email.ToString();
                consultant.Active = Convert.ToBoolean(dr.Active.ToString());
                consultant.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["ConsultantModel"] = consultant;
                }

            var data = ViewData["ConsultantModel"];
            return View(data);
            }
        [HttpGet]
        public ActionResult EditConsultant(int id )
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            ConsultantModel.Rootobject obj = new ConsultantModel.Rootobject();
            obj = JsonConvert.DeserializeObject<ConsultantModel.Rootobject>(response.Content.ToString());
            CONSULTANT consultant = new CONSULTANT();
            foreach (ConsultantModel.Datum dr in obj.Data)
                {
                consultant.ConsultantID = Convert.ToInt32(dr.ConsultantID.ToString());
                consultant.Code = dr.Code.ToString();
                consultant.Name = dr.Name.ToString();
                consultant.MobileNo = dr.MobileNo.ToString();
                consultant.Email = dr.Email.ToString();
                consultant.Active = Convert.ToBoolean(dr.Active.ToString());
                consultant.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["ConsultantModel"] = consultant;
                }

            var data = ViewData["ConsultantModel"];
            return View(data);

            }
        [HttpPost]
        public ActionResult EditConsultant(CONSULTANT _consultant, int id)
        {
            //if (ModelState.IsValid)
            //{
                var CreatedBy = Session["UserID"];
                CONSULTANT objdata = new CONSULTANT();
                objdata.ConsultantID = id;
                objdata.Name = _consultant.Name;
                objdata.MobileNo = _consultant.MobileNo;
                objdata.Email = _consultant.Email;
                objdata.Active = _consultant.Active;
            objdata.Password = _consultant.Password;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=Update&ID=" + id);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                CONSULTANT item = new CONSULTANT();
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
            //}
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteConsultant(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            ConsultantModel.Rootobject obj = new ConsultantModel.Rootobject();
            obj = JsonConvert.DeserializeObject<ConsultantModel.Rootobject>(response.Content.ToString());
            CONSULTANT consultant = new CONSULTANT();
            foreach (ConsultantModel.Datum dr in obj.Data)
                {
                consultant.ConsultantID = Convert.ToInt32(dr.ConsultantID.ToString());
                consultant.Code = dr.Code.ToString();
                consultant.Name = dr.Name.ToString();
                consultant.MobileNo = dr.MobileNo.ToString();
                consultant.Email = dr.Email.ToString();
                consultant.Active = Convert.ToBoolean(dr.Active.ToString());
                consultant.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["ConsultantModel"] = consultant;
                }

            var data = ViewData["ConsultantModel"];
            return View(data);

            }
        [HttpPost]
        public ActionResult DeleteConsultant(CONSULTANT _consultant, int id)
        {
            CONSULTANT objdata = new CONSULTANT();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var json = new JavaScriptSerializer().Serialize(objdata);
            var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=Delete&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                CONSULTANT item = new CONSULTANT();
                item.Code = dr["Code"].ToString();
                item.Message = dr["Message"].ToString();
                if (item.Code == "200")
                {
                    TempData["Message"] = item.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = item.Message;
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        public JsonResult IsUserExists(string Name)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=View&ID=0");
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
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.MobileNo = ds.Tables[0].Rows[i]["MobileNo"].ToString();
                cobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                ConsultList.Add(cobj);
                }
            return Json(!ConsultList.Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            //return Json(!obj.GetAllConsultantList().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            }
        }
}