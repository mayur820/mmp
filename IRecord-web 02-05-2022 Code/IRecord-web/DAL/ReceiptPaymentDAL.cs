using BAL;
using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
    {
   public class ReceiptPaymentDAL
        {
        public List<Narration> GetNarrationList()
            {

            List<Narration> NarrationList = new List<Narration>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetNarrationListMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Narration cobj = new Narration();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                NarrationList.Add(new Narration()
                    {
                    Code = ds.Tables[0].Rows[i]["Narrationid"].ToString(),
                    Name = ds.Tables[0].Rows[i]["Name"].ToString()

                    });
                }
            return NarrationList;
            }
        public List<SaveReceiptPaymentEntry> TypeList()
            {

            List<SaveReceiptPaymentEntry> TypeList = new List<SaveReceiptPaymentEntry>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetReceiptTypeList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            SaveReceiptPaymentEntry cobj = new SaveReceiptPaymentEntry();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                TypeList.Add(new SaveReceiptPaymentEntry()
                    {
                    typeid = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString()),
                    CashBank = ds.Tables[0].Rows[i]["Name"].ToString()

                    });
                }
            return TypeList;
            }
        public List<SaveReceiptPaymentEntry> ModeList()
            {

            List<SaveReceiptPaymentEntry> ModeList = new List<SaveReceiptPaymentEntry>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetReceiptModeList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            SaveReceiptPaymentEntry cobj = new SaveReceiptPaymentEntry();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                ModeList.Add(new SaveReceiptPaymentEntry()
                    {
                    typeid = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString()),
                    Mode = ds.Tables[0].Rows[i]["Name"].ToString()

                    });
                }
            return ModeList;
            }
        public List<AccountMaster> GetAllAccountList()
            {
            List<AccountMaster> AllAccountList = new List<AccountMaster>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllAccountList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            AccountMaster cobj = new AccountMaster();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                AllAccountList.Add(new AccountMaster()
                    {
                    Code = Convert.ToInt32(ds.Tables[0].Rows[i]["accountid"].ToString()),
                    Name = ds.Tables[0].Rows[i]["Name"].ToString()
                    });
                }
            return AllAccountList;
            }
        public List<AccountMaster> GetAllCashBankAccountList(SaveReceiptPaymentEntry _Group)
            {
            string CashBank = _Group.CashBank;
            if (CashBank == "20")
                {
                CashBank = "Bank";
                }
            else if (CashBank == "15")
                {
                CashBank = "Cash";
                }
            List<AccountMaster> CashAccountList = new List<AccountMaster>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllCashBankAccountList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CashBank", CashBank);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            AccountMaster cobj = new AccountMaster();
            if (ds.Tables[0].Rows.Count > 0)
                {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                    CashAccountList.Add(new AccountMaster()
                        {
                        Code = Convert.ToInt32(ds.Tables[0].Rows[i]["accountid"].ToString()),
                        Name = ds.Tables[0].Rows[i]["name"].ToString()
                        });
                    }
                }
            return CashAccountList;
            }
        public List<SaveReceiptPaymentEntry> GetPaymodeList(SaveReceiptPaymentEntry _Group1)
            {
            string CashBank = _Group1.CashBank;
            int Flag = 0;
            if (CashBank == "20")
                {
                CashBank = "Bank";
                Flag = 1;
                }
            else if (CashBank == "15")
                {
                CashBank = "Cash";
                Flag = 2;
                }
            List<SaveReceiptPaymentEntry> PaymodeList = new List<SaveReceiptPaymentEntry>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetPayModeList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CashBank", CashBank);
            com.Parameters.AddWithValue("@Flag", Flag);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            SaveReceiptPaymentEntry cobj = new SaveReceiptPaymentEntry();
            if (ds.Tables[0].Rows.Count > 0)
                {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                    PaymodeList.Add(new SaveReceiptPaymentEntry()
                        {
                        code = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString()),
                        Name = ds.Tables[0].Rows[i]["Name"].ToString()
                        });
                    }
                }
            return PaymodeList;
            }
        public void InsertReceiptvoucher(SaveReceiptPaymentEntry _Receipt)
            {
            string narr = string.Join(",", _Receipt.narration);
            string cheqrefer = "";
            if (!string.IsNullOrEmpty(_Receipt.Cheque) || _Receipt.Cheque != null)
                {
                cheqrefer = _Receipt.Cheque;
                }
            else if (!string.IsNullOrEmpty(_Receipt.refernce) || _Receipt.refernce != null)
                {
                cheqrefer = _Receipt.refernce;
                }
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearID = HttpContext.Current.Session["FinincialYearID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinancialYearMemberID"];
            var CreatedBy = HttpContext.Current.Session["UserID"];
            HttpContext.Current.Session["TransactionID"] = _Receipt.TransactionId;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.AddParam(pcol, "@FinancialYearMemberID", FinancialYearMemberID);
            Adapter.AddParam(pcol, "@TransactionDate", _Receipt.Date);
            Adapter.AddParam(pcol, "@Type", _Receipt.CashBank);
            Adapter.AddParam(pcol, "@Mode", _Receipt.Mode);
            Adapter.AddParam(pcol, "@PayMode", _Receipt.PaymentMode);
            Adapter.AddParam(pcol, "@cheque", cheqrefer);
            Adapter.AddParam(pcol, "@DebitCode", _Receipt.cashbankaccount);
            Adapter.AddParam(pcol, "@CreditCode", _Receipt.AmountAccount);
            Adapter.AddParam(pcol, "@Narration1", narr);
            Adapter.AddParam(pcol, "@BorkerType", "Bk");
            Adapter.AddParam(pcol, "@Amount", _Receipt.amount);
            Adapter.AddParam(pcol, "@Active", 1);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPReceiptEntryInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }
        }
    }
