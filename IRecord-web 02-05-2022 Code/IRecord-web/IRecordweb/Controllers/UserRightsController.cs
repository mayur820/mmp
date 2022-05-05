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
    public class UserRightsController : Controller
    {
        // GET: UserRights
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetUsers()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            List<User> userlist = new List<User>();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                string query = "Select ID,Username from Emp_Mast Where Username != ''";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            userlist.Add(new User
                            {
                                Userid = Convert.ToInt32(sdr["ID"]),
                                UserName = sdr["Username"].ToString(),
                            });
                        }
                    }
                    con.Close();
                }
            }
            return Json(userlist);
        }
        public JsonResult GetRoles()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            List<Role> departments = new List<Role>();
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                string query = "SELECT RoleID, RoleName FROM M_RoleMaster";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            departments.Add(new Role
                            {
                                RoleID = Convert.ToInt32(sdr["RoleID"]),
                                RoleName = sdr["RoleName"].ToString(),
                            });
                        }
                    }
                    con.Close();
                }
            }
            return Json(departments);
        }
        public JsonResult GetMenu(string RoleID)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            List<MenuDetail> departments = new List<MenuDetail>();
            long roleid = Convert.ToInt64(RoleID);
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                string query = "select * from M_Menu m left outer join[dbo].[M_RoleMenuAccess] mr ON mr.MenuID = m.MenuID and mr.RoleID = "+ roleid;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            departments.Add(new MenuDetail
                            {
                                MenuID = Convert.ToInt32(sdr["MenuID"]),
                                Name = sdr["Name"].ToString(),
                                Add = string.IsNullOrEmpty(sdr["AddAccess"].ToString()) ? false : Convert.ToBoolean(sdr["AddAccess"].ToString()),
                                Edit = string.IsNullOrEmpty(sdr["EditAccess"].ToString()) ? false : Convert.ToBoolean(sdr["EditAccess"].ToString()), 
                                Delete = string.IsNullOrEmpty(sdr["DeleteAccess"].ToString()) ? false: Convert.ToBoolean(sdr["DeleteAccess"].ToString()),
                                View = string.IsNullOrEmpty(sdr["ViewAccess"].ToString()) ? false: Convert.ToBoolean(sdr["ViewAccess"].ToString()),
                                SuperAdmin = string.IsNullOrEmpty(sdr["IsSuperAdmin"].ToString()) ? false: Convert.ToBoolean(sdr["IsSuperAdmin"].ToString()),
                                NormalUser = string.IsNullOrEmpty(sdr["IsNormalUser"].ToString()) ? false: Convert.ToBoolean(sdr["IsNormalUser"].ToString()),
                            });
                        }
                    }
                    con.Close();
                }
            }
            return Json(departments);
        }
        public JsonResult InsertMenu(List<MenuDetail> menulist)
        {
            var ID = Session["UserID"];
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            string delete = "delete from M_RoleMenuAccess where RoleID=" + menulist[0].RoleID;
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand(delete))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            foreach (var item in menulist)
            {
                List<MenuDetail> departments = new List<MenuDetail>();
                using (SqlConnection con = new SqlConnection(strConnString))
                {
                    
                    string query = "insert into M_RoleMenuAccess (MenuID,RoleID,AddAccess,EditAccess,DeleteAccess,ViewAccess,IsSuperAdmin,IsNormalUser,CreatedDate,CreatedBy,UserID)" +
                        "values(" + item.MenuID + "," + item.RoleID + ",'" + item.Add + "','" + item.Edit + "','" + item.Delete + "','" + item.View + "','" + item.SuperAdmin + "','" + item.NormalUser + "',getdate()," + 1 + "," + item.UserID + ")";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            return Json("");
        }
        public JsonResult InsertRoleMaster(string RoleName)
        {
            var ID = Session["UserID"];
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            string insertrole = "Insert Into M_RoleMaster (RoleName,CreatedDate) values ('" + RoleName + "',Getdate())";
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand(insertrole))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return Json("");
        }
        public JsonResult InsertUserRoleAssign(string UserIDS,int RoleID)
        {
            var ID = Session["UserID"];
            string[] userlst = UserIDS.Split(',');
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            foreach (var userid in userlst)
            {
                if (!string.IsNullOrEmpty(userid) || !string.IsNullOrWhiteSpace(userid))
                {
                    string delete = "Delete from M_UserRoleAssignMaster Where UserID ="+userid+"";
                    using (SqlConnection con = new SqlConnection(strConnString))
                    {
                        using (SqlCommand cmd = new SqlCommand(delete))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    string insertuserrole = "Insert Into M_UserRoleAssignMaster (UserID,RoleID,CreatedDate,CreatedBy) values (" + userid + "," + RoleID + ",Getdate()," + ID + ")";
                    using (SqlConnection con = new SqlConnection(strConnString))
                    {
                        using (SqlCommand cmd = new SqlCommand(insertuserrole))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            return Json("");
        }
        public JsonResult SelectUsers(int RoleID)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            List<int> departments = new List<int>();
            //long roleid = Convert.ToInt64(RoleID);
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                string query = "select UserID from M_UserRoleAssignMaster where RoleID = " + RoleID;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            departments.Add(Convert.ToInt32(sdr["UserID"]));
                        }
                    }
                    con.Close();
                }
            }
            return Json(departments);
        }
        public class MenuDetail
        {
            public int MenuID { get; set; }
            public string Name { get; set; }
            public bool Add { get; set; }
            public bool Edit { get; set; }
            public bool Delete { get; set; }
            public bool View { get; set; }
            public bool SuperAdmin { get; set; }
            public bool NormalUser { get; set; }
            public int RoleID { get; set; }
            public int UserID { get; set; }
        }
        public class Role
        {
            public int RoleID { get; set; }
            public string RoleName { get; set; }
        }
        public class User
        {
            public int Userid { get; set; }
            public string UserName { get; set; }
        }
    }

}