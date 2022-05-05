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
    public class DematMasterController : Controller
    {

        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: DematMaster
        public ActionResult Index()
        {
            //DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            //var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            //var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            //var CreatedById = Session["UserID"].ToString();

            //var ID = Session["UserID"];
            //System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=ViewByUserId&ID=" + MemberCode + ':' + FinancialYearCode);
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", BasicAuth);
            //request.AddHeader("Content-Type", "application/json");
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            //List<DEMAT> list = new List<DEMAT>();
            //foreach (DataRow dr in data.Tables[0].Rows)
            //{
            //    DEMAT item = new DEMAT();
            //    item.DematID = Convert.ToInt32(dr["DematID"].ToString());
            //    item.Code = dr["Code"].ToString();
            //    item.Name = dr["Name"].ToString();
            //    item.Active = Convert.ToBoolean(dr["Active"].ToString());
            //    item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
            //    list.Add(item);
            //}
            //DEMAT _demat = new DEMAT();
            //_demat.ShowDemat = list;
            return View();
        }
        [HttpGet]
        public ActionResult SaveDemat()
        {

            ViewBag.DemateList = GetAllDemate();
            return View();
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
        public JsonResult GetListofDemat()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_DEMATDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@DematId", SqlDbType.Int).Value = FinancialYearCode;

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
        public IEnumerable<SelectListItem> GetAllDemate()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_DEMATDATA]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEW_ADMIN_DEMATE_DATA";
            cmd.Parameters.Add("@Memberid", SqlDbType.Int).Value = MemberCode;
            
            //cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = username;

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
            SelectList lstobj = null;

            try
            {
                List<SelectListItem> mylist = new List<SelectListItem>();
                foreach (DataRow dr in DT.Rows)
                {
                    SelectListItem Slist = new SelectListItem();
                    Slist.Text = dr["NAME"].ToString();
                    Slist.Value = dr["ID"].ToString();
                    mylist.Add(Slist);
                }
                // Loading.  


                // Setting.  
                lstobj = new SelectList(mylist, "Value", "Text");
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // info.  
            return lstobj;




        }


        [HttpPost]
        public ActionResult SaveDemat(DEMAT _Demat)
        {
            if (ModelState.IsValid)
            {
                foreach (int DEMATEID in _Demat.SelectedMultiDemate)
                {


                    DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                    var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                    var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                    var CreatedById = Session["UserID"].ToString();

                    DEMAT objdata = new DEMAT();
                    objdata.DematID = DEMATEID;
                    objdata.MemberId = Convert.ToInt32(MemberCode.ToString());
                    objdata.FinancialYearMemberID = Convert.ToInt32(FinancialYearCode.ToString());
                    objdata.Name = DEMATEID.ToString();
                    objdata.Active = _Demat.Active;
                    objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                    var json = new JavaScriptSerializer().Serialize(objdata);
                    System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=Insert&ID=0");
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
                        DEMAT item = new DEMAT();
                        item.Code = dr["Code"].ToString();
                        item.Message = dr["Message"].ToString();
                        if (item.Code == "200")
                        {
                            ViewBag.Message = item.Message;
                            TempData["Message"] = item.Message;
                            // return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Message = item.Message;
                            TempData["Message"] = item.Message;
                            return View();
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DetailsDemat(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DematModel.Rootobject obj = new DematModel.Rootobject();
            obj = JsonConvert.DeserializeObject<DematModel.Rootobject>(response.Content.ToString());
            DEMAT demat = new DEMAT();
            foreach (DematModel.Datum dr in obj.Data)
            {
                demat.DematID = Convert.ToInt32(dr.DematID.ToString());
                demat.Code = dr.Code.ToString();
                demat.Name = dr.Name.ToString();
                demat.Active = Convert.ToBoolean(dr.Active.ToString());
                demat.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["DematModel"] = demat;
            }

            var data = ViewData["DematModel"];
            return View(data);
        }
        [HttpGet]
        public ActionResult EditDemat(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DematModel.Rootobject obj = new DematModel.Rootobject();
            obj = JsonConvert.DeserializeObject<DematModel.Rootobject>(response.Content.ToString());
            DEMAT demat = new DEMAT();
            foreach (DematModel.Datum dr in obj.Data)
            {
                demat.DematID = Convert.ToInt32(dr.DematID.ToString());
                demat.Code = dr.Code.ToString();
                demat.Name = dr.Name.ToString();
                demat.Active = Convert.ToBoolean(dr.Active.ToString());
                demat.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["DematModel"] = demat;
            }

            var data = ViewData["DematModel"];
            return View(data);

        }
        [HttpPost]
        public ActionResult EditDemat(DEMAT _Demat, int id)
        {
            if (ModelState.IsValid)
            {
                var CreatedBy = Session["UserID"];
                DEMAT objdata = new DEMAT();
                objdata.DematID = id;
                objdata.Name = _Demat.Name;
                objdata.Active = _Demat.Active;
                objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=Update&ID=" + id);
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
                    DEMAT item = new DEMAT();
                    item.Code = dr["Code"].ToString();
                    item.Message = dr["Message"].ToString();
                    if (item.Code == "200")
                    {
                        ViewBag.Message = item.Message;
                        TempData["Message"] = item.Message;
                        // return RedirectToAction("Index");
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
        public ActionResult DeleteDemat(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DematModel.Rootobject obj = new DematModel.Rootobject();
            obj = JsonConvert.DeserializeObject<DematModel.Rootobject>(response.Content.ToString());
            DEMAT demat = new DEMAT();
            foreach (DematModel.Datum dr in obj.Data)
            {
                demat.DematID = Convert.ToInt32(dr.DematID.ToString());
                demat.Code = dr.Code.ToString();
                demat.Name = dr.Name.ToString();
                demat.Active = Convert.ToBoolean(dr.Active.ToString());
                demat.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["DematModel"] = demat;
            }

            var data = ViewData["DematModel"];
            return View(data);

        }
        [HttpPost]
        public ActionResult DeleteDemat(DEMAT _Demat, int id)
        {
            DEMAT objdata = new DEMAT();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=Delete&ID=" + id);
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
                DEMAT item = new DEMAT();
                item.Code = dr["Code"].ToString();
                item.Message = dr["Message"].ToString();
                if (item.Code == "200")
                {
                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    // return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        public JsonResult IsUserExistsDemat(string Name)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/DEMATMASTER?DBAction=View&ID=0");
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
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                DematList.Add(cobj);
            }
            return Json(!DematList.Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
            //  return Json(!obj.GetAllDematList().Any(x => x.Name == Name), JsonRequestBehavior.AllowGet);
        }
    }
}