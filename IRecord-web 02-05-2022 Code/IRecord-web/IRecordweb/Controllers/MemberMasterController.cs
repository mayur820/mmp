using BAL;
using IRecordweb.Models;
using Newtonsoft.Json;
using PagedList;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace IRecordweb.Controllers
{

    public class MemberMasterController : Controller
    {
        private readonly string URL = ConfigurationManager.AppSettings["ScreenURL"];
        private readonly string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: MemberMaster
        public ActionResult Index(int? page)
        {
            Member mobj = new Member();
            FinancialYear fobj = new FinancialYear();
            object ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol =
           SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=ViewByUserId&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MEMBER> list = new List<MEMBER>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                MEMBER item = new MEMBER
                {
                    MemberID = Convert.ToInt32(dr["MemberID"].ToString()),
                    FamilyID = Convert.ToInt32(dr["FamilyID"].ToString()),
                    Code = dr["Code"].ToString(),
                    Gender = Convert.ToInt32(dr["Gender"].ToString()),
                    MemberName = dr["MemberName"].ToString(),
                    Address = dr["Address"].ToString(),
                    ServTax_No = dr["ServTax_No"].ToString(),
                    AadharCardNo = dr["AadharCardNo"].ToString(),
                    PAN = dr["PAN"].ToString(),
                    Active = Convert.ToBoolean(dr["Active"].ToString()),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString())
                };

                list.Add(item);
            }
            MEMBER member = new MEMBER
            {
                ShowMember = list
            };
            List<MEMBER> dtdata = member.ShowMember;
            return View(dtdata.ToList().ToPagedList(page ?? 1, 5));
        }

        public List<FamilyModel> FamilyList()
        {
            object ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol =
           SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

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
                    //Active = Convert.ToBoolean(dr["Active"].ToString()),
                    //CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString())
                };

                list.Add(item);
            }
            return list;
        }

        public List<FamilyModel> FamilyList1()
        {
            string SubscriberID = Session["UserID"].ToString();
            //int UserID = GetUserID(Convert.ToInt32(SubscriberID.ToString()));
            //var ID = UserID;
            System.Net.ServicePointManager.SecurityProtocol =
                      SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=ViewByUserId&ID=" + SubscriberID)
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
                    //Active = Convert.ToBoolean(dr["Active"].ToString()),
                    //CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString())
                };

                list.Add(item);
            }
            return list;
        }

        public int GetUserID(int ID)
        {
            System.Net.ServicePointManager.SecurityProtocol =
          SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            RestClient client = new RestClient(URL + "api/Master/FAMILYMASTER?DBAction=GetUserID&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
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

        [HttpGet]
        public ActionResult SaveMember()
        {
            ViewBag.FamilyID = new SelectList(FamilyList().ToList(), dataValueField: "FamilyID", dataTextField: "Name");
            int MemberCount = Sessionval();
            int subscription = Convert.ToInt32(Session["MemberSubscription1"].ToString());
            if (MemberCount >= subscription)
            {
                TempData["Message"] = "Subscription Limits Exceeded !!";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public int Sessionval()
        {
            object Id = Session["UserID"];
            Member member = new Member();
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=MemberCountById&ID=" + Id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            // Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MEMBER> list = new List<MEMBER>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                Session["UserID"] = dr["UserId"];
                Session["UserName"] = dr["UserName"];
                Session["Password"] = dr["Password"];
                Session["SubscriberID"] = dr["SubscriberID"];
                Session["MemberSubscription1"] = dr["MemberSubscription"];
                Session["MemberCount1"] = dr["MemberCount"];
                Session["RoleId"] = dr["RoleId"];
                Session["RoleName"] = dr["Name"];
            }
            int MemberCount = Convert.ToInt32(Session["MemberCount1"].ToString());
            return MemberCount;
        }
        [HttpPost]
        public ActionResult SaveMember(MEMBER _Member, HttpPostedFileBase ReportLogoPath)
        {
            ViewBag.FamilyID = new SelectList(FamilyList().ToList(), dataValueField: "FamilyID", dataTextField: "Name");
            if (ModelState.IsValid)
            {
                if (ReportLogoPath != null || ReportLogoPath.ContentLength < (1024 * 1024))    //1 MB = 1048576 bytes 
                {

                    //Extract Image File Name.
                    string fileName = Path.GetFileName(ReportLogoPath.FileName);
                    string Extension = Path.GetExtension(fileName);

                    if (Extension.ToString() == ".png" || Extension.ToString() == ".svg" || Extension.ToString() == ".eps" || Extension.ToString() == ".pdf" || Extension.ToString() == ".jpg")
                    {
                        //Set the Image File Path.
                        string filePath = "~/Files/Logo/" + fileName;
                        Session["Path"] = filePath;
                        string filesize = ReportLogoPath.ContentLength.ToString();
                        ReportLogoPath.SaveAs(Server.MapPath(filePath));

                        object CreatedBy = Session["UserID"];
                        MEMBER objdata = new MEMBER
                        {
                            FamilyID = _Member.FamilyID,
                            MemberName = _Member.MemberName,
                            Address = _Member.Address,
                            Gender = _Member.Gender,
                            ServTax_No = _Member.ServTax_No,
                            AadharCardNo = _Member.AadharCardNo,
                            ReportLogoPathData = filePath,
                            PAN = _Member.PAN,
                            Active = _Member.Active,
                            CreatedBy = Convert.ToInt32(CreatedBy.ToString())
                        };
                        string json = new JavaScriptSerializer().Serialize(objdata);
                        System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=Insert&ID=0")
                        {
                            Timeout = -1
                        };
                        RestRequest request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", BasicAuth);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);

                        // obj.InsertMemberInUserMaster(_Member);
                        DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
                        foreach (DataRow dr in data.Tables[0].Rows)
                        {
                            MEMBER item = new MEMBER
                            {
                                Code = dr["Code"].ToString()
                            };
                            // item.Message = dr["Message"].ToString();
                            if (item.Code == "500")
                            {
                                ViewBag.Message = item.Message;
                                return View();
                            }
                            else
                            {
                                ViewBag.Message = "Record Insert Successfully !!";
                                return RedirectToAction("Index");
                            }
                        }

                        return RedirectToAction("Index");

                    }

                    else
                    {
                        ViewBag.Message = "Please upload valid extension file";
                    }
                }
                else
                {
                    ViewBag.Message = "Maximum logo size exceeded";
                }

                //}

            }
            //else
            //    {
            //    ViewBag.Message = "Member Name Already Exist!";
            //    }
            //}

            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.FamilyID = new SelectList(FamilyList().ToList(), dataValueField: "FamilyID", dataTextField: "Name");
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=ViewById&ID=" + id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            MemberModel.Rootobject objj = new MemberModel.Rootobject();
            objj = JsonConvert.DeserializeObject<MemberModel.Rootobject>(response.Content.ToString());
            MEMBER member = new MEMBER();
            foreach (MemberModel.Datum dr in objj.Data)
            {
                member.MemberID = Convert.ToInt32(dr.MemberID.ToString());
                member.FamilyID = Convert.ToInt32(dr.FamilyID.ToString());
                member.Code = dr.Code.ToString();
                member.MemberName = dr.MemberName.ToString();
                member.Address = dr.Address.ToString();
                member.Gender = Convert.ToInt32(dr.Gender.ToString());
                member.ServTax_No = dr.ServTax_No.ToString();
                member.AadharCardNo = dr.AadharCardNo.ToString();
                member.PAN = dr.PAN.ToString();
                member.UserId = Convert.ToInt32(dr.UserId.ToString());
                member.Active = Convert.ToBoolean(dr.Active.ToString());
                member.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["MEMBERDATA"] = member;
            }
            object data = ViewData["MEMBERDATA"];

            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(MEMBER _Member, int id, HttpPostedFileBase ReportLogoPath)
        {
            ViewBag.FamilyID = new SelectList(FamilyList().ToList(), dataValueField: "FamilyID", dataTextField: "Name");
            if (!string.IsNullOrWhiteSpace(_Member.MemberName))
            {
                if (ReportLogoPath != null || ReportLogoPath.ContentLength < (1024 * 1024))    //1 MB = 1048576 bytes 
                {
                    string fileName = Path.GetFileName(ReportLogoPath.FileName);
                    string Extension = Path.GetExtension(fileName);

                    if (Extension.ToString() == ".png" || Extension.ToString() == ".svg" || Extension.ToString() == ".eps" || Extension.ToString() == ".pdf" || Extension.ToString() == ".jpg")
                    {

                        string filePath = "~/Files/Logo/" + fileName;
                        Session["Path"] = filePath;
                        string filesize = ReportLogoPath.ContentLength.ToString();

                        ReportLogoPath.SaveAs(Server.MapPath(filePath));

                        //  obj.UpdateMemberByID(_Member, ReportLogoPath, id);
                        object CreatedBy = Session["UserID"];
                        MEMBER objdata = new MEMBER
                        {
                            MemberID = id,
                            FamilyID = _Member.FamilyID,
                            MemberName = _Member.MemberName,
                            Address = _Member.Address,
                            Gender = _Member.Gender,
                            ServTax_No = _Member.ServTax_No,
                            AadharCardNo = _Member.AadharCardNo,
                            ReportLogoPathData = filePath,
                            PAN = _Member.PAN,
                            Active = _Member.Active,
                            ModifiedBy = Convert.ToInt32(CreatedBy.ToString())
                        };
                        string json = new JavaScriptSerializer().Serialize(objdata);
                        System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=Update&ID=" + id)
                        {
                            Timeout = -1
                        };
                        RestRequest request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", BasicAuth);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);

                        ViewBag.Message = "Records Updated Sucessfully !!";

                    }
                    else
                    {
                        ViewBag.Message = "Please upload valid extension file";
                    }
                }
                else
                {
                    ViewBag.Message = "Maximum logo size exceeded";
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=ViewById&ID=" + id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            MemberModel.Rootobject obj = new MemberModel.Rootobject();
            obj = JsonConvert.DeserializeObject<MemberModel.Rootobject>(response.Content.ToString());
            MEMBER member = new MEMBER();
            foreach (MemberModel.Datum dr in obj.Data)
            {
                member.MemberID = Convert.ToInt32(dr.MemberID.ToString());
                member.FamilyID = Convert.ToInt32(dr.FamilyID.ToString());
                member.Code = dr.Code.ToString();
                member.MemberName = dr.MemberName.ToString();
                member.Address = dr.Address.ToString();
                member.Gender = Convert.ToInt32(dr.Gender.ToString());
                member.ServTax_No = dr.ServTax_No.ToString();
                member.AadharCardNo = dr.AadharCardNo.ToString();
                member.PAN = dr.PAN.ToString();
                member.UserId = Convert.ToInt32(dr.UserId.ToString());
                member.Active = Convert.ToBoolean(dr.Active.ToString());
                member.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["MEMBERDATA"] = member;
            }
            object data = ViewData["MEMBERDATA"];

            return View(data);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=ViewById&ID=" + id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            MemberModel.Rootobject obj = new MemberModel.Rootobject();
            obj = JsonConvert.DeserializeObject<MemberModel.Rootobject>(response.Content.ToString());
            MEMBER member = new MEMBER();
            foreach (MemberModel.Datum dr in obj.Data)
            {
                member.MemberID = Convert.ToInt32(dr.MemberID.ToString());
                member.FamilyID = Convert.ToInt32(dr.FamilyID.ToString());
                member.Code = dr.Code.ToString();
                member.MemberName = dr.MemberName.ToString();
                member.Address = dr.Address.ToString();
                member.Gender = Convert.ToInt32(dr.Gender.ToString());
                member.ServTax_No = dr.ServTax_No.ToString();
                member.AadharCardNo = dr.AadharCardNo.ToString();
                member.PAN = dr.PAN.ToString();
                member.UserId = Convert.ToInt32(dr.UserId.ToString());
                member.Active = Convert.ToBoolean(dr.Active.ToString());
                member.CreatedDate = Convert.ToDateTime(dr.CreatedDate.ToString());
                ViewData["MEMBERDATA"] = member;
            }
            object data = ViewData["MEMBERDATA"];

            return View(data);

        }
        [HttpPost]
        public ActionResult Delete(Member _Member, int id)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=Delete&ID=" + id)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            ViewBag.Message = "Records Deleted Successfully !!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetDetailList()
        {
            MEMBER mobj = new MEMBER();
            FINANCIALYEAR fobj = new FINANCIALYEAR();
            ViewBag.MemberName = new SelectList(BindMemberList(mobj).ToList(), dataValueField: "MemberID", dataTextField: "MemberName");
            ViewBag.FinancialYear = new SelectList(BindFinancialYearList().ToList(), dataValueField: "FinancialYearID", dataTextField: "Code");
            return View();
        }
        [HttpPost]
        public ActionResult GetDetailList(FINANCIALYEARTRANS _FinancialYearTrans)
        {
            MEMBER mobj = new MEMBER();
            FinancialYear fobj = new FinancialYear();
            ViewBag.MemberName = new SelectList(BindMemberList(mobj).ToList(), dataValueField: "MemberID", dataTextField: "MemberName");
            ViewBag.FinancialYear = new SelectList(BindFinancialYearList().ToList(), dataValueField: "FinancialYearID", dataTextField: "Code");
            if (ModelState.IsValid)
            {
                if (_FinancialYearTrans.FinancialYearID != null && _FinancialYearTrans.MemberID != null)
                {
                    InsertMemberFinYrData(_FinancialYearTrans);
                    return Redirect("~/Home/Index");
                }
                else
                {
                    ViewBag.Message = "Select Member & Finacial Year";
                }
                // ViewBag.Message = "Records Save Sucessfully !!";
            }

            return View();
        }
        public List<MEMBER> BindMemberList(MEMBER _Member)
        {
            //            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            //delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //               System.Security.Cryptography.X509Certificates.X509Chain chain,
            //               System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //return true; // **** Always accept
            //         };
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=ViewMemberList&ID=0")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MEMBER> list = new List<MEMBER>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                MEMBER item = new MEMBER
                {
                    MemberID = Convert.ToInt32(dr["MemberID"].ToString()),
                    Code = dr["Code"].ToString(),
                    MemberName = dr["MemberName"].ToString()
                };
                list.Add(item);
            }
            return list;
        }
        public List<FINANCIALYEAR> BindFinancialYearList()
        {
            //            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            //delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //               System.Security.Cryptography.X509Certificates.X509Chain chain,
            //               System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //return true; // **** Always accept
            //         };
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=ViewFinYearList&ID=0")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<FINANCIALYEAR> list = new List<FINANCIALYEAR>();
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                FINANCIALYEAR item = new FINANCIALYEAR
                {
                    FinancialYearID = Convert.ToInt32(dr["FinancialYearID"].ToString()),
                    Code = dr["Code"].ToString()
                };
                //  item.MemberName = dr["MemberName"].ToString();
                list.Add(item);
            }
            return list;
        }

        public void InsertMemberFinYrData(FINANCIALYEARTRANS _FinancialYearTrans)
        {
            object CreatedBy = Session["UserID"];
            FINANCIALYEARTRANS objdata = new FINANCIALYEARTRANS
            {
                FinancialYearID = _FinancialYearTrans.FinancialYearID,
                MemberID = _FinancialYearTrans.MemberID,
                Active = _FinancialYearTrans.Active,
                CreatedBy = Convert.ToInt32(CreatedBy.ToString())
            };
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERFINYEARMASTER?DBAction=InsertOrDelete&ID=0")
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

                Session["FinincialYearID"] = _FinancialYearTrans.FinancialYearID;
                Session["MemberID"] = _FinancialYearTrans.MemberID;
                Session["FinancialYearMemberID"] = dr["FinancialYearMemberID"];

            }
        }
        public ActionResult Home()
        {
            return View();
        }
        public JsonResult IsUserExists(string MemberName)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=View&ID=0")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            IRestResponse response = client.Execute(request);
            // Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MEMBER> MemberList = new List<MEMBER>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                MEMBER cobj = new MEMBER
                {
                    MemberID = Convert.ToInt32(ds.Tables[0].Rows[i]["MemberID"].ToString()),
                    FamilyID = Convert.ToInt32(ds.Tables[0].Rows[i]["FamilyID"].ToString()),
                    MemberName = ds.Tables[0].Rows[i]["MemberName"].ToString(),
                    Code = ds.Tables[0].Rows[i]["Code"].ToString(),
                    Gender = Convert.ToInt32(ds.Tables[0].Rows[i]["Gender"].ToString()),
                    ServTax_No = ds.Tables[0].Rows[i]["ServTax_No"].ToString(),
                    AadharCardNo = ds.Tables[0].Rows[i]["AadharCardNo"].ToString(),
                    PAN = ds.Tables[0].Rows[i]["PAN"].ToString(),
                    Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString()),
                    CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString())
                };
                MemberList.Add(cobj);
            }
            //check if any of the UserName matches the MemberName specified in the Parameter using the ANY extension method.  
            return Json(!MemberList.Any(x => x.MemberName == MemberName), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult OnboardingMember()
        {
            ViewBag.FamilyID = new SelectList(FamilyList1().ToList(), dataValueField: "FamilyID", dataTextField: "Name");
            int MemberCount;
            if (Session["FinCount"]!=null)
            { 
                 MemberCount = Convert.ToInt32(Session["FinCount"].ToString());
            }
            else
            { 
                 MemberCount = Convert.ToInt32(Session["onMemberCount"].ToString());
            }
        int subscription = Convert.ToInt32(Session["onMemberSubscription"].ToString());
            if (MemberCount == subscription)
            {
                return Redirect("~/MemberMaster/Home");
            }
            else { 
            return View();
                }
        }
        [HttpPost]
        public ActionResult OnboardingMember(MEMBER _Member, string EmailOTP, string VERIFY)
        {

            ViewBag.FamilyID = new SelectList(FamilyList1().ToList(), dataValueField: "FamilyID", dataTextField: "Name");
             int SubscriberID = Convert.ToInt32(Session["UserID"].ToString());
            //int UserID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = GetUserID(Convert.ToInt32(UserID.ToString()));
             int UserID = SubscriberID;
            int CreatedBy = UserID;
            MEMBER objdata = new MEMBER
            {
                FamilyID = _Member.FamilyID,
                MemberName = _Member.MemberName,
                Address = "",
                Gender = 0,
                ServTax_No = "",
                AadharCardNo = "",
                ReportLogoPathData = "",
                PAN = _Member.PAN,
                EmailID = _Member.EmailID,
                Client_Code = _Member.Client_Code,
                Active = _Member.Active,
                SubscriberID = Convert.ToInt32(SubscriberID.ToString()),
                CreatedBy = Convert.ToInt32(CreatedBy.ToString())
            };

            if (!string.IsNullOrEmpty(EmailOTP))
            {
                string EOTP = VerifyEmail(_Member, SubscriberID, UserID);
                _Member.EmailID = objdata.EmailID;
                _Member.EmailOTP = EOTP;
                Session["EmailOTPDT"] = _Member.EmailOTP;
                Models.EmailOTP.OnbMail(_Member);


            }

            else
            {

                int MemberCount = 0;
               
                MemberCount= Convert.ToInt32(Session["onMemberCount"].ToString());
                int subscription = Convert.ToInt32(Session["onMemberSubscription"].ToString());
               
                if (MemberCount > subscription)
                {
                    ViewBag.Message = "Subscription limits omitted !!";
                    return View();
                }
                else if (MemberCount == subscription)
                {
                    Session["vVal"] = 1;
                    TempData["Val1"] = Session["vVal"].ToString();
                    return Redirect("~/MemberMaster/Home");
                    //  return Redirect("~/Dashboard/Index");
                }
                else
                {
                    objdata.MemberID = Convert.ToInt32(Session["ONCreate_MemberID"].ToString());
                    string json = new JavaScriptSerializer().Serialize(objdata);
                    System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=OnbInsert&ID=0")
                    {
                        Timeout = -1
                    };
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", BasicAuth);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    // obj.InsertMemberInUserMaster(_Member);
                    DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);

                    foreach (DataRow dr in data.Tables[0].Rows)
                    {
                        MEMBER item = new MEMBER
                        {
                            Code = dr["Code"].ToString()
                        };


                        if (item.Code == "500")
                        {
                            item.Message = dr["Message"].ToString();
                            ViewBag.Message = item.Message;
                            return View();
                        }
                        else
                        {
                            string MemberId = "0";
                            string FinYear = "0";
                            string MemberBrokerId = "0";
                            string MemberBankId = "0";
                           
                            DataTable Dt_DefaultList = new DataTable();
                            MemberId = dr["MemberId"].ToString();
                            MemberCount = Convert.ToInt32(dr["MemberCount"].ToString());
                            Session["FinCount"] = MemberCount;
                           
                            try
                            {
                                Dt_DefaultList = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Session["JsonDefaultValues"].ToString());
                            }
                            catch
                            {

                            }

                            try
                            {
                                FinYear = InsertFinancialYear(MemberId);
                            }
                            catch { }
                            try
                            {

                                DataTable dt = InsertGroupMaster(MemberId, FinYear);
                                InsertDefaultAccount(MemberId, FinYear);
                                MemberBrokerId = dt.Rows[0]["MemberBrokerId"].ToString();
                                MemberBankId = dt.Rows[0]["MemberBankId"].ToString();
                            }
                            catch { }

                            try
                            {
                                SaveBrokerWithDemat(MemberId, FinYear, MemberBrokerId, MemberBankId);
                            }
                            catch { }
                            try
                            {
                                SaveDemat(MemberId, FinYear, MemberBrokerId, MemberBankId);
                            }
                            catch { }
                            try
                            {
                                SaveBank(MemberId, FinYear, MemberBrokerId, MemberBankId);
                            }
                            catch { }
                            try
                            {
                                SaveAdvisor(MemberId, FinYear, MemberBrokerId, MemberBankId);
                            }
                            catch { }
                            try
                            {
                                SetDefault_BrokerWithDemat(MemberId, FinYear, Dt_DefaultList.Rows[0]["BrokerWithDemate"].ToString().Split(':')[2].ToString());
                            }
                            catch { }
                            try
                            {
                                SetDefault_Demat(MemberId, FinYear, Dt_DefaultList.Rows[0]["Demate"].ToString().Split(':')[2].ToString());
                            }
                            catch { }
                            try
                            {
                                SetDefault_Bank(MemberId, FinYear, Dt_DefaultList.Rows[0]["BankAc"].ToString().Split(':')[2].ToString());
                            }
                            catch { }
                            try
                            {
                                SetDefault_Advisor(MemberId, FinYear, Dt_DefaultList.Rows[0]["Advisor"].ToString().Split(':')[2].ToString());
                            }
                            catch { }
                            if (MemberCount == subscription)
                            {
                                return Redirect("~/Login/MemberLogin");
                            }
                            ViewBag.Message = "Record Insert Successfully !!";
                            // return Redirect("~/Dashboard/Index"); 
                            // return Redirect("~/OpeningStock/OnboardingOpStock");
                            // return Redirect("~/BrokerBillEntry/OnboardingContractNote");
                            // return Redirect("~/ScriptMaster/ScriptMapping");
                            // return Redirect("~/ImportTradefileContract/OnboardingTrade");
                        }
                    }
                }
            }
          //  MEMBER MDATA = new MEMBER();
            return View("OnboardingMember", null);
        }


        public JsonResult VerifyOTP(string VERIFY, MEMBER _Member, string txtEmailOTP)

        {
            MEMBER _member = new MEMBER();
             int SubscriberID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = Convert.ToInt32(Session["UserID"].ToString());
            //int UserID = GetUserID(Convert.ToInt32(SubscriberID.ToString()));
             int UserID = SubscriberID;

            if (!string.IsNullOrEmpty(VERIFY))
            {
                // string EOTP = VerifyEmail(_Member,SubscriberID, UserID);
                _Member.EmailOTP = txtEmailOTP.ToString();

                // Models.EmailOTP.OnbMail(_Member);
                _Member.Action = "MatchEmailOTPMem";
                _Member.SubscriberID = Convert.ToInt32(UserID.ToString());
                InsertOTPDT(_Member);
                if (Session["Message"] != null)
                {
                    TempData["OTPMessage1"] = Session["Message"];
                    ViewBag.Message = Session["Message"];
                }
                return Json(Session["Message"].ToString(), JsonRequestBehavior.AllowGet);

            }
            return Json(Session["Message"].ToString(), JsonRequestBehavior.AllowGet);
        }

        public string InsertFinancialYear(string MemberID)
        {
            string DefaultValues = Session["JsonDefaultValues"] as string;
            DataTable dt_ListOfBrokerWithDemate = new DataTable();
            if (DefaultValues != "[]")
            {

                dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(DefaultValues);

            }
            string FinyearId = dt_ListOfBrokerWithDemate.Rows[0]["FinancialYear"].ToString().Split(':')[1].ToString();
            //  DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[SP_FinincialYear_UTITLIY]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Insert";
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = MemberID;
            cmd.Parameters.Add("@FinincialYearId", SqlDbType.NVarChar).Value = FinyearId;

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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            return DT.Rows[0]["ID"].ToString();



        }

        public DataTable InsertGroupMaster(string MemberID, string FinancialYearMemberId)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[SP_Initialize_Member]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Initialize_Group";
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberID;
            cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;

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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            return DT;



        }
        public DataTable InsertDefaultAccount(string MemberID, string FinancialYearMemberId)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[SP_Initialize_Member]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Initialize_Default_Ac";
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberID;
            cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;

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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            return DT;



        }
        public void VerifyDate()
        {
            object ID = Session["SubscriberID"];
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=VerifyDate&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                SUBSCRIBER item = new SUBSCRIBER
                {
                    OTPVerifiedDate = Convert.ToDateTime(dr["OTPVerifiedDate"].ToString())
                };
                Session["OTPVerifiedDate"] = item.OTPVerifiedDate;
            }
        }
        public void InsertOTPDT(MEMBER _member)
        {

            // string SubscriberID = Session["UserID"].ToString();
            string SubscriberID = Session["UserID"].ToString();
            MEMBER objdata = new MEMBER
            {
                EmailOTP = _member.EmailOTP
            };

            string json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=" + _member.Action + "&ID=" + _member.SubscriberID)
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
                SUBSCRIBER item = new SUBSCRIBER();
                objdata.Code = dr["Code"].ToString();
                Session["Code"] = item.Code;
                objdata.Message = dr["Message"].ToString();
                Session["Message"] = objdata.Message;
            }
        }
        public string VerifyEmail(MEMBER _Member, int SubscriberId, int UserID)
        {
            MEMBER objdata = new MEMBER
            {
                FamilyID = _Member.FamilyID,
                MemberName = "",// _Member.MemberName;
                Address = "",
                Gender = 0,
                ServTax_No = "",
                AadharCardNo = "",
                ReportLogoPathData = "",
                PAN = _Member.PAN,
                EmailID = _Member.EmailID,
                Active = _Member.Active,

                SubscriberID = SubscriberId,
                CreatedBy = UserID
            };

            string json = new JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/MEMBERMASTER?DBAction=InsertOTP&ID=0")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                MEMBER item = new MEMBER
                {
                    EmailOTP = dr["EmailOTP"].ToString()
                };
                Session["EmailOTP"] = item.EmailOTP;
                item.MemberID = Convert.ToInt32(dr["MemberID"].ToString());
                Session["ONCreate_MemberID"] = item.MemberID;
            }
            string EmailOTP = Session["EmailOTP"].ToString();
            return EmailOTP;

        }
        public List<ImportTradefileContract_INDEX_Models> BindBrokerFormat1(int ID)
        {

            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(URL + "api/Master/BROKERFORMATMASTER?DBAction=BrokerBillConfigList&ID=" + ID)
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<ImportTradefileContract_INDEX_Models> BrokerFormatList = new List<ImportTradefileContract_INDEX_Models>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ImportTradefileContract_INDEX_Models cobj = new ImportTradefileContract_INDEX_Models
                {
                    BrokerId = Convert.ToInt32(ds.Tables[0].Rows[i]["Sr_No"].ToString()),
                    Broker = ds.Tables[0].Rows[i]["Name"].ToString()
                };
                Session["BrokerConfig"] = cobj.Broker;
                Session["BrokerConfig"] = cobj.Broker;
                cobj.FileType = ds.Tables[0].Rows[i]["FileType"].ToString();
                cobj.isTradeFile = ds.Tables[0].Rows[i]["isTradefile"].ToString();
                Session["isTradeFile"] = cobj.isTradeFile;
                BrokerFormatList.Add(cobj);
            }
            return BrokerFormatList;
        }
        public JsonResult GetBroker()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_OnboardingMember_Config]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewBroker";

            cmd.Connection = con;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        public JsonResult GetDemate()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_OnboardingMember_Config]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewDemate";
            cmd.Connection = con;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        public JsonResult GetBank()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_OnboardingMember_Config]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewBank";
            cmd.Connection = con;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        public JsonResult GetConsultant()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_OnboardingMember_Config]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewConsultant";
            cmd.Connection = con;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

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
        public JsonResult SetMultiConfiguration(string JsonDefaultValues, string ListOfBrokerWithDemate, string ListOfDemate, string ListOfBankAc, string ListOfAdvisor)
        {

            //DataTable dt_JsonDefaultValues = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonDefaultValues);
            //DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBrokerWithDemate);
            //DataTable dt_ListOfDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfDemate);
            //DataTable dt_ListOfBankAc = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBankAc);
            //DataTable dt_ListOfAdvisor = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfAdvisor);

            Session["OnBorValidMember"] = "1";
            Session["JsonDefaultValues"] = JsonDefaultValues;
            Session["ListOfBrokerWithDemate"] = ListOfBrokerWithDemate;//
            Session["ListOfDemate"] = ListOfDemate;//
            Session["ListOfBankAc"] = ListOfBankAc;
            Session["ListOfAdvisor"] = ListOfAdvisor;
            return Json("1", JsonRequestBehavior.AllowGet);
        }
        public void SetDefault_BrokerWithDemat(string MemberId, string FinancialYearMemberId, string AcId)
        {



            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_Initialize_Member]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Set_Default_Broker_Demate";
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberId;
            cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = AcId;


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            DataTable DT = new DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        public void SetDefault_Demat(string MemberId, string FinancialYearMemberId, string AcId)
        {



            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_Initialize_Member]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Set_Default_Demate";
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberId;
            cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = AcId;


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            DataTable DT = new DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        public void SetDefault_Bank(string MemberId, string FinancialYearMemberId, string AcId)
        {



            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_Initialize_Member]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Set_Default_bank";
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberId;
            cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = AcId;


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            DataTable DT = new DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        public void SetDefault_Advisor(string MemberId, string FinancialYearMemberId, string AcId)
        {



            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_Initialize_Member]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Set_Default_Advisor";
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberId;
            cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = AcId;


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            DataTable DT = new DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
        public void SaveBrokerWithDemat(string MemberId, string FinancialYearMemberId, string MembergroupBrokerId, string MembergroupBankId)
        {
            string ListOfBrokerWithDemate = Session["ListOfBrokerWithDemate"] as string;
            if (ListOfBrokerWithDemate != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBrokerWithDemate);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_Initialize_Member]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Initialize_Broker_Demate";
                    cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberId;
                    cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;
                    cmd.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = dr["ClientCode"].ToString();
                    cmd.Parameters.Add("@BrokerID", SqlDbType.Int).Value = dr["brokerId"].ToString().Split(':')[1];
                    cmd.Parameters.Add("@DemateId", SqlDbType.Int).Value = dr["DemateId"].ToString().Split(':')[1];
                    cmd.Parameters.Add("@MembergroupBrokerId", SqlDbType.Int).Value = MembergroupBrokerId;
                    cmd.Parameters.Add("@MembergroupBankId", SqlDbType.Int).Value = MembergroupBankId;

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }

        public void SaveDemat(string MemberId, string FinancialYearMemberId, string MembergroupBrokerId, string MembergroupBankId)
        {
            string ListOfBrokerWithDemate = Session["ListOfDemate"] as string;
            if (ListOfBrokerWithDemate != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBrokerWithDemate);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_Initialize_Member]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Initialize_Demate";
                    cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberId;
                    cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;
                    //  cmd.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = dr["ClientCode"].ToString();
                    // cmd.Parameters.Add("@BrokerID", SqlDbType.Int).Value = dr["brokerId"].ToString().Split(':')[1];
                    cmd.Parameters.Add("@DemateId", SqlDbType.Int).Value = dr["DemateId"].ToString().Split(':')[1];
                    cmd.Parameters.Add("@MembergroupBrokerId", SqlDbType.Int).Value = MembergroupBrokerId;
                    cmd.Parameters.Add("@MembergroupBankId", SqlDbType.Int).Value = MembergroupBankId;

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
        public string InsertAdvisor(string name, string Mobilenumber, string Emailid, string MemberId)
        {
            #region


            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_CONSULTANTDATA]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "INSERT";
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberId;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;

            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = Mobilenumber;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Emailid;
            cmd.Parameters.Add("@Active", SqlDbType.VarChar).Value = "1";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Session["UserID"].ToString();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            DataTable DT = new DataTable();
            try
            {
                //con.Open();
                /////Only Use Case Of Mapping
                //string query = cmd.CommandText;
                //foreach (SqlParameter p in cmd.Parameters)
                //{
                //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                //}
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            #endregion
            return DT.Rows[0]["ROW_ID"].ToString();
        }
        public void SaveBank(string MemberId, string FinancialYearMemberId, string MembergroupBrokerId, string MembergroupBankId)
        {
            string ListOfBrokerWithDemate = Session["ListOfBankAc"] as string;
            if (ListOfBrokerWithDemate != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBrokerWithDemate);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_Initialize_Member]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Initialize_Bank";
                    cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberId;
                    cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;
                    //  cmd.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = dr["ClientCode"].ToString();
                    // cmd.Parameters.Add("@BrokerID", SqlDbType.Int).Value = dr["brokerId"].ToString().Split(':')[1];
                    cmd.Parameters.Add("@bankId", SqlDbType.Int).Value = dr["BankId"].ToString().Split(':')[1];
                    cmd.Parameters.Add("@MembergroupBrokerId", SqlDbType.Int).Value = MembergroupBrokerId;
                    cmd.Parameters.Add("@MembergroupBankId", SqlDbType.Int).Value = MembergroupBankId;

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
        public void SaveAdvisor(string MemberId, string FinancialYearMemberId, string MembergroupBrokerId, string MembergroupBankId)
        {
            string ListOfBrokerWithDemate = Session["ListOfAdvisor"] as string;
            if (ListOfBrokerWithDemate != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBrokerWithDemate);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region
                    string AdviorId = InsertAdvisor(dr["AdvisorName"].ToString(), dr["Mobile"].ToString(), dr["EmailID"].ToString(), MemberId);
                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_Initialize_Member]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Initialize_Advisor";
                    cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberId;
                    cmd.Parameters.Add("@FinancialYearMemberId", SqlDbType.Int).Value = FinancialYearMemberId;
                    //  cmd.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = dr["ClientCode"].ToString();
                    // cmd.Parameters.Add("@BrokerID", SqlDbType.Int).Value = dr["brokerId"].ToString().Split(':')[1];
                    cmd.Parameters.Add("@Advisorid", SqlDbType.Int).Value = AdviorId;
                    cmd.Parameters.Add("@MembergroupBrokerId", SqlDbType.Int).Value = MembergroupBrokerId;
                    cmd.Parameters.Add("@MembergroupBankId", SqlDbType.Int).Value = MembergroupBankId;

                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }

        public void SaveListOfDemate(string MemberId)
        {
            string ListOfBrokerWithDemate = Session["ListOfDemate"] as string;
            if (ListOfBrokerWithDemate != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBrokerWithDemate);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_Member_Brokerbill_Formatinfo]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@Member_Code", SqlDbType.VarChar).Value = MemberId;
                    cmd.Parameters.Add("@BrokerBill_Format_No", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UseasDefault", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@InvestmentType", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@Selection", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@DematAcID", SqlDbType.VarChar).Value = dr["DemateId"].ToString().Split(':')[1];
                    cmd.Parameters.Add("@CreateBy", SqlDbType.Int).Value = Session["SubscriberID"].ToString();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
        public void SaveJsonDefaultValuesBrokerWithDemat(string MemberId)
        {
            string ListOfBrokerWithDemate = Session["JsonDefaultValues"] as string;
            if (ListOfBrokerWithDemate != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Session["JsonDefaultValues"].ToString());
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_Member_Brokerbill_Formatinfo]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DefaultBrokerWithDemat";
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@Member_Code", SqlDbType.VarChar).Value = MemberId;
                    cmd.Parameters.Add("@BrokerBill_Format_No", SqlDbType.Int).Value = dr["BrokerWithDemate"].ToString().Split(':')[2];
                    cmd.Parameters.Add("@UseasDefault", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@InvestmentType", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@Selection", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@DematAcID", SqlDbType.VarChar).Value = "0";
                    cmd.Parameters.Add("@CreateBy", SqlDbType.Int).Value = Session["SubscriberID"].ToString();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
        public void SaveJsonDefaultDemat(string MemberId)
        {
            string ListOfBrokerWithDemate = Session["JsonDefaultValues"] as string;
            if (ListOfBrokerWithDemate != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBrokerWithDemate);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_Member_Brokerbill_Formatinfo]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DefaultDemat";
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@Member_Code", SqlDbType.VarChar).Value = MemberId;
                    cmd.Parameters.Add("@BrokerBill_Format_No", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@UseasDefault", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@InvestmentType", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@Selection", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@DematAcID", SqlDbType.VarChar).Value = dr["Demate"].ToString().Split(':')[2];
                    cmd.Parameters.Add("@CreateBy", SqlDbType.Int).Value = Session["SubscriberID"].ToString();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
        public void SaveBankInfo(string MemberId)
        {
            string ListOfBankAc = Session["ListOfBankAc"] as string;
            if (ListOfBankAc != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBankAc);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_BROKER_BANK_MASTER_INFO]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@BANK_ID", SqlDbType.NVarChar).Value = dr["BankId"].ToString().Split(':')[1].ToString();
                    cmd.Parameters.Add("@MEMBER_ID", SqlDbType.Int).Value = MemberId;
                    cmd.Parameters.Add("@DEFAULT_SELETED", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@CREATED_BY", SqlDbType.Int).Value = Session["SubscriberID"].ToString();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
        public void SaveDefultBankInfo(string MemberId)
        {
            string ListOfBankAc = Session["JsonDefaultValues"] as string;
            if (ListOfBankAc != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBankAc);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_BROKER_BANK_MASTER_INFO]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATEDEFAULT";
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@BANK_ID", SqlDbType.NVarChar).Value = dr["BankAc"].ToString().Split(':')[2].ToString();
                    cmd.Parameters.Add("@MEMBER_ID", SqlDbType.Int).Value = MemberId;
                    cmd.Parameters.Add("@DEFAULT_SELETED", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@CREATED_BY", SqlDbType.Int).Value = Session["SubscriberID"].ToString();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
        public void SaveAdvisorInfo(string MemberId)
        {
            string ListOfBankAc = Session["ListOfAdvisor"] as string;
            if (ListOfBankAc != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBankAc);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_BROKER_ADVISOR_MASTER_INFO]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@ADVISOR_ID", SqlDbType.NVarChar).Value = dr["AdvisorId"].ToString().Split(':')[1].ToString();
                    cmd.Parameters.Add("@MEMBER_ID", SqlDbType.Int).Value = MemberId;
                    cmd.Parameters.Add("@DEFAULT_SELETED", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@CREATED_BY", SqlDbType.Int).Value = Session["SubscriberID"].ToString();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
        public void SaveDefultAdvisorInfo(string MemberId)
        {
            string ListOfBankAc = Session["JsonDefaultValues"] as string;
            if (ListOfBankAc != "[]")
            {
                DataTable dt_ListOfBrokerWithDemate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ListOfBankAc);
                foreach (DataRow dr in dt_ListOfBrokerWithDemate.Rows)
                {
                    #region

                    string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = con,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "[DBO].[SP_BROKER_ADVISOR_MASTER_INFO]"
                    };
                    cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATEDEFAULT";
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@ADVISOR_ID", SqlDbType.NVarChar).Value = dr["Advisor"].ToString().Split(':')[2].ToString();
                    cmd.Parameters.Add("@MEMBER_ID", SqlDbType.Int).Value = MemberId;
                    cmd.Parameters.Add("@DEFAULT_SELETED", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@CREATED_BY", SqlDbType.Int).Value = Session["SubscriberID"].ToString();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    DataTable DT = new DataTable();
                    try
                    {
                        //con.Open();
                        /////Only Use Case Of Mapping
                        //string query = cmd.CommandText;
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += " "+p.ParameterName+"="+p.Value.ToString()+"\n";
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                    #endregion
                }
            }
        }
    }
}