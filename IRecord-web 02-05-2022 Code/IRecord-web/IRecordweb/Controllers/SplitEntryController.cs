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
    public class SplitEntryController : Controller
    {
        // GET: SplitEntry
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SplitEntryView()
        {
            return View();
        }
        public ActionResult AddSplitEntry()
        {
            return View();
        }
        public JsonResult GetScpirt()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_CADetails]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ViewForSplitEntry";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@Var2", SqlDbType.Int).Value = FinancialYearCode;

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

        public JsonResult GetAllList()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_CADetails]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ViewHeader";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@Var2", SqlDbType.Int).Value = FinancialYearCode;

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
        public JsonResult SaveData(string JsonData, string Details)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Details);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            SaveINBrokerBill(dtJsonData.Rows[0]["txt_TransactionNo"].ToString(), dtJsonData);
            SaveINBrokerBill_Trans(dtJsonData.Rows[0]["txt_TransactionNo"].ToString(), dtDetails, dtJsonData);
            SaveINBrokerBill_DmatInfo(dtJsonData.Rows[0]["txt_TransactionNo"].ToString(), dtDetails, dtJsonData);

            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public void SaveINBrokerBill(string Trans_no, DataTable dtdata)
        {
            DataTable Op_HeadSection = Session["Op_HeadSection"] as DataTable;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
          

            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();

            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_SplitEntry_BrokerBill]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@Trans_no", SqlDbType.Int).Value = Trans_no;
            cmd.Parameters.Add("@Bill_no", SqlDbType.VarChar).Value = "SPLIT";
            cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(dtdata.Rows[0]["txt_RecordDate"].ToString()).ToString("yyyy/MM/dd");
            cmd.Parameters.Add("@Valan_code", SqlDbType.VarChar).Value = "";
            cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
            cmd.Parameters.Add("@Broker_ID", SqlDbType.VarChar).Value = "";
            cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = "D";
            cmd.Parameters.Add("@Brok_Rate", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Brok_Amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Stt_amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@ServTax_Amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Other_amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Net_amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Book_Type", SqlDbType.VarChar).Value = "SP";
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
            cmd.Parameters.Add("@Ref_TransNo", SqlDbType.Int).Value = "0";
            cmd.Parameters.Add("@isInternalEntry", SqlDbType.VarChar).Value = "N";
            cmd.Parameters.Add("@isFNOBill", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@Narrat1", SqlDbType.VarChar).Value = dtdata.Rows[0]["txt_Description"].ToString();
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

        public void SaveINBrokerBill_Trans(string Trans_no, DataTable dtdata, DataTable dtJsonData)
        {
            DataTable Op_HeadSection = Session["Op_HeadSection"] as DataTable;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
         
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            int srno = 1;
            foreach (DataRow DR in dtdata.Rows)
            {
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
               
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_SplitEntry_BrokerBill_Trans]";
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = Trans_no;
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = srno;
                cmd.Parameters.Add("@Bill_no", SqlDbType.VarChar).Value = "SPLIT";
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(DR["Date"].ToString()).ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@TransType", SqlDbType.VarChar).Value = "Bought";
                cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["ddlHoldingType"].ToString();
                cmd.Parameters.Add("@Script_ID", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["Script_id"].ToString(); 
                cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = DR["SplitQuantity"].ToString();
                cmd.Parameters.Add("@G_Rate", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@Brok_Amt", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@ServTax_Amount", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@Rate", SqlDbType.Float).Value = DR["SplitRate"].ToString();
                cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = DR["Amount"].ToString(); 
                cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
                cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
                cmd.Parameters.Add("@Consultant_ID", SqlDbType.VarChar).Value = DR["Consultant_ID"].ToString();
                cmd.Parameters.Add("@EntrySource", SqlDbType.Int).Value = 2;
                cmd.Parameters.Add("@isIntraDay", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Brok_PerUnit", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@isFNOBill", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FrmNm", SqlDbType.NVarChar).Value = "SP";
                cmd.Parameters.Add("@Bal_qty", SqlDbType.Float).Value = DR["SplitQuantity"].ToString();
                cmd.Parameters.Add("@frac_Qty", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@Exp_Rate", SqlDbType.Float).Value = DR["SplitRate"].ToString();
                cmd.Parameters.Add("@C_Gross_Rate", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@old_Trans_No", SqlDbType.Int).Value = DR["Trans_No"].ToString();
                cmd.Parameters.Add("@old_Sr_No", SqlDbType.Int).Value = DR["Sr_No"].ToString();
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
                srno++;
            }
        }

        public void SaveINBrokerBill_DmatInfo(string Trans_no, DataTable dtdata,DataTable dtJsonData)
        {
            DataTable Op_HeadSection = Session["Op_HeadSection"] as DataTable;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
          
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            int srno = 1;
            foreach (DataRow DR in dtdata.Rows)
            {
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
               
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_SplitEntry_BrokerBill_DmatInfo]";
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = Trans_no;
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = DR["Sr_No"].ToString();
                cmd.Parameters.Add("@Local_SrNo", SqlDbType.Int).Value = srno;
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(DR["Date"].ToString()).ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
                cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
                cmd.Parameters.Add("@DemateID", SqlDbType.VarChar).Value = DR["DematID"].ToString();
                cmd.Parameters.Add("@Script_ID", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["Script_id"].ToString(); ;
                cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = DR["SplitQuantity"].ToString(); ;
                cmd.Parameters.Add("@Trans_Type", SqlDbType.VarChar).Value = "I";
                cmd.Parameters.Add("@internalEntry", SqlDbType.Int).Value = "0";
                cmd.Parameters.Add("@Consultant_ID", SqlDbType.VarChar).Value = DR["Consultant_ID"].ToString() ;
                cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = dtJsonData.Rows[0]["ddlHoldingType"].ToString(); ;
                cmd.Parameters.Add("@frmpg", SqlDbType.VarChar).Value = "SP";
                cmd.Parameters.Add("@old_Trans_No", SqlDbType.Int).Value = DR["Trans_No"].ToString();
                cmd.Parameters.Add("@old_Sr_No", SqlDbType.Int).Value = DR["Sr_No"].ToString();
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
                srno++;
            }
        }
        public JsonResult GetData(string Script_Id,string Trans_Dt)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_CADetails]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ViewData";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = Script_Id.Split(':')[1].ToString();
            cmd.Parameters.Add("@Var2", SqlDbType.NVarChar).Value =Convert.ToDateTime(Trans_Dt).ToString("yyyy/MM/dd");
            cmd.Parameters.Add("@Var3", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@Var4", SqlDbType.NVarChar).Value = FinancialYearCode;
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

        public JsonResult GetTableData()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[Sp_split_bill_view_data]"
            };
            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();
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


        public JsonResult GetDataWithView(string Trans_No)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_CADetails]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "GetDataWithView";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = Trans_No.ToString();
           
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

        public JsonResult IsDeleteData(string Trans_No)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_CADetails]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "DELETE_SPLIT";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = Trans_No.ToString();

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
                    dict[col.ColumnName] = (row[col]);
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

        public JsonResult GetTranstionNO()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            return Json(GetGlobalTransactionNumber("SPLIT"), JsonRequestBehavior.AllowGet);

        }
    }
}