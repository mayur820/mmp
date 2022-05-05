using IRecordweb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class OperatorMasterController : Controller
    {

  
        //
        // GET: /OperatorMaster/
        public ActionResult Index()
        {
            #region set User Rights
            FnUserRights Rights = new FnUserRights();
            string url = Request.Url.PathAndQuery;
            Rights.SetUserRight(url);
            #endregion
            

            return View();
        }
        public JsonResult GetValidEmailId(string Mailid)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_UNIQUE_MAILID]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Operator";
            cmd.Parameters.Add("@mailid", SqlDbType.NVarChar).Value = Mailid.ToString();
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
            if (DT.Rows[0]["RowCount"].ToString() == "0")
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public void IndexSave(OperatorMaster_Index_Models data)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            // string date = obj.FromDate;
            //return View();
            Session["data"] = data;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Irecord_Operator_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = "0";
            cmd.Parameters.Add("@OperatorName", SqlDbType.NVarChar).Value = data.OperatorName;
            cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = data.Mobile;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = data.Email;
            // cmd.Parameters.Add("@ManagerId", SqlDbType.Int).Value = data.ManagerId;
            cmd.Parameters.Add("@Sucriberid", SqlDbType.Int).Value = CreatedById.ToString(); //Session["UserID"].ToString();
            //  cmd.Parameters.Add("@Ismanager", SqlDbType.Int).Value = data.Ismanager;
            cmd.Parameters.Add("@Createddate", SqlDbType.NVarChar).Value = data.Createddate;
            cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = CreatedById;
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = data.Password;
            //cmd.Parameters.Add("@Createddate", SqlDbType.DateTime).Value = "";
            cmd.Connection = con;
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
        }
        [HttpPost]
        public void edit(OperatorMaster_Index_Models data1)
        {
            Session["data1"] = data1;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Irecord_Operator_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATE";
            cmd.Parameters.Add("@OperatorName", SqlDbType.NVarChar).Value = data1.OperatorName;
            cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = data1.Mobile;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = data1.Email;
            // cmd.Parameters.Add("@ManagerId", SqlDbType.Int).Value = data1.ManagerId;
            cmd.Parameters.Add("@Sucriberid", SqlDbType.Int).Value = Session["UserID"].ToString();
            //  cmd.Parameters.Add("@Ismanager", SqlDbType.Int).Value = data1.Ismanager;
            cmd.Parameters.Add("@Createddate", SqlDbType.NVarChar).Value = data1.Createddate;
            cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = data1.Createdby;
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = data1.Password;
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = data1.ID;
            //cmd.Parameters.Add("@Createddate", SqlDbType.DateTime).Value = "";
            cmd.Connection = con;
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

        }
        [HttpPost]
        public void Delete(OperatorMaster_Index_Models data1)
        {
            Session["data1"] = data1;
            int DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Irecord_Operator_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DELETE";
            //cmd.Parameters.Add("@OperatorName", SqlDbType.NVarChar).Value = data1.OperatorName;
            //cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = data1.Mobile;
            //cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = data1.Email;
            cmd.Parameters.Add("@DeletedBy", SqlDbType.BigInt).Value = DeletedBy;
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = data1.ID;
            //cmd.Parameters.Add("@Createddate", SqlDbType.DateTime).Value = "";
            cmd.Connection = con;
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

        }

        public JsonResult GetAllOperator()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Irecord_Operator_Master]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW";
            cmd.Parameters.Add("@Sucriberid", SqlDbType.Int).Value = Session["UserID"].ToString();
            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
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
        public JsonResult GetMANAGER()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Irecord_Operator_Master]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWMANAGER";

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
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(list);
        }
    }
}