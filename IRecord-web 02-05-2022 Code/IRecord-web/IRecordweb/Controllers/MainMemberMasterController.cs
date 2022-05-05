using IRecordweb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class MainMemberMasterController : Controller
    {
        //
        // GET: /MainMemberMaster/
        public ActionResult Index()
        {
            #region set User Rights
            FnUserRights Rights = new FnUserRights();
            string url = Request.Url.PathAndQuery;
            Rights.SetUserRight(url);
            #endregion
            return View();
        }


        [HttpPost]
        public void Upload()
        {


            foreach (string key in Request.Files)
            {
                HttpPostedFileBase FilePath = Request.Files[key];
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(FilePath.InputStream))
                {
                    bytes = br.ReadBytes(FilePath.ContentLength);
                }

                string uploadfilepath = "~/Memberuploadfile/" + FilePath.FileName;
                FilePath.SaveAs(Server.MapPath(uploadfilepath));
                Session["UserFilePath"] = uploadfilepath;
            }

            //return Content("Success");
        }
        [HttpPost]
        public void IndexSave(MainMemberMasterModel data)
        {


            // string date = obj.FromDate;
            //return View();
            Session["data"] = data;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Main_Member_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = "0";
            cmd.Parameters.Add("@FamilyID", SqlDbType.Int).Value = data.FamilyID;
            cmd.Parameters.Add("@MemberName", SqlDbType.NVarChar).Value = (data.MemberName == null ? "" : data.MemberName);

            cmd.Parameters.Add("@Address_1", SqlDbType.NVarChar).Value = (data.Address_1 == null ? "" : data.Address_1);
            cmd.Parameters.Add("@Address_2", SqlDbType.NVarChar).Value = (data.Address_2 == null ? "" : data.Address_2);
            cmd.Parameters.Add("@Address_3", SqlDbType.NVarChar).Value = (data.Address_3 == null ? "" : data.Address_3);
            cmd.Parameters.Add("@Gender", SqlDbType.Int).Value = data.Gender;
            cmd.Parameters.Add("@ServTax_No", SqlDbType.NVarChar).Value = (data.ServTax_No == null ? "" : data.ServTax_No);
            cmd.Parameters.Add("@AadharCardNo", SqlDbType.NVarChar).Value = (data.AadharCardNo == null ? "" : data.AadharCardNo);
            cmd.Parameters.Add("@ReportLogoPath", SqlDbType.NVarChar).Value = (Session["UserFilePath"] != null ? Session["UserFilePath"].ToString() : "");
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = Session["UserID"].ToString();
            cmd.Parameters.Add("@PAN", SqlDbType.NVarChar).Value = (data.PAN == null ? "" : data.PAN);
            //    cmd.Parameters.Add("@Active", SqlDbType.Bool).Value = data.Active;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = data.CreatedBy;
            //  cmd.Parameters.Add("@CreatedDate", SqlDbType.NVarChar).Value = data.CreatedDate;
            //     cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = data.ModifiedBy;
            //   cmd.Parameters.Add("@ModifiedDate", SqlDbType.NVarChar).Value = data.ModifiedDate;
            cmd.Parameters.Add("@SubscriberID", SqlDbType.Int).Value = Session["UserID"].ToString();
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = (data.Email == null ? "" : data.Email);
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = (data.Password == null ? "" : data.Password);
            cmd.Parameters.Add("@Client_Code", SqlDbType.NVarChar).Value = (data.Client_Code == null ? "" : data.Client_Code);
            cmd.Parameters.Add("@OperatorID", SqlDbType.Int).Value = data.OperatorID;
            //cmd.Parameters.Add("@Createddate", SqlDbType.DateTime).Value = "";
            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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
            // Insertsub(data);
        }
        [HttpPost]
        public void Insertsub(MainMemberMasterModel data)
        {


            // string date = obj.FromDate;
            //return View();
            Session["data"] = data;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[ALGEBRA].[SP_INSERT_MEM]";
            cmd.Parameters.Add("@Sub_id", SqlDbType.Int).Value = Session["UserID"].ToString();
            cmd.Parameters.Add("@mem_id", SqlDbType.Int).Value = data.MemberID;
            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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

        }
        [HttpPost]
        public void edit(MainMemberMasterModel data1)
        {
            // Session["UserID"] = Convert.ToInt32(dr["UserId"].ToString());
            Session["data1"] = data1;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Main_Member_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATE";
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = data1.MemberID;
            cmd.Parameters.Add("@FamilyID", SqlDbType.Int).Value = data1.FamilyID;
            cmd.Parameters.Add("@MemberName", SqlDbType.NVarChar).Value = (data1.MemberName == null ? "" : data1.MemberName);

            cmd.Parameters.Add("@Address_1", SqlDbType.NVarChar).Value = (data1.Address_1 == null ? "" : data1.Address_1);
            cmd.Parameters.Add("@Address_2", SqlDbType.NVarChar).Value = (data1.Address_2 == null ? "" : data1.Address_2);
            cmd.Parameters.Add("@Address_3", SqlDbType.NVarChar).Value = (data1.Address_3 == null ? "" : data1.Address_3);
            cmd.Parameters.Add("@Gender", SqlDbType.Int).Value = data1.Gender;
            cmd.Parameters.Add("@ServTax_No", SqlDbType.NVarChar).Value = (data1.ServTax_No == null ? "" : data1.ServTax_No);
            cmd.Parameters.Add("@AadharCardNo", SqlDbType.NVarChar).Value = (data1.AadharCardNo == null ? "" : data1.AadharCardNo);
            /// cmd.Parameters.Add("@ReportLogoPath", SqlDbType.NVarChar).Value = data.ReportLogoPath;
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = "0";
            cmd.Parameters.Add("@PAN", SqlDbType.NVarChar).Value = (data1.PAN == null ? "" : data1.PAN);
            //    cmd.Parameters.Add("@Active", SqlDbType.Bool).Value = data.Active;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = data1.CreatedBy;
            //  cmd.Parameters.Add("@CreatedDate", SqlDbType.NVarChar).Value = data1.CreatedDate;
            //     cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = data.ModifiedBy;
            //   cmd.Parameters.Add("@ModifiedDate", SqlDbType.NVarChar).Value = data.ModifiedDate;
            string id = Session["UserID"].ToString();
            cmd.Parameters.Add("@SubscriberID", SqlDbType.Int).Value = id.ToString();
            cmd.Parameters.Add("@OperatorID", SqlDbType.Int).Value = data1.OperatorID;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = (data1.Email == null ? "" : data1.Email);
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = (data1.Password == null ? "" : data1.Password);
            cmd.Parameters.Add("@Client_Code", SqlDbType.NVarChar).Value = (data1.Client_Code == null ? "" : data1.Client_Code);
            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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

        }
        [HttpPost]
        public void editselect(MainMemberMasterModel data1)
        {
            // Session["UserID"] = Convert.ToInt32(dr["UserId"].ToString());
            Session["data1"] = data1;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Main_Member_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATEBYOPEARTOR";
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = data1.Code;
            cmd.Parameters.Add("@SubscriberID", SqlDbType.Int).Value = Session["UserID"].ToString();
            cmd.Parameters.Add("@OperatorID", SqlDbType.Int).Value = data1.OperatorID;

            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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

        }
        [HttpPost]
        public void Delete(MainMemberMasterModel data1)
        {
            Session["data1"] = data1;
            int DeletedBy = Convert.ToInt32(Session["UserID"].ToString());
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Main_Member_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DELETE";
            //cmd.Parameters.Add("@OperatorName", SqlDbType.NVarChar).Value = data1.OperatorName;
            //cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = data1.Mobile;
            //cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = data1.Email;
            cmd.Parameters.Add("@DeletedBy", SqlDbType.BigInt).Value = DeletedBy;
            cmd.Parameters.Add("@MemberID", SqlDbType.BigInt).Value = data1.MemberID;
            //cmd.Parameters.Add("@Createddate", SqlDbType.DateTime).Value = "";
            cmd.Connection = con;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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

        }

        public JsonResult GetAllMember()
        {
         
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Main_Member_Master]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW";
            cmd.Parameters.Add("@Login_UserId", SqlDbType.Int).Value = Session["UserID"].ToString();
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
        public JsonResult GetOperator()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Main_Member_Master]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWOPERATOR";
            cmd.Parameters.Add("@Login_UserId", SqlDbType.Int).Value = Session["UserID"].ToString();
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

        public JsonResult GetValidEmailId(string Mailid)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_UNIQUE_MAILID]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Member";
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
        public JsonResult GetFAMILY()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Main_Member_Master]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWFAMILY";
            cmd.Parameters.Add("@Login_UserId", SqlDbType.Int).Value = Session["UserID"].ToString();
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
        [HttpPost]
        public void IndexSavenew(OperatorMaster_Index_Models data)
        {

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
            cmd.Parameters.Add("@Sucriberid", SqlDbType.Int).Value = Session["UserID"].ToString();
            //  cmd.Parameters.Add("@Ismanager", SqlDbType.Int).Value = data.Ismanager;
            cmd.Parameters.Add("@Createddate", SqlDbType.NVarChar).Value = data.Createddate;
            cmd.Parameters.Add("@Createdby", SqlDbType.Int).Value = data.Createdby;
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