using BAL;
using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
    {
    public class UserController : Controller
        {
       
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: User
        public ActionResult Index()
            {
            return View();
            }
        public List<ROLE> BindRole()
            {
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
        public List<SUBSCRIBER> GetSubscriber()
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SUBSCRIBERMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<SUBSCRIBER> list = new List<SUBSCRIBER>();
            foreach (DataRow dr in data.Tables[0].Rows)
                {
                SUBSCRIBER item = new SUBSCRIBER();
                item.SubscriberID = Convert.ToInt32(dr["SubscriberID"].ToString());
                item.SubscriberName = dr["SubscriberName"].ToString();

                list.Add(item);
                }
            return list;
            }
        [HttpGet]
        public ActionResult SaveUser()
            {
            ViewBag.RoleId = new SelectList(BindRole().ToList(), dataValueField: "RoleId", dataTextField: "Name");
            ViewBag.SubscriberID = new SelectList(GetSubscriber().ToList(), dataValueField: "SubscriberID", dataTextField: "SubscriberName");
            return View();
            }
        [HttpPost]
        public ActionResult SaveUser(User _User)
            {
            ViewBag.RoleId = new SelectList(BindRole().ToList(), dataValueField: "RoleId", dataTextField: "Name");
            ViewBag.SubscriberID = new SelectList(GetSubscriber().ToList(), dataValueField: "SubscriberID", dataTextField: "SubscriberName");
            if (ModelState.IsValid)
                {
                var CreatedBy = Session["UserID"];
                USER objdata = new USER();
                objdata.UserName = _User.UserName;
                objdata.Password = _User.Password;
                objdata.RoleId = _User.RoleId;
                objdata.PAN = _User.PAN;
                objdata.SubscriberID = _User.SubscriberID;
                objdata.IsMobile = _User.IsMobile;
                objdata.Active = _User.Active;
                objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
                var json = new JavaScriptSerializer().Serialize(objdata);
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(URL + "api/Master/USERMASTER?DBAction=Insert&ID=0");
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
                    USER item = new USER();
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
                    }
              //  ViewBag.Message = "Records Save Sucessfully !!";
                }
            return View();
            }

        //public void InsertUserMaster(USER _User)
        //    {
        //    var CreatedBy = Session["UserID"];
        //    USER objdata = new USER();
        //    objdata.UserName = _User.UserName;
        //    objdata.Password = _User.Password;
        //    objdata.RoleId = _User.RoleId;
        //    objdata.PAN = _User.PAN;
        //    objdata.SubscriberID = _User.SubscriberID;
        //    objdata.IsMobile = _User.IsMobile;
        //    objdata.Active = _User.Active;
        //    objdata.CreatedBy = Convert.ToInt32(CreatedBy.ToString());
        //    var json = new JavaScriptSerializer().Serialize(objdata);
        //    var client = new RestClient(URL + "api/Master/USERMASTER?DBAction=Insert&ID=0");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Authorization", BasicAuth);
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    Console.WriteLine(response.Content);
        //    DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
        //    foreach (DataRow dr in data.Tables[0].Rows)
        //        {
        //        USER item = new USER();
        //        item.Code = dr["Code"].ToString();
        //        Session["Code"] = item.Code;
        //        item.Message = dr["Message"].ToString();
        //        Session["Message"] = item.Message;
        //        if (item.Code == "200")
        //            {
        //            ViewBag.Message = item.Message;
        //            }
        //        else
        //            {
        //            ViewBag.Message = item.Message;
        //            }
        //        }
        //    }

        public ActionResult UserRoleAssign()
        {
            return View();
        }
    }
    }