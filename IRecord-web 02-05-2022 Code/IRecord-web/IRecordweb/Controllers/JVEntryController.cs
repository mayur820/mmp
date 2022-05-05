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
    public class JVEntryController : Controller
    {
        // GET: JVEntry
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewJVEntry(string TrnsNo)
        {
            DataTable DTHEAD = GET_db_head_Data(TrnsNo);
            DataTable DTDetails = GET_db_Details_Data(TrnsNo);
            Session["dthead"] = DTHEAD;
            Session["DTDetails"] = DTDetails;
            return View("Index");
        }
        public JsonResult Get_HeadLevel_Info()
        {
            DataTable dthead = Session["dthead"] as DataTable;
            return Json(DataTableToJSON(dthead), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_DeatilsLevel_Info()
        {
            DataTable DTDetails = Session["DTDetails"] as DataTable;
            return Json(DataTableToJSON(DTDetails), JsonRequestBehavior.AllowGet);
        }
        public DataTable GET_db_head_Data(string TrnsNo)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Jv_Entry]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_HEAD_JV";
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = TrnsNo;
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
            return DT;

        }
        public DataTable GET_db_Details_Data(string TrnsNo)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Jv_Entry]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_DETAILS_JV";
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = TrnsNo;
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
            return DT;

        }

        public ActionResult ViewJV()
        {
            return View();
        }
        public JsonResult GetJVUNIQUENO()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_YOUR_SP_JV_Entry_Unique_Id]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            //DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            //var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            //var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            //var CreatedById = Session["UserID"].ToString();
            //cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            //cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;


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
        public JsonResult Getledger()
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

        public JsonResult ViewJVByUser()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Jv_Entry]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWJV";
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

        public JsonResult DeleteJv(string ID)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Jv_Entry]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "DELETEJV";
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = CreatedById;
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
            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public JsonResult Getledger2(string Legerid)
        {
         
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_Jv_Entry]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWLEDGERNOT";
            cmd.Parameters.Add("@ACID", SqlDbType.NVarChar).Value = Legerid.Split(':')[1].ToString();
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
        public int GetGlobalTransactionNumber(string bktyp)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            con.Open();
            string dttod = DateTime.Today.ToString("yyyy-MM-dd");
            SqlCommand cmd43 = new SqlCommand("Insert into Global_no(Book_typ, Date, MemberID, FinancialYearMemberID, CreatedBy, CreatedDate) values('" + bktyp + "',getdate()," + MemberCode + "," + FinancialYearCode + "," + CreatedById + ",getdate());SELECT SCOPE_IDENTITY();", con);
            var vchno = cmd43.ExecuteScalar();
            return Convert.ToInt32(vchno);
        }
        [HttpPost]
        public void IndexSave(string JsonData, string JsonData2)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            string Trans_No = GetGlobalTransactionNumber("JV").ToString();
            DataTable dtdata2 = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData2);
            DataTable dtdata1 = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);

            DataColumnCollection columns = dtdata2.Columns;
            if (!columns.Contains("Narration"))
            {
                dtdata2.Columns.Add("Narration", typeof(string));
            }
            int rowcounter = 1;
            foreach (DataRow dr in dtdata2.Rows)
            {


                String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_AC_Trans]";

                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = "0";
                cmd.Parameters.Add("@Created_By", SqlDbType.Int).Value = CreatedById;
                
                cmd.Parameters.Add("@Trans_No", SqlDbType.NVarChar).Value = Trans_No;
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = rowcounter;
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.NVarChar).Value = dtdata1.Rows[0]["Trans_Dt"].ToString();
                cmd.Parameters.Add("@Member_Code", SqlDbType.NVarChar).Value = MemberCode;
                cmd.Parameters.Add("@Year_Code", SqlDbType.NVarChar).Value = FinancialYearCode;
                cmd.Parameters.Add("@Vouch_No", SqlDbType.NVarChar).Value = dtdata1.Rows[0]["Vouch_No"].ToString();
                cmd.Parameters.Add("@Dr_Code", SqlDbType.NVarChar).Value = (dr["DRCR"].ToString() == "1" ? dr["Ledger"].ToString() : dtdata1.Rows[0]["Parent_Ac_Code"].ToString());
                cmd.Parameters.Add("@Cr_Code", SqlDbType.NVarChar).Value = (dr["DRCR"].ToString() == "2" ? dr["Ledger"].ToString() : dtdata1.Rows[0]["Parent_Ac_Code"].ToString());
                cmd.Parameters.Add("@Book_Type1", SqlDbType.NVarChar).Value = "JV";
                cmd.Parameters.Add("@Internal", SqlDbType.NVarChar).Value = "N";
                cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = dr["AMOUNT"].ToString();
                cmd.Parameters.Add("@InvestmentType", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@EntryType", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@PayAgtBill", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@isImported", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@ExpenseType", SqlDbType.NVarChar).Value = "0";
                cmd.Parameters.Add("@IsMoneyBack", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@IsInsurance", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@IsDisplayInAc", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@frmpg", SqlDbType.NVarChar).Value = "JV";
                cmd.Parameters.Add("@Parent_Narr", SqlDbType.NVarChar).Value = (dtdata1.Rows[0]["Parent_Narr"].ToString() == "" ? "" : dtdata1.Rows[0]["Parent_Narr"].ToString());

                cmd.Parameters.Add("@Counter_Narr", SqlDbType.NVarChar).Value = (dr["Narration"].ToString() == "" ? "" : dr["Narration"].ToString());

                cmd.Parameters.Add("@Entry_Narr", SqlDbType.NVarChar).Value = (dtdata1.Rows[0]["Entry_Narr"].ToString() == "" ? "" : dtdata1.Rows[0]["Entry_Narr"].ToString());
                cmd.Parameters.Add("@Parent_Ac_Code", SqlDbType.NVarChar).Value = dtdata1.Rows[0]["Parent_Ac_Code"].ToString();
                cmd.Parameters.Add("@Parent_Ac_Type", SqlDbType.NVarChar).Value = dtdata1.Rows[0]["Parent_Ac_Type"].ToString();
                cmd.Parameters.Add("@Parent_Amt", SqlDbType.Decimal).Value = dtdata1.Rows[0]["Parent_Amt"].ToString();
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
                }
                rowcounter++;
            }
        }
        [HttpPost]
        public void UpdateJV(string JsonData, string JsonData2)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //string Trans_No = GetGlobalTransactionNumber("JV").ToString();
            DataTable dtdata2 = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData2);
            DataTable dtdata1 = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);

            DataColumnCollection columns = dtdata2.Columns;
            if (!columns.Contains("Narration"))
            {
                dtdata2.Columns.Add("Narration", typeof(string));
            }
            int rowcounter = 1;
            foreach (DataRow dr in dtdata2.Rows)
            {


                String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_AC_Trans]";

                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATE";
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = dr["ID"].ToString(); ;
                cmd.Parameters.Add("@Created_By", SqlDbType.Int).Value = CreatedById;

                cmd.Parameters.Add("@Trans_No", SqlDbType.NVarChar).Value = "0";
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = rowcounter;
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.NVarChar).Value = dtdata1.Rows[0]["Trans_Dt"].ToString();
                cmd.Parameters.Add("@Member_Code", SqlDbType.NVarChar).Value = MemberCode;
                cmd.Parameters.Add("@Year_Code", SqlDbType.NVarChar).Value = FinancialYearCode;
                cmd.Parameters.Add("@Vouch_No", SqlDbType.NVarChar).Value = dtdata1.Rows[0]["Vouch_No"].ToString();
                cmd.Parameters.Add("@Dr_Code", SqlDbType.NVarChar).Value = (dr["DRCR"].ToString() == "1" ? dr["Ledger"].ToString() : dtdata1.Rows[0]["Parent_Ac_Code"].ToString());
                cmd.Parameters.Add("@Cr_Code", SqlDbType.NVarChar).Value = (dr["DRCR"].ToString() == "2" ? dr["Ledger"].ToString() : dtdata1.Rows[0]["Parent_Ac_Code"].ToString());
                cmd.Parameters.Add("@Book_Type1", SqlDbType.NVarChar).Value = "JV";
                cmd.Parameters.Add("@Internal", SqlDbType.NVarChar).Value = "N";
                cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = dr["AMOUNT"].ToString();
                cmd.Parameters.Add("@InvestmentType", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@EntryType", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@PayAgtBill", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@isImported", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@ExpenseType", SqlDbType.NVarChar).Value = "0";
                cmd.Parameters.Add("@IsMoneyBack", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@IsInsurance", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@IsDisplayInAc", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@frmpg", SqlDbType.NVarChar).Value = "JV";
                cmd.Parameters.Add("@Parent_Narr", SqlDbType.NVarChar).Value = (dtdata1.Rows[0]["Parent_Narr"].ToString() == "" ? "" : dtdata1.Rows[0]["Parent_Narr"].ToString());

                cmd.Parameters.Add("@Counter_Narr", SqlDbType.NVarChar).Value = (dr["Narration"].ToString() == "" ? "" : dr["Narration"].ToString());

                cmd.Parameters.Add("@Entry_Narr", SqlDbType.NVarChar).Value = (dtdata1.Rows[0]["Entry_Narr"].ToString() == "" ? "" : dtdata1.Rows[0]["Entry_Narr"].ToString());
                cmd.Parameters.Add("@Parent_Ac_Code", SqlDbType.NVarChar).Value = dtdata1.Rows[0]["Parent_Ac_Code"].ToString();
                cmd.Parameters.Add("@Parent_Ac_Type", SqlDbType.NVarChar).Value = dtdata1.Rows[0]["Parent_Ac_Type"].ToString();
                cmd.Parameters.Add("@Parent_Amt", SqlDbType.Decimal).Value = dtdata1.Rows[0]["Parent_Amt"].ToString();
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
                }
                rowcounter++;
            }
        }
    }
}