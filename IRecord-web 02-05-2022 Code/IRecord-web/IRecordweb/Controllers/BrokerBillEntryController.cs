using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using BAL;
using System.Web.Mvc;
using System.Web;
using IRecordweb.Models;
using RestSharp;
using Newtonsoft.Json;
using IRecordweb.ViewModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Net;

namespace IRecordweb.Controllers
{
    public class BrokerBillEntryController : Controller
    {
        DAL.BrokerBillDAL obj = new DAL.BrokerBillDAL();

        string PdfConfig = ConfigurationManager.AppSettings["PdfConfig"];
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        MType mtype = new MType();
        // GET: BrokerBillEntry
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get_Set_As_Default(string SegmentId)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_SET_AS_DEFAULT_CN]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEWBYID";
            cmd.Parameters.Add("@SegmentId", SqlDbType.Int).Value = SegmentId;

            cmd.Parameters.Add("@MemberID", SqlDbType.BigInt).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.BigInt).Value = FinancialYearCode;
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
            return Json((DT != null ? DataTableToJSON(DT) : "1"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Set_Set_As_Default(string JsonData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_SET_AS_DEFAULT_CN]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@SegmentId", SqlDbType.Int).Value = dtJsonData.Rows[0]["ddlInvestmentType"].ToString();
            cmd.Parameters.Add("@BrokerId", SqlDbType.Int).Value = dtJsonData.Rows[0]["ddlBroker"].ToString();

            try
            {
                cmd.Parameters.Add("@HoldingTypeId", SqlDbType.Int).Value = dtJsonData.Rows[0]["ddlHoldingType"].ToString();
            }
            catch { }
            try
            {
                cmd.Parameters.Add("@DematId", SqlDbType.Int).Value = dtJsonData.Rows[0]["Demat_Ac_Id"].ToString();
            }
            catch { }
            
            cmd.Parameters.Add("@ConsultantId", SqlDbType.Int).Value = dtJsonData.Rows[0]["ddlConsultant"].ToString();
            cmd.Parameters.Add("@FormatSr_No", SqlDbType.Int).Value = dtJsonData.Rows[0]["ContractNoteId"].ToString();
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = dtJsonData.Rows[0]["Password"].ToString();
            cmd.Parameters.Add("@MemberID", SqlDbType.BigInt).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.BigInt).Value = FinancialYearCode;
           
            cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedById;
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
            return Json((DT != null ? DataTableToJSON(DT) : "1"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteHeadPart(string Trans_no)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_VIEW_CN_NOTE]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "Delete_Head";
            cmd.Parameters.Add("@Trans_no", SqlDbType.BigInt).Value = Trans_no;
            cmd.Parameters.Add("@DELETED_BY", SqlDbType.Int).Value = CreatedById;

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
        public JsonResult DeleteDetails(string ID)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_VIEW_CN_NOTE]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "Delete_deatils";
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
            cmd.Parameters.Add("@DELETED_BY", SqlDbType.Int).Value = CreatedById;

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
        public ActionResult BrokerBillList()
        {
            return View();
        }
        public JsonResult GetListofCn()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_VIEW_CN_NOTE]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ViewCn";
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
        public ActionResult ManualEntry()
        {
            return View();
        }
        [HttpPost]
        public JsonResult FnSaveManualEntryForEquity(string JsonData, string ExpenseData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            share_Inv_Eqty_AC = dtJsonData.Rows[0]["ddlHoldingType"].ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ExpenseData);

            string bktyp = "BB";
            int vchno = GetGlobalTransactionNumber(bktyp);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            cmd.CommandTimeout = 0;
            transaction = con.BeginTransaction("SampleTransaction");
            #region //save Enq seg
            try
            {
                string LedgerDate = Convert.ToDateTime(dtJsonData.Rows[0]["txtDate"].ToString()).ToString("yyyy-MM-dd");
                int counter = 0;
                string drorcr = "";
                double amtbfacI = 0;
                double amtbfacT = 0;
                double amtsfacI = 0;
                double amtsfacT = 0;
                double _totalINV_Shareval = 0;
                double _totalINV_StockTradeval = 0;
                int nre = 0;

                string BrokerCode = "0";
                string styp = "0";
                txtBrokerage.Text = "0";
                txtNetAmt.Text = "0";
                txtGrossNetAmt.Text = "0";
                for (counter = 0; counter < dtJsonData.Rows.Count; counter++)
                {
                    BrokerCode = dtJsonData.Rows[counter]["ddlBroker"].ToString();
                    txtBillNo.Text = dtJsonData.Rows[counter]["txt_BillNo"].ToString();
                    txtSettlementNo.Text = dtJsonData.Rows[counter]["txt_SettlementNo"].ToString();
                    txtBrokerage.Text = (Convert.ToDouble(txtBrokerage.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_BrokerageAmount"].ToString())).ToString();
                    txtNetAmt.Text = (Convert.ToDouble(txtNetAmt.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_NetAmount"].ToString())).ToString();
                    txtExpenses.Text = "0";
                    txtGrossNetAmt.Text = (Convert.ToDouble(txtGrossNetAmt.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_GrossAmount"].ToString())).ToString();
                    txtRemarks.Text = "";
                    txtInvestment.Text = "0";
                    invstyp = dtJsonData.Rows[counter]["ddlSegment"].ToString();
                    styp = dtJsonData.Rows[counter]["HoldingTypetext"].ToString();
                    string[] detailss = styp.Split('-');
                    if (dtJsonData.Rows[counter]["Type"].ToString() == "Bought" & detailss[0] == "I")
                        amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                    if (dtJsonData.Rows[counter]["Type"].ToString() == "Bought" & detailss[0] == "T")
                        amtbfacT = Math.Round(amtbfacT + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                    if (dtJsonData.Rows[counter]["Type"].ToString() == "Sold" & detailss[0] == "I")
                        amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                    if (dtJsonData.Rows[counter]["Type"].ToString() == "Sold" & detailss[0] == "T")
                        amtsfacT = Math.Round(amtsfacT + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                    if (amtbfacI > amtsfacI)
                        _totalINV_Shareval = (amtbfacI - amtsfacI);
                    else
                        _totalINV_Shareval = (amtbfacI - amtsfacI);

                    #region stock in trade  
                    if (amtbfacT > amtsfacT)
                        _totalINV_StockTradeval = (amtbfacT - amtsfacT);
                    else
                        _totalINV_StockTradeval = (amtbfacT - amtsfacT);
                    #endregion
                }
                double TotalAmount = Convert.ToDouble(_totalINV_Shareval - _totalINV_StockTradeval * -1);

                if (TotalAmount > 0)
                    drorcr = "C";
                else
                    drorcr = "D";

                if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    drorcr = "C";

                if (Convert.ToInt32(vchno) > 0)
                {
                    cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,Amount,"
                        + " Stt_amt,ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('" + vchno + "','"
                        + txtBillNo.Text + "','" + LedgerDate + "','" + txtSettlementNo.Text + "','" + MemberCode + "','" + BrokerCode + "','" + drorcr + "','0','"
                        + Math.Abs(Convert.ToDouble(txtBrokerage.Text)) + "','" + (Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text))) + "','0','0','"
                        + (Math.Abs(Convert.ToDouble(txtExpenses.Text))) + "','" + (Math.Abs(Convert.ToDouble(txtNetAmt.Text))) + "','BB','" + FinancialYearCode + "','','N','"
                        + invstyp + "','" + txtRemarks.Text + "')", con);
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();

                    string narr0 = txtBillNo.Text.ToString();
                    string narrr = txtSettlementNo.Text.ToString();
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    string drcd = "";
                    string crcd = "";
                    int trsrno = 1;

                    if (!checkBox3.Checked)
                    {
                        #region vasant share Investment                     
                        if (Convert.ToDouble((_totalINV_Shareval)) > 0)
                        {
                            drcd = share_Inv_Eqty_AC;
                            crcd = BrokerCode;
                        }
                        else
                        {
                            drcd = BrokerCode;
                            crcd = share_Inv_Eqty_AC;
                        }
                        // If Gross Amont Less Then Expense Amount Share Investment
                        if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtInvestment.Text))
                        {
                            cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                + "[Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]) VALUES('" + vchno + "','" + trsrno + "','"
                                + LedgerDate + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                                + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_Shareval) + "','" + invstyp + "','BB','0')", con);
                            cmd.Transaction = transaction;
                            nre = cmd.ExecuteNonQuery();

                            if (nre > 0)
                                trsrno = trsrno + 1;
                        }
                        //// If Expense Amont Less Then Gross Amount Share Investment END
                        else
                        {
                            cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                               + " [Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]) VALUES('" + vchno + "','" + trsrno + "','"
                               + LedgerDate + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                               + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_Shareval) + "','" + invstyp + "','BB','0')", con);
                            cmd.Transaction = transaction;
                            nre = cmd.ExecuteNonQuery();
                            if (nre > 0)
                                trsrno = trsrno + 1;
                        }
                        #endregion

                        #region vasant Stock In Trade
                        if (Convert.ToDouble((_totalINV_StockTradeval)) > 0)
                        {
                            drcd = shrtradeinveqty;
                            crcd = BrokerCode;
                        }
                        else
                        {
                            drcd = BrokerCode;
                            crcd = shrtradeinveqty;
                        }

                        // if  Gross Amont Stock In Trade Entry Less Then Expense Amount

                        if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                        {
                            cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                + " [Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                                + LedgerDate + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                                + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_StockTradeval) + "','" + invstyp + "','BB','0')", con);
                            cmd.Transaction = transaction;
                            nre = cmd.ExecuteNonQuery();
                            if (nre > 0)
                                trsrno = trsrno + 1;
                        }
                        // if Stock In Trade Entry Stock Amount Less Then Expense Amount END
                        else
                        {
                            cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                + " [Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                                + LedgerDate + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                                + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_StockTradeval) + "','" + invstyp + "','BB','0')", con);
                            cmd.Transaction = transaction;
                            nre = cmd.ExecuteNonQuery();
                            if (nre > 0)
                                trsrno = trsrno + 1;
                        }
                        #endregion
                    }
                    else // SLBM ENTRY
                    {
                        if (Convert.ToDouble((_totalINV_Shareval)) > 0)
                        {
                            drcd = "1100034";

                            crcd = BrokerCode;
                        }
                        else
                        {
                            drcd = BrokerCode;
                            crcd = "1100034";
                        }

                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]) VALUES('" + vchno + "','" + trsrno + "','"
                            + LedgerDate + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                            + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_Shareval) + "','" + invstyp + "','BB','0')", con);
                        cmd.Transaction = transaction;
                        nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }
                    for (int counter1 = 0; counter1 < (dtJsonData.Rows.Count); counter1++)
                    {
                        string actyp = "";
                        styp = "";
                        if (!checkBox3.Checked)
                        {
                            styp = dtJsonData.Rows[counter1]["HoldingTypetext"].ToString();
                            string[] details = styp.Split('-');
                            if (details[0] == "I")
                                actyp = share_Inv_Eqty_AC;
                            else if (details[0] == "T")
                                actyp = shrtradeinveqty;
                        }
                        else // SLBM ENTRY Holding Type
                        {
                            actyp = "1100034";
                        }

                        int intrday = 0;
                        if (dtJsonData.Columns.Contains("chk_intraday"))
                        {
                            if (dtJsonData.Rows[counter1]["chk_intraday"] != null
                                && bool.Parse(dtJsonData.Rows[counter1]["chk_intraday"].ToString()))
                                intrday = 1;
                        }
                        else
                        {
                            intrday = 0;
                        }
                        string sccd = "";
                        if (dtJsonData.Rows[counter1]["ddlScript_Id"] != null)
                            sccd = dtJsonData.Rows[counter1]["ddlScript_Id"].ToString().Trim();
                        string ccd = "";

                        if (dtJsonData.Rows[counter1]["ddlConsultant"] != null)
                            ccd = dtJsonData.Rows[counter1]["ddlConsultant"].ToString().Trim();

                        string dmcd = "";
                        if (dtJsonData.Rows[counter1]["ddlHoldingType"] != null)
                            dmcd = dtJsonData.Rows[counter1]["ddlHoldingType"].ToString().Trim();

                        string trnstypnw = "";
                        if (dtJsonData.Rows[counter1]["Type"].ToString() == "Bought")
                            trnstypnw = "I";
                        else
                            trnstypnw = "O";

                        var srnr = counter1 + 1;
                        if (intrday == 0)
                        {
                            cmd = new SqlCommand("INSERT INTO BrokerBill_DmatInfo(Trans_No,Sr_No,Local_SrNo,Trans_Dt,MemberID,FinancialYearMemberID,DemateID,Script_ID,Qty,Trans_Type,"
                                + " Consultant_ID,Ac_Type,internalEntry,frmpg)values('" + vchno + "','" + srnr + "','1','" + LedgerDate + "','" + MemberCode + "','"
                                + FinancialYearCode + "','" + dmcd + "','" + sccd + "','" + dtJsonData.Rows[counter1]["txt_Quantity"].ToString() + "','" + trnstypnw + "','"
                                + ccd + "','" + actyp + "','0','BB')", con);
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                        Trasndate = dtJsonData.Rows[counter1]["txtDate"].ToString();
                        int SLBS = checkBox3.Checked ? 1 : 0;
                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(Trasndate).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(Trasndate).ToString("yyyy-MM-dd");
                        }

                        double bkrch = Math.Round(Math.Round(double.Parse(dtJsonData.Rows[counter1]["txt_BrokerageAmount"].ToString()), 4) / Math.Round(double.Parse(dtJsonData.Rows[counter1]["txt_Quantity"].ToString()), 4), 4);
                        cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No,Sr_No,Bill_no,Trans_Dt,TransType,Ac_Type,Script_ID,Qty,Bal_qty,G_Rate,Brok_Amt,ServTax_Amount,"
                            + " Rate,Amount,MemberID,FinancialYearMemberID,Consultant_ID,EntrySource,isIntraDay,Brok_PerUnit,isFNOBill,C_Gross_Rate,Exp_Rate,FrmNm,IsSLBS)values('" + vchno + "','"
                            + srnr + "','" + txtBillNo.Text + "','" + Trasndate + "','" + dtJsonData.Rows[counter1]["Type"].ToString() + "','" + actyp + "','" + sccd + "','"
                            + dtJsonData.Rows[counter1]["txt_Quantity"].ToString() + "','" + dtJsonData.Rows[counter1]["txt_Quantity"].ToString() + "','"
                            + dtJsonData.Rows[counter1]["txt_GrossRate"].ToString() + "','" + dtJsonData.Rows[counter1]["txt_BrokerageAmount"].ToString() + "','0','"
                            + dtJsonData.Rows[counter1]["txt_NetRate"].ToString() + "','" + dtJsonData.Rows[counter1]["txt_NetAmount"].ToString() + "','"
                            + MemberCode + "','" + FinancialYearCode + "','" + ccd + "','0','" + intrday + "','" + bkrch + "','" + invstyp + "','"
                            + dtJsonData.Rows[counter1]["txt_GrossRate"].ToString() + "','" + dtJsonData.Rows[counter1]["txt_NetRate"].ToString() + "','BB','"
                            + SLBS + "')", con);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }
                    //Expense Code Added By Poonam
                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {
                        if (Dt_Expanses.Rows[counter1]["AMOUNT"] != null)
                        {
                            string accttyypp4 = ""; int Sr_No = 1;
                            double otrxpc = Convert.ToDouble(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString()));
                            if (otrxpc > 0)
                            {
                                string selextype = "";
                                if (Dt_Expanses.Rows[counter1]["ADDLESS"].ToString() == "A")
                                {
                                    selextype = "A";
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],[EntryType]) VALUES('" + vchno + "','"
                                        + trsrno + "','" + LedgerDate + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + Dt_Expanses.Rows[counter1]["EXPID"].ToString() + "','"
                                        + BrokerCode + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString()), decmlpl) + "','"
                                        + invstyp + "','" + selextype + "','BB','10')", con);
                                    cmd.Transaction = transaction;
                                    nre = cmd.ExecuteNonQuery();
                                }
                                else if (Dt_Expanses.Rows[counter1]["ADDLESS"].ToString() == "L")
                                {
                                    selextype = "L";
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],[EntryType]) VALUES('" + vchno + "','"
                                        + trsrno + "','" + LedgerDate + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + BrokerCode + "','"
                                        + Dt_Expanses.Rows[counter1]["EXPID"].ToString() + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"]
                                        .ToString()), decmlpl) + "','" + invstyp + "','" + selextype + "','BB','10')", con);
                                    cmd.Transaction = transaction;
                                    nre = cmd.ExecuteNonQuery();
                                }
                                if (nre > 0)
                                    trsrno = trsrno + 1;
                            }
                        }
                    }

                    transaction.Commit();
                    con.Close();
                    //End Here 
                    //MessageBox.Show("Record Saved Successfully !!");
                    //btnSave.Enabled = false;
                }


                //transaction.Commit();
                //con.Close();
            }
            catch (Exception ex) { transaction.Rollback(); con.Close(); }
            #endregion




            return Json("1", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult FnSaveManualEntryForFNO(string JsonData, string ExpenseData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ExpenseData);

            string bktyp = "BB";
            int vchno = GetGlobalTransactionNumber(bktyp);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            cmd.CommandTimeout = 0;
            transaction = con.BeginTransaction("SampleTransaction");
            #region //save Enq seg
            try
            {
                // if (Validate_FNO())
                if (1 == 1)
                {
                    //int vchno = 0;
                    //if (flag == 1)
                    //{
                    //    string bktyp = "BB";
                    //    vchno = GetGlobalTransactionNumber(bktyp);
                    //}
                    //else if (flag == 2)
                    //{
                    //    vchno = Convert.ToInt32(transnobb.Text.Trim());
                    //    cmd = new SqlCommand("delete from  BrokerBill where Trans_no='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code
                    //        + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };
                    //    cmd.ExecuteNonQuery();

                    //    cmd = new SqlCommand("delete from  BrokerBill_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code
                    //        + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };
                    //    cmd.ExecuteNonQuery();

                    //    cmd = new SqlCommand("delete from  AC_Trans where Trans_No='" + transnobb.Text.Trim() + "'  and MemberID='" + MenuMaster.member_code
                    //        + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };
                    //    cmd.ExecuteNonQuery();
                    //}
                    string theDate1 = Convert.ToDateTime(dtJsonData.Rows[0]["txtDate"].ToString()).ToString("yyyy-MM-dd");
                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where Ac_Name='" + txtBroker.Text + "' and  (Member_Code='"
                    //    + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};

                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where  (Member_Code='"
                    //  + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //cmd = new SqlCommand("SELECT cast(AccountId as Nvarchar) as AccountId FROM [M_Account] where  (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)

                    //{
                    //    Transaction = transaction
                    //};
                    //SqlDataReader dr2 = cmd.ExecuteReader();
                    //dr2.Read();ddlBroker
                    string oc2 = dtJsonData.Rows[0]["ddlBroker"].ToString();
                    //  dr2.Close();

                    double AmountBrought = 0;
                    double Amountsold = 0;
                    string narr0 = dtJsonData.Rows[0]["txt_BillNo"].ToString();
                    string narrr = dtJsonData.Rows[0]["txt_SettlementNo"].ToString();
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    string drorcr = "";
                    double amtbfacI = 0.0;
                    int nre;
                    double amtsfacI = 0.0;
                    txtBrokerage.Text = "0";
                    txtNetAmt.Text = "0";
                    txtGrossNetAmt.Text = "0";
                    for (int counter = 0; counter < (dtJsonData.Rows.Count); counter++)
                    {
                        string BrokerCode = dtJsonData.Rows[counter]["ddlBroker"].ToString();
                        txtBillNo.Text = dtJsonData.Rows[counter]["txt_BillNo"].ToString();
                        txtSettlementNo.Text = dtJsonData.Rows[counter]["txt_SettlementNo"].ToString();
                        txtBrokerage.Text = (Convert.ToDouble(txtBrokerage.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_BrokerageAmount"].ToString())).ToString();
                        txtNetAmt.Text = (Convert.ToDouble(txtNetAmt.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_NetAmount"].ToString())).ToString();
                        txtExpenses.Text = "0";
                        txtGrossNetAmt.Text = (Convert.ToDouble(txtGrossNetAmt.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_GrossAmount"].ToString())).ToString();
                        txtRemarks.Text = "";
                        txtInvestment.Text = "0";
                        txtInvestment.Text = "0";
                        invstyp = dtJsonData.Rows[counter]["ddlSegment"].ToString();
                        string styp = dtJsonData.Rows[counter]["HoldingTypetext"].ToString();


                        if (dtJsonData.Rows[counter]["Type"].ToString() == "Bought")
                            amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                        if (dtJsonData.Rows[counter]["Type"].ToString() == "Sold")
                            amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                        if (amtbfacI > amtsfacI)
                            AmountBrought = AmountBrought + (amtbfacI - amtsfacI);
                        else
                            Amountsold = Amountsold + (amtbfacI - amtsfacI);
                        amtsfacI = 0;
                        amtbfacI = 0;
                    }
                    double Amount = (AmountBrought - (Math.Abs(Amountsold)));
                    if (Amount > 0)
                        drorcr = "C";
                    else
                        drorcr = "D";

                    //if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    //    drorcr = "C";

                    int trsrno = 1;
                    string drcd = "";
                    string crcd = "";

                    cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,Amount,"
                        + " Stt_amt,ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('" + vchno + "','"
                        + txtBillNo.Text + "','" + theDate1 + "','" + txtSettlementNo.Text + "','" + MemberCode + "','" + oc2 + "','" + drorcr + "','0','"
                        + (Math.Abs(Convert.ToDouble(txtBrokerage.Text))) + "','" + (Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text))) + "','0','0','"
                        + (Math.Abs(Convert.ToDouble(txtExpenses.Text))) + "','" + (Math.Abs(Convert.ToDouble(txtNetAmt.Text))) + "','BB','" + FinancialYearCode + "','','N','"
                        + invstyp + "','" + txtRemarks.Text + "');SELECT SCOPE_IDENTITY();", con)
                    {
                        Transaction = transaction
                    };

                    cmd.ExecuteNonQuery();
                    // label3.Refresh();
                    if (Convert.ToDouble(Math.Round(Amount)) > 0)
                    {
                        drcd = fnoshrtrd;
                        crcd = oc2;
                    }
                    else
                    {
                        drcd = oc2;
                        crcd = fnoshrtrd;
                    }

                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_Code] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]  ) VALUES('" + vchno + "','"
                            + trsrno + "','" + theDate1 + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','"
                            + drcd + "','" + crcd + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]  ) VALUES('" + vchno + "','" + trsrno + "','"
                            + theDate1 + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                            + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }

                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {
                        if (Dt_Expanses.Rows[counter1]["AMOUNT"] != null)
                        {
                            double otrxpc = (double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString()));
                            if (otrxpc > 0)
                            {
                                //cmd = new SqlCommand("SELECT AccountId FROM M_Account where Name='" + Dt_Expanses.Rows[counter1]["expnm"].ToString() + "' and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)
                                //{
                                //    Transaction = transaction
                                //};
                                //SqlDataReader dr4034 = cmd.ExecuteReader();
                                //// dr4034.Read();
                                ////dr4034.Close();
                                //// string accttyypp5 = "";
                                //string accttyypp5 = "";
                                //if (dr4034.HasRows)
                                //{

                                //    dr4034.Read();
                                //    accttyypp5 = dr4034[0].ToString();
                                //}
                                //dr4034.Close();
                                //  int accttyypp5 = Convert.ToInt32(dr4034.GetString(0));
                                //  string accttyypp5 = accttyypp15.ToString();
                                // dr4034.Close();

                                string selextype = "";
                                if (Dt_Expanses.Rows[counter1]["ADDLESS"].ToString() == "A")
                                    selextype = "A";
                                if (Dt_Expanses.Rows[counter1]["ADDLESS"].ToString() == "L")
                                    selextype = "L";
                                int nre2 = 0;

                                if (selextype == "A") // If Expense Type Add
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],"
                                        + " [EntryType] ) VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MemberCode + "','" + FinancialYearCode
                                        + "','" + txtBillNo.Text + "','" + Dt_Expanses.Rows[counter1]["EXPID"].ToString() + "','" + oc2 + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString()), decmlpl) + "','"
                                        + invstyp + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                else  // if Expanse type Less
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,"
                                        + " [Cr_AccountId] ,[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],"
                                        + " [EntryType] ) VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MemberCode + "','" + FinancialYearCode
                                        + "','" + txtBillNo.Text + "','" + oc2 + "','" + Dt_Expanses.Rows[counter1]["EXPID"].ToString() + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString()), decmlpl) + "','" + invstyp + "','"
                                        + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }

                                if (nre2 > 0)
                                    trsrno = trsrno + 1;
                            }
                        }
                    }

                    for (int counter1 = 0; counter1 < (dtJsonData.Rows.Count); counter1++)
                    {
                        double bkrch = Math.Round(double.Parse(dtJsonData.Rows[counter1]["txt_BrokerageAmount"].ToString()) / double.Parse(dtJsonData.Rows[counter1]["txt_Quantity"].ToString()), decmlpl);
                        string expdt = "";
                        string expmonth = "";

                        if (dtJsonData.Rows[counter1]["ExpiryDate"] != null)
                            expdt = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");

                        string sccd = dtJsonData.Rows[counter1]["ddlScript_Id"].ToString();
                        string ccd = dtJsonData.Rows[counter1]["ddlConsultant"].ToString();

                        if (dtJsonData.Rows[counter1]["ExpiryDate"] != null)
                            expmonth = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("MMMM-yyyy");

                        var srnr = counter1 + 1;
                        string optyp = "";
                        string sttyp = "";

                        if (dtJsonData.Rows[counter1]["OptionType"] != null)
                            optyp = dtJsonData.Rows[counter1]["OptionType"].ToString();

                        if (dtJsonData.Rows[counter1]["StockType"] != null)
                            sttyp = dtJsonData.Rows[counter1]["StockType"].ToString();

                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(dtJsonData.Rows[counter1]["txtDate"].ToString()).ToString("yyyy-MM-dd");
                        }
                        cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No, Sr_No, Bill_no, Trans_Dt, TransType, Ac_Type, Script_ID, Qty, G_Rate, Brok_Amt,"
                            + " ServTax_Amount, Rate, Amount, MemberID, FinancialYearMemberID, Consultant_ID, EntrySource,Brok_PerUnit, MonthYear, Last_Date, LotSize, LotQty,"
                            + " isFNOBill, Strike_Price, OptionType, isstock,FrmNm)values('" + vchno + "', '" + srnr + "', '" + txtBillNo.Text + "', '" + Trasndate
                            + "', '" + dtJsonData.Rows[counter1]["Type"].ToString() + "', '', '" + sccd + "', '"
                            + dtJsonData.Rows[counter1]["txt_Quantity"].ToString() + "', '" + dtJsonData.Rows[counter1]["txt_GrossRate"].ToString()
                            + "', '" + dtJsonData.Rows[counter1]["txt_BrokerageAmount"].ToString() + "', '0', '" + dtJsonData.Rows[counter1]["txt_NetRate"].ToString()
                            + "', '" + dtJsonData.Rows[counter1]["txt_NetAmount"].ToString() + "', '" + MemberCode + "', '" + FinancialYearCode + "', '"
                            + ccd + "', '0', '" + bkrch + "','" + expmonth + "','" + expdt + "','" + dtJsonData.Rows[counter1]["Lott"].ToString() + "','"
                            + dtJsonData.Rows[counter1]["LotQty"].ToString() + "','" + invstyp + "','"
                            + dtJsonData.Rows[counter1]["StrikePrice"].ToString() + "','" + optyp + "','" + sttyp + "','BB')", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();
                        if (_GlobalFormateNo == 9003)
                        {
                            string st = dtJsonData.Rows[counter1]["StockType"].ToString();
                            if (st.Contains("CF"))
                            {
                                cmd = new SqlCommand("Insert Into BFCFCLOSING(Script_code,Script_name,Type,Stock_Type,Expiry_Date,Qty,Trans_Date,closing_rate,Format_No,Member_code,Year_code,Trans_No)"
                                              + "Values('" + sccd + "','" + dtJsonData.Rows[counter1]["Scripttext"].ToString() + "','" + dtJsonData.Rows[counter1]["Type"].ToString() + "','" + dtJsonData.Rows[counter1]["StockType"].ToString() + "','"
                                              + expdt + "','" + dtJsonData.Rows[counter1]["txt_Quantity"].ToString() + "','" + Trasndate + "','" + dtJsonData.Rows[counter1]["txt_GrossRate"].ToString() + "','" + _GlobalFormateNo + "','" + MemberCode + "','" + FinancialYearCode + "','" + vchno + "')", con)
                                {
                                    Transaction = transaction
                                };
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            #endregion




            return Json("1", JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult FnSaveManualEntryForCurrency(string JsonData, string ExpenseData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ExpenseData);

            string bktyp = "BB";
            int vchno = GetGlobalTransactionNumber(bktyp);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            cmd.CommandTimeout = 0;
            transaction = con.BeginTransaction("SampleTransaction");
            #region //save Enq seg
            try
            {
                if (1 == 1)
                {
                    //int vchno = 0;

                    //if (flag == 1)
                    //{
                    //    string bktyp = "BB";
                    //    vchno = GetGlobalTransactionNumber(bktyp);
                    //}
                    //else if (flag == 2)
                    //{
                    //    vchno = Convert.ToInt32(transnobb.Text.Trim());

                    //    cmd = new SqlCommand("delete from  BrokerBill where Trans_no='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };
                    //    cmd.ExecuteNonQuery();

                    //    cmd = new SqlCommand("delete from  BrokerBill_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };

                    //    cmd.ExecuteNonQuery();

                    //    cmd = new SqlCommand("delete from  AC_Trans where Trans_No='" + transnobb.Text.Trim() + "' and  Book_Type1='BB' and MemberID='" + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };

                    //    cmd.ExecuteNonQuery();
                    //}
                    string theDate1 = Convert.ToDateTime(dtJsonData.Rows[0]["txtDate"].ToString()).ToString("yyyy-MM-dd");

                    //cmd = new SqlCommand("SELECT Member_Code FROM Mst_Member where Member_Name='" + txtMember.Text + "'", con)
                    //{
                    //    Transaction = transaction
                    //};
                    //SqlDataReader dr = cmd.ExecuteReader();
                    //dr.Read();
                    //dr.Close();

                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where Ac_Name='" + txtBroker.Text + "' and (Member_Code='' or Member_Code='" + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};
                    //cmd = new SqlCommand("SELECT cast(AccountId as nvarchar) as AccountId FROM M_Account where  (MemberID='' or MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='' or FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};
                    //SqlDataReader dr2 = cmd.ExecuteReader();
                    //dr2.Read();
                    //string oc2 = dr2.GetString(0);
                    //dr2.Close();
                    string oc2 = dtJsonData.Rows[0]["ddlBroker"].ToString();
                    string narr0 = dtJsonData.Rows[0]["txt_BillNo"].ToString();
                    string narrr = dtJsonData.Rows[0]["txt_SettlementNo"].ToString();
                    double AmountBrought = 0;
                    double Amountsold = 0;
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    string drorcr = "";
                    double amtbfacI = 0.0;
                    double amtsfacI = 0.0;
                    txtBrokerage.Text = "0";
                    txtNetAmt.Text = "0";
                    txtGrossNetAmt.Text = "0";
                    for (int counter = 0; counter < (dtJsonData.Rows.Count); counter++)
                    {
                        string BrokerCode = dtJsonData.Rows[counter]["ddlBroker"].ToString();
                        txtBillNo.Text = dtJsonData.Rows[counter]["txt_BillNo"].ToString();
                        txtSettlementNo.Text = dtJsonData.Rows[counter]["txt_SettlementNo"].ToString();
                        txtBrokerage.Text = (Convert.ToDouble(txtBrokerage.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_BrokerageAmount"].ToString())).ToString();
                        txtNetAmt.Text = (Convert.ToDouble(txtNetAmt.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_NetAmount"].ToString())).ToString();
                        txtExpenses.Text = "0";
                        txtGrossNetAmt.Text = (Convert.ToDouble(txtGrossNetAmt.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_GrossAmount"].ToString())).ToString();
                        txtRemarks.Text = "";
                        txtInvestment.Text = "0";
                        txtInvestment.Text = "0";
                        invstyp = dtJsonData.Rows[counter]["ddlSegment"].ToString();
                        string styp = dtJsonData.Rows[counter]["HoldingTypetext"].ToString();

                        if (dtJsonData.Rows[counter]["Type"].ToString() == "Bought")
                            amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                        if (dtJsonData.Rows[counter]["Type"].ToString() == "Sold")
                            amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                        if (amtbfacI > amtsfacI)
                            AmountBrought = AmountBrought + (amtbfacI - amtsfacI);
                        else
                            Amountsold = Amountsold + (amtbfacI - amtsfacI);
                        amtsfacI = 0;
                        amtbfacI = 0;
                    }

                    double Amount = (AmountBrought - (Math.Abs(Amountsold)));
                    int trsrno = 1;
                    string drcd = "";
                    string crcd = "";
                    if (Amount > 0)
                        drorcr = "C";
                    else
                        drorcr = "D";
                    //if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    //    drorcr = "C";

                    cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,Amount,Stt_amt,"
                        + " ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('" + vchno + "','" + txtBillNo.Text + "','"
                        + theDate1 + "','" + txtSettlementNo.Text + "','" + MemberCode + "','" + oc2 + "','" + drorcr + "','0','"
                        + Math.Abs(Convert.ToDouble(txtBrokerage.Text)) + "','"
                        + Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text)) + "','0','0','" + Math.Abs(Convert.ToDouble(txtExpenses.Text)) + "','"
                        + Math.Abs(Convert.ToDouble(txtNetAmt.Text)) + "','BB','"
                        + FinancialYearCode + "','','N','" + invstyp + "','" + txtRemarks.Text + "');SELECT SCOPE_IDENTITY();", con)
                    {
                        Transaction = transaction
                    };
                    int bbins = cmd.ExecuteNonQuery();
                    if (Convert.ToDouble(Math.Round(Amount)) > 0)
                    {
                        drcd = currencyacc;
                        crcd = oc2;
                    }
                    else
                    {
                        drcd = oc2;
                        crcd = currencyacc;
                    }

                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                        + "[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType] ,[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                        + theDate1 + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                        + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        int nre = cmd.ExecuteNonQuery();

                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                        + "[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType] ,[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                        + theDate1 + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                        + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        int nre = cmd.ExecuteNonQuery();

                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }

                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {
                        if (Dt_Expanses.Rows[counter1]["AMOUNT"] != null)
                        {
                            double otrxpc = double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString());
                            if (otrxpc > 0)
                            {
                                //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where AC_name='" + Dt_Expanses.Rows[counter1]["expnm"].ToString() + "' and (Member_Code='' or Member_Code='" + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                                //{
                                //    Transaction = transaction
                                //};
                                //SqlDataReader dr4034 = cmd.ExecuteReader();
                                //dr4034.Read();
                                //string accttyypp5 = dr4034.GetString(0);
                                //dr4034.Close();
                                string selextype = "";

                                if (Dt_Expanses.Rows[counter1]["ADDLESS"].ToString() == "A")
                                    selextype = "A";
                                if (Dt_Expanses.Rows[counter1]["ADDLESS"].ToString() == "L")
                                    selextype = "L";

                                int nre2 = 0;
                                if (selextype == "A")
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg] ,[EntryType]) VALUES('" + vchno + "','"
                                        + trsrno + "','" + theDate1 + "','" + MemberCode + "','" + MemberCode + "','" + txtBillNo.Text + "','" + Dt_Expanses.Rows[counter1]["EXPID"].ToString() + "','"
                                        + oc2 + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"]
                                        .ToString()), decmlpl) + "','" + invstyp + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg] ,[EntryType]) VALUES('" + vchno + "','" + trsrno + "','"
                                        + theDate1 + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + oc2 + "','" + Dt_Expanses.Rows[counter1]["EXPID"].ToString() + "','BB','"
                                        + nartn1 + "','" + nartn2 + "','N','" + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString()), decmlpl) + "','"
                                        + invstyp + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }

                                if (nre2 > 0)
                                    trsrno = trsrno + 1;
                            }
                        }
                    }

                    for (int counter1 = 0; counter1 < (dtJsonData.Rows.Count); counter1++)
                    {
                        string sccd = dtJsonData.Rows[counter1]["ddlScript_Id"].ToString();
                        string ccd = dtJsonData.Rows[counter1]["ddlConsultant"].ToString();
                        double bkrch = double.Parse(dtJsonData.Rows[counter1]["txt_BrokerageAmount"].ToString()) / double.Parse(dtJsonData.Rows[counter1]["txt_Quantity"].ToString());
                        string expdt = "";
                        string expmonth = "";

                        if (dtJsonData.Rows[counter1]["ExpiryDate"] != null)
                            expdt = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        if (dtJsonData.Rows[counter1]["ExpiryDate"] != null)
                            expmonth = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("MMMM-yyyy");

                        string optyp = "";
                        if (dtJsonData.Rows[counter1]["OptionType"] != null)
                            optyp = dtJsonData.Rows[counter1]["OptionType"].ToString();
                        string sttyp = "";

                        if (dtJsonData.Rows[counter1]["StockType"] != null)
                            sttyp = dtJsonData.Rows[counter1]["StockType"].ToString();

                        var srnr = counter1 + 1;
                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(dtJsonData.Rows[counter1]["txtDate"].ToString()).ToString("yyyy-MM-dd");
                        }
                        cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No, Sr_No, Bill_no, Trans_Dt, TransType, Ac_Type, Script_ID, Qty, G_Rate, Brok_Amt,"
                            + " ServTax_Amount, Rate, Amount, MemberID, FinancialYearMemberID, Consultant_ID, EntrySource,Brok_PerUnit, MonthYear, Last_Date, LotSize, LotQty, "
                            + " isFNOBill, Strike_Price, OptionType, isstock,FrmNm)values('" + vchno + "', '" + srnr + "', '" + txtBillNo.Text + "', '" + Trasndate + "', '"
                            + dtJsonData.Rows[counter1]["Type"].ToString() + "', '', '" + sccd + "', '" + dtJsonData.Rows[counter1]["txt_Quantity"].ToString() + "', '"
                            + dtJsonData.Rows[counter1]["txt_GrossRate"].ToString() + "', '" + dtJsonData.Rows[counter1]["txt_BrokerageAmount"].ToString() + "', '0', '"
                            + dtJsonData.Rows[counter1]["txt_NetRate"].ToString() + "', '" + dtJsonData.Rows[counter1]["txt_NetAmount"].ToString() + "', '"
                            + MemberCode + "', '" + FinancialYearCode + "', '" + ccd + "', '0', '" + bkrch + "','" + expmonth + "','" + expdt + "','"
                            + dtJsonData.Rows[counter1]["Lott"].ToString() + "','" + dtJsonData.Rows[counter1]["LotQty"].ToString() + "','"
                            + invstyp + "','" + dtJsonData.Rows[counter1]["StrikePrice"].ToString() + "','" + optyp + "','" + sttyp + "','BB')", con)
                        {
                            Transaction = transaction
                        };
                        int brtrns = cmd.ExecuteNonQuery();
                    }
                    //  MessageBox.Show("Records Saved Successfully");
                    transaction.Commit();
                    // btnf.btnfunc(btnAdd, btnEdit, btnDelete, btnSave, btncanel, btnSearch, btnprint, btnclose2);

                    // Disable();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                //  MessageBox.Show(ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
            #endregion




            return Json("1", JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult FnSaveManualEntryForMCX(string JsonData, string ExpenseData)
        {
            DataTable dtJsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(ExpenseData);


            string bktyp = "BB";
            int vchno = GetGlobalTransactionNumber(bktyp);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            cmd.CommandTimeout = 0;
            transaction = con.BeginTransaction("SampleTransaction");
            #region //save Enq seg
            try
            {
                if (1 == 1)
                {
                    //int vchno = 0;
                    //if (flag == 1)
                    //{
                    //    string bktyp = "BB";
                    //    vchno = GetGlobalTransactionNumber(bktyp);
                    //}
                    //else if (flag == 2)
                    //{
                    //    vchno = Convert.ToInt32(transnobb.Text.Trim());
                    //    cmd = new SqlCommand("delete from  BrokerBill where Trans_no='" + transnobb.Text.Trim() + "' and MemberID='"
                    //        + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };
                    //    cmd.ExecuteNonQuery();

                    //    cmd = new SqlCommand("delete from  BrokerBill_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='"
                    //        + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };
                    //    cmd.ExecuteNonQuery();

                    //    cmd = new SqlCommand("delete from  AC_Trans where Trans_No='" + transnobb.Text.Trim() + "' and  Book_Type1='BB' and MemberID='"
                    //        + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                    //    {
                    //        Transaction = transaction
                    //    };
                    //    cmd.ExecuteNonQuery();
                    //}
                    string theDate1 = Convert.ToDateTime(dtJsonData.Rows[0]["txtDate"].ToString()).ToString("yyyy-MM-dd");

                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where Ac_Name='" + txtBroker.Text + "' and (Member_Code='"
                    //    + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};
                    //cmd = new SqlCommand("SELECT AccountId FROM M_Account where  (MemberID='"
                    // + MenuMaster.member_code + "') and (FinancialYearMemberID='' or FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};
                    //SqlDataReader dr2 = cmd.ExecuteReader();
                    //dr2.Read();
                    //string oc2 = dr2[0].ToString();
                    //dr2.Close();
                    string oc2 = dtJsonData.Rows[0]["ddlBroker"].ToString();
                    string narr0 = dtJsonData.Rows[0]["txt_BillNo"].ToString();
                    string narrr = dtJsonData.Rows[0]["txt_SettlementNo"].ToString();
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    double AmountBrought = 0;
                    double Amountsold = 0;
                    string drorcr = "";
                    double amtbfacI = 0.0;
                    double amtsfacI = 0.0;
                    txtBrokerage.Text = "0";
                    txtNetAmt.Text = "0";
                    txtGrossNetAmt.Text = "0";
                    for (int counter = 0; counter < (dtJsonData.Rows.Count); counter++)
                    {
                        string BrokerCode = dtJsonData.Rows[counter]["ddlBroker"].ToString();
                        txtBillNo.Text = dtJsonData.Rows[counter]["txt_BillNo"].ToString();
                        txtSettlementNo.Text = dtJsonData.Rows[counter]["txt_SettlementNo"].ToString();
                        txtBrokerage.Text = (Convert.ToDouble(txtBrokerage.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_BrokerageAmount"].ToString())).ToString();
                        txtNetAmt.Text = (Convert.ToDouble(txtNetAmt.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_NetAmount"].ToString())).ToString();
                        txtExpenses.Text = "0";
                        txtGrossNetAmt.Text = (Convert.ToDouble(txtGrossNetAmt.Text) + Convert.ToDouble(dtJsonData.Rows[counter]["txt_GrossAmount"].ToString())).ToString();
                        txtRemarks.Text = "";
                        txtInvestment.Text = "0";
                        txtInvestment.Text = "0";
                        invstyp = dtJsonData.Rows[counter]["ddlSegment"].ToString();
                        string styp = dtJsonData.Rows[counter]["HoldingTypetext"].ToString();
                        if (dtJsonData.Rows[counter]["Type"].ToString() == "Bought")
                            amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                        if (dtJsonData.Rows[counter]["Type"].ToString() == "Sold")
                            amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(dtJsonData.Rows[counter]["txt_NetAmount"].ToString()), decmlpl), decmlpl);
                        if (amtbfacI > amtsfacI)
                            AmountBrought = AmountBrought + (amtbfacI - amtsfacI);
                        else
                            Amountsold = Amountsold + (amtbfacI - amtsfacI);
                        amtsfacI = 0;
                        amtbfacI = 0;
                    }

                    double Amount = (AmountBrought - (Math.Abs(Amountsold)));
                    int trsrno = 1;
                    string drcd = "";
                    string crcd = "";

                    if (Amount > 0)
                        drorcr = "C";
                    else
                        drorcr = "D";
                    //if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    //    drorcr = "C";

                    cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,"
                        + " Amount,Stt_amt,ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('"
                        + vchno + "','" + txtBillNo.Text + "','" + theDate1 + "','" + txtSettlementNo.Text + "','" + MemberCode + "','" + oc2 + "','"
                        + drorcr + "','0','" + (Math.Abs(Convert.ToDouble(txtBrokerage.Text))) + "','" + Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text)) + "','0','0','"
                        + Math.Abs(Convert.ToDouble(txtExpenses.Text)) + "','" + Math.Abs(Convert.ToDouble(txtNetAmt.Text)) + "','BB','"
                        + FinancialYearCode + "','','N','" + invstyp + "','" + txtRemarks.Text + "');SELECT SCOPE_IDENTITY();", con)
                    {
                        Transaction = transaction
                    };
                    cmd.ExecuteNonQuery();

                    if (Convert.ToDouble(Math.Round(Amount)) > 0)
                    {
                        drcd = commdityacc;
                        crcd = oc2;
                    }
                    else
                    {
                        drcd = oc2;
                        crcd = commdityacc;
                    }
                    //* Expense Amount is Higher then Gross Amount*//
                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType] ,[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                            + theDate1 + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                            + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        int nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }
                    //* Expense Amount is Higher then Gross Amount END*//
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType] ,[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                            + theDate1 + "','" + MemberCode + "','" + FinancialYearCode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                            + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        int nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }


                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {
                        if (Dt_Expanses.Rows[counter1]["AMOUNT"].ToString() != null && Dt_Expanses.Rows[counter1]["AMOUNT"].ToString() != "")
                        {
                            double otrxpc = double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString());
                            if (otrxpc > 0)
                            {

                                string selextype = "";

                                if (Dt_Expanses.Rows[counter1]["ADDLESS"].ToString() == "A")
                                    selextype = "A";
                                if (Dt_Expanses.Rows[counter1]["ADDLESS"].ToString() == "L")
                                    selextype = "L";

                                int nre2 = 0;
                                if (selextype == "A") // If Expense Type Add
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] "
                                        + ",[Cr_AccountId] ,[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],[EntryType] )"
                                        + " VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MemberCode + "','" + FinancialYearCode
                                        + "','" + txtBillNo.Text + "','" + Dt_Expanses.Rows[counter1]["EXPID"].ToString() + "','" + oc2 + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString()), decmlpl) + "','" + invstyp
                                        + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                else      //if Expesne Type Less
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] "
                                        + " ,[Cr_AccountId] ,[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],[EntryType] ) "
                                        + " VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MemberCode + "','" + FinancialYearCode
                                        + "','" + txtBillNo.Text + "','" + oc2 + "','" + Dt_Expanses.Rows[counter1]["EXPID"].ToString() + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["AMOUNT"].ToString()), decmlpl) + "','"
                                        + invstyp + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                if (nre2 > 0)
                                    trsrno = trsrno + 1;
                            }
                        }
                    }

                    for (int counter1 = 0; counter1 < (dtJsonData.Rows.Count); counter1++)
                    {
                        string sccd = dtJsonData.Rows[counter1]["ddlScript_Id"].ToString();
                        string ccd = dtJsonData.Rows[counter1]["ddlConsultant"].ToString();
                        double bkrch = double.Parse(dtJsonData.Rows[counter1]["txt_BrokerageAmount"].ToString()) / double.Parse(dtJsonData.Rows[counter1]["txt_Quantity"].ToString());
                        string expdt = "";
                        string expmonth = "";

                        if (dtJsonData.Rows[counter1]["ExpiryDate"] != null)
                            expdt = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        if (dtJsonData.Rows[counter1]["ExpiryDate"] != null)
                            expmonth = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("MMMM-yyyy");

                        string optyp = "";
                        if (dtJsonData.Rows[counter1]["OptionType"] != null)
                            optyp = dtJsonData.Rows[counter1]["OptionType"].ToString();

                        string sttyp = "";
                        if (dtJsonData.Rows[counter1]["StockType"] != null)
                            sttyp = dtJsonData.Rows[counter1]["StockType"].ToString();

                        var srnr = counter1 + 1;
                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(dtJsonData.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(dtJsonData.Rows[counter1]["txtDate"].ToString()).ToString("yyyy-MM-dd");
                        }

                        cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No, Sr_No, Bill_no, Trans_Dt, TransType, Ac_Type, Script_ID, Qty,"
                            + " G_Rate, Brok_Amt, ServTax_Amount, Rate, Amount, MemberID, FinancialYearMemberID, Consultant_ID, EntrySource,Brok_PerUnit,"
                            + " MonthYear, Last_Date, LotSize, LotQty, isFNOBill, Strike_Price, OptionType, isstock,FrmNm)values('" + vchno + "', '"
                            + srnr + "', '" + txtBillNo.Text + "', '" + Trasndate + "', '" + dtJsonData.Rows[counter1]["Type"].ToString()
                            + "', '', '" + sccd + "', '" + dtJsonData.Rows[counter1]["txt_Quantity"].ToString() + "', '"
                            + dtJsonData.Rows[counter1]["txt_GrossRate"].ToString() + "', '" + dtJsonData.Rows[counter1]["txt_BrokerageAmount"].ToString()
                            + "', '0', '" + dtJsonData.Rows[counter1]["txt_NetRate"].ToString() + "', '"
                            + dtJsonData.Rows[counter1]["txt_NetAmount"].ToString() + "', '" + MemberCode + "', '" + FinancialYearCode
                            + "', '" + ccd + "', '0', '" + bkrch + "','" + expmonth + "','" + expdt + "','" + dtJsonData.Rows[counter1]["Lott"].ToString()
                            + "','" + dtJsonData.Rows[counter1]["LotQty"].ToString() + "','" + invstyp + "','"
                            + dtJsonData.Rows[counter1]["StrikePrice"].ToString() + "','" + optyp + "','" + sttyp + "','BB')", con)
                        {
                            Transaction = transaction
                        };
                        int brtrns = cmd.ExecuteNonQuery();
                    }
                    //  MessageBox.Show("Records Saved Successfully");
                    transaction.Commit();
                    // btnf.btnfunc(btnAdd, btnEdit, btnDelete, btnSave, btncanel, btnSearch, btnprint, btnclose2);

                    //   Disable();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                transaction.Rollback();

                // MessageBox.Show("  Message: {0}", ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
            #endregion




            return Json("1", JsonRequestBehavior.AllowGet);

        }

        public DataTable GetDBhead(string Trans_no)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //int ID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = GetSubscriberID(ID);
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_VIEW_CN_NOTE]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_HEAD";
            cmd.Parameters.Add("@Trans_no", SqlDbType.BigInt).Value = Trans_no;

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
        public DataTable GetDBEquity_Details(string Trans_no,string action)
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //int ID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = GetSubscriberID(ID);
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_VIEW_CN_NOTE]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = action;
            cmd.Parameters.Add("@Trans_no", SqlDbType.BigInt).Value = Trans_no;

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
        public DataTable GetDBEquity_Expense(string Trans_no,string action )
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //int ID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = GetSubscriberID(ID);
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_VIEW_CN_NOTE]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = action;
            cmd.Parameters.Add("@Trans_no", SqlDbType.BigInt).Value = Trans_no;

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
        public ActionResult ViewCnNote(string TrnsNo)
        {
            #region get header data from database
            Session["SaveBREntry"] = null;
            Session["Dt_Header"] = null;
            Session["Dt_Expanses"] = null;
            DataTable dtheaderdb = GetDBhead(TrnsNo);
            IRecordweb.ViewModel.BREntryModel data = new BREntryModel();
            data.Broker_Name = dtheaderdb.Rows[0]["BrokerName"].ToString();
            data.Demat_Ac_Name = dtheaderdb.Rows[0]["DematName"].ToString();
            data.ConsultantCode = dtheaderdb.Rows[0]["ConsultantCode"].ToString();
            data.Demat_Ac_Id = dtheaderdb.Rows[0]["Demat_Ac_Id"].ToString();
            data.HoldingTypeCode = dtheaderdb.Rows[0]["HoldingTypeCode"].ToString();
            data.invstyp = dtheaderdb.Rows[0]["invstyp"].ToString();
            Session["SaveBREntry"] = data;
            Session["Dt_Header"] = dtheaderdb;
          

            #endregion
            
            #region ///Deatils
            string action = "";
            string expaction = "";
            if (data.invstyp=="0")
            {
                expaction = "VIEW_Equity_Expense";
                   action = "VIEW_Equity_deatils";
            }
            if (data.invstyp == "1")
            {
                expaction = "VIEW_MCX_Expense";
                action = "VIEW_MCX_deatils";
            }
            if (data.invstyp == "3")
            {
                expaction = "VIEW_MCX_Expense";
                action = "VIEW_MCX_deatils";
            }
            if (data.invstyp == "4")
            {
                expaction = "VIEW_MCX_Expense";
                action = "VIEW_MCX_deatils";
            }
            Session["Dt_Expanses"] = GetDBEquity_Expense(TrnsNo, expaction); 
             DataTable Dt_Details = GetDBEquity_Details(TrnsNo, action);
            Session["Dt_Details"] = Dt_Details;
            #endregion
            return View("BRDetails");
        }
        public JsonResult GetAllExpense()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_M_EXPENSE_MAPPING]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWMEXPENSES";
            cmd.Parameters.Add("@MEMBERID", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@FINANCIALYEARID", SqlDbType.Int).Value = FinancialYearCode;

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
        public ActionResult BRDetailsUpdate(string JsonData,string JsonHead,string JsonExpense)
        {
            DataTable dtdata_head = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>("["+JsonHead+"]");
            DataTable dtdata_details = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dtdata_Expse = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonExpense);
            if(dtdata_head.Rows[0]["invstyp"].ToString()=="0")
            {
                DbFnUpdateDeatils(dtdata_details, dtdata_head);
            }
            else
            {
                DbFnUpdateDetailsForFno(dtdata_details);
            }

            
            DbFnUpdateHead(dtdata_details, dtdata_head);

            DbFnUpdateExpense(dtdata_Expse);

            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public void DbFnUpdateDetailsForFno(DataTable dtdetails)
        {
            foreach(DataRow dr in dtdetails.Rows)
            {
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[SP_UPDATE_FNO_DETAILS_CN_NOTES]";
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATE";
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = dr["ID"].ToString();
               
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = dr["ExpiryDate"].ToString();
                cmd.Parameters.Add("@TransType", SqlDbType.VarChar).Value = dr["Type"].ToString();
                cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Script_ID", SqlDbType.VarChar).Value = dr["ScriptCode"].ToString();
                string expmonth = Convert.ToDateTime(dr["ExpiryDate"].ToString()).ToString("MMMM-yyyy");
                cmd.Parameters.Add("@MonthYear", SqlDbType.VarChar).Value = expmonth;
                cmd.Parameters.Add("@Last_Date", SqlDbType.DateTime).Value = Convert.ToDateTime(dr["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@LotSize", SqlDbType.Float).Value = dr["Lott"].ToString();
                cmd.Parameters.Add("@LotQty", SqlDbType.Float).Value =dr["LotQty"].ToString();
                cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = dr["Qty"].ToString();

                cmd.Parameters.Add("@G_Rate", SqlDbType.Float).Value = dr["GrossRate"].ToString();
                cmd.Parameters.Add("@Brok_Amt", SqlDbType.Float).Value = dr["BrokAmt"].ToString();
                cmd.Parameters.Add("@ServTax_Amount", SqlDbType.Float).Value = "0";
                cmd.Parameters.Add("@Rate", SqlDbType.Float).Value = dr["Rate"].ToString();
                cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = dr["Amount"].ToString();
           
  
                cmd.Parameters.Add("@Consultant_ID", SqlDbType.VarChar).Value = dr["ConsultantCode"].ToString(); ;
                cmd.Parameters.Add("@EntrySource", SqlDbType.Int).Value = "0";

                cmd.Parameters.Add("@Strike_Price", SqlDbType.Float).Value = dr["StrikePrice"].ToString();
                cmd.Parameters.Add("@OptionType", SqlDbType.VarChar).Value =dr["OptionType"].ToString(); ;

                double bkrch = Math.Round(double.Parse(dr["BrokAmt"].ToString()) / double.Parse(dr["Qty"].ToString()), decmlpl);
                cmd.Parameters.Add("@Brok_PerUnit", SqlDbType.Float).Value = bkrch;

                cmd.Parameters.Add("@isstock", SqlDbType.VarChar).Value =dr["StockType"].ToString(); ;
             
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
        }

        public void DbFnUpdateDeatils(DataTable Dt_Details,DataTable dtHead)
        {
           
            for (int counter1 = 0; counter1 < Dt_Details.Rows.Count; counter1++)
            {
                String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_UPDATE_EQUITY_DETAILS_CN_NOTES]";
                cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "UPDATE_DETAILS";
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = Dt_Details.Rows[counter1]["ID"].ToString();
              
                cmd.Parameters.Add("@Bill_no", SqlDbType.NVarChar).Value = dtHead.Rows[0]["BillNo"].ToString();//
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.Date).Value = Convert.ToDateTime(dtHead.Rows[0]["Date"].ToString()).ToString("yyyy/MM/dd");//
                cmd.Parameters.Add("@TransType", SqlDbType.NVarChar).Value = Dt_Details.Rows[counter1]["Type"].ToString();
                
                cmd.Parameters.Add("@Script_ID", SqlDbType.NVarChar).Value = Dt_Details.Rows[counter1]["ScriptCode"].ToString();
                cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = Dt_Details.Rows[counter1]["Qty"].ToString();
                cmd.Parameters.Add("@Consultant_ID", SqlDbType.NVarChar).Value = Dt_Details.Rows[counter1]["ConsultantCode"].ToString();
                cmd.Parameters.Add("@Bal_qty", SqlDbType.Float).Value = Dt_Details.Rows[counter1]["Qty"].ToString();
                cmd.Parameters.Add("@G_Rate", SqlDbType.Float).Value = Dt_Details.Rows[counter1]["GrossRate"].ToString();
                cmd.Parameters.Add("@Brok_Amt", SqlDbType.Float).Value = Dt_Details.Rows[counter1]["BrokAmt"].ToString();
                cmd.Parameters.Add("@ServTax_Amount", SqlDbType.Float).Value =0;
                cmd.Parameters.Add("@Rate", SqlDbType.Float).Value = Dt_Details.Rows[counter1]["Rate"].ToString();
                cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = Dt_Details.Rows[counter1]["Amount"].ToString();
                cmd.Parameters.Add("@isIntraDay", SqlDbType.Int).Value = (bool.Parse(Dt_Details.Rows[counter1]["IntraDay"].ToString())==true?1:0);
                double bkrch = Math.Round(Math.Round(double.Parse(Dt_Details.Rows[counter1]["BrokAmt"].ToString()), 4) / Math.Round(double.Parse(Dt_Details.Rows[counter1]["Qty"].ToString()), 4), 4);
                cmd.Parameters.Add("@Brok_PerUnit", SqlDbType.Float).Value = bkrch;
                cmd.Parameters.Add("@C_Gross_Rate", SqlDbType.Float).Value = Dt_Details.Rows[counter1]["GrossRate"].ToString();
                cmd.Parameters.Add("@Exp_Rate", SqlDbType.NVarChar).Value = Dt_Details.Rows[counter1]["Rate"].ToString();
         


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
            }

        }
        public void DbFnUpdateExpense(DataTable Dt_Expese)
        {

            for (int counter1 = 0; counter1 < Dt_Expese.Rows.Count; counter1++)
            {
                String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[DBO].[SP_UPDATE_EQUITY_DETAILS_CN_NOTES]";
                cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "UPDATE_Expense";
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = Dt_Expese.Rows[counter1]["ID"].ToString();

                cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = Dt_Expese.Rows[counter1]["ExpAmount"].ToString();//
            



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
            }

        }
        public void DbFnUpdateHead(DataTable Dt_Details, DataTable dtHead)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_UPDATE_EQUITY_DETAILS_CN_NOTES]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "UPDATE_HEADER";
            cmd.Parameters.Add("@Bill_no", SqlDbType.NVarChar).Value = dtHead.Rows[0]["BillNo"].ToString();//
            cmd.Parameters.Add("@Trans_Dt", SqlDbType.Date).Value = Convert.ToDateTime(dtHead.Rows[0]["Date"].ToString()).ToString("yyyy/MM/dd");//
            cmd.Parameters.Add("@Trans_no", SqlDbType.NVarChar).Value = Dt_Details.Rows[0]["ID"].ToString();//
            cmd.Parameters.Add("@Valan_code", SqlDbType.NVarChar).Value = dtHead.Rows[0]["SettlementNo"].ToString();//




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
        }

        [HttpPost]
        public void Upload()
        {
            string path = Server.MapPath("~/UploadedFiles/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string key in Request.Files)
            {
                //HttpPostedFileBase postedFile = Request.Files[key];
                //string fileurl = path + Guid.NewGuid()+postedFile.FileName;
                //postedFile.SaveAs(path + postedFile.FileName);
                //Session["fileurl"] = fileurl;
                HttpPostedFileBase FilePath = Request.Files[key];
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(FilePath.InputStream))
                {
                    bytes = br.ReadBytes(FilePath.ContentLength);
                }
                string _FileName = Path.GetFileName(FilePath.FileName);
                string fileextenion = Path.GetExtension(FilePath.FileName);
                string ContentType = FilePath.ContentType.ToString();
                Session["FilesString"] = Convert.ToBase64String(bytes);
                Session["FileName"] = _FileName;
                Session["Fileextenion"] = fileextenion;
                Session["ContentType"] = ContentType;
            }

            //return Content("Success");
        }
        [HttpGet]
        public ActionResult SaveBREntry()
        {

            //Session["member_code"] = "M0001";
            //Session["year_code"] = "1120212022";
            return View();
        }

        [HttpGet]
        public ActionResult BRDetails()
        {
            IRestResponse response = null;
            try
            {
                Session["Dt_Details"] = null;
                 Session["Dt_Expanses"] = null;
                Session["Dt_RunningBalance"] = null;
                DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
                var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
                var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
                var CreatedById = Session["UserID"].ToString();


                IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
                FilesTans dataobj = new FilesTans();
                dataobj.contentType = data.ContentType;
                dataobj.fileextension = data.Fileextenion;
                dataobj.fileName = data.FileName;
                dataobj.pdfcode = data.ContractNoteId;
                dataobj.pdfbytes = data.FilesString;
                dataobj.member_code = MemberCode;
                dataobj.year_code = FinancialYearCode;
                dataobj.Brokercode = data.Broker_Id;
                dataobj.invstyp = data.invstyp;
                dataobj.invstyptext = data.invstyptext;
                dataobj.ConsultantCode = data.ConsultantCode;
                dataobj.Consultant = data.Consultant;
                dataobj.HoldingTypeCode = data.HoldingTypeCode;
                dataobj.HoldingType = data.HoldingType;
                dataobj.Password = data.Password;
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var client = new RestClient(PdfConfig + "api/PdfProcess/pdfconfig");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
                request.AddHeader("Content-Type", "application/json");
                var body = Newtonsoft.Json.JsonConvert.SerializeObject(dataobj);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                 response = client.Execute(request);
                //Console.WriteLine(response.Content);
                DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(response.Content);
                Session["Dt_Header"] = myDataSet.Tables[0];

                DataTable dtdeatils = myDataSet.Tables[1];
                dtdeatils.Columns.Add("HoldingTypeId", typeof(string));
                //dtdeatils.Columns["ConsultantCode"].DataType = typeof(Int32);
                //dtdeatils.AcceptChanges();
                DataTable dtCloned = dtdeatils.Clone();
                dtCloned.Columns["ConsultantCode"].DataType = typeof(Int32);
                foreach (DataRow row in dtdeatils.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                dtdeatils = dtCloned;
                foreach (DataRow dr in dtdeatils.Rows)
                {

                    dr["IntraDay"] = dr["IntraDay"].ToString().ToLower();
                    dr["HoldingTypeId"] = data.HoldingTypeCode;
                    dr["ConsultantCode"] = data.ConsultantCode;
                    try
                    {
                        dr["DematCode1"] = data.Demat_Ac_Id;
                    }
                    catch
                    {

                    }
                    
                }


                Session["Dt_Details"] = dtdeatils;
                Session["Dt_Expanses"] = myDataSet.Tables[2];
                Session["Dt_RunningBalance"] = myDataSet.Tables[3];
                if (data.invstyp == "0")//SET INTRA DAY LOGIC
                {
                    Session["Dt_Eq_Details"] = dtdeatils;
                    SaveEquity_Save_For_Equity();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
                //ex.Data.Add("UserMessage", response.Content);
                //throw;
            }
            return View();
        }

        public string GetTrnsNo(string billno, string bill_date, string createby)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BrokerBill_Generate_TRN_Process]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";

            cmd.Parameters.Add("@billno", SqlDbType.NVarChar).Value = billno;

            cmd.Parameters.Add("@bill_date", SqlDbType.DateTime).Value = bill_date;
            cmd.Parameters.Add("@createby", SqlDbType.Int).Value = createby;
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {

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
            return DT.Rows[0]["TRN_NUMBER"].ToString();
        }
        public void SaveEquity_Save_For_Equity()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            FilesTans dataobj = new FilesTans();
            clsbbm MenuMaster = new clsbbm();
            MenuMaster.member_code = MemberCode.ToString();
            MenuMaster.yearcode = FinancialYearCode.ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Session["Dt_Expanses"] as DataTable;
            DataTable Dt_RunningBalance = Session["Dt_RunningBalance"] as DataTable;
            txtExpenses.Text = Dt_RunningBalance.Rows[0]["txtExpenses"].ToString();
            txtGrossNetAmt.Text = Dt_RunningBalance.Rows[0]["txtGrossNetAmt"].ToString();
            txtBillNo.Text = Dt_Header.Rows[0]["BillNo"].ToString();
            txtSettlementNo.Text = Dt_Header.Rows[0]["SettlementNo"].ToString();
            txtBrokerage.Text = Dt_RunningBalance.Rows[0]["txtBrokerage"].ToString();
            txtNetAmt.Text = Dt_RunningBalance.Rows[0]["txtNetAmt"].ToString();
            txtInvestment.Text = Dt_RunningBalance.Rows[0]["txtInvestment"].ToString();
            textBox5.Text = Dt_RunningBalance.Rows[0]["textBox5"].ToString();
            txtRemarks.Text = "";
            flag = 1;
            transnobb.Text = "95";
            txtBroker.Text = data.Broker_Name;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            cmd.CommandTimeout = 0;
            string vchno = "0";
            transaction = con.BeginTransaction("SampleTransaction");
            try
            {
                //if (Validate_FNO())
                //{

                if (flag == 1)
                {
                    string bktyp = "BB";
                    vchno = GetTrnsNo(txtBillNo.Text, Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd"), "1");

                }
                else if (flag == 2)
                {

                }

                int counter = 0;
                string BrokerCode = "";


                string LedgerDate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                string drorcr = "";
                double amtbfacI = 0;
                double amtbfacT = 0;
                double amtsfacI = 0;
                double amtsfacT = 0;
                double _totalINV_Shareval = 0;
                double _totalINV_StockTradeval = 0;
                int nre = 0;

                for (counter = 0; counter < Dt_Details.Rows.Count; counter++)
                {
                    string styp = Dt_Details.Rows[counter]["HoldingType"].ToString();
                    string[] detailss = styp.Split('-');
                    if (Dt_Details.Rows[counter]["Type"].ToString() == "Bought" & detailss[0] == "I")
                        amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                    if (Dt_Details.Rows[counter]["Type"].ToString() == "Bought" & detailss[0] == "T")
                        amtbfacT = Math.Round(amtbfacT + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                    if (Dt_Details.Rows[counter]["Type"].ToString() == "Sold" & detailss[0] == "I")
                        amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                    if (Dt_Details.Rows[counter]["Type"].ToString() == "Sold" & detailss[0] == "T")
                        amtsfacT = Math.Round(amtsfacT + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                    if (amtbfacI > amtsfacI)
                        _totalINV_Shareval = (amtbfacI - amtsfacI);
                    else
                        _totalINV_Shareval = (amtbfacI - amtsfacI);

                    #region stock in trade  
                    if (amtbfacT > amtsfacT)
                        _totalINV_StockTradeval = (amtbfacT - amtsfacT);
                    else
                        _totalINV_StockTradeval = (amtbfacT - amtsfacT);
                    #endregion
                }
                double TotalAmount = Convert.ToDouble(_totalINV_Shareval - _totalINV_StockTradeval * -1);

                if (TotalAmount > 0)
                    drorcr = "C";
                else
                    drorcr = "D";

                if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    drorcr = "C";

                if (Convert.ToInt32(vchno) > 0)
                {


                    string narr0 = txtBillNo.Text.ToString();
                    string narrr = txtSettlementNo.Text.ToString();
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    string drcd = "";
                    string crcd = "";
                    int trsrno = 1;

                    if (!checkBox3.Checked)
                    {
                        #region vasant share Investment                     
                        if (Convert.ToDouble((_totalINV_Shareval)) > 0)
                        {
                            drcd = share_Inv_Eqty_AC;
                            crcd = BrokerCode;
                        }
                        else
                        {
                            drcd = BrokerCode;
                            crcd = share_Inv_Eqty_AC;
                        }
                        // If Gross Amont Less Then Expense Amount Share Investment
                        if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtInvestment.Text))
                        {

                        }
                        //// If Expense Amont Less Then Gross Amount Share Investment END
                        else
                        {

                        }
                        #endregion

                        #region vasant Stock In Trade
                        if (Convert.ToDouble((_totalINV_StockTradeval)) > 0)
                        {
                            drcd = shrtradeinveqty;
                            crcd = BrokerCode;
                        }
                        else
                        {
                            drcd = BrokerCode;
                            crcd = shrtradeinveqty;
                        }

                        // if  Gross Amont Stock In Trade Entry Less Then Expense Amount

                        if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(textBox5.Text))
                        {

                        }
                        // if Stock In Trade Entry Stock Amount Less Then Expense Amount END
                        else
                        {

                        }
                        #endregion
                    }
                    else // SLBM ENTRY
                    {

                    }
                    for (int counter1 = 0; counter1 < (Dt_Details.Rows.Count); counter1++)
                    {
                        string actyp = "";
                        string styp = "";
                        if (!checkBox3.Checked)
                        {
                            styp = Dt_Details.Rows[counter1]["HoldingType"].ToString();
                            string[] details = styp.Split('-');
                            if (details[0] == "I")
                                actyp = share_Inv_Eqty_AC;
                            else if (details[0] == "T")
                                actyp = shrtradeinveqty;
                        }
                        else // SLBM ENTRY Holding Type
                        {
                            actyp = "1100034";
                        }

                        int intrday = 0;
                        try
                        {
                            if (Dt_Details.Rows[counter1]["IntraDay"] != null
                                && bool.Parse(Dt_Details.Rows[counter1]["IntraDay"].ToString()))
                                intrday = 1;
                        }
                        catch { }
                        string sccd = "";
                        if (Dt_Details.Rows[counter1]["ScriptCode"] != null)
                            sccd = Dt_Details.Rows[counter1]["ScriptCode"].ToString().Trim();
                        string ccd = "";

                        if (Dt_Details.Rows[counter1]["ConsultantCode"] != null)
                            ccd = Dt_Details.Rows[counter1]["ConsultantCode"].ToString().Trim();

                        string dmcd = "";
                        if (Dt_Details.Rows[counter1]["DematCode1"] != null)
                            dmcd = Dt_Details.Rows[counter1]["DematCode1"].ToString().Trim();

                        string trnstypnw = "";
                        if (Dt_Details.Rows[counter1]["Type"].ToString() == "Bought")
                            trnstypnw = "I";
                        else
                            trnstypnw = "O";

                        var srnr = counter1 + 1;
                        if (intrday == 0)
                        {

                        }

                        int SLBS = checkBox3.Checked ? 1 : 0;
                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                        }

                        double bkrch = Math.Round(Math.Round(double.Parse(Dt_Details.Rows[counter1]["BrokAmt"].ToString()), 4) / Math.Round(double.Parse(Dt_Details.Rows[counter1]["Qty"].ToString()), 4), 4);
                        cmd = new SqlCommand("Insert into BrokerBill_Trans_Process(ScriptName,Trans_No,Sr_No,Bill_no,Trans_Dt,TransType,Ac_Type,Script_Code,Qty,Bal_qty,G_Rate,Brok_Amt,ServTax_Amount,"
                            + " Rate,Amount,Member_code,YearCode,Consultant_Code,EntrySource,isIntraDay,Brok_PerUnit,isFNOBill,C_Gross_Rate,Exp_Rate,FrmNm,IsSLBS)values('" + Dt_Details.Rows[counter1]["ScriptName"].ToString() + "','" + vchno + "','"
                            + srnr + "','" + txtBillNo.Text + "','" + Trasndate + "','" + Dt_Details.Rows[counter1]["Type"].ToString() + "','" + actyp + "','" + sccd + "','"
                            + Dt_Details.Rows[counter1]["Qty"].ToString() + "','" + Dt_Details.Rows[counter1]["Qty"].ToString() + "','"
                            + Dt_Details.Rows[counter1]["GrossRate"].ToString() + "','" + Dt_Details.Rows[counter1]["BrokAmt"].ToString() + "','0','"
                            + Dt_Details.Rows[counter1]["Rate"].ToString() + "','" + Dt_Details.Rows[counter1]["Amount"].ToString() + "','"
                            + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + ccd + "','0','" + intrday + "','" + bkrch + "','" + invstyp + "','"
                            + Dt_Details.Rows[counter1]["GrossRate"].ToString() + "','" + Dt_Details.Rows[counter1]["Rate"] + "','BB','"
                            + SLBS + "')", con);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }
                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {

                    }
                    transaction.Commit();
                    con.Close();

                    //MessageBox.Show("Record Saved Successfully !!");
                    //btnSave.Enabled = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // MessageBox.Show(ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
            SetIntraDays(vchno);
        }
        public void SetIntraDays(string vchno)
        {



            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_TRANS_NO_SPLIT]";
            cmd.Parameters.Add("@Trans_no", SqlDbType.Int).Value = vchno;

            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {

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
            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            DataTable dtold = Session["Dt_Eq_Details"] as DataTable;
            foreach (DataRow dr in DT.Rows)
            {
                //dr["IntraDay"] = Convert.ToBoolean(dr["IntraDay"].ToString());
                try
                {
                    var idata = dtold.Select("ScriptCode='" + dr["ScriptCode"] + "'");
                    dr["ScriptName"] = idata[0]["ScriptName"].ToString();



                }
                catch { }
                dr["HoldingType"] = data.HoldingTypeCode;
                try
                {
                    dr["DematCode1"] = data.Demat_Ac_Id;
                }
                catch
                {

                }
                dr["ConsultantCode"] = data.ConsultantCode;
            }
            Session["Dt_Details"] = DT;

        }

        public JsonResult Get_Header()
        {
            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            DataTable dt = Session["Dt_Header"] as DataTable;
            try
            {//try to convert date fromate
                dt.Rows[0]["Date"] = Convert.ToDateTime(dt.Rows[0]["Date"].ToString()).ToString("dd/MMM/yyyy");
                dt.AcceptChanges();
            }
            catch { }


            if (!dt.Columns.Contains("MemberName"))
            {
                try
                {
                    dt.Columns.Add("MemberName", typeof(string));

                    dt.Columns.Add("BrokerName", typeof(string));
                    dt.Columns.Add("DematName", typeof(string));
                    dt.Columns.Add("ConsultantCode", typeof(string));
                    //dt.Columns.Add("Date", typeof(string));
                    dt.Columns.Add("Demat_Ac_Id", typeof(string));
                    dt.Columns.Add("HoldingTypeCode", typeof(string));
                    dt.Columns.Add("invstyp", typeof(string));
                    dt.Columns.Add("txt_invstyp", typeof(string));
                }
                catch { }
            }


            dt.Rows[0]["MemberName"] = Session["UserName"].ToString();
            dt.Rows[0]["BrokerName"] = (Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel).Broker_Name.ToString();
            try
            {
                dt.Rows[0]["DematName"] = (Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel).Demat_Ac_Name.ToString();
            }
            catch
            {
                dt.Rows[0]["DematName"] = "";
            }
            
            //
            dt.Rows[0]["ConsultantCode"] = (Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel).ConsultantCode.ToString();
            //dt.Rows[0]["Date"] = (Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel).Date.ToString();
            try
            {
                dt.Rows[0]["Demat_Ac_Id"] = (Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel).Demat_Ac_Id.ToString();
            }
            catch { }
            try
            {
                dt.Rows[0]["HoldingTypeCode"] = (Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel).HoldingTypeCode.ToString();
            }
            catch
            {

            }
            
            dt.Rows[0]["invstyp"] = (Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel).invstyp.ToString();
            if (dt.Rows[0]["invstyp"].ToString() == "0")
                dt.Rows[0]["txt_invstyp"] = "Equity";
            if (dt.Rows[0]["invstyp"].ToString() == "1")
                dt.Rows[0]["txt_invstyp"] = "F & O";
            if (dt.Rows[0]["invstyp"].ToString() == "3")
                dt.Rows[0]["txt_invstyp"] = "MCX";
            if (dt.Rows[0]["invstyp"].ToString() == "4")
                dt.Rows[0]["txt_invstyp"] = "Currency";
            if (dt.Rows[0]["invstyp"].ToString() == "5")
                dt.Rows[0]["txt_invstyp"] = "NCDEX";

            string json = JsonConvert.SerializeObject(dt).Replace("[","").Replace("]",""); //DataTableToJSON(dt).ToString();
            return Json((dt != null ? json : "1"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Details()
        {

            DataTable dt = Session["Dt_Details"] as DataTable;
            // string jsonstr = DataTableToJsonObj(dt);

            DataTable dtCloned = dt.Clone();
            dtCloned.Columns["ConsultantCode"].DataType = typeof(string);
            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }
            string JSONresult = JsonConvert.SerializeObject(dtCloned);

            //  return Json((dt != null ? DataTableToJSON(dt) : "1"), JsonRequestBehavior.AllowGet);
            return Json((dt != null ? JSONresult : "1"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Expenses()
        {
            DataTable dt = Session["Dt_Expanses"] as DataTable;
            return Json((dt != null ? DataTableToJSON(dt) : "1"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_All_Scprit()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_SCPRIT_MAPPING";
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
            return Json((DT != null ? DataTableToJSON(DT) : "1"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_Validate_billNo(string BillNo)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VALIDATE_BILL_NO";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = BillNo.Trim();
            cmd.Parameters.Add("@VAR2", SqlDbType.NVarChar).Value = MemberCode.Trim();
            cmd.Parameters.Add("@VAR3", SqlDbType.NVarChar).Value = FinancialYearCode.Trim();
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

            if(DT.Rows.Count>0)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
           
        }
        [HttpGet]
        public ActionResult BRExpense()
        {
            return View();
        }

        public ActionResult BRDetailsSave(string JsonData,string BillNo)
        {
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            Dt_Header.Rows[0]["BillNo"] = BillNo.Trim();
            Dt_Header.AcceptChanges();
            Session["Dt_Header"] = Dt_Header;
            DataTable dtdata = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            DataTable dt_orge = Session["Dt_Details"] as DataTable;
            //  DataTable dtdata = CreateDataTable(data);

            Session["Dt_Details"] = dtdata;
            return Json("1", JsonRequestBehavior.AllowGet);
        }
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        public JsonResult SaveAllDeailts()
        {
            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            if (data.invstyp == "0")
            {
                SaveEquity();
            }
            if (data.invstyp == "1")
            {//fno
                SaveFNO();
            }
            if (data.invstyp == "3")
            {
                SaveMCX();
            }
            if (data.invstyp == "4")
            {
                SaveCurrency();
            }


            return Json("Data Submit Successfully", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBREntry(BREntryModel data)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            data.FilesString = Session["FilesString"].ToString();
            data.FileName = Session["FileName"].ToString();
            data.Fileextenion = Session["Fileextenion"].ToString();
            data.ContentType = Session["ContentType"].ToString();
            data.year_code = FinancialYearCode.ToString();
            data.member_code = MemberCode.ToString();
            //string Date = Session["BRDate"].ToString();
            //IFormatProvider culture = new CultureInfo("en-IN", true);

            //DateTime date = Convert.ToDateTime(Date);
            //data.Date = date.ToString("yyyy-MM-dd");
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_BROKER_BILL";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = data.Broker_Id;
            cmd.Parameters.Add("@VAR2", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@VAR3", SqlDbType.NVarChar).Value = FinancialYearCode;
            cmd.Parameters.Add("@VAR4", SqlDbType.NVarChar).Value = data.invstyp;
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
            data.Brokercode = DT.Select("ID='" + data.ContractNoteId + "'").CopyToDataTable().Rows[0]["Member_Code"].ToString();
            Session["SaveBREntry"] = data;
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OnboardingContractNote()
        {
            //Session["member_code"] = "M0001";
            //Session["year_code"] = "1120212022";
            return View();
        }
        [HttpPost]
        public JsonResult OnboardingContractNote(BREntryModel data)
        {

            data.FilesString = Session["FilesString"].ToString();
            data.FileName = Session["FileName"].ToString();
            data.Fileextenion = Session["Fileextenion"].ToString();
            data.ContentType = Session["ContentType"].ToString();
            data.year_code = Session["year_code"].ToString();
            data.member_code = Session["member_code"].ToString();
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_BROKER_BILL";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = data.invstyp;
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
            data.Brokercode = DT.Select("ID='" + data.ContractNoteId + "'").CopyToDataTable().Rows[0]["Member_Code"].ToString();
            Session["SaveBREntry"] = data;
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Import()
        {
            BrokerBill _Broker = new BrokerBill();
            ViewBag.BrokerFormat = new SelectList(obj.BindBrokerFormat().ToList(), dataValueField: "BrokerBillFormatID", dataTextField: "BrokerFormatName");
            return View(_Broker);
        }
        [HttpPost]
        public ActionResult Import(string Close, String Upload, HttpPostedFileBase FilePath, BrokerBill _Broker)
        {
            ViewBag.DematAC = new SelectList(obj.BindDematMaster().ToList(), dataValueField: "DematID", dataTextField: "Name");
            ViewBag.Consultant = new SelectList(obj.BindConsultantMaster().ToList(), dataValueField: "ConsultantID", dataTextField: "Name");
            ViewBag.InvestmentType = new SelectList(obj.BindInvenstmentType(mtype).ToList(), dataValueField: "TypeId", dataTextField: "Name");
            ViewBag.Broker = new SelectList(obj.BindBrokerList().ToList(), dataValueField: "AccountId", dataTextField: "Name");
            ViewBag.BrokerFormat = new SelectList(obj.BindBrokerFormat().ToList(), dataValueField: "BrokerBillFormatID", dataTextField: "BrokerFormatName");
            BrokerBill _Brokerobj = new BrokerBill();
            if (!string.IsNullOrEmpty(Close))
            {
                return View("SaveBREntry");
            }
            if (!string.IsNullOrEmpty(Upload))
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(FilePath.InputStream))
                {
                    bytes = br.ReadBytes(FilePath.ContentLength);
                }
                string _FileName = Path.GetFileName(FilePath.FileName);
                string fileextenion = Path.GetExtension(FilePath.FileName);
                string ContentType = FilePath.ContentType.ToString();

                FilesTans dataobj = new FilesTans();
                dataobj.contentType = ContentType;
                dataobj.fileextension = fileextenion;
                dataobj.fileName = _FileName;
                dataobj.pdfcode = "1";
                dataobj.pdfbytes = Convert.ToBase64String(bytes);
                var client = new RestClient("http://localhost:64918/api/PdfProcess/pdfconfig");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
                request.AddHeader("Content-Type", "application/json");
                var body = Newtonsoft.Json.JsonConvert.SerializeObject(dataobj);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);
                DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(response.Content);
                Session["BrokerDt"] = myDataSet.Tables[0];
                Session["BrokerExpense"] = myDataSet.Tables[1];
                ViewBag.BrokerFormat = "";

                ViewBag.Message = "File Uploaded Successfully..!";
                //  string _FileName = Path.GetFileName(FilePath.FileName);

                return RedirectToAction("SaveBREntry");

            }


            return View();
        }

        [HttpPost]
        public JsonResult GetScript(string Prefix)
        {
            List<Script> scriptlist = obj.SelectallScript(Prefix);
            //var scriptlistdata = (from N in scriptlist
            //                      where N.ScriptName.StartsWith(Prefix)
            //                select new { N.ScriptName });
            return Json(scriptlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllConsultant()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //int ID = Convert.ToInt32(Session["UserID"].ToString());
            //int SubscriberID = GetSubscriberID(ID);
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_CONSULTANTDATA]";
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
            DataTable dtCloned = DT.Clone();
            dtCloned.Columns["ID"].DataType = typeof(Int32);
            foreach (DataRow row in DT.Rows)
            {
                dtCloned.ImportRow(row);
            }
            return Json(DataTableToJSON(dtCloned), JsonRequestBehavior.AllowGet);

        }
        public int GetSubscriberID(int ID)
        {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/ACCOUNTMASTER?DBAction=GetSubscriberID&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            UserModel.Rootobject obj = new UserModel.Rootobject();
            obj = JsonConvert.DeserializeObject<UserModel.Rootobject>(response.Content.ToString());

            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            int UserID = 0;
            USER sub = new USER();

            foreach (UserModel.Datum dr in obj.Data)
            {
                sub.SubscriberID = Convert.ToInt32(dr.SubscriberID.ToString());
                UserID = sub.SubscriberID;
            }
            return UserID;
        }
        public JsonResult GetAllBroker()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
           
          
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_ACCOUNTDATA]";
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
        public JsonResult GetAllBILLS(string Invtype, string BrokerID)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //IFormatProvider culture = new CultureInfo("en-IN", true);
            //DateTime date = DateTime.ParseExact(Date, "dd-MMM-yyyy", culture);
            //Session["BRDate"] = date;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_BROKER_BILL_UTILITY]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEW_BROKER_BILL";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = BrokerID.Split(':')[1].ToString();
            cmd.Parameters.Add("@VAR2", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@VAR3", SqlDbType.NVarChar).Value = FinancialYearCode;
            cmd.Parameters.Add("@VAR4", SqlDbType.NVarChar).Value = Invtype;
            //cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = Invtype;
            //cmd.Parameters.Add("@Date", SqlDbType.NVarChar).Value = date.ToString("yyyy-MM-dd");
            //cmd.Parameters.Add("@VAR3", SqlDbType.Int).Value = BrokerID.Split(':')[1].ToString();
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
               // throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
        public void SaveCurrency()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            share_Inv_Eqty_AC = data.HoldingTypeCode;

            FilesTans dataobj = new FilesTans();
            clsbbm MenuMaster = new clsbbm();
            MenuMaster.member_code = MemberCode.ToString();
            MenuMaster.yearcode = FinancialYearCode.ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Session["Dt_Expanses"] as DataTable;
            DataTable Dt_RunningBalance = Session["Dt_RunningBalance"] as DataTable;

            decimal TotalExpenses = 0;
            decimal TotalGrossNetAmt = 0;
            decimal TotalBrokerage = 0;
            decimal TotalNetAmt = 0;
            decimal TotalShrInInvestment = 0;
            decimal TotalStockInTrede = 0;
            TotalOfDeatilsEquity(ref TotalExpenses, ref TotalGrossNetAmt, ref TotalBrokerage, ref TotalNetAmt, ref TotalShrInInvestment, ref TotalStockInTrede);




            txtExpenses.Text = TotalExpenses.ToString();
            txtGrossNetAmt.Text = TotalGrossNetAmt.ToString();
            txtBillNo.Text = Dt_Header.Rows[0]["BillNo"].ToString();
            txtSettlementNo.Text = Dt_Header.Rows[0]["SettlementNo"].ToString();
            txtBrokerage.Text = TotalBrokerage.ToString();
            txtNetAmt.Text = TotalNetAmt.ToString();
            txtInvestment.Text = TotalShrInInvestment.ToString();
            textBox5.Text = TotalStockInTrede.ToString();
            txtRemarks.Text = "";
            flag = 1;
            transnobb.Text = "95";
            txtBroker.Text = data.Broker_Name;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("SampleTransaction");
            try
            {
                if (Validate_FNO())
                {
                    int vchno = 0;

                    if (flag == 1)
                    {
                        string bktyp = "BB";
                        vchno = GetGlobalTransactionNumber(bktyp);
                    }
                    else if (flag == 2)
                    {
                        vchno = Convert.ToInt32(transnobb.Text.Trim());

                        cmd = new SqlCommand("delete from  BrokerBill where Trans_no='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  BrokerBill_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };

                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  AC_Trans where Trans_No='" + transnobb.Text.Trim() + "' and  Book_Type1='BB' and MemberID='" + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };

                        cmd.ExecuteNonQuery();
                    }
                    string theDate1 = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");

                    //cmd = new SqlCommand("SELECT Member_Code FROM Mst_Member where Member_Name='" + txtMember.Text + "'", con)
                    //{
                    //    Transaction = transaction
                    //};
                    //SqlDataReader dr = cmd.ExecuteReader();
                    //dr.Read();
                    //dr.Close();

                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where Ac_Name='" + txtBroker.Text + "' and (Member_Code='' or Member_Code='" + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};
                    cmd = new SqlCommand("SELECT cast(AccountId as nvarchar) as AccountId FROM M_Account where  (MemberID='' or MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='' or FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)
                    {
                        Transaction = transaction
                    };
                    SqlDataReader dr2 = cmd.ExecuteReader();
                    dr2.Read();
                    string oc2 = data.Broker_Id;
                    dr2.Close();

                    string narr0 = txtBillNo.Text.ToString();
                    string narrr = txtSettlementNo.Text.ToString();
                    double AmountBrought = 0;
                    double Amountsold = 0;
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    string drorcr = "";
                    double amtbfacI = 0.0;
                    double amtsfacI = 0.0;
                    for (int counter = 0; counter < (Dt_Details.Rows.Count); counter++)
                    {
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Bought")
                            amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Sold")
                            amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (amtbfacI > amtsfacI)
                            AmountBrought = AmountBrought + (amtbfacI - amtsfacI);
                        else
                            Amountsold = Amountsold + (amtbfacI - amtsfacI);
                        amtsfacI = 0;
                        amtbfacI = 0;
                    }

                    double Amount = (AmountBrought - (Math.Abs(Amountsold)));
                    int trsrno = 1;
                    string drcd = "";
                    string crcd = "";
                    if (Amount > 0)
                        drorcr = "C";
                    else
                        drorcr = "D";
                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                        drorcr = "C";

                    cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,Amount,Stt_amt,"
                        + " ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('" + vchno + "','" + txtBillNo.Text + "','"
                        + theDate1 + "','" + txtSettlementNo.Text + "','" + MenuMaster.member_code + "','" + oc2 + "','" + drorcr + "','0','"
                        + Math.Abs(Convert.ToDouble(txtBrokerage.Text)) + "','"
                        + Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text)) + "','0','0','" + Math.Abs(Convert.ToDouble(txtExpenses.Text)) + "','"
                        + Math.Abs(Convert.ToDouble(txtNetAmt.Text)) + "','BB','"
                        + MenuMaster.yearcode + "','','N','" + 4 + "','" + txtRemarks.Text + "');SELECT SCOPE_IDENTITY();", con)
                    {
                        Transaction = transaction
                    };
                    int bbins = cmd.ExecuteNonQuery();
                    if (Convert.ToDouble(Math.Round(Amount)) > 0)
                    {
                        drcd = currencyacc;
                        crcd = oc2;
                    }
                    else
                    {
                        drcd = oc2;
                        crcd = currencyacc;
                    }

                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                        + "[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType] ,[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                        + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                        + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + 4 + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        int nre = cmd.ExecuteNonQuery();

                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                        + "[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType] ,[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                        + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                        + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + 4 + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        int nre = cmd.ExecuteNonQuery();

                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }

                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {
                        if (Dt_Expanses.Rows[counter1]["ExpAmount"] != null)
                        {
                            double otrxpc = double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString());
                            if (otrxpc > 0)
                            {
                                cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where AC_name='" + Dt_Expanses.Rows[counter1]["expnm"].ToString() + "' and (Member_Code='' or Member_Code='" + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                                {
                                    Transaction = transaction
                                };
                                SqlDataReader dr4034 = cmd.ExecuteReader();
                                dr4034.Read();
                                string accttyypp5 = dr4034.GetString(0);
                                dr4034.Close();
                                string selextype = "";

                                if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Add")
                                    selextype = "A";
                                if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Less")
                                    selextype = "L";

                                int nre2 = 0;
                                if (selextype == "A")
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg] ,[EntryType]) VALUES('" + vchno + "','"
                                        + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + accttyypp5 + "','"
                                        + oc2 + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"]
                                        .ToString()), decmlpl) + "','" + 4 + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg] ,[EntryType]) VALUES('" + vchno + "','" + trsrno + "','"
                                        + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + oc2 + "','" + accttyypp5 + "','BB','"
                                        + nartn1 + "','" + nartn2 + "','N','" + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()), decmlpl) + "','"
                                        + 4 + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }

                                if (nre2 > 0)
                                    trsrno = trsrno + 1;
                            }
                        }
                    }

                    for (int counter1 = 0; counter1 < (Dt_Details.Rows.Count); counter1++)
                    {
                        string sccd = Dt_Details.Rows[counter1]["ScriptCode"].ToString();
                        string ccd = Dt_Details.Rows[counter1]["ConsultantCode"].ToString();
                        double bkrch = double.Parse(Dt_Details.Rows[counter1]["BrokAmt"].ToString()) / double.Parse(Dt_Details.Rows[counter1]["Qty"].ToString());
                        string expdt = "";
                        string expmonth = "";

                        if (Dt_Details.Rows[counter1]["ExpiryDate"] != null)
                            expdt = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        if (Dt_Details.Rows[counter1]["ExpiryDate"] != null)
                            expmonth = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("MMMM-yyyy");

                        string optyp = "";
                        if (Dt_Details.Rows[counter1]["OptionType"] != null)
                            optyp = Dt_Details.Rows[counter1]["OptionType"].ToString();
                        string sttyp = "";

                        if (Dt_Details.Rows[counter1]["StockType"] != null)
                            sttyp = Dt_Details.Rows[counter1]["StockType"].ToString();

                        var srnr = counter1 + 1;
                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                        }
                        cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No, Sr_No, Bill_no, Trans_Dt, TransType, Ac_Type, Script_ID, Qty, G_Rate, Brok_Amt,"
                            + " ServTax_Amount, Rate, Amount, MemberID, FinancialYearMemberID, Consultant_ID, EntrySource,Brok_PerUnit, MonthYear, Last_Date, LotSize, LotQty, "
                            + " isFNOBill, Strike_Price, OptionType, isstock,FrmNm)values('" + vchno + "', '" + srnr + "', '" + txtBillNo.Text + "', '" + Trasndate + "', '"
                            + Dt_Details.Rows[counter1]["Type"].ToString() + "', '', '" + sccd + "', '" + Dt_Details.Rows[counter1]["Qty"].ToString() + "', '"
                            + Dt_Details.Rows[counter1]["GrossRate"].ToString() + "', '" + Dt_Details.Rows[counter1]["BrokAmt"].ToString() + "', '0', '"
                            + Dt_Details.Rows[counter1]["Rate"].ToString() + "', '" + Dt_Details.Rows[counter1]["Amount"].ToString() + "', '"
                            + MenuMaster.member_code + "', '" + MenuMaster.yearcode + "', '" + ccd + "', '0', '" + bkrch + "','" + expmonth + "','" + expdt + "','"
                            + Dt_Details.Rows[counter1]["Lott"].ToString() + "','" + Dt_Details.Rows[counter1]["LotQty"].ToString() + "','"
                            + 4 + "','" + Dt_Details.Rows[counter1]["StrikePrice"].ToString() + "','" + optyp + "','" + sttyp + "','BB')", con)
                        {
                            Transaction = transaction
                        };
                        int brtrns = cmd.ExecuteNonQuery();
                    }
                    //  MessageBox.Show("Records Saved Successfully");
                    transaction.Commit();
                    // btnf.btnfunc(btnAdd, btnEdit, btnDelete, btnSave, btncanel, btnSearch, btnprint, btnclose2);

                    // Disable();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                //  MessageBox.Show(ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
        }
        public JsonResult GetAllDemate()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            int ID = Convert.ToInt32(Session["UserID"].ToString());
            int SubscriberID = GetSubscriberID(ID);
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_DEMATDATA]";
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
        public bool Validate_1()
        {
            return true;
        }
        public string share_Inv_Eqty_AC = "1100014";
        public string SHAREINVESTMENT_EQUITY_ACC_Name = "SHARE INVESTMENT (EQUITY) A/C";
        public string STOCKINTRADEEQUITY_ACC_Name = "STOCK IN TRADE (EQUITY) A/C";
        public string shrtradeinveqty = "1100001";
        public string fnoshrtrd = "1100023";
        public string F_O_SHARETRADING_ACC_Name = "F & O SHARE TRADING A/C";
        public string commdityacc = "1100024";
        public string COMMODITY_SHARETRADING_ACC_Name = "COMMODITY SHARE TRADING A/C";
        public string currencyacc = "1100025";
        public string ncdexacc = "1100033";
        public string NCDEX_SHARETRADING_ACC_Name = "NCDEX SHARE TRADING A/C";
        public string ROUND_OFF_ACC_Code = "1100033";
        public string ROUND_OFF_ACC_Name = "NCDEX SHARE TRADING A/C";
        System.Web.UI.WebControls.TextBox txtdemateCode = new System.Web.UI.WebControls.TextBox();
        string dematname = "";
        System.Web.UI.WebControls.TextBox txtdemate = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtConsultantCode = new System.Web.UI.WebControls.TextBox();
        string Consultant_Name = "SELF CONSULTANT";
        System.Web.UI.WebControls.TextBox txtConsultant = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox transnobb = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtBroker = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtExpenses = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtGrossNetAmt = new System.Web.UI.WebControls.TextBox();

        string roundoff = "1100109";
        int flag = 2;
        private bool Validate_FNO()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            clsbbm MenuMaster = new clsbbm();
            MenuMaster.member_code = MemberCode.ToString();
            MenuMaster.yearcode = FinancialYearCode.ToString();
            txtdemateCode.Text = data.Demat_Ac_Id;
            txtdemate.Text = data.Demat_Ac_Name;
            txtConsultantCode.Text = data.Consultant;
            txtConsultant.Text = data.ConsultantCode;

            try
            {
                string str = "";
                int internalentexs = 0;

                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnectionNew")].ConnectionString);
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
                con.Open();
                SqlCommand cmd450 = new SqlCommand("SELECT * from M_Account where AccountId='" + share_Inv_Eqty_AC + "'"
                    + "and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr450 = cmd450.ExecuteReader();
                if (!dr450.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + share_Inv_Eqty_AC + "]" + SHAREINVESTMENT_EQUITY_ACC_Name;
                }
                dr450.Close();
                SqlCommand cmd451 = new SqlCommand("SELECT * from M_Account where AccountId='" + shrtradeinveqty + "'"
                    + " and ( MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr451 = cmd451.ExecuteReader();
                if (!dr451.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + shrtradeinveqty + "]" + STOCKINTRADEEQUITY_ACC_Name;
                }
                dr451.Close();
                SqlCommand cmd452 = new SqlCommand("SELECT * from M_Account where AccountId='" + fnoshrtrd + "'"
                    + " and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr452 = cmd452.ExecuteReader();
                if (!dr452.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + fnoshrtrd + "]" + F_O_SHARETRADING_ACC_Name;
                }
                dr452.Close();
                SqlCommand cmd454 = new SqlCommand("SELECT * from M_Account where AccountId='" + commdityacc + "'"
                    + " and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr454 = cmd454.ExecuteReader();
                if (!dr454.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + commdityacc + "]" + COMMODITY_SHARETRADING_ACC_Name;
                }
                dr454.Close();
                SqlCommand cmd455 = new SqlCommand("SELECT * from M_Account where AccountId='" + currencyacc + "'"
                    + " and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr455 = cmd455.ExecuteReader();

                if (!dr455.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + currencyacc + "]";
                }
                dr455.Close();

                SqlCommand cmd879 = new SqlCommand("SELECT * from M_Account where AccountId='" + ncdexacc + "'"
                    + " and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr879 = cmd879.ExecuteReader();
                if (!dr879.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + ncdexacc + "]" + NCDEX_SHARETRADING_ACC_Name;
                }
                dr879.Close();


                SqlCommand cmd4567 = new SqlCommand("SELECT * from M_Account where AccountId='" + roundoff + "'"
                    + " and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr4557 = cmd4567.ExecuteReader();

                if (!dr4557.HasRows)
                {
                    internalentexs = 1;
                    str = str + ROUND_OFF_ACC_Code + ROUND_OFF_ACC_Name;
                }
                dr4557.Close();
                //SqlCommand cmd4568 = new SqlCommand("SELECT DP_Name from Mst_Demat where DP_Code='" + txtdemateCode.Text + "'"
                //    + " and (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);

                SqlCommand cmd4568 = new SqlCommand("SELECT Name from M_Demat where DematID='" + txtdemateCode.Text + "'"
                   + " and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);

                SqlDataReader dr4568 = cmd4568.ExecuteReader();
                if (!dr4568.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + txtdemateCode.Text + "]" + dematname;
                }
                else
                {
                    dr4568.Read();
                    txtdemate.Text = dr4568.GetValue(0).ToString();
                }
                dr4568.Close();
                SqlCommand cmd457 = new SqlCommand("SELECT Name from M_Consultant where ConsultantID='" + txtConsultantCode.Text + "'", con);
                SqlDataReader dr457 = cmd457.ExecuteReader();
                if (!dr457.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + txtConsultantCode.Text + "]" + Consultant_Name;
                }
                else
                {
                    dr457.Read();
                    txtConsultant.Text = dr457.GetValue(0).ToString();
                }
                dr457.Close();
                if (internalentexs < 0)
                {
                    //  MessageBox.Show("Internal entries are missing, please check database" + str);
                    return false;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
            return true;
        }
        private bool Validate_FNOOld()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            clsbbm MenuMaster = new clsbbm();
            MenuMaster.member_code = MemberCode.ToString();
            MenuMaster.yearcode = FinancialYearCode.ToString();
            txtdemateCode.Text = "1100002";
            txtdemate.Text = "";
            txtConsultantCode.Text = "";
            txtConsultant.Text = "SELF CONSULTANT";

            try
            {
                string str = "";
                int internalentexs = 0;

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnectionNew")].ConnectionString);
                con.Open();
                SqlCommand cmd450 = new SqlCommand("SELECT * from Mst_Account where AC_Code='" + share_Inv_Eqty_AC + "'"
                    + "and (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr450 = cmd450.ExecuteReader();
                if (!dr450.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + share_Inv_Eqty_AC + "]" + SHAREINVESTMENT_EQUITY_ACC_Name;
                }
                dr450.Close();
                SqlCommand cmd451 = new SqlCommand("SELECT * from Mst_Account where AC_Code='" + shrtradeinveqty + "'"
                    + " and ( Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr451 = cmd451.ExecuteReader();
                if (!dr451.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + shrtradeinveqty + "]" + STOCKINTRADEEQUITY_ACC_Name;
                }
                dr451.Close();
                SqlCommand cmd452 = new SqlCommand("SELECT * from Mst_Account where AC_Code='" + fnoshrtrd + "'"
                    + " and (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr452 = cmd452.ExecuteReader();
                if (!dr452.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + fnoshrtrd + "]" + F_O_SHARETRADING_ACC_Name;
                }
                dr452.Close();
                SqlCommand cmd454 = new SqlCommand("SELECT * from Mst_Account where AC_Code='" + commdityacc + "'"
                    + " and (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr454 = cmd454.ExecuteReader();
                if (!dr454.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + commdityacc + "]" + COMMODITY_SHARETRADING_ACC_Name;
                }
                dr454.Close();
                SqlCommand cmd455 = new SqlCommand("SELECT * from Mst_Account where AC_Code='" + currencyacc + "'"
                    + " and (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr455 = cmd455.ExecuteReader();

                if (!dr455.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + currencyacc + "]";
                }
                dr455.Close();

                SqlCommand cmd879 = new SqlCommand("SELECT * from Mst_Account where AC_Code='" + ncdexacc + "'"
                    + " and (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr879 = cmd879.ExecuteReader();
                if (!dr879.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + ncdexacc + "]" + NCDEX_SHARETRADING_ACC_Name;
                }
                dr879.Close();


                SqlCommand cmd4567 = new SqlCommand("SELECT * from Mst_Account where AC_Code='" + roundoff + "'"
                    + " and (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr4557 = cmd4567.ExecuteReader();

                if (!dr4557.HasRows)
                {
                    internalentexs = 1;
                    str = str + ROUND_OFF_ACC_Code + ROUND_OFF_ACC_Name;
                }
                dr4557.Close();
                SqlCommand cmd4568 = new SqlCommand("SELECT DP_Name from Mst_Demat where DP_Code='" + txtdemateCode.Text + "'"
                    + " and (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='" + MenuMaster.yearcode + "')", con);
                SqlDataReader dr4568 = cmd4568.ExecuteReader();
                if (!dr4568.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + txtdemateCode.Text + "]" + dematname;
                }
                else
                {
                    dr4568.Read();
                    txtdemate.Text = dr4568.GetValue(0).ToString();
                }
                dr4568.Close();
                SqlCommand cmd457 = new SqlCommand("SELECT Consultant_Name from Mst_Consultant where Consultant_Code='" + txtConsultantCode.Text + "'", con);
                SqlDataReader dr457 = cmd457.ExecuteReader();
                if (!dr457.HasRows)
                {
                    internalentexs = 1;
                    str = str + "[" + txtConsultantCode.Text + "]" + Consultant_Name;
                }
                else
                {
                    dr457.Read();
                    txtConsultant.Text = dr457.GetValue(0).ToString();
                }
                dr457.Close();
                if (internalentexs < 0)
                {
                    //  MessageBox.Show("Internal entries are missing, please check database" + str);
                    return false;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
            return true;
        }
        public  int GetGlobalTransactionNumber(string bktyp)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            con.Open();
            string dttod = DateTime.Today.ToString("yyyy-MM-dd");
            SqlCommand cmd43 = new SqlCommand("Insert into Global_no(Book_typ, Date, MemberID, FinancialYearMemberID, CreatedBy, CreatedDate) values('" + bktyp + "',getdate(),"+ MemberCode + ","+ FinancialYearCode + ","+ CreatedById + ",getdate());SELECT SCOPE_IDENTITY();", con);
            var vchno = cmd43.ExecuteScalar();
            return Convert.ToInt32(vchno);
        }
        System.Web.UI.WebControls.TextBox txtBillNo = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtSettlementNo = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtBrokerage = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtNetAmt = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtInvestment = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox textBox5 = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.TextBox txtRemarks = new System.Web.UI.WebControls.TextBox();
        System.Web.UI.WebControls.CheckBox checkBox3 = new System.Web.UI.WebControls.CheckBox();//Slab
        int Purchasedate = 0;
        string Trasndate = "";
        string invstyp = "0";
        int decmlpl = 2;
        int _GlobalFormateNo = 0;

        public void TotalOfDeatilsEquity(ref decimal TotalExpenses, ref decimal TotalGrossNetAmt,
            ref decimal TotalBrokerage,
            ref decimal TotalNetAmt, ref decimal TotalShrInInvestment,ref decimal TotalStockInTrede)
        {
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Session["Dt_Expanses"] as DataTable;
            foreach(DataRow dr in Dt_Expanses.Rows)
            {
                TotalExpenses += Convert.ToDecimal(dr["ExpAmount"]);
            }
            foreach (DataRow dr in Dt_Details.Rows)
            {
                TotalGrossNetAmt += Convert.ToDecimal(dr["GrossAmt"]);
                TotalBrokerage += Convert.ToDecimal(dr["BrokAmt"]);
                TotalNetAmt += Convert.ToDecimal(dr["Amount"]);
                TotalShrInInvestment +=(dr["HoldingType"].ToString()=="0"? Convert.ToDecimal(dr["Amount"]):0) ;
                TotalStockInTrede += (dr["HoldingType"].ToString() == "1" ? Convert.ToDecimal(dr["Amount"]) : 0);
            }
            TotalNetAmt = TotalNetAmt + TotalExpenses;
        }
        public void SaveEquity()
        {
            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            share_Inv_Eqty_AC = data.HoldingTypeCode;

            FilesTans dataobj = new FilesTans();
            clsbbm MenuMaster = new clsbbm();
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

           
            string BrokerCode = data.Broker_Id;
            MenuMaster.member_code = MemberCode.ToString();
            MenuMaster.yearcode = FinancialYearCode.ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Session["Dt_Expanses"] as DataTable;
            DataTable Dt_RunningBalance = Session["Dt_RunningBalance"] as DataTable;
            decimal TotalExpenses = 0;
            decimal TotalGrossNetAmt = 0;
            decimal TotalBrokerage = 0;
            decimal TotalNetAmt = 0;
            decimal TotalShrInInvestment = 0;
            decimal TotalStockInTrede = 0;
            TotalOfDeatilsEquity(ref TotalExpenses, ref TotalGrossNetAmt, ref TotalBrokerage, ref TotalNetAmt, ref TotalShrInInvestment, ref TotalStockInTrede);



            txtExpenses.Text = TotalExpenses.ToString();//total of ExpAmount
            txtGrossNetAmt.Text = TotalGrossNetAmt.ToString();// total of GrossAmt
            txtBillNo.Text = Dt_Header.Rows[0]["BillNo"].ToString();
            txtSettlementNo.Text = Dt_Header.Rows[0]["SettlementNo"].ToString();
            txtBrokerage.Text = TotalBrokerage.ToString();// total of BrokAmt
            txtNetAmt.Text = TotalNetAmt.ToString();//total of Amount
            txtInvestment.Text = TotalShrInInvestment.ToString();//holing type =0  then total of Amount
            textBox5.Text = TotalStockInTrede.ToString(); //holing type =1  then total of Amount

            txtRemarks.Text = "";
            flag = 1;
            transnobb.Text = "95";
            txtBroker.Text = data.Broker_Name;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            cmd.CommandTimeout = 0;
            transaction = con.BeginTransaction("SampleTransaction");
            try
            {
                if (Validate_FNO())
                {
                    int vchno = 0;
                    if (flag == 1)
                    {
                        string bktyp = "BB";
                        vchno = GetGlobalTransactionNumber(bktyp);
                    }
                    else if (flag == 2)
                    {
                        vchno = Convert.ToInt32(transnobb.Text.Trim());
                        cmd = new SqlCommand("delete from  BrokerBill where Trans_no='" + transnobb.Text.Trim() + "' and MemberID='"
                            + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  BrokerBill_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='"
                            + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  BrokerBill_DmatInfo where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='"
                            + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  AC_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='"
                            + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    int counter = 0;


                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where (Member_Code='" + MenuMaster.member_code + "') and (Year_Code='"
                    //    + MenuMaster.yearcode + "') and Ac_Name='" + txtBroker.Text.Trim() + "'", con);
                    //cmd.Transaction = transaction;
                    //SqlDataReader dr2 = cmd.ExecuteReader();
                    //if (dr2.HasRows)
                    //{
                    //    dr2.Read();
                    //    BrokerCode = dr2.GetString(0).Trim();
                    //}
                    //dr2.Close();

                    string LedgerDate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                    string drorcr = "";
                    double amtbfacI = 0;
                    double amtbfacT = 0;
                    double amtsfacI = 0;
                    double amtsfacT = 0;
                    double _totalINV_Shareval = 0;
                    double _totalINV_StockTradeval = 0;
                    int nre = 0;

                    for (counter = 0; counter < Dt_Details.Rows.Count; counter++)
                    {
                        string styp = Dt_Details.Rows[counter]["HoldingType"].ToString();
                        string[] detailss = styp.Split('-');
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Bought" & detailss[0] == "I")
                            amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Bought" & detailss[0] == "T")
                            amtbfacT = Math.Round(amtbfacT + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Sold" & detailss[0] == "I")
                            amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Sold" & detailss[0] == "T")
                            amtsfacT = Math.Round(amtsfacT + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (amtbfacI > amtsfacI)
                            _totalINV_Shareval = (amtbfacI - amtsfacI);
                        else
                            _totalINV_Shareval = (amtbfacI - amtsfacI);

                        #region stock in trade  
                        if (amtbfacT > amtsfacT)
                            _totalINV_StockTradeval = (amtbfacT - amtsfacT);
                        else
                            _totalINV_StockTradeval = (amtbfacT - amtsfacT);
                        #endregion
                    }
                    double TotalAmount = Convert.ToDouble(_totalINV_Shareval - _totalINV_StockTradeval * -1);

                    if (TotalAmount > 0)
                        drorcr = "C";
                    else
                        drorcr = "D";

                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                        drorcr = "C";

                    if (Convert.ToInt32(vchno) > 0)
                    {
                        cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,Amount,"
                            + " Stt_amt,ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('" + vchno + "','"
                            + txtBillNo.Text + "','" + LedgerDate + "','" + txtSettlementNo.Text + "','" + MenuMaster.member_code + "','" + BrokerCode + "','" + drorcr + "','0','"
                            + Math.Abs(Convert.ToDouble(txtBrokerage.Text)) + "','" + (Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text))) + "','0','0','"
                            + (Math.Abs(Convert.ToDouble(txtExpenses.Text))) + "','" + (Math.Abs(Convert.ToDouble(txtNetAmt.Text))) + "','BB','" + MenuMaster.yearcode + "','','N','"
                            + invstyp + "','" + txtRemarks.Text + "')", con);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                        string narr0 = txtBillNo.Text.ToString();
                        string narrr = txtSettlementNo.Text.ToString();
                        string narr1 = "BILL NO: ";
                        string narr2 = "Sett.NO: ";
                        string nartn1 = narr1 + narr0;
                        string nartn2 = narr2 + narrr;
                        string drcd = "";
                        string crcd = "";
                        int trsrno = 1;

                        if (!checkBox3.Checked)
                        {
                            #region vasant share Investment                     
                            if (Convert.ToDouble((_totalINV_Shareval)) > 0)
                            {
                                drcd = share_Inv_Eqty_AC;
                                crcd = BrokerCode;
                            }
                            else
                            {
                                drcd = BrokerCode;
                                crcd = share_Inv_Eqty_AC;
                            }
                            // If Gross Amont Less Then Expense Amount Share Investment
                            if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtInvestment.Text))
                            {
                                cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                    + "[Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]) VALUES('" + vchno + "','" + trsrno + "','"
                                    + LedgerDate + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                                    + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_Shareval) + "','" + invstyp + "','BB','0')", con);
                                cmd.Transaction = transaction;
                                nre = cmd.ExecuteNonQuery();

                                if (nre > 0)
                                    trsrno = trsrno + 1;
                            }
                            //// If Expense Amont Less Then Gross Amount Share Investment END
                            else
                            {
                                cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                   + " [Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]) VALUES('" + vchno + "','" + trsrno + "','"
                                   + LedgerDate + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                                   + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_Shareval) + "','" + invstyp + "','BB','0')", con);
                                cmd.Transaction = transaction;
                                nre = cmd.ExecuteNonQuery();
                                if (nre > 0)
                                    trsrno = trsrno + 1;
                            }
                            #endregion

                            #region vasant Stock In Trade
                            if (Convert.ToDouble((_totalINV_StockTradeval)) > 0)
                            {
                                drcd = shrtradeinveqty;
                                crcd = BrokerCode;
                            }
                            else
                            {
                                drcd = BrokerCode;
                                crcd = shrtradeinveqty;
                            }

                            // if  Gross Amont Stock In Trade Entry Less Then Expense Amount

                            if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(textBox5.Text))
                            {
                                cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                    + " [Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                                    + LedgerDate + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                                    + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_StockTradeval) + "','" + invstyp + "','BB','0')", con);
                                cmd.Transaction = transaction;
                                nre = cmd.ExecuteNonQuery();
                                if (nre > 0)
                                    trsrno = trsrno + 1;
                            }
                            // if Stock In Trade Entry Stock Amount Less Then Expense Amount END
                            else
                            {
                                cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                    + " [Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                                    + LedgerDate + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                                    + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_StockTradeval) + "','" + invstyp + "','BB','0')", con);
                                cmd.Transaction = transaction;
                                nre = cmd.ExecuteNonQuery();
                                if (nre > 0)
                                    trsrno = trsrno + 1;
                            }
                            #endregion
                        }
                        else // SLBM ENTRY
                        {
                            if (Convert.ToDouble((_totalINV_Shareval)) > 0)
                            {
                                drcd = "1100034";

                                crcd = BrokerCode;
                            }
                            else
                            {
                                drcd = BrokerCode;
                                crcd = "1100034";
                            }

                            cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                + " [Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]) VALUES('" + vchno + "','" + trsrno + "','"
                                + LedgerDate + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                                + nartn1 + "','" + nartn2 + "','" + txtRemarks.Text + "','N','" + Math.Abs(_totalINV_Shareval) + "','" + invstyp + "','BB','0')", con);
                            cmd.Transaction = transaction;
                            nre = cmd.ExecuteNonQuery();
                            if (nre > 0)
                                trsrno = trsrno + 1;
                        }
                        for (int counter1 = 0; counter1 < (Dt_Details.Rows.Count); counter1++)
                        {
                            string actyp = "";
                            string styp = "";
                            if (!checkBox3.Checked)
                            {
                                styp = Dt_Details.Rows[counter1]["HoldingType"].ToString();
                                string[] details = styp.Split('-');
                                if (details[0] == "I")
                                    actyp = share_Inv_Eqty_AC;
                                else if (details[0] == "T")
                                    actyp = shrtradeinveqty;
                            }
                            else // SLBM ENTRY Holding Type
                            {
                                actyp = "1100034";
                            }

                            int intrday = 0;
                            if (Dt_Details.Rows[counter1]["IntraDay"] != null
                                && bool.Parse(Dt_Details.Rows[counter1]["IntraDay"].ToString()))
                                intrday = 1;
                            string sccd = "";
                            if (Dt_Details.Rows[counter1]["ScriptCode"] != null)
                                sccd = Dt_Details.Rows[counter1]["ScriptCode"].ToString().Trim();
                            string ccd = "";

                            if (Dt_Details.Rows[counter1]["ConsultantCode"] != null)
                                ccd = Dt_Details.Rows[counter1]["ConsultantCode"].ToString().Trim();

                            string dmcd = "";
                            if (Dt_Details.Rows[counter1]["DematCode1"] != null)
                                dmcd = Dt_Details.Rows[counter1]["DematCode1"].ToString().Trim();

                            string trnstypnw = "";
                            if (Dt_Details.Rows[counter1]["Type"].ToString() == "Bought")
                                trnstypnw = "I";
                            else
                                trnstypnw = "O";

                            var srnr = counter1 + 1;
                            if (intrday == 0)
                            {
                                cmd = new SqlCommand("INSERT INTO BrokerBill_DmatInfo(Trans_No,Sr_No,Local_SrNo,Trans_Dt,MemberID,FinancialYearMemberID,DemateID,Script_ID,Qty,Trans_Type,"
                                    + " Consultant_ID,Ac_Type,internalEntry,frmpg)values('" + vchno + "','" + srnr + "','1','" + LedgerDate + "','" + MenuMaster.member_code + "','"
                                    + MenuMaster.yearcode + "','" + dmcd + "','" + sccd + "','" + Dt_Details.Rows[counter1]["Qty"].ToString() + "','" + trnstypnw + "','"
                                    + ccd + "','" + actyp + "','0','BB')", con);
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }

                            int SLBS = checkBox3.Checked ? 1 : 0;
                            if (Purchasedate == 1)
                                Trasndate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                            else
                            {
                                Trasndate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                            }

                            double bkrch = Math.Round(Math.Round(double.Parse(Dt_Details.Rows[counter1]["BrokAmt"].ToString()), 4) / Math.Round(double.Parse(Dt_Details.Rows[counter1]["Qty"].ToString()), 4), 4);
                            cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No,Sr_No,Bill_no,Trans_Dt,TransType,Ac_Type,Script_ID,Qty,Bal_qty,G_Rate,Brok_Amt,ServTax_Amount,"
                                + " Rate,Amount,MemberID,FinancialYearMemberID,Consultant_ID,EntrySource,isIntraDay,Brok_PerUnit,isFNOBill,C_Gross_Rate,Exp_Rate,FrmNm,IsSLBS)values('" + vchno + "','"
                                + srnr + "','" + txtBillNo.Text + "','" + Trasndate + "','" + Dt_Details.Rows[counter1]["Type"].ToString() + "','" + actyp + "','" + sccd + "','"
                                + Dt_Details.Rows[counter1]["Qty"].ToString() + "','" + Dt_Details.Rows[counter1]["Qty"].ToString() + "','"
                                + Dt_Details.Rows[counter1]["GrossRate"].ToString() + "','" + Dt_Details.Rows[counter1]["BrokAmt"].ToString() + "','0','"
                                + Dt_Details.Rows[counter1]["Rate"].ToString() + "','" + Dt_Details.Rows[counter1]["Amount"].ToString() + "','"
                                + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + ccd + "','0','" + intrday + "','" + bkrch + "','" + invstyp + "','"
                                + Dt_Details.Rows[counter1]["GrossRate"].ToString() + "','" + Dt_Details.Rows[counter1]["Rate"] + "','BB','"
                                + SLBS + "')", con);
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                        for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                        {
                            if (Dt_Expanses.Rows[counter1]["ExpAmount"] != null)
                            {
                                string accttyypp4 = "";
                                double otrxpc = Convert.ToDouble(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()));
                                if (otrxpc > 0)
                                {
                                    cmd = new SqlCommand("SELECT cast(AccountId as Nvarchar) as AccountId FROM [M_Account] where  Name='" + Dt_Expanses.Rows[counter1]["expnm"].ToString()
                                        + "' and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con);
                                    cmd.Transaction = transaction;
                                    SqlDataReader dr404 = cmd.ExecuteReader();
                                    if (dr404.HasRows)
                                    {

                                        dr404.Read();
                                        accttyypp4 = dr404.GetString(0);
                                    }
                                    dr404.Close();
                                    string selextype = "";
                                    if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Add")
                                    {
                                        selextype = "A";
                                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],[EntryType]) VALUES('" + vchno + "','"
                                            + trsrno + "','" + LedgerDate + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + accttyypp4 + "','"
                                            + BrokerCode + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()), decmlpl) + "','"
                                            + invstyp + "','" + selextype + "','BB','10')", con);
                                        cmd.Transaction = transaction;
                                        nre = cmd.ExecuteNonQuery();
                                    }
                                    else if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Less")
                                    {
                                        selextype = "L";
                                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],[EntryType]) VALUES('" + vchno + "','"
                                            + trsrno + "','" + LedgerDate + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + BrokerCode + "','"
                                            + accttyypp4 + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"]
                                            .ToString()), decmlpl) + "','" + invstyp + "','" + selextype + "','BB','10')", con);
                                        cmd.Transaction = transaction;
                                        nre = cmd.ExecuteNonQuery();
                                    }
                                    if (nre > 0)
                                        trsrno = trsrno + 1;
                                }
                            }
                        }
                        transaction.Commit();
                        con.Close();

                        //MessageBox.Show("Record Saved Successfully !!");
                        //btnSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // MessageBox.Show(ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
        }
        public void SaveMCX()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
           


            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            share_Inv_Eqty_AC = data.HoldingTypeCode;
            string invstyp = data.invstyp;
            FilesTans dataobj = new FilesTans();
            clsbbm MenuMaster = new clsbbm();
            MenuMaster.member_code = MemberCode.ToString();
            MenuMaster.yearcode = FinancialYearCode.ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Session["Dt_Expanses"] as DataTable;
            DataTable Dt_RunningBalance = Session["Dt_RunningBalance"] as DataTable;


            decimal TotalExpenses = 0;
            decimal TotalGrossNetAmt = 0;
            decimal TotalBrokerage = 0;
            decimal TotalNetAmt = 0;
            decimal TotalShrInInvestment = 0;
            decimal TotalStockInTrede = 0;
            TotalOfDeatilsEquity(ref TotalExpenses, ref TotalGrossNetAmt, ref TotalBrokerage, ref TotalNetAmt, ref TotalShrInInvestment, ref TotalStockInTrede);



            txtExpenses.Text = TotalExpenses.ToString();
            txtGrossNetAmt.Text = TotalGrossNetAmt.ToString();
            txtBillNo.Text = Dt_Header.Rows[0]["BillNo"].ToString();
            txtSettlementNo.Text = Dt_Header.Rows[0]["SettlementNo"].ToString();
            txtBrokerage.Text = TotalBrokerage.ToString();
            txtNetAmt.Text = TotalNetAmt.ToString();
            txtInvestment.Text = TotalShrInInvestment.ToString();
            textBox5.Text = TotalStockInTrede.ToString();
            txtRemarks.Text = "";
            flag = 1;
            transnobb.Text = "95";
            txtBroker.Text = data.Broker_Name.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("SampleTransaction");
            try
            {
                if (Validate_FNO())
                {
                    int vchno = 0;
                    if (flag == 1)
                    {
                        string bktyp = "BB";
                        vchno = GetGlobalTransactionNumber(bktyp);
                    }
                    else if (flag == 2)
                    {
                        vchno = Convert.ToInt32(transnobb.Text.Trim());
                        cmd = new SqlCommand("delete from  BrokerBill where Trans_no='" + transnobb.Text.Trim() + "' and MemberID='"
                            + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  BrokerBill_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='"
                            + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  AC_Trans where Trans_No='" + transnobb.Text.Trim() + "' and  Book_Type1='BB' and MemberID='"
                            + MenuMaster.member_code + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();
                    }
                    string theDate1 = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");

                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where Ac_Name='" + txtBroker.Text + "' and (Member_Code='"
                    //    + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};
                    //cmd = new SqlCommand("SELECT AccountId FROM M_Account where  (MemberID='"
                    // + MenuMaster.member_code + "') and (FinancialYearMemberID='' or FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};
                    //SqlDataReader dr2 = cmd.ExecuteReader();
                    //dr2.Read();
                    string oc2 = data.Broker_Id;
                    //dr2.Close();

                    string narr0 = txtBillNo.Text.ToString();
                    string narrr = txtSettlementNo.Text.ToString();
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    double AmountBrought = 0;
                    double Amountsold = 0;
                    string drorcr = "";
                    double amtbfacI = 0.0;
                    double amtsfacI = 0.0;

                    for (int counter = 0; counter < (Dt_Details.Rows.Count); counter++)
                    {
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Bought")
                            amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Sold")
                            amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (amtbfacI > amtsfacI)
                            AmountBrought = AmountBrought + (amtbfacI - amtsfacI);
                        else
                            Amountsold = Amountsold + (amtbfacI - amtsfacI);
                        amtsfacI = 0;
                        amtbfacI = 0;
                    }

                    double Amount = (AmountBrought - (Math.Abs(Amountsold)));
                    int trsrno = 1;
                    string drcd = "";
                    string crcd = "";

                    if (Amount > 0)
                        drorcr = "C";
                    else
                        drorcr = "D";
                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                        drorcr = "C";

                    cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,"
                        + " Amount,Stt_amt,ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('"
                        + vchno + "','" + txtBillNo.Text + "','" + theDate1 + "','" + txtSettlementNo.Text + "','" + MenuMaster.member_code + "','" + oc2 + "','"
                        + drorcr + "','0','" + (Math.Abs(Convert.ToDouble(txtBrokerage.Text))) + "','" + Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text)) + "','0','0','"
                        + Math.Abs(Convert.ToDouble(txtExpenses.Text)) + "','" + Math.Abs(Convert.ToDouble(txtNetAmt.Text)) + "','BB','"
                        + MenuMaster.yearcode + "','','N','" + invstyp + "','" + txtRemarks.Text + "');SELECT SCOPE_IDENTITY();", con)
                    {
                        Transaction = transaction
                    };
                    cmd.ExecuteNonQuery();

                    if (Convert.ToDouble(Math.Round(Amount)) > 0)
                    {
                        drcd = commdityacc;
                        crcd = oc2;
                    }
                    else
                    {
                        drcd = oc2;
                        crcd = commdityacc;
                    }
                    //* Expense Amount is Higher then Gross Amount*//
                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType] ,[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                            + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                            + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        int nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }
                    //* Expense Amount is Higher then Gross Amount END*//
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType] ,[frmpg],[EntryType] ) VALUES('" + vchno + "','" + trsrno + "','"
                            + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                            + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        int nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }


                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {
                        if (Dt_Expanses.Rows[counter1]["ExpAmount"].ToString() != null && Dt_Expanses.Rows[counter1]["ExpAmount"].ToString() != "")
                        {
                            double otrxpc = double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString());
                            if (otrxpc > 0)
                            {
                                cmd = new SqlCommand("SELECT AccountId FROM M_Account where Name='" + Dt_Expanses.Rows[counter1]["expnm"].ToString() + "' and (MemberID='' or MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='' or FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)
                                {
                                    Transaction = transaction
                                };
                                SqlDataReader dr4034 = cmd.ExecuteReader();
                                dr4034.Read();
                                string accttyypp5 = "";
                                try
                                {//error case
                                    accttyypp5 = dr4034[0].ToString();
                                }
                                catch
                                {

                                }

                               
                                dr4034.Close();
                                string selextype = "";

                                if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Add")
                                    selextype = "A";
                                if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Less")
                                    selextype = "L";

                                int nre2 = 0;
                                if (selextype == "A") // If Expense Type Add
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] "
                                        + ",[Cr_AccountId] ,[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],[EntryType] )"
                                        + " VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode
                                        + "','" + txtBillNo.Text + "','" + accttyypp5 + "','" + oc2 + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()), decmlpl) + "','" + invstyp
                                        + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                else      //if Expesne Type Less
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] "
                                        + " ,[Cr_AccountId] ,[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],[EntryType] ) "
                                        + " VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode
                                        + "','" + txtBillNo.Text + "','" + oc2 + "','" + accttyypp5 + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()), decmlpl) + "','"
                                        + invstyp + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                if (nre2 > 0)
                                    trsrno = trsrno + 1;
                            }
                        }
                    }

                    for (int counter1 = 0; counter1 < (Dt_Details.Rows.Count); counter1++)
                    {
                        string sccd = Dt_Details.Rows[counter1]["ScriptCode"].ToString();
                        string ccd = Dt_Details.Rows[counter1]["ConsultantCode"].ToString();
                        double bkrch = double.Parse(Dt_Details.Rows[counter1]["BrokAmt"].ToString()) / double.Parse(Dt_Details.Rows[counter1]["Qty"].ToString());
                        string expdt = "";
                        string expmonth = "";

                        if (Dt_Details.Rows[counter1]["ExpiryDate"] != null)
                            expdt = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        if (Dt_Details.Rows[counter1]["ExpiryDate"] != null)
                            expmonth = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("MMMM-yyyy");

                        string optyp = "";
                        if (Dt_Details.Rows[counter1]["OptionType"] != null)
                            optyp = Dt_Details.Rows[counter1]["OptionType"].ToString();

                        string sttyp = "";
                        if (Dt_Details.Rows[counter1]["StockType"] != null)
                            sttyp = Dt_Details.Rows[counter1]["StockType"].ToString();

                        var srnr = counter1 + 1;
                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(Dt_Header.Rows[0]["Date"]).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(Dt_Header.Rows[0]["Date"]).ToString("yyyy-MM-dd");
                        }

                        cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No, Sr_No, Bill_no, Trans_Dt, TransType, Ac_Type, Script_ID, Qty,"
                            + " G_Rate, Brok_Amt, ServTax_Amount, Rate, Amount, MemberID, FinancialYearMemberID, Consultant_ID, EntrySource,Brok_PerUnit,"
                            + " MonthYear, Last_Date, LotSize, LotQty, isFNOBill, Strike_Price, OptionType, isstock,FrmNm)values('" + vchno + "', '"
                            + srnr + "', '" + txtBillNo.Text + "', '" + Trasndate + "', '" + Dt_Details.Rows[counter1]["Type"].ToString()
                            + "', '', '" + sccd + "', '" + Dt_Details.Rows[counter1]["Qty"].ToString() + "', '"
                            + Dt_Details.Rows[counter1]["GrossRate"].ToString() + "', '" + Dt_Details.Rows[counter1]["BrokAmt"].ToString()
                            + "', '0', '" + Dt_Details.Rows[counter1]["Rate"].ToString() + "', '"
                            + Dt_Details.Rows[counter1]["Amount"].ToString() + "', '" + MenuMaster.member_code + "', '" + MenuMaster.yearcode
                            + "', '" + ccd + "', '0', '" + bkrch + "','" + expmonth + "','" + expdt + "','" + Dt_Details.Rows[counter1]["Lott"].ToString()
                            + "','" + Dt_Details.Rows[counter1]["LotQty"].ToString() + "','" + invstyp + "','"
                            + Dt_Details.Rows[counter1]["StrikePrice"].ToString() + "','" + optyp + "','" + sttyp + "','BB')", con)
                        {
                            Transaction = transaction
                        };
                        int brtrns = cmd.ExecuteNonQuery();
                    }
                    //  MessageBox.Show("Records Saved Successfully");
                    transaction.Commit();
                    // btnf.btnfunc(btnAdd, btnEdit, btnDelete, btnSave, btncanel, btnSearch, btnprint, btnclose2);

                    //   Disable();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                transaction.Rollback();
                
                // MessageBox.Show("  Message: {0}", ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
        }

        public void SaveFNO()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            share_Inv_Eqty_AC = data.HoldingTypeCode;

            FilesTans dataobj = new FilesTans();
            clsbbm MenuMaster = new clsbbm();
            string invstyp = data.invstyp;
            MenuMaster.member_code = MemberCode.ToString();
            MenuMaster.yearcode = FinancialYearCode.ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Session["Dt_Expanses"] as DataTable;
            DataTable Dt_RunningBalance = Session["Dt_RunningBalance"] as DataTable;

            decimal TotalExpenses = 0;
            decimal TotalGrossNetAmt = 0;
            decimal TotalBrokerage = 0;
            decimal TotalNetAmt = 0;
            decimal TotalShrInInvestment = 0;
            decimal TotalStockInTrede = 0;
            TotalOfDeatilsEquity(ref TotalExpenses, ref TotalGrossNetAmt, ref TotalBrokerage, ref TotalNetAmt, ref TotalShrInInvestment, ref TotalStockInTrede);


            txtExpenses.Text = TotalExpenses.ToString();
            txtGrossNetAmt.Text = TotalGrossNetAmt.ToString();
            txtBillNo.Text = Dt_Header.Rows[0]["BillNo"].ToString();
            txtSettlementNo.Text = Dt_Header.Rows[0]["SettlementNo"].ToString();
            txtBrokerage.Text = TotalBrokerage.ToString();
            txtNetAmt.Text = TotalNetAmt.ToString();
            txtInvestment.Text = TotalShrInInvestment.ToString();
            textBox5.Text = TotalStockInTrede.ToString();
            txtRemarks.Text = "";
            _GlobalFormateNo = Convert.ToInt32(data.ContractNoteId);
            transnobb.Text = "118";
            txtBroker.Text = data.Broker_Name.ToString();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnectionNew")].ConnectionString);
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            cmd.CommandTimeout = 0;
            transaction = con.BeginTransaction("SampleTransaction");
            flag = 1;


            try
            {
                if (Validate_FNO())
                {
                    int vchno = 0;
                    if (flag == 1)
                    {
                        string bktyp = "BB";
                        vchno = GetGlobalTransactionNumber(bktyp);
                    }
                    else if (flag == 2)
                    {
                        vchno = Convert.ToInt32(transnobb.Text.Trim());
                        cmd = new SqlCommand("delete from  BrokerBill where Trans_no='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code
                            + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  BrokerBill_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code
                            + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  AC_Trans where Trans_No='" + transnobb.Text.Trim() + "'  and MemberID='" + MenuMaster.member_code
                            + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();
                    }
                    string theDate1 = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where Ac_Name='" + txtBroker.Text + "' and  (Member_Code='"
                    //    + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};

                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where  (Member_Code='"
                    ////  + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //cmd = new SqlCommand("SELECT cast(AccountId as Nvarchar) as AccountId FROM [M_Account] where  (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)

                    //{
                    //    Transaction = transaction
                    //};
                    //SqlDataReader dr2 = cmd.ExecuteReader();
                    //dr2.Read();
                    string oc2 = data.Broker_Id.ToString();
                    //dr2.Close();

                    double AmountBrought = 0;
                    double Amountsold = 0;
                    string narr0 = txtBillNo.Text.ToString();
                    string narrr = txtSettlementNo.Text.ToString();
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    string drorcr = "";
                    double amtbfacI = 0.0;
                    int nre;
                    double amtsfacI = 0.0;

                    for (int counter = 0; counter < (Dt_Details.Rows.Count); counter++)
                    {
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Bought")
                            amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Sold")
                            amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (amtbfacI > amtsfacI)
                            AmountBrought = AmountBrought + (amtbfacI - amtsfacI);
                        else
                            Amountsold = Amountsold + (amtbfacI - amtsfacI);
                        amtsfacI = 0;
                        amtbfacI = 0;
                    }
                    double Amount = (AmountBrought - (Math.Abs(Amountsold)));
                    if (Amount > 0)
                        drorcr = "C";
                    else
                        drorcr = "D";

                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                        drorcr = "C";

                    int trsrno = 1;
                    string drcd = "";
                    string crcd = "";

                    cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,Amount,"
                        + " Stt_amt,ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('" + vchno + "','"
                        + txtBillNo.Text + "','" + theDate1 + "','" + txtSettlementNo.Text + "','" + MenuMaster.member_code + "','" + oc2 + "','" + drorcr + "','0','"
                        + (Math.Abs(Convert.ToDouble(txtBrokerage.Text))) + "','" + (Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text))) + "','0','0','"
                        + (Math.Abs(Convert.ToDouble(txtExpenses.Text))) + "','" + (Math.Abs(Convert.ToDouble(txtNetAmt.Text))) + "','BB','" + MenuMaster.yearcode + "','','N','"
                        + invstyp + "','" + txtRemarks.Text + "');SELECT SCOPE_IDENTITY();", con)
                    {
                        Transaction = transaction
                    };

                    cmd.ExecuteNonQuery();
                    // label3.Refresh();
                    if (Convert.ToDouble(Math.Round(Amount)) > 0)
                    {
                        drcd = fnoshrtrd;
                        crcd = oc2;
                    }
                    else
                    {
                        drcd = oc2;
                        crcd = fnoshrtrd;
                    }

                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_Code] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]  ) VALUES('" + vchno + "','"
                            + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','"
                            + drcd + "','" + crcd + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]  ) VALUES('" + vchno + "','" + trsrno + "','"
                            + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                            + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }

                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {
                        if (Dt_Expanses.Rows[counter1]["ExpAmount"] != null)
                        {
                            double otrxpc = (double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()));
                            if (otrxpc > 0)
                            {
                                cmd = new SqlCommand("SELECT AccountId FROM M_Account where Name='" + Dt_Expanses.Rows[counter1]["expnm"].ToString() + "' and (MemberID='" + MenuMaster.member_code + "') and (FinancialYearMemberID='" + MenuMaster.yearcode + "')", con)
                                {
                                    Transaction = transaction
                                };
                                SqlDataReader dr4034 = cmd.ExecuteReader();
                                // dr4034.Read();
                                //dr4034.Close();
                                // string accttyypp5 = "";
                                string accttyypp5 = "";
                                if (dr4034.HasRows)
                                {

                                    dr4034.Read();
                                    accttyypp5 = dr4034[0].ToString();
                                }
                                dr4034.Close();
                                //  int accttyypp5 = Convert.ToInt32(dr4034.GetString(0));
                                //  string accttyypp5 = accttyypp15.ToString();
                                dr4034.Close();

                                string selextype = "";
                                if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Add")
                                    selextype = "A";
                                if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Less")
                                    selextype = "L";
                                int nre2 = 0;

                                if (selextype == "A") // If Expense Type Add
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],"
                                        + " [EntryType] ) VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode
                                        + "','" + txtBillNo.Text + "','" + accttyypp5 + "','" + oc2 + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()), decmlpl) + "','"
                                        + invstyp + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                else  // if Expanse type Less
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,"
                                        + " [Cr_AccountId] ,[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],"
                                        + " [EntryType] ) VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode
                                        + "','" + txtBillNo.Text + "','" + oc2 + "','" + accttyypp5 + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()), decmlpl) + "','" + invstyp + "','"
                                        + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }

                                if (nre2 > 0)
                                    trsrno = trsrno + 1;
                            }
                        }
                    }

                    for (int counter1 = 0; counter1 < (Dt_Details.Rows.Count); counter1++)
                    {
                        double bkrch = Math.Round(double.Parse(Dt_Details.Rows[counter1]["BrokAmt"].ToString()) / double.Parse(Dt_Details.Rows[counter1]["Qty"].ToString()), decmlpl);
                        string expdt = "";
                        string expmonth = "";

                        if (Dt_Details.Rows[counter1]["ExpiryDate"] != null)
                            expdt = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");

                        string sccd = Dt_Details.Rows[counter1]["ScriptCode"].ToString();
                        string ccd = Dt_Details.Rows[counter1]["ConsultantCode"].ToString();

                        if (Dt_Details.Rows[counter1]["ExpiryDate"] != null)
                            expmonth = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("MMMM-yyyy");

                        var srnr = counter1 + 1;
                        string optyp = "";
                        string sttyp = "";

                        if (Dt_Details.Rows[counter1]["OptionType"] != null)
                            optyp = Dt_Details.Rows[counter1]["OptionType"].ToString();

                        if (Dt_Details.Rows[counter1]["StockType"] != null)
                            sttyp = Dt_Details.Rows[counter1]["StockType"].ToString();

                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                        }
                        cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No, Sr_No, Bill_no, Trans_Dt, TransType, Ac_Type, Script_ID, Qty, G_Rate, Brok_Amt,"
                            + " ServTax_Amount, Rate, Amount, MemberID, FinancialYearMemberID, Consultant_ID, EntrySource,Brok_PerUnit, MonthYear, Last_Date, LotSize, LotQty,"
                            + " isFNOBill, Strike_Price, OptionType, isstock,FrmNm)values('" + vchno + "', '" + srnr + "', '" + txtBillNo.Text + "', '" + Trasndate
                            + "', '" + Dt_Details.Rows[counter1]["Type"].ToString() + "', '', '" + sccd + "', '"
                            + Dt_Details.Rows[counter1]["Qty"].ToString() + "', '" + Dt_Details.Rows[counter1]["GrossRate"].ToString()
                            + "', '" + Dt_Details.Rows[counter1]["BrokAmt"].ToString() + "', '0', '" + Dt_Details.Rows[counter1]["Rate"].ToString()
                            + "', '" + Dt_Details.Rows[counter1]["Amount"].ToString() + "', '" + MenuMaster.member_code + "', '" + MenuMaster.yearcode + "', '"
                            + ccd + "', '0', '" + bkrch + "','" + expmonth + "','" + expdt + "','" + Dt_Details.Rows[counter1]["Lott"].ToString() + "','"
                            + Dt_Details.Rows[counter1]["LotQty"].ToString() + "','" + invstyp + "','"
                            + Dt_Details.Rows[counter1]["StrikePrice"].ToString() + "','" + optyp + "','" + sttyp + "','BB')", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();
                        if (_GlobalFormateNo == 9003)
                        {
                            string st = Dt_Details.Rows[counter1]["StockType"].ToString();
                            if (st.Contains("CF"))
                            {
                                cmd = new SqlCommand("Insert Into BFCFCLOSING(Script_code,Script_name,Type,Stock_Type,Expiry_Date,Qty,Trans_Date,closing_rate,Format_No,Member_code,Year_code,Trans_No)"
                                              + "Values('" + sccd + "','" + Dt_Details.Rows[counter1]["ScriptName"].ToString() + "','" + Dt_Details.Rows[counter1]["Type"].ToString() + "','" + Dt_Details.Rows[counter1]["StockType"].ToString() + "','"
                                              + expdt + "','" + Dt_Details.Rows[counter1]["Qty"].ToString() + "','" + Trasndate + "','" + Dt_Details.Rows[counter1]["GrossRate"].ToString() + "','" + _GlobalFormateNo + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + vchno + "')", con)
                                {
                                    Transaction = transaction
                                };
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    transaction.Commit();
                    //dr2.Close();
                    // drcd.Close();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
        }
        public void SaveFNOOLD()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();

            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            FilesTans dataobj = new FilesTans();
            clsbbm MenuMaster = new clsbbm();
            string invstyp = data.invstyp;
            MenuMaster.member_code = MemberCode.ToString();
            MenuMaster.yearcode = FinancialYearCode.ToString();
            DataTable Dt_Header = Session["Dt_Header"] as DataTable;
            DataTable Dt_Details = Session["Dt_Details"] as DataTable;
            DataTable Dt_Expanses = Session["Dt_Expanses"] as DataTable;
            DataTable Dt_RunningBalance = Session["Dt_RunningBalance"] as DataTable;
            txtExpenses.Text = Dt_RunningBalance.Rows[0]["txtExpenses"].ToString();
            txtGrossNetAmt.Text = Dt_RunningBalance.Rows[0]["txtGrossNetAmt"].ToString();
            txtBillNo.Text = Dt_Header.Rows[0]["BillNo"].ToString();
            txtSettlementNo.Text = Dt_Header.Rows[0]["SettlementNo"].ToString();
            txtBrokerage.Text = Dt_RunningBalance.Rows[0]["txtBrokerage"].ToString();
            txtNetAmt.Text = Dt_RunningBalance.Rows[0]["txtNetAmt"].ToString();
            txtInvestment.Text = Dt_RunningBalance.Rows[0]["txtInvestment"].ToString();
            textBox5.Text = Dt_RunningBalance.Rows[0]["textBox5"].ToString();
            txtRemarks.Text = "";
            _GlobalFormateNo = Convert.ToInt32(data.ContractNoteId);
            transnobb.Text = "118";
            txtBroker.Text = data.Broker_Name.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            con.Open();
            SqlTransaction transaction;
            cmd.CommandTimeout = 0;
            transaction = con.BeginTransaction("SampleTransaction");
            flag = 1;


            try
            {
                if (Validate_FNO())
                {
                    int vchno = 0;
                    if (flag == 1)
                    {
                        string bktyp = "BB";
                        vchno = GetGlobalTransactionNumber(bktyp);
                    }
                    else if (flag == 2)
                    {
                        vchno = Convert.ToInt32(transnobb.Text.Trim());
                        cmd = new SqlCommand("delete from  BrokerBill where Trans_no='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code
                            + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  BrokerBill_Trans where Trans_No='" + transnobb.Text.Trim() + "' and MemberID='" + MenuMaster.member_code
                            + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("delete from  AC_Trans where Trans_No='" + transnobb.Text.Trim() + "'  and MemberID='" + MenuMaster.member_code
                            + "' and FinancialYearMemberID='" + MenuMaster.yearcode + "'", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();
                    }
                    string theDate1 = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                    //cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where Ac_Name='" + txtBroker.Text + "' and  (Member_Code='"
                    //    + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                    //{
                    //    Transaction = transaction
                    //};
                    cmd = new SqlCommand("SELECT CAST(AccountId AS NVARCHAR) AS AccountId FROM M_Account where  (MemberID="
                      + MenuMaster.member_code + ") and (FinancialYearMemberID=" + MenuMaster.yearcode + ")", con)
                    {
                        Transaction = transaction
                    };
                    SqlDataReader dr2 = cmd.ExecuteReader();
                    dr2.Read();
                    string oc2 = dr2.GetString(0);
                    dr2.Close();

                    double AmountBrought = 0;
                    double Amountsold = 0;
                    string narr0 = txtBillNo.Text.ToString();
                    string narrr = txtSettlementNo.Text.ToString();
                    string narr1 = "BILL NO: ";
                    string narr2 = "Sett.NO: ";
                    string nartn1 = narr1 + narr0;
                    string nartn2 = narr2 + narrr;
                    string drorcr = "";
                    double amtbfacI = 0.0;
                    int nre;
                    double amtsfacI = 0.0;

                    for (int counter = 0; counter < (Dt_Details.Rows.Count); counter++)
                    {
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Bought")
                            amtbfacI = Math.Round(amtbfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (Dt_Details.Rows[counter]["Type"].ToString() == "Sold")
                            amtsfacI = Math.Round(amtsfacI + Math.Round(double.Parse(Dt_Details.Rows[counter]["Amount"].ToString()), decmlpl), decmlpl);
                        if (amtbfacI > amtsfacI)
                            AmountBrought = AmountBrought + (amtbfacI - amtsfacI);
                        else
                            Amountsold = Amountsold + (amtbfacI - amtsfacI);
                        amtsfacI = 0;
                        amtbfacI = 0;
                    }
                    double Amount = (AmountBrought - (Math.Abs(Amountsold)));
                    if (Amount > 0)
                        drorcr = "C";
                    else
                        drorcr = "D";

                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                        drorcr = "C";

                    int trsrno = 1;
                    string drcd = "";
                    string crcd = "";

                    cmd = new SqlCommand("INSERT INTO BrokerBill(Trans_no,Bill_no,Trans_Dt,Valan_code,MemberID,Broker_ID,Ac_Type,Brok_Rate,Brok_Amt,Amount,"
                        + " Stt_amt,ServTax_Amt,Other_amt,Net_amt,Book_Type,FinancialYearMemberID,Ref_TransNo,isInternalEntry,isFNOBill,Narrat1)values('" + vchno + "','"
                        + txtBillNo.Text + "','" + theDate1 + "','" + txtSettlementNo.Text + "','" + MenuMaster.member_code + "','" + oc2 + "','" + drorcr + "','0','"
                        + (Math.Abs(Convert.ToDouble(txtBrokerage.Text))) + "','" + (Math.Abs(Convert.ToDouble(txtGrossNetAmt.Text))) + "','0','0','"
                        + (Math.Abs(Convert.ToDouble(txtExpenses.Text))) + "','" + (Math.Abs(Convert.ToDouble(txtNetAmt.Text))) + "','BB','" + MenuMaster.yearcode + "','','N','"
                        + invstyp + "','" + txtRemarks.Text + "');SELECT SCOPE_IDENTITY();", con)
                    {
                        Transaction = transaction
                    };

                    cmd.ExecuteNonQuery();
                    // label3.Refresh();
                    if (Convert.ToDouble(Math.Round(Amount)) > 0)
                    {
                        drcd = fnoshrtrd;
                        crcd = oc2;
                    }
                    else
                    {
                        drcd = oc2;
                        crcd = fnoshrtrd;
                    }

                    if (Convert.ToDouble(txtExpenses.Text) > Convert.ToDouble(txtGrossNetAmt.Text))
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_Code] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]  ) VALUES('" + vchno + "','"
                            + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','"
                            + drcd + "','" + crcd + "','BB','" + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }
                    else
                    {
                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                            + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[frmpg],[EntryType]  ) VALUES('" + vchno + "','" + trsrno + "','"
                            + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + txtBillNo.Text + "','" + drcd + "','" + crcd + "','BB','"
                            + nartn1 + "','" + nartn2 + "','N','" + Math.Abs(Amount) + "','" + invstyp + "','BB','0')", con)
                        {
                            Transaction = transaction
                        };
                        nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                            trsrno = trsrno + 1;
                    }

                    for (int counter1 = 0; counter1 < (Dt_Expanses.Rows.Count); counter1++)
                    {
                        if (Dt_Expanses.Rows[counter1]["ExpAmount"] != null)
                        {
                            double otrxpc = (double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()));
                            if (otrxpc > 0)
                            {
                                cmd = new SqlCommand("SELECT AC_Code FROM Mst_Account where AC_name='" + Dt_Expanses.Rows[counter1]["expnm"].ToString() + "' and (Member_Code='' or Member_Code='" + MenuMaster.member_code + "') and (Year_Code='' or Year_Code='" + MenuMaster.yearcode + "')", con)
                                {
                                    Transaction = transaction
                                };
                                SqlDataReader dr4034 = cmd.ExecuteReader();
                                dr4034.Read();

                                string accttyypp5 = dr4034.GetString(0);
                                dr4034.Close();

                                string selextype = "";
                                if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Add")
                                    selextype = "A";
                                if (Dt_Expanses.Rows[counter1]["extype"].ToString() == "Less")
                                    selextype = "L";
                                int nre2 = 0;

                                if (selextype == "A") // If Expense Type Add
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,[Cr_AccountId] ,"
                                        + " [Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],"
                                        + " [EntryType] ) VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode
                                        + "','" + txtBillNo.Text + "','" + accttyypp5 + "','" + oc2 + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()), decmlpl) + "','"
                                        + invstyp + "','" + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }
                                else  // if Expanse type Less
                                {
                                    cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[MemberID],[FinancialYearMemberID],[Vouch_No],[Dr_AccountId] ,"
                                        + " [Cr_AccountId] ,[Book_Type1],[Narr1] ,[Narr2] ,[Internal],[Amount],[InvestmentType],[ExpenseType],[frmpg],"
                                        + " [EntryType] ) VALUES('" + vchno + "','" + trsrno + "','" + theDate1 + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode
                                        + "','" + txtBillNo.Text + "','" + oc2 + "','" + accttyypp5 + "','BB','" + nartn1 + "','" + nartn2 + "','N','"
                                        + Math.Round(double.Parse(Dt_Expanses.Rows[counter1]["ExpAmount"].ToString()), decmlpl) + "','" + invstyp + "','"
                                        + selextype + "','BB','10')", con)
                                    {
                                        Transaction = transaction
                                    };
                                    nre2 = cmd.ExecuteNonQuery();
                                }

                                if (nre2 > 0)
                                    trsrno = trsrno + 1;
                            }
                        }
                    }

                    for (int counter1 = 0; counter1 < (Dt_Details.Rows.Count); counter1++)
                    {
                        double bkrch = Math.Round(double.Parse(Dt_Details.Rows[counter1]["BrokAmt"].ToString()) / double.Parse(Dt_Details.Rows[counter1]["Qty"].ToString()), decmlpl);
                        string expdt = "";
                        string expmonth = "";

                        if (Dt_Details.Rows[counter1]["ExpiryDate"] != null)
                            expdt = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");

                        string sccd = Dt_Details.Rows[counter1]["ScriptCode"].ToString();
                        string ccd = Dt_Details.Rows[counter1]["ConsultantCode"].ToString();

                        if (Dt_Details.Rows[counter1]["ExpiryDate"] != null)
                            expmonth = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("MMMM-yyyy");

                        var srnr = counter1 + 1;
                        string optyp = "";
                        string sttyp = "";

                        if (Dt_Details.Rows[counter1]["OptionType"] != null)
                            optyp = Dt_Details.Rows[counter1]["OptionType"].ToString();

                        if (Dt_Details.Rows[counter1]["StockType"] != null)
                            sttyp = Dt_Details.Rows[counter1]["StockType"].ToString();

                        if (Purchasedate == 1)
                            Trasndate = Convert.ToDateTime(Dt_Details.Rows[counter1]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                        else
                        {
                            Trasndate = Convert.ToDateTime(data.Date).ToString("yyyy-MM-dd");
                        }
                        cmd = new SqlCommand("Insert into BrokerBill_Trans(Trans_No, Sr_No, Bill_no, Trans_Dt, TransType, Ac_Type, Script_ID, Qty, G_Rate, Brok_Amt,"
                            + " ServTax_Amount, Rate, Amount, MemberID, FinancialYearMemberID, Consultant_ID, EntrySource,Brok_PerUnit, MonthYear, Last_Date, LotSize, LotQty,"
                            + " isFNOBill, Strike_Price, OptionType, isstock,FrmNm)values('" + vchno + "', '" + srnr + "', '" + txtBillNo.Text + "', '" + Trasndate
                            + "', '" + Dt_Details.Rows[counter1]["Type"].ToString() + "', '', '" + sccd + "', '"
                            + Dt_Details.Rows[counter1]["Qty"].ToString() + "', '" + Dt_Details.Rows[counter1]["GrossRate"].ToString()
                            + "', '" + Dt_Details.Rows[counter1]["BrokAmt"].ToString() + "', '0', '" + Dt_Details.Rows[counter1]["Rate"].ToString()
                            + "', '" + Dt_Details.Rows[counter1]["Amount"].ToString() + "', '" + MenuMaster.member_code + "', '" + MenuMaster.yearcode + "', '"
                            + ccd + "', '0', '" + bkrch + "','" + expmonth + "','" + expdt + "','" + Dt_Details.Rows[counter1]["Lott"].ToString() + "','"
                            + Dt_Details.Rows[counter1]["LotQty"].ToString() + "','" + invstyp + "','"
                            + Dt_Details.Rows[counter1]["StrikePrice"].ToString() + "','" + optyp + "','" + sttyp + "','BB')", con)
                        {
                            Transaction = transaction
                        };
                        cmd.ExecuteNonQuery();
                        // added by latika as on 01.11.2020
                        if (_GlobalFormateNo == 9003)
                        {
                            string st = Dt_Details.Rows[counter1]["StockType"].ToString();
                            if (st.Contains("CF"))
                            {
                                cmd = new SqlCommand("Insert Into BFCFCLOSING(Script_ID,Script_Name,Type,Stock_Type,Expiry_Date,Qty,Trans_Date,closing_rate,Format_No,MemberID,FinancialYearMemberID,Trans_No)"
                                              + "Values('" + sccd + "','" + Dt_Details.Rows[counter1]["ScriptName"].ToString() + "','" + Dt_Details.Rows[counter1]["Type"].ToString() + "','" + Dt_Details.Rows[counter1]["StockType"].ToString() + "','"
                                              + expdt + "','" + Dt_Details.Rows[counter1]["Qty"].ToString() + "','" + Trasndate + "','" + Dt_Details.Rows[counter1]["GrossRate"].ToString() + "','" + _GlobalFormateNo + "','" + MenuMaster.member_code + "','" + MenuMaster.yearcode + "','" + vchno + "')", con)
                                {
                                    Transaction = transaction
                                };
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //end here added by latika as on 01.11.2020

                    }

                    //  MessageBox.Show("Records Saved Successfully");
                    transaction.Commit();
                    //btnf.btnfunc(btnAdd, btnEdit, btnDelete, btnSave, btncanel, btnSearch, btnprint, btnclose2);
                    //Disable();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                //  MessageBox.Show(ex.Message);
                //ErrorLogging.WriteToErrorLog(ex.Message, ex.StackTrace, ex.GetType().ToString(), ClsCommon.GlobalVar);
                //MessageBox.Show("Kindly Contact Support Team!!!!");

            }
        }
        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
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
                    dict[col.ColumnName] = (row[col]);
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(list);
        }

        public void InsertScriptMapping(string MapScript_ID,string Script_Name)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();


            IRecordweb.ViewModel.BREntryModel data = Session["SaveBREntry"] as IRecordweb.ViewModel.BREntryModel;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_M_Member_Broker_Script_Master]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = "0";
            cmd.Parameters.Add("@MemberID", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@Member_Broker_ID", SqlDbType.NVarChar).Value = data.Broker_Id;
            cmd.Parameters.Add("@Script_ID", SqlDbType.NVarChar).Value = MapScript_ID;
            cmd.Parameters.Add("@InvestmentType", SqlDbType.SmallInt).Value = data.invstyp;
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

    }

}