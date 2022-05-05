using DAL;
using IRecordweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.SessionState;
using RestSharp;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Net;
using System.Data.SqlClient;

namespace IRecordweb.Controllers
    {
    public class MenuController : Controller
        {
        // GET: Menu

        DAL.Master obj = new Master();
        DbContext db = new DbContext();
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];

        public ActionResult Index()
            {
            return View();
            }


        //public PartialViewResult GenerateMenu()
        //{
        //    var menulist = db.Menu.ToList();
        //    var menufordisplay = menu.MenuTree(menulist, null);
        //    return PartialView(menulist);
        //}
        public virtual PartialViewResult Menu(string UserId)
            {
            IEnumerable<MENU> Menu = null;
            if (Session["Role_Type"] != "System")
            {
                if (Session["_UserChange"] != null)
                {
                    string userid = Session["UserID"].ToString();
                    Menu = GetMenus(Session["Act_User_ID"] != null ? Session["Act_User_ID"].ToString() : userid); // pass User id here
                    Session["_Menu"] = Menu;
                    Session["_UserChange"] = null;
                }


                if (Session["_Menu"] != null)
                {
                    Menu = (IEnumerable<MENU>)Session["_Menu"];
                }
                else
                {
                    string userid = Session["UserID"].ToString();
                    Menu = GetMenus(Session["Act_User_ID"] != null ? Session["Act_User_ID"].ToString() : userid); // pass User id here
                    Session["_Menu"] = Menu;
                }

            }
            else
            {
                string userid = Session["UserID"].ToString();
                Menu = GetMenusSystem(userid); // pass User id here
                Session["_Menu"] = Menu;
                // Menu = (IEnumerable<MENU>)Session["SystemUserMenuDetail"];
            }
            return PartialView("_MenuPartial", Menu);
            }

            public IList<MENU> GetMenus(string UserId)
            {


            if (Session["Login_Flags"].ToString() == "3")
            {
                DataTable DTID = Session["UserLoginData1"] as DataTable;
                UserId = DTID.Rows[0]["Memeber_ID"].ToString();
            }
            if (Session["Login_Flags"].ToString() == "2")
            {
                DataTable DTID = Session["UserLoginData1"] as DataTable;
                UserId = DTID.Rows[0]["Opertor_ID"].ToString();
            }




            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_USERMENUTRANS_NEW]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "GETMENU";
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = (UserId);
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
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


            List<MENU> menuList = new List<MENU>();
            foreach (DataRow sdr in DT.Rows)
                {
                MENU item = new MENU();
                item.MenuID = Convert.ToInt32(sdr["MenuID"].ToString());
                item.Name = sdr["Name"].ToString();
                item.url = sdr["url"].ToString();
                item.ParentMenuID = sdr["ParentMenuID"] != DBNull.Value ? Convert.ToInt32(sdr["ParentMenuID"]) : (int?)null;
                menuList.Add(item);
                }
            Session["ViewMenuRightsForUser"] = DT;
            return menuList;

            }
        public IList<MENU> GetMenusSystem(string UserId)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[usp_GetSystemUserMenu]";
            cmd.Parameters.Add("@UserID", SqlDbType.BigInt).Value = (UserId);
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
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


            List<MENU> menuList = new List<MENU>();
            foreach (DataRow sdr in DT.Rows)
            {
                MENU item = new MENU();
                item.MenuID = Convert.ToInt32(sdr["MenuID"].ToString());
                item.Name = sdr["Name"].ToString();
                item.url = sdr["url"].ToString();
                item.ParentMenuID = sdr["ParentMenuID"] != DBNull.Value ? Convert.ToInt32(sdr["ParentMenuID"]) : (int?)null;
                menuList.Add(item);
            }
            Session["ViewMenuRightsForUser"] = DT;
            return menuList;

        }
        public ActionResult UserRights()
            {
            MENU _Menu = new MENU();
            _Menu.MenuList = DisplayMenuData(_Menu);
            List<SelectListItem> ObjList = GetUser();
            ViewBag.UserList = ObjList;
            return View(_Menu);
            }
        public List<MENU> DisplayMenuData(MENU _Menu)
            {
            var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/USERMASTER?DBAction=GetMenuMasterList&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MENU> menuList = new List<MENU>();
            foreach (DataRow sdr in data.Tables[0].Rows)
                {
                MENU item = new MENU();
                item.MenuID = Convert.ToInt32(sdr["MenuID"].ToString());
                item.Name = sdr["Name"].ToString();
                item.UserId = Convert.ToInt32(sdr["UserId"].ToString());
                item.UserMenuID = Convert.ToInt32(sdr["UserMenuID"].ToString());
                item.Add = Convert.ToBoolean(sdr["Add"]);
                item.Edit = Convert.ToBoolean(sdr["Edit"]);
                item.Delete = Convert.ToBoolean(sdr["Delete"]);
                item.View = Convert.ToBoolean(sdr["View"]);
                menuList.Add(item);
                }
            return menuList;
            }
        [HttpPost]
        public ActionResult UserRights(MENU _Menu, string Search, string Save, string MenuName,string UserList)
            {
            if (!string.IsNullOrEmpty(Search))
                {
                _Menu.MenuList = SearchVal(MenuName, _Menu);
                }
            if (!string.IsNullOrEmpty(Save))
                {
                // _Menu.MenuList = obj.DisplayMenuData(_Menu);      
                _Menu.MenuList = InsertRights(_Menu, UserList);
                ViewBag.Message = "Data Saved Successfully !!";
                // menudisplay = obj.DisplayMenuData(_Menu);
                }
            List<SelectListItem> ObjList = GetUser();
            ViewBag.UserList = ObjList;
            return View(_Menu);
            }
        public List<MENU> SearchVal(string MenuName, MENU _Menu)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/USERMASTER?DBAction=SearchMenuMaster&ID=" + MenuName);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MENU> menuList = new List<MENU>();
            foreach (DataRow sdr in data.Tables[0].Rows)
                {
                MENU item = new MENU();
                item.MenuID = Convert.ToInt32(sdr["MenuID"].ToString());
                item.Name = sdr["Name"].ToString();
                menuList.Add(item);
                }
            return menuList;
            }
        public DataSet GetMenuMaster(MENU _Menu)
            {
            var ID = Session["UserID"];
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/USERMASTER?DBAction=GetMenuMasterList&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            return data;
            }

        public List<MENU> InsertRights(MENU _Menu,string UserList)
            {
            var ID = Session["UserID"];
            DataSet data = GetMenuMaster(_Menu);
            //var client = new RestClient(URL + "api/Master/USERMASTER?DBAction=GetMenuMasterList&ID=" + ID);
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", BasicAuth);
            //request.AddHeader("Content-Type", "application/json");
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MENU> MenuList = new List<MENU>();
            //foreach (DataRow sdr in data.Tables[0].Rows)
            //    {
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                    MENU item = new MENU();
                    item.MenuID = Convert.ToInt32(data.Tables[0].Rows[i]["MenuID"].ToString());
                    item.Name = data.Tables[0].Rows[i]["Name"].ToString();
                    item.UserId = Convert.ToInt32(UserList);
                    item.UserMenuID = Convert.ToInt32(data.Tables[0].Rows[i]["UserMenuID"].ToString());
                    item.Add = Convert.ToBoolean(data.Tables[0].Rows[i]["Add"]);
                    item.Edit = Convert.ToBoolean(data.Tables[0].Rows[i]["Edit"]);
                    item.Delete = Convert.ToBoolean(data.Tables[0].Rows[i]["Delete"]);
                    item.View = Convert.ToBoolean(data.Tables[0].Rows[i]["View"]);
                    MenuList.Add(item);
                    for (var ij = 0; ij < _Menu.MenuList.Count; ij++)
                        {
                        if (_Menu.MenuList[i].Add == true || _Menu.MenuList[i].Edit == true || _Menu.MenuList[i].Delete == true || _Menu.MenuList[i].View == true)
                            {
                            MENU objdata = new MENU();
                            objdata.UserId = Convert.ToInt32(UserList.ToString());
                            objdata.MenuID = Convert.ToInt32(_Menu.MenuList[i].MenuID.ToString());
                            objdata.Add = _Menu.MenuList[i].Add;
                            objdata.Edit = _Menu.MenuList[i].Edit;
                            objdata.View = _Menu.MenuList[i].View;
                            objdata.Delete = _Menu.MenuList[i].Delete;
                            //  objdata.Active = _Menu.Active;
                            objdata.CreatedBy = Convert.ToInt32(ID.ToString());
                            var json = new JavaScriptSerializer().Serialize(objdata);
                            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                            var client = new RestClient(URL + "api/Master/USERMENUTRANSMASTER?DBAction=Insertorupdate&ID=0");
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
                //}
            return MenuList;

            }

        public List<SelectListItem> GetUser()
        { 
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[Sp_GetUser]";
            //cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "GETMENU";
            //cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = (UserId);
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
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
            List<SelectListItem> lst = new List<SelectListItem>();
            
            foreach (DataRow row in DT.Rows)
            {
                lst.Add(new SelectListItem()
                {
                    Text = row["USERNAME"].ToString(),
                    Value = row["UserID"].ToString()
                });
            }



            return lst;

        }

    }
    }