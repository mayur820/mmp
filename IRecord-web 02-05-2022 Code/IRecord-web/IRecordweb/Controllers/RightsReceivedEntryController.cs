using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class RightsReceivedEntryController : Controller
    {
        // GET: RightsReceivedEntry
        public ActionResult Index()
        {
            //string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            //List<IPO> list = new List<IPO>();
            //using (SqlConnection con = new SqlConnection(strConnString))
            //{
            //    string query = "Select Top 1 Trans_no,Application_no,Qty,Rate,Amount from [IPO] Order by Trans_no Desc";
            //    using (SqlCommand cmd = new SqlCommand(query))
            //    {
            //        cmd.Connection = con;
            //        con.Open();
            //        using (SqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            while (sdr.Read())
            //            {
            //                IPO item1 = new IPO();
            //                item1.ApplicationNo = Convert.ToString(sdr["Application_no"]);
            //                item1.TransactionId = Convert.ToInt32(sdr["Trans_no"]);
            //                item1.Qty = Convert.ToInt32(sdr["Qty"]);
            //                item1.Rate = Convert.ToInt64(sdr["Rate"]);
            //                item1.Amount = Convert.ToInt32(sdr["Amount"]);
            //                list.Add(item1);
            //            }
            //        }
            //        con.Close();
            //    }
            //}
            //IPO _ipo = new IPO();
            //_ipo.Showipo = list;
            return View();
        }
        public int GetGlobalTransactionNumber(string bktyp)
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
        public JsonResult GetTranstionNO()
        {

            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            return Json(GetGlobalTransactionNumber("Rights"), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetScpirt()
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
                CommandText = "[dbo].[SP_CADetails]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ViewForRightsReceived";
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
        public JsonResult FillBank()
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
                CommandText = "[dbo].[Sp_Rights_Received]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "ViewBank";
            cmd.Parameters.Add("@Var1", SqlDbType.NVarChar).Value = MemberCode;
            cmd.Parameters.Add("@Var2", SqlDbType.NVarChar).Value = FinancialYearCode;

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
        public JsonResult GetTableInfo()
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
                CommandText = "[dbo].[Sp_Rights_Received]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "View";
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

        public JsonResult GetTableData()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[dbo].[Sp_Bonus_Rights_Div_savedata]"
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


        [HttpPost]
        public JsonResult SaveDb(string JsonHead, string Deatils)
        {

            //string Trans_No = GetGlobalTransactionNumber("JV").ToString();
            DataTable dtJsonHead = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonHead);
            DataTable dtDeatils = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Deatils);
            SavedatainBonus_Rights_Div(dtJsonHead);
            SavedatainBrokerBill(dtJsonHead);
            SavedatainBrokerBill_Trans(dtJsonHead);
            SavedatainBrokerBill_DmatInfo(dtJsonHead, dtDeatils);
            if (dtJsonHead.Columns.Contains("txtPDate"))
            {
                SavedatainBrokerAC_Trans(dtJsonHead);
            }

            return Json("1", JsonRequestBehavior.AllowGet);

        }
        public void SavedatainBonus_Rights_Div(DataTable dt)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_insert_RightsReceivedEntry_Bonus_Rights_Div]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = dt.Rows[0]["TransactionNo"].ToString();
            cmd.Parameters.Add("@Entry_type", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@Script_code", SqlDbType.VarChar).Value = dt.Rows[0]["Script_id"].ToString();
            cmd.Parameters.Add("@Declared_Date", SqlDbType.VarChar).Value = Convert.ToDateTime(dt.Rows[0]["RecordDate"].ToString()).ToString("yyyy/MM/dd");
            cmd.Parameters.Add("@Record_Date", SqlDbType.VarChar).Value = Convert.ToDateTime(dt.Rows[0]["RecordDate"].ToString()).ToString("yyyy/MM/dd");

            cmd.Parameters.Add("@Bonus_Rights_qty", SqlDbType.Float).Value = dt.Rows[0]["Qty1"].ToString();
            cmd.Parameters.Add("@PerShare", SqlDbType.Float).Value = dt.Rows[0]["Qty2"].ToString();
            cmd.Parameters.Add("@PerShare_Rate", SqlDbType.Float).Value = dt.Rows[0]["ReteParShare"].ToString(); ;
            cmd.Parameters.Add("@Div_Rate", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Div_Type", SqlDbType.VarChar).Value = 0;
            cmd.Parameters.Add("@FromMonth", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@FromYear", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@ToMonth", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@ToYear", SqlDbType.SmallInt).Value = 0;
            cmd.Parameters.Add("@Member_Code", SqlDbType.VarChar).Value = MemberCode;
            cmd.Parameters.Add("@YearCode", SqlDbType.VarChar).Value = FinancialYearCode;
            cmd.Parameters.Add("@Additional_Qty", SqlDbType.Int).Value = (Convert.ToInt64(dt.Rows[0]["AdditionalQtyStockinTrade"].ToString()) + Convert.ToInt64(dt.Rows[0]["AdditionalQtyShareInvestment"].ToString()));
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
        public void SavedatainBrokerBill(DataTable dt)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_insert_RightsReceivedEntry_BrokerBill]"
            };

            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@Trans_no", SqlDbType.Int).Value = dt.Rows[0]["TransactionNo"].ToString();
            cmd.Parameters.Add("@Bill_no", SqlDbType.VarChar).Value = "RIGHTS";
            cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows[0]["RecordDate"].ToString()).ToString("yyyy/MM/dd"); ;
            cmd.Parameters.Add("@Valan_code", SqlDbType.VarChar).Value = "";
            cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
            cmd.Parameters.Add("@Broker_ID", SqlDbType.VarChar).Value = "0";
            cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = "D";
            cmd.Parameters.Add("@Brok_Rate", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Brok_Amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Stt_amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@ServTax_Amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Other_amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Net_amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Book_Type", SqlDbType.VarChar).Value = "RI";
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
            cmd.Parameters.Add("@Ref_TransNo", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@isInternalEntry", SqlDbType.VarChar).Value = "N";
            cmd.Parameters.Add("@isFNOBill", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@Narrat1", SqlDbType.VarChar).Value = "";

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
                // throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        public void SavedatainBrokerBill_Trans(DataTable dt)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_insert_RightsReceivedEntry_BrokerBill_Trans]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = dt.Rows[0]["TransactionNo"].ToString();
            cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@Bill_no", SqlDbType.VarChar).Value = "RIGHTS";
            cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows[0]["RecordDate"].ToString()).ToString("yyyy/MM/dd");
            cmd.Parameters.Add("@TransType", SqlDbType.VarChar).Value = "Bought";
            cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = "1";
            cmd.Parameters.Add("@Script_ID", SqlDbType.VarChar).Value = dt.Rows[0]["Script_id"].ToString();
            //cmd.Parameters.Add("@MonthYear", SqlDbType.VarChar).Value = yourCShap_variable;
            //cmd.Parameters.Add("@Last_Date", SqlDbType.DateTime).Value = yourCShap_variable;
            //cmd.Parameters.Add("@LotSize", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@LotQty", SqlDbType.Float).Value = yourCShap_variable;
            cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = (Convert.ToInt64(dt.Rows[0]["AdditionalQtyStockinTrade"].ToString()) + Convert.ToInt64(dt.Rows[0]["AdditionalQtyShareInvestment"].ToString()));
            // cmd.Parameters.Add("@Disp_qty", SqlDbType.Float).Value = yourCShap_variable;
            cmd.Parameters.Add("@Bal_qty", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@G_Rate", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Brok_Amt", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@ServTax_Amount", SqlDbType.Float).Value = 0;
            cmd.Parameters.Add("@Rate", SqlDbType.Float).Value = dt.Rows[0]["ReteParShare"].ToString(); ;
            cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = ((Convert.ToInt64(dt.Rows[0]["AdditionalQtyStockinTrade"].ToString()) + Convert.ToInt64(dt.Rows[0]["AdditionalQtyShareInvestment"].ToString())) * Convert.ToInt64(dt.Rows[0]["ReteParShare"].ToString())); ;
            //   cmd.Parameters.Add("@Stt_Amount", SqlDbType.Float).Value = yourCShap_variable;
            cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
            cmd.Parameters.Add("@Consultant_ID", SqlDbType.VarChar).Value = "0";
            //cmd.Parameters.Add("@EntrySource", SqlDbType.Int).Value = yourCShap_variable;
            //cmd.Parameters.Add("@Ref_TransNo", SqlDbType.Int).Value = yourCShap_variable;
            //cmd.Parameters.Add("@Ref_SrNo", SqlDbType.Int).Value = yourCShap_variable;
            //cmd.Parameters.Add("@MergedEntry", SqlDbType.Int).Value = yourCShap_variable;
            cmd.Parameters.Add("@isFNOBill", SqlDbType.Int).Value = 0;
            //cmd.Parameters.Add("@Strike_Price", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@OptionType", SqlDbType.VarChar).Value = yourCShap_variable;
            cmd.Parameters.Add("@isIntraDay", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@Brok_PerUnit", SqlDbType.Float).Value = 0;
            //  cmd.Parameters.Add("@lockedbymerger", SqlDbType.Int).Value = yourCShap_variable;
            cmd.Parameters.Add("@frac_Qty", SqlDbType.Float).Value = 0;
            //cmd.Parameters.Add("@Org_Trans_No", SqlDbType.Int).Value = yourCShap_variable;
            //cmd.Parameters.Add("@Org_SrNo", SqlDbType.Int).Value = yourCShap_variable;
            //cmd.Parameters.Add("@isDeleteMerger", SqlDbType.VarChar).Value = yourCShap_variable;
            //cmd.Parameters.Add("@isstock", SqlDbType.VarChar).Value = yourCShap_variable;
            //cmd.Parameters.Add("@ScripwiseExpense", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@isClient_code", SqlDbType.VarChar).Value = yourCShap_variable;
            //cmd.Parameters.Add("@isClient_Brokrage", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@c_Qty", SqlDbType.Float).Value = yourCShap_variable;
            cmd.Parameters.Add("@C_Gross_Rate", SqlDbType.Float).Value = 0;
            //cmd.Parameters.Add("@C_Net_Rate", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@C_Amount", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@C_STT", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@C_STax", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@C_MisChg", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@TransactionChg", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@C_StampDuty", SqlDbType.Float).Value = yourCShap_variable;
            cmd.Parameters.Add("@Exp_Rate", SqlDbType.Float).Value = 0;
            //cmd.Parameters.Add("@PerExp_Unit", SqlDbType.Float).Value = yourCShap_variable;
            //cmd.Parameters.Add("@FrmNm", SqlDbType.NVarChar).Value = yourCShap_variable;

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
                //throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public void SavedatainBrokerBill_DmatInfo(DataTable dtJsonHead, DataTable dtDeatils)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            int Sr_No = 1;
            foreach (DataRow DR in dtDeatils.Rows)
            {
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "[DBO].[SP_insert_RightsReceivedEntry_BrokerBill_DmatInfo]"
                };
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                cmd.Parameters.Add("@Trans_No", SqlDbType.Int).Value = dtJsonHead.Rows[0]["TransactionNo"].ToString();
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = Sr_No;
                cmd.Parameters.Add("@Local_SrNo", SqlDbType.Int).Value = Sr_No;
                cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(dtJsonHead.Rows[0]["RecordDate"].ToString()).ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
                cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
                cmd.Parameters.Add("@DemateID", SqlDbType.VarChar).Value = DR["DematAccountId"];
                cmd.Parameters.Add("@Script_ID", SqlDbType.VarChar).Value = dtJsonHead.Rows[0]["Script_id"].ToString();
                cmd.Parameters.Add("@Qty", SqlDbType.Float).Value = DR["ReceivedQty"];
                cmd.Parameters.Add("@Trans_Type", SqlDbType.VarChar).Value = "I";
                cmd.Parameters.Add("@internalEntry", SqlDbType.Int).Value = "0";
                //cmd.Parameters.Add("@Ref_Trans_No", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@Ref_Sr_No", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@entrySource", SqlDbType.Int).Value = yourCShap_variable;
                cmd.Parameters.Add("@Consultant_ID", SqlDbType.VarChar).Value = "0";//"Self Consultant A/c"
                //cmd.Parameters.Add("@Ref_Local_SrNo", SqlDbType.Int).Value = yourCShap_variable;
                //cmd.Parameters.Add("@MergedEntry", SqlDbType.Int).Value = yourCShap_variable;
                cmd.Parameters.Add("@Ac_Type", SqlDbType.VarChar).Value = "1";
                cmd.Parameters.Add("@frmpg", SqlDbType.VarChar).Value = "RI";
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
                Sr_No++;
            }


        }


        public void SavedatainBrokerAC_Trans(DataTable dt)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_insert_RightsReceivedEntry_AC_Trans]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";

            cmd.Parameters.Add("@Trans_No", SqlDbType.VarChar).Value = dt.Rows[0]["TransactionNo"].ToString(); ;
            cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@Trans_Dt", SqlDbType.DateTime).Value = Convert.ToDateTime(dt.Rows[0]["txtPDate"].ToString()).ToString("yyyy/MM/dd");
            cmd.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.VarChar).Value = FinancialYearCode;
            cmd.Parameters.Add("@Vouch_No", SqlDbType.VarChar).Value = "ABC";
            cmd.Parameters.Add("@Chq_No", SqlDbType.VarChar).Value = dt.Rows[0]["txt_reference"].ToString();
            //cmd.Parameters.Add("@Dr_AccountId", SqlDbType.VarChar).Value = yourCShap_variable;
            //cmd.Parameters.Add("@Cr_AccountId", SqlDbType.VarChar).Value = yourCShap_variable;
            cmd.Parameters.Add("@Sub_Type1", SqlDbType.VarChar).Value = "B";
            cmd.Parameters.Add("@Sub_Type2", SqlDbType.VarChar).Value = "P";
            cmd.Parameters.Add("@Book_Type1", SqlDbType.VarChar).Value = "BK";
            //cmd.Parameters.Add("@Narr1", SqlDbType.VarChar).Value = yourCShap_variable;
            //cmd.Parameters.Add("@Narr2", SqlDbType.VarChar).Value = yourCShap_variable;
            //cmd.Parameters.Add("@Narr3", SqlDbType.VarChar).Value = yourCShap_variable;
            cmd.Parameters.Add("@Internal", SqlDbType.VarChar).Value = "Y";
            cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = dt.Rows[0]["txtAmount"].ToString();
            cmd.Parameters.Add("@InvestmentType", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@EntryType", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@PayAgtBill", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@Reco", SqlDbType.Int).Value = 0;

            cmd.Parameters.Add("@isImported", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@ExpenseType", SqlDbType.VarChar).Value = 0;
            cmd.Parameters.Add("@IsMoneyBack", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@IsInsurance", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@IsDisplayInAc", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@frmpg", SqlDbType.VarChar).Value = "Rights";
            cmd.Parameters.Add("@pay_mode", SqlDbType.Int).Value = dt.Rows[0]["ddlPaymentMode"].ToString();
            cmd.Parameters.Add("@IsTallyTransfer", SqlDbType.Bit).Value = 0;

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
    }
}