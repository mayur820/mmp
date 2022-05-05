using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRecordweb.Models;
using System.Web.UI.WebControls;
using DAL;
using System.Configuration;
using System.Data.SqlClient;
using PagedList.Mvc;
using PagedList;
using RestSharp;
using Newtonsoft.Json;
using System.Data;
using System.Web.Script.Serialization;
using System.Net;
using System.Web.Security;

namespace IRecordweb.Controllers
{
    public class ScriptMasterController : Controller
    {

        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: ScriptMaster
        public ActionResult ScriptIndex()
        {
            return View();
        }
        public JsonResult GetListofScript()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_SCRIPTDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedById;
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
        public List<INDUSTRY> GetIndustry()
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/INDUSTRYMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<INDUSTRY> list = new List<INDUSTRY>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                INDUSTRY item = new INDUSTRY();
                item.IndustryID = Convert.ToInt32(dr["IndustryID"].ToString());
                item.Name = dr["Name"].ToString();
                list.Add(item);
            }
            return list;
        }
        public List<SECTOR> GetSector1()
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SECTORMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<SECTOR> list = new List<SECTOR>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SECTOR item = new SECTOR();
                item.SectorID = Convert.ToInt32(dr["SectorID"].ToString());
                item.Name = dr["Name"].ToString();
                list.Add(item);
            }
            return list;
        }
        public List<SCRIPT> GetScriptInvestment(int ID)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=ViewByInScriptId&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<SCRIPT> list = new List<SCRIPT>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SCRIPT item = new SCRIPT();
                item.ScriptID = Convert.ToInt32(dr["ScriptID"].ToString());
                item.InvestmentType = dr["Name"].ToString();
                item.TypeId = Convert.ToInt32(dr["TypeId"].ToString());
                list.Add(item);
            }
            return list;
        }
        [HttpGet]
        public ActionResult SaveScript()
        {
            Script _script = new Script();
            ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.IndustryID = new SelectList(GetIndustry().ToList(), "IndustryID", "Name", _script.IndustryID);
            ViewBag.SectorName = new SelectList(GetSector1().ToList(), "SectorID", "Name", _script.SectorName);
            return View();
        }
        [HttpPost]
        public ActionResult SaveScript(SCRIPT _script, MTYPE mtype)
        {
            ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.IndustryID = new SelectList(GetIndustry().ToList(), "IndustryID", "Name", _script.IndustryID);
            ViewBag.SectorName = new SelectList(GetSector1().ToList(), "SectorID", "Name", _script.SectorName);
            if (ModelState.IsValid)
            {
                _script.ScriptID = InsertScript(_script);
                InsertscriptInvestment(_script);
                if (Session["Code"].ToString() == "200")
                {
                    ViewBag.Message = Session["Message"].ToString();
                    TempData["Message"] = Session["Message"].ToString();
                    return RedirectToAction("ScriptIndex");

                }
                else
                {
                    ViewBag.Message = Session["Message"].ToString();
                    TempData["Message"] = Session["Message"].ToString();
                    return View();
                }
                //  ViewBag.Message = "Records Save Sucessfully !!";
            }

            return RedirectToAction("ScriptIndex");
        }
        public void ScriptByID(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            ScriptModel.Rootobject obj = new ScriptModel.Rootobject();
            obj = JsonConvert.DeserializeObject<ScriptModel.Rootobject>(response.Content.ToString());
            SCRIPT script = new SCRIPT();
            foreach (ScriptModel.Datum dr in obj.Data)
            {
                script.ScriptID = Convert.ToInt32(dr.ScriptID.ToString());
                script.ScriptName = dr.ScriptName.ToString();
                script.IndustryID = Convert.ToInt32(dr.IndustryID.ToString());
                Session["IndustryID"] = script.IndustryID;
                script.BSECode = dr.BSECode.ToString();
                script.NSECode = dr.NSECode.ToString();
                script.GroupName = dr.GroupName.ToString();
                script.InvestmentType = dr.InvestmentType.ToString();
                script.IsMcx = Convert.ToBoolean(dr.IsMcx.ToString());
                script.IsCurrency = Convert.ToBoolean(dr.IsCurrency.ToString());
                script.IsNcdx = Convert.ToBoolean(dr.IsNcdx.ToString());
                script.IsFO = Convert.ToBoolean(dr.IsFO.ToString());
                script.FaceValue = dr.FaceValue.ToString();
                script.ISIN = dr.ISIN.ToString();
                script.SectorName = dr.SectorID.ToString();
                Session["SectorID"] = dr.SectorID.ToString();
                if (script.SectorName != "0")
                {
                    SectorName(script.SectorName);
                    if (Session["Name"].ToString() != null)
                    {
                        script.SectorName = Session["Name"].ToString();
                    }
                }
                script.ListType = dr.ListType.ToString();
                script.Active = Convert.ToBoolean(dr.Active.ToString());
                script.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["ScriptModel"] = script;
            }
        }
        public List<SECTOR> SectorName(string ID)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=GetSectorName&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<SECTOR> list = new List<SECTOR>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SECTOR item = new SECTOR();
                item.Name = dr["Name"].ToString();
                Session["Name"] = item.Name;
                list.Add(item);
            }
            return list;
        }
        [HttpGet]
        public ActionResult ScriptDetails(int id)
        {

            ScriptByID(id);
            var data = ViewData["ScriptModel"];
            return View(data);
        }
        [HttpGet]
        public ActionResult ScriptEdit(int id)
        {

            SCRIPT _script = new SCRIPT();
            var model = new Script();
            ScriptByID(id);
            ViewBag.AllInvestmentTypelist = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.InvestmentType = new SelectList(GetScriptInvestment(id).ToList(), dataValueField: "TypeId", dataTextField: "InvestmentType");
            ViewBag.SectorName = new SelectList(GetSector1().ToList(), "SectorID", "Name", Session["SectorID"]);
            ViewBag.IndustryID = new SelectList(GetIndustry().ToList(), "IndustryID", "Name", Session["IndustryID"]);
            ViewBag.DefaultBrand = Session["IndustryID"].ToString();
            ViewBag.DefaultSectorID = Session["SectorID"].ToString();
            //  _script.Industrylist = _script.IndustryID.ToString();

            var data = ViewData["ScriptModel"];
            if (_script.ListType == "Unlisted")
            {
                UpdateScriptMaster(_script, id);
            }

            return View(data);
        }
        [HttpPost]
        public ActionResult ScriptEdit(SCRIPT _script, int id, MTYPE mtype, string Approved)
        {
            ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.SectorName = new SelectList(GetSector1().ToList(), dataValueField: "SectorID", dataTextField: "Name");
            // ViewBag.IndustryID = new SelectList(GetIndustry().ToList(), dataValueField: "IndustryID", dataTextField: "Name");
            ViewBag.IndustryID = new SelectList(GetIndustry().ToList(), "IndustryID", "Name", _script.IndustryID);
            ViewBag.DefaultBrand = _script.IndustryID;
            if (!string.IsNullOrWhiteSpace(_script.ScriptName))
            {
                // UpdateScriptMaster(_script, id);
                //  UpdatescriptInvestment(_script, id);
                InsertscriptSector(_script);

            }
            return RedirectToAction("ScriptIndex");
        }
        [HttpGet]
        public ActionResult ScriptDelete(int id)
        {
            ScriptByID(id);
            var data = ViewData["ScriptModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult ScriptDelete(SCRIPT _script, int id)
        {
            SCRIPT objdata = new SCRIPT();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=Delete&ID=" + id);
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
                SCRIPT item = new SCRIPT();

                item.Code = dr["Code"].ToString();
                item.Message = dr["Message"].ToString();
                if (item.Code == "200")
                {

                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    //  Redirect("~/ScriptMaster/Index");

                }
                else
                {
                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    // Redirect("~/ScriptMaster/ScriptEdit");
                }
            }

                return RedirectToAction("ScriptIndex");
        }
        public JsonResult IsUserExists(string ScriptName)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<SCRIPT> list = new List<SCRIPT>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SCRIPT item = new SCRIPT();
                item.ScriptID = Convert.ToInt32(dr["ScriptID"].ToString());
                item.ScriptName = dr["ScriptName"].ToString();
                item.BSECode = dr["BSECode"].ToString();
                item.NSECode = dr["NSECode"].ToString();
                item.GroupName = dr["GroupName"].ToString();
                item.InvestmentType = dr["InvestmentType"].ToString();
                item.IsMcx = Convert.ToBoolean(dr["IsMcx"].ToString());
                item.IsCurrency = Convert.ToBoolean(dr["IsCurrency"].ToString());
                item.IsNcdx = Convert.ToBoolean(dr["IsNcdx"].ToString());
                item.IsFO = Convert.ToBoolean(dr["IsFO"].ToString());
                item.FaceValue = dr["FaceValue"].ToString();
                item.ISIN = dr["ISIN"].ToString();
                item.SectorName = dr["SectorID"].ToString();
                item.ListType = dr["ListType"].ToString();
                item.Active = Convert.ToBoolean(dr["Active"].ToString());
                item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                list.Add(item);
            }

            return Json(!list.Any(x => x.ScriptName == ScriptName), JsonRequestBehavior.AllowGet);

        }
        public JsonResult Approvedlink(SCRIPT sc, int id, MTYPE mt, string Approved)
        {
            if (string.IsNullOrEmpty(Approved))
            {
                sc.ListType = "listed";
                UpdateScriptMaster(sc, id);
            }
            return Json("Index", JsonRequestBehavior.AllowGet);

        }
        public Int64 InsertScript(SCRIPT _script)
        {
            var CreatedBy = Session["UserID"];
            SCRIPT objdata = new SCRIPT();
            objdata.ScriptName = _script.ScriptName;
            objdata.BSECode = _script.BSECode;
            objdata.NSECode = _script.NSECode;
            objdata.IndustryID = _script.IndustryID;
            objdata.GroupName = _script.GroupName;
            objdata.InvestmentType = _script.scriptlist[0];
            objdata.IsMcx = _script.IsMcx;
            objdata.IsCurrency = _script.IsCurrency;
            objdata.IsNcdx = _script.IsNcdx;
            objdata.IsFO = _script.IsFO;
            objdata.FaceValue = _script.FaceValue;
            objdata.ISIN = _script.ISIN;
            objdata.MutualFundID = _script.MutualFundID;
            objdata.SectorName = _script.SectorName;
            objdata.ListType = _script.ListType;
            objdata.Active = _script.Active;
            objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=Insert&ID=0");
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
                SCRIPT item = new SCRIPT();

                Session["Scriptid"] = item.ScriptID;
                item.Code = dr["Code"].ToString();
                Session["Code"] = item.Code;
                item.Message = dr["Message"].ToString();
                Session["Message"] = item.Message;
                if (item.Code == "200")
                {
                    item.ScriptID = Convert.ToInt32(dr["ScriptID"].ToString());
                    Session["Scriptid"] = item.ScriptID;
                    ViewBag.Message = item.Message;
                    //  Redirect("~/ScriptMaster/Index");

                }
                else
                {
                    ViewBag.Message = item.Message;
                    // Redirect("~/ScriptMaster/ScriptEdit");
                }

            }
            Int64 scriptID = Convert.ToInt32(Session["Scriptid"].ToString());
            return scriptID;
        }
        public void InsertscriptSector(SCRIPT _script)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable; //Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();

            var CreatedBy = Session["UserID"];
            SCRIPT objdata = new SCRIPT();
            objdata.ScriptID = _script.ScriptID;
            objdata.MemberID = Convert.ToInt32(MemberCode.ToString());//Convert.ToInt32(Session["Act_User_ID"].ToString());
            objdata.SectorName = _script.SectorName;
            objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=InsertSector&ID=0");
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
                SCRIPT item = new SCRIPT();

                Session["Scriptid"] = item.ScriptID;
                item.Code = dr["Code"].ToString();
                Session["Code"] = item.Code;
                item.Message = dr["Message"].ToString();
                Session["Message"] = item.Message;
                if (item.Code == "200")
                {
                    //item.ScriptID = Convert.ToInt32(dr["ScriptID"].ToString());
                    //Session["Scriptid"] = item.ScriptID;
                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    //  Redirect("~/ScriptMaster/Index");

                }
                else
                {
                    ViewBag.Message = item.Message;
                    TempData["Message"] = item.Message;
                    // Redirect("~/ScriptMaster/ScriptEdit");
                }

            }

        }
        public void InsertscriptInvestment(SCRIPT _script)
        {
            var CreatedBy = Session["UserID"];
            if (_script.scriptlist != null)
            {
                for (int i = 0; i < _script.scriptlist.Count(); i++)
                {

                    SCRIPT objdata = new SCRIPT();
                    objdata.ScriptID = _script.ScriptID;
                    objdata.InvestmentType = _script.scriptlist[i];
                    objdata.Active = _script.Active;
                    objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
                    var json = new JavaScriptSerializer().Serialize(objdata);

                    List<Script> scriptlist = new List<Script>();
                    System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var client = new RestClient(URL + "api/Master/SCRIPTINVESTMENTMASTER?DBAction=Insert&ID=0");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", BasicAuth);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                }
            }

        }
        public void UpdateScriptMaster(SCRIPT _script, int id)
        {
            var CreatedBy = Session["UserID"];
            if (_script.scriptlist != null)
            {
                for (int i = 0; i < _script.scriptlist.Count(); i++)
                {

                    SCRIPT objdata = new SCRIPT();
                    objdata.ScriptID = id;
                    objdata.ScriptName = _script.ScriptName;
                    objdata.BSECode = _script.BSECode;
                    objdata.NSECode = _script.NSECode;
                    objdata.IndustryID = _script.IndustryID;
                    objdata.GroupName = _script.GroupName;
                    objdata.InvestmentType = _script.scriptlist[0];
                    objdata.IsMcx = _script.IsMcx;
                    objdata.IsCurrency = _script.IsCurrency;
                    objdata.IsNcdx = _script.IsNcdx;
                    objdata.IsFO = _script.IsFO;
                    objdata.FaceValue = _script.FaceValue;
                    objdata.ISIN = _script.ISIN;
                    objdata.MutualFundID = _script.MutualFundID;
                    objdata.SectorName = _script.SectorName;
                    objdata.ListType = _script.ListType;
                    objdata.Active = _script.Active;
                    objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                    var json = new JavaScriptSerializer().Serialize(objdata);
                    List<Script> scriptlist = new List<Script>();
                    System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var client = new RestClient(URL + "api/Master/SCRIPTMASTER?DBAction=Update&ID=" + id);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", BasicAuth);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                }
            }

        }
        public void UpdatescriptInvestment(SCRIPT _script, int ScriptID)
        {
            var CreatedBy = Session["UserID"];
            if (_script.scriptlist != null)
            {
                for (int i = 0; i < _script.scriptlist.Count(); i++)
                {

                    SCRIPT objdata = new SCRIPT();
                    objdata.ScriptID = ScriptID;
                    objdata.InvestmentType = _script.scriptlist[i];
                    objdata.Active = _script.Active;
                    objdata.ModifiedBy = Convert.ToInt32(CreatedBy.ToString());
                    var json = new JavaScriptSerializer().Serialize(objdata);

                    List<Script> scriptlist = new List<Script>();
                    System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var client = new RestClient(URL + "api/Master/SCRIPTINVESTMENTMASTER?DBAction=Update&ID=" + ScriptID);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", BasicAuth);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                }
            }

        }
        [HttpGet]
        public ActionResult ScriptMapping()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ScriptMapping(SCRIPT script)
        {


            // return Redirect("~/Home/Index");
            return View();
        }
        [HttpGet]
        public ActionResult ScriptMapping1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ScriptMapping1(SCRIPT script)
        {
            return View();
        }
    }
}