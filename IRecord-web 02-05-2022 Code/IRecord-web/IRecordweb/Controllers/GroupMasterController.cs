using System;
using System.Collections.Generic;
using DAL;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL;
using RestSharp;
using IRecordweb.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using System.Net;
using System.Data.SqlClient;

namespace IRecordweb.Controllers
{
    public class GroupMasterController : Controller
    {
        DAL.GroupDAL obj = new DAL.GroupDAL();
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        int Flag = 0;
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetListofGroup()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_GROUPDATA]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYUSERID";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = MemberCode;
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
        public JsonResult GetGroupByID(string id)
        {
            string id1 = id.TrimStart('A');
            
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GROUPDATA";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEWBYID";
            cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = Convert.ToInt32(id1);
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = MemberCode;
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
        public ActionResult SaveGroup()
        {
            ViewBag.UGroupName = new SelectList(GroupList().ToList(), dataValueField: "UGroupID", dataTextField: "UGroupName");
            return View();
        }
        [HttpPost]
        public ActionResult SaveGroup(GROUP _Group)
        {
            ViewBag.UGroupName = new SelectList(GroupList().ToList(), dataValueField: "UGroupID", dataTextField: "UGroupName");
            if (ModelState.IsValid)
            {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                GROUP objdata = new GROUP();
                //  objdata.Group_ID = _Group.Group_ID;
                objdata.MemberID = Convert.ToInt32(MemberCode);
                objdata.FinancialYearMemberID = Convert.ToInt32(FinancialYearCode);
                objdata.Group_Name = _Group.Group_Name;
                objdata.UGroupID = Convert.ToInt32(_Group.UGroupName);
                objdata.UGroupName = _Group.UGroupName;
                objdata.Active = _Group.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/GROUPMASTER/?DBAction=Insert&ID=0");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    GROUP item = new GROUP();
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


        [HttpGet]
        public ActionResult EditGroup(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/GROUPMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            GroupModel.Rootobject obj = new GroupModel.Rootobject();
            obj = JsonConvert.DeserializeObject<GroupModel.Rootobject>(response.Content.ToString());
            GROUP Group = new GROUP();
            int SelectedValues = obj.Data[0].UGroupID;
            ViewData["UGroupId"] = SelectedValues;
            ViewBag.UGroupName = new SelectList(GroupList().ToList(), dataValueField: "UGroupID", dataTextField: "UGroupName", selectedValue: SelectedValues.ToString());


            foreach (GroupModel.Datum dr in obj.Data)
            {
                Group.Group_ID = Convert.ToInt32(dr.GroupID.ToString());
                Group.Group_Code = SetDefaultString(dr.Group_Code);
                Group.Group_Name = SetDefaultString(dr.Group_Name);
                Group.UGroupName = SetDefaultString(dr.UGroupName);
                Group.Active = Convert.ToBoolean(dr.Active.ToString());
                Group.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["GroupModel"] = Group;
                ViewBag.UGroup_Name = Group;
            }
            var data = ViewData["GroupModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult EditGroup(GROUP _Group, int ID)
        {
            // ViewBag.UGroupName = new SelectList(GroupList().ToList(), dataValueField: "UGroupID", dataTextField: "UGroupName");
            if (ModelState.IsValid == true || ModelState.IsValid == false)
            {
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();
                var CreatedBy = Session["UserID"];
                GROUP objdata = new GROUP();
                objdata.Group_ID = _Group.Group_ID;
                //objdata.Group_Code = _Group.Group_Code.ToString();
                //objdata.Group_Name = _Group.Group_Name;
                //objdata.UGroupID = Convert.ToInt32(0);
                //objdata.UGroupName = _Group.UGroupName;
                objdata.Active = _Group.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedById.ToString());
                objdata.ModifiedBy = Convert.ToInt32(CreatedById.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/GROUPMASTER/?DBAction=Update&ID=" + ID);
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
                    GROUP item = new GROUP();
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
            return View();

        }
        public ActionResult DeleteGroup(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/GROUPMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            GroupModel.Rootobject obj = new GroupModel.Rootobject();
            obj = JsonConvert.DeserializeObject<GroupModel.Rootobject>(response.Content.ToString());
            GROUP Group = new GROUP();
            foreach (GroupModel.Datum dr in obj.Data)
            {
                Group.Group_ID = Convert.ToInt32(dr.GroupID.ToString());
                Group.Group_Code = SetDefaultString(dr.Group_Code);
                Group.Group_Name = SetDefaultString(dr.Group_Name);
                Group.UGroupName = SetDefaultString(dr.UGroupName);
                Group.Active = Convert.ToBoolean(dr.Active.ToString());
                Group.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["GroupModel"] = Group;
            }
            var data = ViewData["GroupModel"];
            return View(data);
        }
        [HttpPost]
        public ActionResult DeleteGroup(Group _Group, int id)
        {
            Group objdata = new Group();
            objdata.DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            var json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/GROUPMASTER/?DBAction=Delete&ID=" + id);
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
                GROUP item = new GROUP();
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
            return RedirectToAction("Index");
        }
        public ActionResult DetailsGroup(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/GROUPMASTER?DBAction=ViewById&ID=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            GroupModel.Rootobject obj = new GroupModel.Rootobject();
            obj = JsonConvert.DeserializeObject<GroupModel.Rootobject>(response.Content.ToString());
            GROUP Group = new GROUP();
            foreach (GroupModel.Datum dr in obj.Data)
            {
                Group.Group_ID = Convert.ToInt32(dr.GroupID.ToString());
                Group.Group_Code = SetDefaultString(dr.Group_Code);
                Group.Group_Name = SetDefaultString(dr.Group_Name);
                Group.UGroupName = SetDefaultString((dr.UGroupName));
                Group.Active = Convert.ToBoolean(dr.Active.ToString());
                Group.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["GroupModel"] = Group;
            }
            var data = ViewData["GroupModel"];
            return View(data);
        }
        public JsonResult IsUserExistsGroup(string Group_Name)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/CONSULTANTMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<GROUP> GroupList = new List<GROUP>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                GROUP cobj = new GROUP();
                cobj.Group_Name = ds.Tables[0].Rows[i]["Group_Name"].ToString();
                cobj.Group_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["GroupID"].ToString());
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                GroupList.Add(cobj);
            }
            return Json(!obj.GroupList().Any(x => x.Group_Name == Group_Name), JsonRequestBehavior.AllowGet);
        }
        public List<GROUP> GroupList()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/GROUPMASTER?DBAction=ViewUnderGroup&ID=" + MemberCode + ":" + FinancialYearCode);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<GROUP> list = new List<GROUP>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                GROUP item = new GROUP();
                item.UGroupID = Convert.ToInt32(dr["UGroupID"].ToString());
                item.UGroupName = dr["UGroupName"].ToString();
                list.Add(item);
            }
            return list;
        }

        
    }
}