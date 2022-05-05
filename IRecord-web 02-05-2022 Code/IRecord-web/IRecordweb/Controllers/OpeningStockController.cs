using IRecordweb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class OpeningStockController : Controller
    {
        private readonly string URL = ConfigurationManager.AppSettings["ScreenURL"];
        private readonly string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        private readonly string PdfConfig = ConfigurationManager.AppSettings["PdfConfig"];


        // GET: OpeningStock
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewOpeningStock()
        {
            return View();
        }
        [HttpGet]
        public ActionResult OpeningStock()
        {

            return View();
        }

        [HttpPost]
        public ActionResult OpeningStock(OpeningStock op, HttpPostedFileBase FileUpload)
        {




            return View();
        }

        public ActionResult OpeningStockList()
        {
            return View();
        }
        public JsonResult GetListofOpeningStock()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_INSERT_OPENING_STOCK_BrokerBill]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBROKERLIST";
            cmd.Parameters.Add("@MemberID", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.NVarChar).Value = FinancialYearCode;

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();

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
        public JsonResult FnSaveManualEntry(string JsonData, string ExcelFile)
        {
            Session["OpDetailsdt"] = null;
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            string filename = "/OpeningStock/" + Guid.NewGuid().ToString() + ".xlxs";
            if (ExcelFile != "")
            {
                WriteByteArrayToPdf(ExcelFile, System.Web.Hosting.HostingEnvironment.MapPath(filename));
                DataTable dt = ReadExcelFile(Server.MapPath(filename), dtJsonData.Rows[0]["ddlHoldingType"].ToString(), dtJsonData.Rows[0]["ddlHoldingTypetext"].ToString(), dtJsonData.Rows[0]["ddlConsultant"].ToString(), dtJsonData.Rows[0]["ddlConsultanttext"].ToString());
                Session["OpDetailsdt"] = dt;
            }
            Session["Op_HeadSection"] = dtJsonData;
            //  DataTable dtJsonData1 = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData1);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();

            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public int GetGlobalTransactionNumber1(string bktyp)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            con.Open();
            string dttod = DateTime.Today.ToString("yyyy-MM-dd");
            SqlCommand cmd43 = new SqlCommand("Insert into Global_no(Book_typ, Date, MemberID, FinancialYearMemberID, CreatedBy, CreatedDate) values('" + bktyp + "',getdate()," + MemberCode + "," + FinancialYearCode + "," + CreatedById + ",getdate());SELECT SCOPE_IDENTITY();", con);
            object vchno = cmd43.ExecuteScalar();
            return Convert.ToInt32(vchno);
        }
        [HttpPost]
        public JsonResult FnFinalSubmit(string JsonData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            Session["DisplayData"] = dtJsonData;

            string Trans_no = GetGlobalTransactionNumber1("OP").ToString();
            SaveINBrokerBill(Trans_no, dtJsonData, "INSERT");
            SaveINBrokerBill_Trans(Trans_no, dtJsonData, "INSERT");
            SaveINBrokerBill_DmatInfo(Trans_no, dtJsonData, "INSERT");
            return Json("1", JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult FnFinalUpdate(string JsonData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);


            string Trans_no = dtJsonData.Rows[0]["Trans_no"].ToString();
            SaveINBrokerBill(Trans_no, dtJsonData, "UPDATE");
            SaveINBrokerBill_Trans(Trans_no, dtJsonData, "UPDATE");
            SaveINBrokerBill_DmatInfo(Trans_no, dtJsonData, "UPDATE");
            return Json("1", JsonRequestBehavior.AllowGet);

        }

        public ActionResult ViewOpeningStockData(string TrnsNo)
        {

            #region get header data from database
            DataTable dtheaderdb = GetDBhead(TrnsNo);
            DataTable dtDeatilsdb = GetDeatilsdb(TrnsNo);
            Session["Op_HeadSection"] = dtheaderdb;
            Session["OpDetailsdt"] = dtDeatilsdb;
            #endregion

            return View("ViewOpeningStock");
        }
        public DataTable GetDBhead(string Trans_no)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_INSERT_OPENING_STOCK_BrokerBill]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWHEAD";
            cmd.Parameters.Add("@TransNo", SqlDbType.VarChar).Value = Trans_no;

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();

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

        public DataTable GetDeatilsdb(string Trans_no)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_INSERT_OPENING_STOCK_BrokerBill]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWDETAILS";
            cmd.Parameters.Add("@TransNo", SqlDbType.BigInt).Value = Trans_no;

            cmd.Connection = con;
            System.Data.DataTable DT = new System.Data.DataTable();
            try
            {
                con.Open();

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

        public void SaveINBrokerBill(string Trans_no, DataTable dtdata, string Action)
        {
            DataTable Op_HeadSection = Session["Op_HeadSection"] as DataTable;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            decimal TOTAL = 0;
            foreach (DataRow DR in dtdata.Rows)
            {
                TOTAL += Convert.ToDecimal(DR["InvestmentAmount"].ToString());
            }

            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[SP_INSERT_OPENING_STOCK_BrokerBill]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = Action;
            cmd.Parameters.Add("@Trans_no", SqlDbType.Int).Value = Trans_no;
            cmd.Parameters.Add("@Bill_no", SqlDbType.VarChar).Value = "Opening";
            cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(Session["FinFromDate"].ToString()).ToString("yyyy-MM-dd");
            cmd.Parameters.Add("@Valan_code", SqlDbType.VarChar).Value = "";
            cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
            cmd.Parameters.Add("@Broker_ID", SqlDbType.VarChar).Value = Op_HeadSection.Rows[0]["ddlBroker"].ToString();
            cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = "D";//"C"
            cmd.Parameters.Add("@Brok_Rate", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Brok_Amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = TOTAL;//sumofInvestment Amount
            cmd.Parameters.Add("@Stt_amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@ServTax_Amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Other_amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Net_amt", SqlDbType.Float).Value = TOTAL;//sumofInvestment Amount
            cmd.Parameters.Add("@Book_Type", SqlDbType.VarChar).Value = "OP";
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
            cmd.Parameters.Add("@isInternalEntry", SqlDbType.VarChar).Value = "N";
            cmd.Parameters.Add("@isFNOBill", SqlDbType.Int).Value = Op_HeadSection.Rows[0]["ddlSegment"].ToString();//SEGMENTVALUE


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

        public void SaveINBrokerBill_Trans(string Trans_no, DataTable dtdata, string Action)
        {
            DataTable Op_HeadSection = Session["Op_HeadSection"] as DataTable;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            decimal TOTAL = 0;
            foreach (DataRow DR in dtdata.Rows)
            {
                TOTAL += Convert.ToDecimal(DR["InvestmentAmount"].ToString());
            }

            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            int srno = 1;
            foreach (DataRow DR in dtdata.Rows)
            {
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[dbo].[SP_INSERT_OPENING_STOCK_BrokerBill_Trans]"
                };
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = Action;
                cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = Trans_no;
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = (Action == "UPDATE" ? DR["Sr_No"].ToString() : srno.ToString());
                cmd.Parameters.Add("@Bill_no", SqlDbType.VarChar).Value = "Opening";
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(DR["DateofPurchase"].ToString()).ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@TransType", SqlDbType.VarChar).Value = "Bought";
                cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = DR["InvestmentType"].ToString();
                cmd.Parameters.Add("@Script_ID", SqlDbType.VarChar).Value = DR["ScriptID"].ToString();
                //cmd.Parameters.Add("@MonthYear", SqlDbType.VarChar).Value = yourCShap_variable;
                //cmd.Parameters.Add("@Last_Date", SqlDbType.DateTime).Value = yourCShap_variable;
                //cmd.Parameters.Add("@LotSize", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@LotQty", SqlDbType.Float).Value = yourCShap_variable;
                cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = DR["Quantity"].ToString();
                // cmd.Parameters.Add("@Disp_qty", SqlDbType.Float).Value = yourCShap_variable;
                cmd.Parameters.Add("@Bal_qty", SqlDbType.Float).Value = DR["Quantity"].ToString();
                cmd.Parameters.Add("@G_Rate", SqlDbType.Float).Value = DR["RateofPurchase"].ToString();
                cmd.Parameters.Add("@Brok_Amt", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@ServTax_Amount", SqlDbType.Float).Value = 0;
                cmd.Parameters.Add("@Rate", SqlDbType.Float).Value = DR["RateofPurchase"].ToString();
                cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = DR["InvestmentAmount"].ToString();
                // cmd.Parameters.Add("@Stt_Amount", SqlDbType.Float).Value = yourCShap_variable;
                cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
                cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
                cmd.Parameters.Add("@Consultant_ID", SqlDbType.VarChar).Value = DR["ConsultantId"].ToString();
                cmd.Parameters.Add("@EntrySource", SqlDbType.Int).Value = 0;
                // cmd.Parameters.Add("@Ref_TransNo", SqlDbType.Int).Value = yourCShap_variable;
                // cmd.Parameters.Add("@Ref_SrNo", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@MergedEntry", SqlDbType.Int).Value = yourCShap_variable;
                cmd.Parameters.Add("@isFNOBill", SqlDbType.Int).Value = Op_HeadSection.Rows[0]["ddlSegment"].ToString();//SEGMENTVALUE;
                //cmd.Parameters.Add("@Strike_Price", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@OptionType", SqlDbType.VarChar).Value = yourCShap_variable;
                //cmd.Parameters.Add("@isIntraDay", SqlDbType.Int).Value = yourCShap_variable;
                cmd.Parameters.Add("@Brok_PerUnit", SqlDbType.Float).Value = 0;
                //cmd.Parameters.Add("@lockedbymerger", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@frac_Qty", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@Org_Trans_No", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@Org_SrNo", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@isDeleteMerger", SqlDbType.VarChar).Value = yourCShap_variable;
                //cmd.Parameters.Add("@isstock", SqlDbType.VarChar).Value = yourCShap_variable;
                //cmd.Parameters.Add("@ScripwiseExpense", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@isClient_code", SqlDbType.VarChar).Value = yourCShap_variable;
                //cmd.Parameters.Add("@isClient_Brokrage", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@c_Qty", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@C_Gross_Rate", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@C_Net_Rate", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@C_Amount", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@C_STT", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@C_STax", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@C_MisChg", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@TransactionChg", SqlDbType.Float).Value = yourCShap_variable;
                //cmd.Parameters.Add("@C_StampDuty", SqlDbType.Float).Value = yourCShap_variable;
                cmd.Parameters.Add("@Exp_Rate", SqlDbType.Float).Value = DR["RateofPurchase"].ToString();
                //  cmd.Parameters.Add("@PerExp_Unit", SqlDbType.Float).Value = yourCShap_variable;
                cmd.Parameters.Add("@FrmNm", SqlDbType.NVarChar).Value = "OP";
                //  cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = yourCShap_variable;
                //cmd.Parameters.Add("@IsSLBS", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@IsTallyTransfer", SqlDbType.Bit).Value = yourCShap_variable;
                //cmd.Parameters.Add("@IS_DELETED", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@DELETED_DATE", SqlDbType.DateTime).Value = yourCShap_variable;
                //cmd.Parameters.Add("@DELETED_BY", SqlDbType.Int).Value = yourCShap_variable;
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
                srno++;
            }
        }

        public void SaveINBrokerBill_DmatInfo(string Trans_no, DataTable dtdata, string Action)
        {
            DataTable Op_HeadSection = Session["Op_HeadSection"] as DataTable;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            decimal TOTAL = 0;
            foreach (DataRow DR in dtdata.Rows)
            {
                TOTAL += Convert.ToDecimal(DR["InvestmentAmount"].ToString());
            }

            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            int srno = 1;
            foreach (DataRow DR in dtdata.Rows)
            {
                String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[SP_INSERT_OPENING_STOCK_BrokerBill_DmatInfo]";
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = Action;
                cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = Trans_no;
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = (Action == "UPDATE" ? DR["Sr_No"].ToString() : srno.ToString());
                cmd.Parameters.Add("@Local_SrNo", SqlDbType.Int).Value = "1";
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(DR["DateofPurchase"].ToString()).ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
                cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
                cmd.Parameters.Add("@DemateID", SqlDbType.VarChar).Value = Op_HeadSection.Rows[0]["Demat_Ac_Id"].ToString();//SEGMENTVALUE;;
                cmd.Parameters.Add("@Script_ID", SqlDbType.VarChar).Value = DR["ScriptID"].ToString();
                cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = DR["Quantity"].ToString(); ;
                cmd.Parameters.Add("@Trans_Type", SqlDbType.VarChar).Value = "I";
                cmd.Parameters.Add("@internalEntry", SqlDbType.Int).Value = "0";
                //cmd.Parameters.Add("@Ref_Trans_No", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@Ref_Sr_No", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@entrySource", SqlDbType.Int).Value = yourCShap_variable;
                cmd.Parameters.Add("@Consultant_ID", SqlDbType.VarChar).Value = DR["ConsultantId"].ToString();
                //cmd.Parameters.Add("@Ref_Local_SrNo", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@MergedEntry", SqlDbType.Int).Value = yourCShap_variable;
                cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = DR["InvestmentType"].ToString();
                cmd.Parameters.Add("@frmpg", SqlDbType.VarChar).Value = "OP";
                //cmd.Parameters.Add("@IS_DELETED", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@DELETED_DATE", SqlDbType.DateTime).Value = yourCShap_variable;
                //cmd.Parameters.Add("@DELETED_BY", SqlDbType.Int).Value = yourCShap_variable;
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

        public void InsertScriptMapping(string MapScript_ID, string Script_Name)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            DataTable Op_HeadSection = Session["Op_HeadSection"] as DataTable;
            var data = Session["Op_HeadSection"];
            // IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_M_Member_Broker_Script_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = "0";
            cmd.Parameters.Add("@MemberID", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@Member_Broker_ID", SqlDbType.NVarChar).Value = Op_HeadSection.Rows[0]["ddlBroker"].ToString();
            cmd.Parameters.Add("@Script_ID", SqlDbType.NVarChar).Value = MapScript_ID;
            cmd.Parameters.Add("@InvestmentType", SqlDbType.SmallInt).Value = Op_HeadSection.Rows[0]["ddlSegment"].ToString();
            cmd.Parameters.Add("@Script_Name", SqlDbType.NVarChar).Value = Script_Name;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;

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


        public JsonResult GetOpDetailsdt()
        {
            DataTable DT = Session["OpDetailsdt"] as DataTable;


            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetOp_HeadSection()
        {

            DataTable DT = Session["Op_HeadSection"] as DataTable;
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAllBroker()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();


            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_ACCOUNTDATA]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ONBOARDBROKER";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@AccountId", SqlDbType.Int).Value = FinancialYearCode;

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
        public JsonResult GetAllConsultant()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            //int ID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = GetSubscriberID(ID);
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_CONSULTANTDATA]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ONBOARDCONSULTANT";
            cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = MemberCode;

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
            DataTable dtCloned = DT.Clone();
            dtCloned.Columns["ID"].DataType = typeof(int);
            foreach (DataRow row in DT.Rows)
            {
                dtCloned.ImportRow(row);
            }
            return Json(DataTableToJSON(dtCloned), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAllDemate()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            int ID = Convert.ToInt32(Session["UserID"].ToString());
            // int SubscriberID = GetSubscriberID(ID);
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_DEMATDATA]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ONBOARDDEMAT";

            cmd.Parameters.Add("@Memberid", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = FinancialYearCode;

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
                        dict[col.ColumnName] = (row[col]);
                    }
                    list.Add(dict);
                }
           
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(list);
        }
        public void WriteByteArrayToPdf(string inPDFByteArrayStream, string path)
        {
            byte[] data = Convert.FromBase64String(inPDFByteArrayStream.Replace("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,", ""));
            //if (Directory.Exists(path))s
            //{

            using (FileStream Writer = new System.IO.FileStream(path, FileMode.Create, FileAccess.Write))
            {

                Writer.Write(data, 0, data.Length);
            }
            //}
            //else
            //{
            //    throw new System.Exception("PDF Shared Location not found");
            //}

        }
        public DataTable ReadExcelFile(string FilePath, string InvestmentType, string InvestmentTypeName, string ConsultantId, string ConsultantName)
        {
            DataTable dtExcel = ReadExcelFilesWithCols(FilePath, ".xlsx");
            int rowcounter = 0;
            foreach (DataColumn column in dtExcel.Columns)
            {
                dtExcel.Columns[column.ColumnName].ColumnName = "Col_" + rowcounter;
                rowcounter++;

            }
            dtExcel.AcceptChanges();

            foreach (DataRow dr in dtExcel.Rows)
            {
                if (dr["Col_0"].ToString().Trim() == "Purchase Date")
                {
                    dr.Delete();
                    break;
                }
                else
                {
                    dr.Delete();
                }

            }
            dtExcel.AcceptChanges();


            DataTable dtNew = LoadDataView();
            foreach (DataRow dr in dtExcel.Rows)
            {
                DataRow drNew = dtNew.NewRow();
                try
                {
                    drNew["DateofPurchase"] = Convert.ToDateTime(dr["Col_0"].ToString()).ToString("dd-MMM-yyyy");
                }
                catch
                {
                    drNew["DateofPurchase"] = dr["Col_0"].ToString();
                }

                string ScriptName = dr["Col_1"].ToString();
                string ISINNo = dr["Col_2"].ToString();
                string dbScriptName = dr["Col_1"].ToString();
                string dbScriptID = "";
                string ScriptColor = "Red";
                try
                {
                    ScriptMaping(ScriptName, ISINNo, ref dbScriptName, ref dbScriptID, ref ScriptColor);
                    drNew["ScriptName"] = dbScriptName.ToString();
                    drNew["ScriptID"] = dbScriptID.ToString();
                    drNew["ScriptColor"] = ScriptColor.ToString();
                }
                catch
                {
                    drNew["ScriptName"] = dbScriptName.ToString();
                    drNew["ScriptID"] = dbScriptID.ToString();
                    drNew["ScriptColor"] = ScriptColor.ToString();
                }


                drNew["Quantity"] = dr["Col_3"].ToString();
                drNew["RateofPurchase"] = dr["Col_4"].ToString();
                try
                {
                    drNew["InvestmentAmount"] = (Convert.ToDouble(dr["Col_3"].ToString()) * Convert.ToDouble(dr["Col_4"].ToString())).ToString();
                }
                catch
                {
                    drNew["InvestmentAmount"] = "0";
                }
                drNew["InvestmentType"] = InvestmentType.ToString();//
                drNew["InvestmentTypeName"] = InvestmentTypeName.ToString();//
                drNew["ConsultantId"] = ConsultantId.ToString();//
                drNew["ConsultantName"] = ConsultantName.ToString();//
                dtNew.Rows.Add(drNew);
            }

            return dtNew;
        }

        public DataTable LoadDataView()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DateofPurchase", typeof(string));
            dt.Columns.Add("InvestmentType", typeof(string));
            dt.Columns.Add("InvestmentTypeName", typeof(string));
            dt.Columns.Add("ScriptName", typeof(string));
            dt.Columns.Add("ScriptID", typeof(string));
            dt.Columns.Add("ScriptColor", typeof(string));
            dt.Columns.Add("Quantity", typeof(string));
            dt.Columns.Add("RateofPurchase", typeof(string));
            dt.Columns.Add("InvestmentAmount", typeof(string));
            dt.Columns.Add("ConsultantId", typeof(string));
            dt.Columns.Add("ConsultantName", typeof(string));
            return dt;
        }

        public DataTable ReadExcelFilesWithCols(string FilePath, string Extension)
        {
            string isHDR = "Yes";
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = string.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();
            return dt;
        }

        public void ScriptMaping(string ScriptName, string ISINNo, ref string dbScriptName, ref string dbScriptID, ref string ScriptColor)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[SP_SCRIPT_MAPPING]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = (ISINNo != "" ? "MAPBYISIN" : "MAPBYNAME");

            cmd.Parameters.Add("@VAR", SqlDbType.NVarChar).Value = (ISINNo != "" ? ISINNo : ScriptName);

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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(DT);
                }
                if (DT.Rows.Count > 0)
                {
                    dbScriptID = DT.Rows[0]["ScriptID"].ToString();
                    dbScriptName = DT.Rows[0]["ScriptName"].ToString();
                    ScriptColor = "";
                }
                else
                {
                    dbScriptID = "";
                    dbScriptName = ScriptName;
                    ScriptColor = "Red";
                    ScriptMapingByUserDefind(ScriptName, ref dbScriptName, ref dbScriptID, ref ScriptColor);
                }
            }
            catch (Exception ex)
            {
                dbScriptID = "";
                dbScriptName = ScriptName;
                ScriptColor = "Red";
                ScriptMapingByUserDefind(ScriptName, ref dbScriptName, ref dbScriptID, ref ScriptColor);
                // throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            //return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);
        }

        public void ScriptMapingByUserDefind(string ScriptName, ref string dbScriptName, ref string dbScriptID, ref string ScriptColor)
        {
            DataTable Op_HeadSection = Session["Op_HeadSection"] as DataTable;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[SP_SCRIPT_MAPPING]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "MAPBY_USER_DEFIND_MAP";
            cmd.Parameters.Add("@MemberID", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@Member_Broker_ID", SqlDbType.NVarChar).Value = Op_HeadSection.Rows[0]["ddlBroker"].ToString();
            cmd.Parameters.Add("@Script_Name", SqlDbType.NVarChar).Value = ScriptName.Trim();
            cmd.Parameters.Add("@InvestmentType", SqlDbType.NVarChar).Value = Op_HeadSection.Rows[0]["ddlSegment"].ToString();


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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(DT);
                }
                if (DT.Rows.Count > 0)
                {
                    dbScriptID = DT.Rows[0]["ScriptID"].ToString();
                    dbScriptName = DT.Rows[0]["ScriptName"].ToString();
                    ScriptColor = "";
                }
                else
                {

                    dbScriptID = "";
                    dbScriptName = ScriptName;
                    ScriptColor = "Red";

                }
            }
            catch (Exception ex)
            {
                dbScriptID = "";
                dbScriptName = ScriptName;
                ScriptColor = "Red";

                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            //return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);
        }
    }
}