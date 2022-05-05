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
    public class UserSecurityController : Controller
        {
        //DataFunction obj = new DataFunction();
        // GET: /UserSecurity/
        [HttpGet]
        public ActionResult Index()
            {

            //DataTable dt = obj.select_data_dt("exec [SP_USER_DOCUMENTS] 'VIEW_ALL_DOC'");
            //List<ViewDocumentModels> sm = new List<ViewDocumentModels>();
            //sm = obj.ConvertToList<ViewDocumentModels>(dt);
            //return View(sm);
            VIEW_ALL_MEMEBER();
            VIEWBYPAGENAME();

            return View();

            // Session["DT_ALL_MEMBER"] as DataTable();

            // return View();
            }
        public void VIEW_ALL_MEMEBER()
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_Irecord_User_Rights_Master]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_ALL_MEMEBER";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Session["UserID"].ToString();

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
                {
                con.Open();
                /////Only Use Case Of Mapping
                string query = cmd.CommandText;
                foreach (SqlParameter p in cmd.Parameters)
                    {
                    query += " " + p.ParameterName + "=" + p.Value.ToString() + "\n";
                    }
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
            Session["DT_ALL_MEMBER"] = DT;
            }
        public void VIEWBYPAGENAME()
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_Irecord_User_Rights_Master]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBYPAGENAME";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Session["UserID"].ToString();
            //  cmd.Parameters.Add("@userid", SqlDbType.Int).Value = USERID;

            cmd.Connection = con;
            System.Data.DataTable DTPAGE = new System.Data.DataTable();
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
                    da.Fill(DTPAGE);
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

            Session["DT_ALL_VIEWBYPAGENAME"] = DTPAGE;
            }
        //[HttpGet]
        public ActionResult GET_ALL_MEMEBERlevel2(int USERID)
            {


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            // Session["DT_ALL_MEMBER"] as DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_Irecord_User_Rights_Master]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_ALL_MEMEBERlevel2";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Session["UserID"].ToString();
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = USERID;

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
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
            Session["DT_ALL_MEMBERLevel2"] = DT;
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


        [HttpPost]
        public JsonResult UpdateValue(string Action, string check, string ID)
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[UPDATE_USER_RIGHTS]";

            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = Action;
            cmd.Parameters.Add("@CHECK", SqlDbType.NVarChar).Value = check;
            cmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = ID;
            //  cmd.Parameters.Add("@userid", SqlDbType.Int).Value = USERID;

            cmd.Connection = con;
            System.Data.DataTable DTPAGE = new System.Data.DataTable();
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
                    da.Fill(DTPAGE);
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
            return Json("1");
            }


        [HttpPost]
        public JsonResult UpdateAllValue(string varid1, string varid2, string Col1, string Col2, string Status)
            {

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_User_Rights_Update_All]";

            cmd.Parameters.Add("@varid1", SqlDbType.NVarChar).Value = varid1;
            cmd.Parameters.Add("@varid2", SqlDbType.NVarChar).Value = varid2;
            cmd.Parameters.Add("@Col1", SqlDbType.NVarChar).Value = Col1;
            cmd.Parameters.Add("@Col2", SqlDbType.NVarChar).Value = Col2;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
            //  cmd.Parameters.Add("@userid", SqlDbType.Int).Value = USERID;

            cmd.Connection = con;
            System.Data.DataTable DTPAGE = new System.Data.DataTable();
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
                    da.Fill(DTPAGE);
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
            return Json("1");
            }
        }
    }