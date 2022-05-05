using Newtonsoft.Json;
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
    public class dematstocktransferController : Controller
        {
        // GET: dematstocktransfer
        public ActionResult Index()
            {
            return View();
            }
        public JsonResult GetInvestment()
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Jv_Entry]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWLEDGER";
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;
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
        public JsonResult GetAllDemate()
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_DEMAT_BILL";

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
        public JsonResult GetAllConsultant()
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnectionNew"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_CONSULTANT_UTITLIY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW";

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
        public JsonResult GetGridDAta(string DemateId)
            {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_DEMAT_TRANSFER";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = DemateId;
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
        public static int GetGlobalTransactionNumber(string bktyp)
            {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            con.Open();
            string dttod = DateTime.Today.ToString("yyyy-MM-dd");
            SqlCommand cmd43 = new SqlCommand("Insert into Global_no(Book_typ,Date) values('" + bktyp + "','" + dttod + "');SELECT SCOPE_IDENTITY();", con);
            var vchno = cmd43.ExecuteScalar();
            return Convert.ToInt32(vchno);
            }
        [HttpPost]
        public void SaveData(string JsonData, string JsonData2)
            {


            DataTable dtJsonData = (DataTable)JsonConvert.DeserializeObject(JsonData, (typeof(DataTable)));
            DataTable dtJsonData2 = (DataTable)JsonConvert.DeserializeObject(JsonData2, (typeof(DataTable)));
            int rowcounter = 1;
            int Global_No = GetGlobalTransactionNumber("MF");
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            #region TABL1
            foreach (DataRow dr in dtJsonData2.Rows)
                {
                strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                con = new SqlConnection(strConnString);
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_BrokerBill_DmatInfo]";
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = Global_No;//
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = rowcounter;
                cmd.Parameters.Add("@Local_SrNo", SqlDbType.Int).Value = "1";
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(dtJsonData.Rows[0]["Date"].ToString()).ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@Member_code", SqlDbType.VarChar).Value = "M0001";
                cmd.Parameters.Add("@YearCode", SqlDbType.VarChar).Value = "1120212022";
                cmd.Parameters.Add("@dp_code", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["ddl_from_Demat_id"].ToString();
                cmd.Parameters.Add("@Script_Code", SqlDbType.VarChar).Value = dr["Script_Code"].ToString();
                cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = dr["TransferQty"].ToString();
                cmd.Parameters.Add("@Trans_Type", SqlDbType.VarChar).Value = "O";
                cmd.Parameters.Add("@internalEntry", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@Ref_Trans_No", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@Ref_Sr_No", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@entrySource", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@Consultant_Code", SqlDbType.VarChar).Value = dr["Consultant_Code"].ToString();
                cmd.Parameters.Add("@Ref_Local_SrNo", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@MergedEntry", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["Investment"].ToString();
                cmd.Parameters.Add("@frmpg", SqlDbType.VarChar).Value = (dtJsonData.Rows[0]["Investment"].ToString() == "0" ? "ST" : "MFST");
                cmd.Parameters.Add("@ddl_from_Demat_id", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["ddl_from_Demat_id"].ToString();
                cmd.Parameters.Add("@ddl_to_Demat_id", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["ddl_to_Demat_id"].ToString();


                if (con.State == ConnectionState.Open) con.Close();
                if (con.State == ConnectionState.Closed) con.Open();
                DataTable DT1 = new DataTable();
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
                        da.Fill(DT1);
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
                rowcounter++;

                }
            #endregion
            #region TABL2
            strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            con = new SqlConnection(strConnString);
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Dmat_transfer]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = Global_No;
            cmd.Parameters.Add("@Trans_dt", SqlDbType.VarChar).Value = Convert.ToDateTime(dtJsonData.Rows[0]["Date"].ToString()).ToString("yyyy/MM/dd");
            cmd.Parameters.Add("@from_dp", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["ddl_from_Demat_id"].ToString();
            cmd.Parameters.Add("@to_dp", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["ddl_to_Demat_id"].ToString();
            cmd.Parameters.Add("@from_cons", SqlDbType.VarChar).Value = "0";
            cmd.Parameters.Add("@to_cons", SqlDbType.VarChar).Value = "0";
            cmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = "0";

            cmd.Parameters.Add("@membercode", SqlDbType.VarChar).Value = "M0001";
            cmd.Parameters.Add("@yearcode", SqlDbType.VarChar).Value = "1120212022";
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


            #endregion
            }
        }
    }