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
  public  class JournalVoucherDAL
        {
        public List<Group> GroupList()
            {

            List<Group> UnderGroupList = new List<Group>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetGroupListMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Group cobj = new Group();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                UnderGroupList.Add(new Group()
                    {
                    UGroupID = Convert.ToInt32(ds.Tables[0].Rows[i]["UGroupID"].ToString()),
                    UGroupName = ds.Tables[0].Rows[i]["UGroupName"].ToString()

                    });
                }
            return UnderGroupList;
            }

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
        public void InsertJournalvoucher(JournalVoucherEntry _Journal)
            {
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearID = HttpContext.Current.Session["FinincialYearID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinancialYearMemberID"];

            var CreatedBy = HttpContext.Current.Session["UserID"];
            HttpContext.Current.Session["TransactionID"] = _Journal.TransactionId;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.AddParam(pcol, "@FinancialYearMemberID", FinancialYearMemberID);
            Adapter.AddParam(pcol, "@TransactionDate", _Journal.Date);
            Adapter.AddParam(pcol, "@DebitCode", _Journal.DebitAccount);
            Adapter.AddParam(pcol, "@CreditCode", _Journal.CreditAccount);
            Adapter.AddParam(pcol, "@Narration1", _Journal.Narration);
            Adapter.AddParam(pcol, "@BorkerType", "JV");
            Adapter.AddParam(pcol, "@Amount", _Journal.Amount);
            Adapter.AddParam(pcol, "@Active", 1);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPJournalTransInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));


            }
        public List<AccountMaster> JournalAccountList(Group _Group)
            {
            int CashBank = _Group.UGroupID;
            if (CashBank == 20)
                {
                CashBank = 7;
                }
            else if (CashBank == 15)
                {
                CashBank = 7;
                }
            else
                {
                CashBank = _Group.UGroupID;
                }

            List<AccountMaster> JournalAccountList = new List<AccountMaster>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetJournalAccountList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@GroupID", CashBank);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            AccountMaster cobj = new AccountMaster();
            if (ds.Tables[0].Rows.Count > 0)
                {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                    JournalAccountList.Add(new AccountMaster()
                        {
                        Code = Convert.ToInt32(ds.Tables[0].Rows[i]["accountid"].ToString()),
                        Name = ds.Tables[0].Rows[i]["name"].ToString()
                        });
                    }
                }
            return JournalAccountList;
            }
        }
    }
