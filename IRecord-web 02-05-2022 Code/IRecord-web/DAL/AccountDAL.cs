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
     public class AccountDAL
        {
        public List<AccountMaster> AllAccountList()
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
                    Name = ds.Tables[0].Rows[i]["Name"].ToString(),
                    GroupName = ds.Tables[0].Rows[i]["Group_Name"].ToString(),
                    OpeningBalance = Convert.ToDouble(ds.Tables[0].Rows[i]["OpeningBalance"].ToString()),
                    OpeningCal = ds.Tables[0].Rows[i]["OpeningCal"].ToString(),
                    Contactperson = ds.Tables[0].Rows[i]["Contactperson"].ToString(),
                    Address = ds.Tables[0].Rows[i]["Address"].ToString(),
                    Mobile = ds.Tables[0].Rows[i]["Mobile"].ToString(),
                    Telephone = ds.Tables[0].Rows[i]["Telephone"].ToString(),
                    Emailid = ds.Tables[0].Rows[i]["Emailid"].ToString(),
                    PAN = ds.Tables[0].Rows[i]["PAN"].ToString(),
                    GSTIN = ds.Tables[0].Rows[i]["GSTIN"].ToString(),
                    AadharCardNo = ds.Tables[0].Rows[i]["AadharCardNo"].ToString(),
                    Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString()),
                    CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString()),
                    InternalEntry = Convert.ToInt32(ds.Tables[0].Rows[i]["InternalEntry"].ToString())
                });
                }
            return AllAccountList;
            }
        public List<AccountMaster> AccountList()
            {

            List<AccountMaster> AccountList = new List<AccountMaster>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAccountList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            AccountMaster cobj = new AccountMaster();
            if (ds.Tables[0].Rows.Count > 0)
                {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                    AccountList.Add(new AccountMaster()
                        {
                        GroupID = Convert.ToInt32(ds.Tables[0].Rows[i]["GroupID"].ToString()),
                        GroupName = ds.Tables[0].Rows[i]["Group_Name"].ToString()

                        });
                    }
                }
            return AccountList;
            }
        public void InsertAccountMaster(AccountMaster _Account, int Flag)
            {
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinincialYearID"];
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Ac_Code", _Account.Code);
            Adapter.AddParam(pcol, "@Name", _Account.Name);
            Adapter.AddParam(pcol, "@MemberId", _Account.MemberID);
            Adapter.AddParam(pcol, "@FinancialYearMemberID", _Account.FinancialYearMemberID);
            Adapter.AddParam(pcol, "@Groupid", _Account.GroupID);
            Adapter.AddParam(pcol, "@OpeningBalance", _Account.OpeningBalance);
            Adapter.AddParam(pcol, "@OpeningCal", _Account.OpeningCal);
            Adapter.AddParam(pcol, "@Contactperson", _Account.Contactperson);
            Adapter.AddParam(pcol, "@Address", _Account.Address);
            Adapter.AddParam(pcol, "@Mobile", _Account.Mobile);
            Adapter.AddParam(pcol, "@Telephone", _Account.Telephone);
            Adapter.AddParam(pcol, "@Emailid", _Account.Emailid);
            Adapter.AddParam(pcol, "@PAN", _Account.PAN);
            Adapter.AddParam(pcol, "@GSTIN", _Account.GSTIN);
            Adapter.AddParam(pcol, "@AadharCardNo", _Account.AadharCardNo);
            Adapter.AddParam(pcol, "@Active", _Account.Active);
            Adapter.AddParam(pcol, "@CreatedBy", _Account.CreatedBy);
            Adapter.AddParam(pcol, "@Flag", Flag);
            Adapter.ExecutenNonQuery("USPAccountInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }
        public List<AccountMaster> AccountListCheck()
            {
            string count = "";
            List<AccountMaster> AccountList = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAccountMaster", Adapter.Connection);
            // com.Parameters.Add(new SqlParameter("@Name", _Account.Name));
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            AccountList = new List<AccountMaster>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                AccountMaster cobj = new AccountMaster();
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Accountid = Convert.ToInt32(ds.Tables[0].Rows[i]["Accountid"].ToString());
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                AccountList.Add(cobj);
                }
            return AccountList;
            }
        }
    }
