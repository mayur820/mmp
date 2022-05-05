using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using BAL;
using System.Data;
using System.Data.SqlClient;
using IrecordDAL;
using System.Net.Http;
using System.Web.SessionState;
using System.Web.Security;
using System.Net;
using System.IO;
using LinqToExcel;
using System.Data.OleDb;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Data.Common;
using IRecordweb.Models;

//using IRecordweb.Models;

namespace DAL
    {
    public class Master
        {
        //public object ViewBag { get; private set; }

        //To Insert Subscriber Master data
        public Subscriber InsertSubscriberMaster(Subscriber _Subscriber)
            {
            Subscriber obj = new Subscriber();
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var RoleId = HttpContext.Current.Session["RoleId"];
            HttpContext.Current.Session["SubscriptionLimits"] = _Subscriber.MemberSubscription;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            // Adapter.AddParam(pcol, "@CountryId", _CompanyObj.CountryId);
            Adapter.AddParam(pcol, "@SubscriberName", _Subscriber.SubscriberName);
            Adapter.AddParam(pcol, "@EmailID", _Subscriber.EmailID);
            Adapter.AddParam(pcol, "@MobileNo", _Subscriber.MobileNo);
            Adapter.AddParam(pcol, "@MobileOTP", _Subscriber.MobileOTP);
            Adapter.AddParam(pcol, "@EmailOTP", _Subscriber.EmailOTP);
            Adapter.AddParam(pcol, "@RoleId", _Subscriber.RoleId);
            Adapter.AddParam(pcol, "@MemberSubscription", _Subscriber.MemberSubscription);
            Adapter.AddParam(pcol, "@Active", _Subscriber.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            //   Adapter.ExecutenNonQuery("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            dt = Adapter.ExecuteDataTable("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt.Rows.Count > 0)
                {
                obj.Code = dt.Rows[0]["Code"].ToString();
                obj.Message = dt.Rows[0]["Message"].ToString();
                if (obj.Code == "200")
                    {
                    obj.SubscriberID = (Int32)dt.Rows[0]["SubscriberID"];
                    obj.MemberSubscription = (Int32)dt.Rows[0]["MemberSubscription"];
                    HttpContext.Current.Session["SubscriberID"] = dt.Rows[0]["SubscriberID"];
                    HttpContext.Current.Session["MemberSubscription"] = dt.Rows[0]["MemberSubscription"];
                    }

                }

            return obj;
            }
        public DataSet GetSubscriberMaster(Subscriber __subscriber)

            {
            //DataTable dt = new DataTable();
            //Adapter.ExecutenNonQuery("USPGetCompanyMaster", CommandType.StoredProcedure);
            SqlCommand com = new SqlCommand("USPGetSubscriberMaster", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);

            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;

            }
        //To Insert Family Master data
        public void InsertFamilyMaster(Family _Family)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Name", _Family.Name);
            Adapter.AddParam(pcol, "@Active", _Family.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            //  Adapter.ExecutenNonQuery("USPFamilyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            dt = Adapter.ExecuteDataTable("USPFamilyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt.Rows.Count > 0)
                {
                HttpContext.Current.Session["FamilyID"] = _Family.FamilyID;
                }

            }
        //To Insert Member Master data
        public void InsertMemberMaster(Member _Member, HttpPostedFileBase ReportLogoPath)
            {

            var ReportLogoPath1 = HttpContext.Current.Session["Path"].ToString();
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FamilyID", _Member.FamilyID);
            Adapter.AddParam(pcol, "@MemberName", _Member.MemberName);
            Adapter.AddParam(pcol, "@Address", _Member.Address);
            Adapter.AddParam(pcol, "@Gender", _Member.Gender);
            Adapter.AddParam(pcol, "@ServTax_No", _Member.ServTax_No);
            Adapter.AddParam(pcol, "@AadharCardNo", _Member.AadharCardNo);
            Adapter.AddParam(pcol, "@ReportLogoPath", ReportLogoPath1);
            //Adapter.AddParam(pcol, "@UserId", _Member.UserId);
            Adapter.AddParam(pcol, "@PAN", _Member.PAN);
            Adapter.AddParam(pcol, "@Active", _Member.Active);
            Adapter.AddParam(pcol, "@CreatedBy", HttpContext.Current.Session["UserID"]);
            dt = Adapter.ExecuteDataTable("USPMemberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            // dt = Adapter.ExecuteDataTable("USPMemberEntryInUser", CommandType.StoredProcedure, Adapter.param(pcol));

            //if (dt != null)
            //    {
            //    if (dt.Rows.Count > 0)
            //        {
            //        HttpContext.Current.Session["MemberID"] = dt.Rows[0]["MemberID"];
            //        }
            //    }
            }
        //To Update Member Master data
        public void UpdateMemberByID(Member _Member, HttpPostedFileBase ReportLogoPath, int MemberId)
            {

            var ReportLogoPath1 = HttpContext.Current.Session["Path"].ToString();
            var UserID = HttpContext.Current.Session["UserID"];
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FamilyID", _Member.FamilyID);
            Adapter.AddParam(pcol, "@MemberName", _Member.MemberName);
            Adapter.AddParam(pcol, "@Address", _Member.Address);
            Adapter.AddParam(pcol, "@Gender", _Member.Gender);
            Adapter.AddParam(pcol, "@ServTax_No", _Member.ServTax_No);
            Adapter.AddParam(pcol, "@AadharCardNo", _Member.AadharCardNo);
            Adapter.AddParam(pcol, "@ReportLogoPath", ReportLogoPath1);
            Adapter.AddParam(pcol, "@UserId", UserID);
            Adapter.AddParam(pcol, "@PAN", _Member.PAN);
            Adapter.AddParam(pcol, "@Active", _Member.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", UserID);
            Adapter.AddParam(pcol, "@MemberID", MemberId);
            Adapter.ExecutenNonQuery("USPUpdateMemberByID", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Update Member Master data
        public void UpdateFamilyByID(Family _Family, int FamilyID)
            {

            var UserID = HttpContext.Current.Session["UserID"];
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;

            Adapter.AddParam(pcol, "@Name", _Family.Name);
            Adapter.AddParam(pcol, "@UserId", UserID);
            Adapter.AddParam(pcol, "@Active", _Family.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", HttpContext.Current.Session["UserID"]);
            Adapter.AddParam(pcol, "@FamilyID", FamilyID);
            Adapter.ExecutenNonQuery("USPUpdateFamilyByID", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Update Financial Year Master data
        public void UpdateFinYrByID(FinancialYear _FinYr, int FinancialYearID)
            {

            var ModifiedBy = HttpContext.Current.Session["UserID"];
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FromDate", _FinYr.FromDate);
            Adapter.AddParam(pcol, "@ToDate", _FinYr.ToDate);
            Adapter.AddParam(pcol, "@Active", _FinYr.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", ModifiedBy);
            Adapter.AddParam(pcol, "@FinancialYearID", FinancialYearID);
            Adapter.ExecutenNonQuery("USPUpdateFinYrByID", CommandType.StoredProcedure, Adapter.param(pcol));
            }



        //To Get Member over Member Id for View, Delete, Update
        public List<Member> GetMemberByID(int MemberID)
            {

            List<Member> list = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetMemberByID", Adapter.Connection);
            com.Parameters.AddWithValue("@MemberID", MemberID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            list = new List<Member>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Member cobj = new Member();
                cobj.MemberID = Convert.ToInt32(ds.Tables[0].Rows[i]["MemberID"].ToString());
                cobj.FamilyID = Convert.ToInt32(ds.Tables[0].Rows[i]["FamilyID"].ToString());
                cobj.MemberName = ds.Tables[0].Rows[i]["MemberName"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                cobj.Gender = Convert.ToInt32(ds.Tables[0].Rows[i]["Gender"].ToString());
                cobj.ServTax_No = ds.Tables[0].Rows[i]["ServTax_No"].ToString();
                cobj.AadharCardNo = ds.Tables[0].Rows[i]["AadharCardNo"].ToString();
                cobj.PAN = ds.Tables[0].Rows[i]["PAN"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                list.Add(cobj);

                }
            return list;
            }

        //To Get Member over Member Id for View, Delete, Update
        public List<Family> GetFamilyByID(int FamilyID)
            {

            List<Family> list = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetFamilyByID", Adapter.Connection);
            com.Parameters.AddWithValue("@FamilyID", FamilyID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            list = new List<Family>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Family cobj = new Family();
                cobj.FamilyID = Convert.ToInt32(ds.Tables[0].Rows[i]["FamilyID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                list.Add(cobj);

                }
            return list;
            }

        // To Bind Broker bill Entry DropdownList
        public List<Account> BindBrokerList()
            {
            List<Account> BrokierBillList = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetBrokerList", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            BrokierBillList = new List<Account>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Account _Account = new Account();
                _Account.AccountId = Convert.ToInt32(ds.Tables[0].Rows[i]["AccountId"].ToString());
                _Account.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                BrokierBillList.Add(_Account);
                }

            return BrokierBillList;

            }

        //To Get Financial Year over FinancialYearID for View, Delete, Update
        public List<FinancialYear> GetFinancialByID(int FinancialYearID)
            {

            List<FinancialYear> list = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetFinYearByID", Adapter.Connection);
            com.Parameters.AddWithValue("@FinancialYearID", FinancialYearID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            list = new List<FinancialYear>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FinancialYear cobj = new FinancialYear();
                cobj.FinancialYearID = Convert.ToInt32(ds.Tables[0].Rows[i]["FinancialYearID"].ToString());
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FromDate"].ToString());
                cobj.ToDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ToDate"].ToString());
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                list.Add(cobj);

                }
            return list;
            }


        //To Delete Financial Year 
        public void DeleteFinYrByID(int FinancialYearID)
            {
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FinancialYearID", FinancialYearID);
            Adapter.ExecutenNonQuery("USPDeleteFinYrByID", CommandType.StoredProcedure, Adapter.param(pcol));
            }
        //To Delete Member 
        public void DeleteMemberByID(int MemberID)
            {

            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.ExecutenNonQuery("USPDeleteMemberByID", CommandType.StoredProcedure, Adapter.param(pcol));
            }
        //To Delete Family 
        public void DeleteFamilyByID(int FamilyID)
            {

            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FamilyID", FamilyID);
            Adapter.ExecutenNonQuery("USPDeleteFamilyByID", CommandType.StoredProcedure, Adapter.param(pcol));
            }



        //To Insert User  Details Master data
        public void InsertUserDetailsMaster(UserDetails _UserDetails)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@UserId", _UserDetails.UserId);
            Adapter.AddParam(pcol, "@FirstName", _UserDetails.FirstName);
            Adapter.AddParam(pcol, "@MiddleName", _UserDetails.MiddleName);
            Adapter.AddParam(pcol, "@LastName", _UserDetails.LastName);
            Adapter.AddParam(pcol, "@ProfilePic", _UserDetails.ProfilePic);
            Adapter.AddParam(pcol, "@Address", _UserDetails.Address);
            Adapter.AddParam(pcol, "@IsSms", _UserDetails.IsSms);
            Adapter.AddParam(pcol, "@IsEmail", _UserDetails.IsEmail);
            Adapter.AddParam(pcol, "@Active", _UserDetails.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPUserDetailsInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        //To Insert User Master data
        public void InsertUserMaster(User _User)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var SubscriberID = HttpContext.Current.Session["SubscriberID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@UserName", _User.UserName);
            Adapter.AddParam(pcol, "@Password", _User.Password);
            Adapter.AddParam(pcol, "@RoleId", _User.RoleId);
            Adapter.AddParam(pcol, "@PAN", _User.PAN);
            Adapter.AddParam(pcol, "@IsMobile", _User.IsMobile);
            //Adapter.AddParam(pcol, "@LastLoginCMS", _User.LastLoginCMS);
            //Adapter.AddParam(pcol, "@LastLoginMobile", _User.LastLoginMobile);
            //Adapter.AddParam(pcol, "@PasswordChangeDateTime", _User.PasswordChangeDateTime);
            Adapter.AddParam(pcol, "@Active", _User.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.AddParam(pcol, "@SubscriberID", _User.SubscriberID);
            Adapter.ExecutenNonQuery("USPUserInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));


            }
        //To Insert User Master data
        public void InsertMemberInUserMaster(Member _Member)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var SubscriberID = HttpContext.Current.Session["SubscriberID"];
            var RoleId = HttpContext.Current.Session["RoleId"];

            //if (MemberCount > subscription)
            //    {

            //    }
            //else
            //    {

            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@UserName", _Member.PAN);
            Adapter.AddParam(pcol, "@Password", _Member.PAN);
            // Adapter.AddParam(pcol, "@RoleId", RoleId);
            Adapter.AddParam(pcol, "@PAN", _Member.PAN);
            Adapter.AddParam(pcol, "@IsMobile", 0);
            //Adapter.AddParam(pcol, "@LastLoginCMS", _User.LastLoginCMS);
            //Adapter.AddParam(pcol, "@LastLoginMobile", _User.LastLoginMobile);
            //Adapter.AddParam(pcol, "@PasswordChangeDateTime", _User.PasswordChangeDateTime);
            Adapter.AddParam(pcol, "@Active", _Member.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.AddParam(pcol, "@SubscriberID", SubscriberID);
            Adapter.ExecutenNonQuery("USPMemberEntryInUser", CommandType.StoredProcedure, Adapter.param(pcol));
            // dt = Adapter.ExecuteDataTable("USPMemberEntryInUser", CommandType.StoredProcedure, Adapter.param(pcol));
            //}
            }

        //To Insert Consultant Master data
        public void InsertConsultantMaster(Consultant _Consultant)
            {
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.AddParam(pcol, "@Name", _Consultant.Name);
            Adapter.AddParam(pcol, "@MobileNo", _Consultant.MobileNo);
            Adapter.AddParam(pcol, "@Email", _Consultant.Email);
            Adapter.AddParam(pcol, "@Active", _Consultant.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPConsultantInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        public void Dropdown()
            {
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            SqlDataAdapter _da = new SqlDataAdapter("Select MemberID, MemberName From M_Member", Adapter.Connection);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            //   ViewBag.memberlist = ToSelectList(_dt, "MemberID", "MemberName");

            }



        //To Insert Investment Master data by its User
        public void InsertInvestmentMaster(Investment _Investment)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinancialYearMemberID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FinancialYearMemberID", FinancialYearMemberID);
            Adapter.AddParam(pcol, "@Name", _Investment.Name);
            Adapter.AddParam(pcol, "@Active", _Investment.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPInvestmentInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        //To Update Investment Master data by its User
        public void UpdateInvestmentMaster(Investment _Investment, int InvestmentId)
            {
            DataTable dt = new DataTable();
            var ModifiedBy = HttpContext.Current.Session["UserID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinancialYearMemberID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FinancialYearMemberID", FinancialYearMemberID);
            Adapter.AddParam(pcol, "@Name", _Investment.Name);
            Adapter.AddParam(pcol, "@Active", _Investment.Active);
            Adapter.AddParam(pcol, "@ModifiedBy", ModifiedBy);
            Adapter.AddParam(pcol, "@InvestmentID", InvestmentId);
            Adapter.ExecutenNonQuery("USPUpdateInvestmentByID", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        //To Delete Investment Master data by its User
        public void DeleteInvestmentMaster(int InvestmentId)
            {
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@InvestmentID", InvestmentId);
            Adapter.ExecutenNonQuery("USPDeleteInvestmentByID", CommandType.StoredProcedure, Adapter.param(pcol));

            }

        //To Insert Investment Master data by its Id
        public List<Investment> InvestmentMasterByID(int InvestmentId)
            {
            List<Investment> IList = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetInvestmentByID", Adapter.Connection);
            com.Parameters.AddWithValue("@InvestmentID", InvestmentId);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Investment>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Investment cobj = new Investment();
                cobj.InvestmentID = Convert.ToInt32(ds.Tables[0].Rows[i]["InvestmentID"].ToString());
                cobj.FinancialYearMemberID = Convert.ToInt32(ds.Tables[0].Rows[i]["FinancialYearMemberID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }

            return IList;

            }
        //To Get Investment Master data by CreatedBy
        public List<Investment> GetInvestmentById()
            {
            List<Investment> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetInvestment", Adapter.Connection);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Investment>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Investment cobj = new Investment();
                cobj.InvestmentID = Convert.ToInt32(ds.Tables[0].Rows[i]["InvestmentID"].ToString());
                cobj.FinancialYearMemberID = Convert.ToInt32(ds.Tables[0].Rows[i]["FinancialYearMemberID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }

            return IList;
            }

        //To Get Investment Master data by CreatedBy
        public List<Investment> GetAllInvestment()
            {
            List<Investment> IList = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllInvestment", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            IList = new List<Investment>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Investment cobj = new Investment();
                cobj.InvestmentID = Convert.ToInt32(ds.Tables[0].Rows[i]["InvestmentID"].ToString());
                cobj.FinancialYearMemberID = Convert.ToInt32(ds.Tables[0].Rows[i]["FinancialYearMemberID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                IList.Add(cobj);
                }

            return IList;
            }

        //To Insert Demat Master data
        public void InsertDematMaster(Demat _Demat)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FinancialYearMemberID", _Demat.FinancialYearMemberID);
            Adapter.AddParam(pcol, "@Name", _Demat.Name);
            Adapter.AddParam(pcol, "@Active", _Demat.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPDematInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));

            }


        //New

        public List<Script> SelectallScriptdata()
            {
            List<Script> scripdata = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetScriptMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            scripdata = new List<Script>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Script cobj = new Script();
                cobj.ScriptID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.ScriptName = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                cobj.BSECode = ds.Tables[0].Rows[i]["BSECode"].ToString();
                cobj.NSECode = ds.Tables[0].Rows[i]["NSECode"].ToString();
                cobj.GroupName = ds.Tables[0].Rows[i]["GroupName"].ToString();
                cobj.FaceValue = ds.Tables[0].Rows[i]["FaceValue"].ToString();
                cobj.ISIN = ds.Tables[0].Rows[i]["ISIN"].ToString();
                cobj.InvestmentType = ds.Tables[0].Rows[i]["InvestmentType"].ToString();
                //cobj.PAN = ds.Tables[0].Rows[i]["PAN"].ToString();
                //cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                //cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                scripdata.Add(cobj);
                }

            return scripdata;
            }


        //End 
        public List<Subscriber> GetSubscriber()
            {
            List<Subscriber> Companylist = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetSubscriberMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            Companylist = new List<Subscriber>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Subscriber cobj = new Subscriber();
                cobj.SubscriberID = Convert.ToInt32(ds.Tables[0].Rows[i]["SubscriberID"].ToString());
                cobj.SubscriberName = ds.Tables[0].Rows[i]["SubscriberName"].ToString();
                Companylist.Add(cobj);
                }
            return Companylist;
            }
        public List<Sector> GetSector()
            {
            List<Sector> SectorList = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetSectorData", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            SectorList = new List<Sector>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Sector cobj = new Sector();
                cobj.SectorID = Convert.ToInt32(ds.Tables[0].Rows[i]["SectorID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                SectorList.Add(cobj);
                }
            return SectorList;
            }


        public List<Script> SelectallScript(string Scriptname)
            {


            List<Script> scripdata = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetScriptNM", Adapter.Connection);
            com.Parameters.AddWithValue("@ScriptName", Scriptname);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            scripdata = new List<Script>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Script cobj = new Script();
                cobj.ScriptID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                cobj.ScriptName = ds.Tables[0].Rows[i]["ScriptName"].ToString();
                scripdata.Add(cobj);
                }
            return scripdata;
            }

        // Bind Family Master Dropdown in Member 
        public List<Family> BindFamily()
            {
            var CreatedBy = HttpContext.Current.Session["UserID"];
            List<Family> FamilyList = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetFamilyMaster", Adapter.Connection);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            FamilyList = new List<Family>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Family cobj = new Family();
                cobj.FamilyID = Convert.ToInt32(ds.Tables[0].Rows[i]["FamilyID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                FamilyList.Add(cobj);
                }
            return FamilyList;
            }

        // Bind Family Master Dropdown in Member 
        public List<Family> BindAllFamily()
            {
            var CreatedBy = HttpContext.Current.Session["UserID"];
            List<Family> FamilyList = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllFamilyMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            FamilyList = new List<Family>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Family cobj = new Family();
                cobj.FamilyID = Convert.ToInt32(ds.Tables[0].Rows[i]["FamilyID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                FamilyList.Add(cobj);
                }
            return FamilyList;
            }


        //To Insert Member & Financial Year data entry
        public void InsertMemberFinYrData(FinancialYearTrans __FinancialYearTrans)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FinincialYearID", __FinancialYearTrans.FinancialYearID);
            Adapter.AddParam(pcol, "@MemberID", __FinancialYearTrans.MemberID);
            Adapter.AddParam(pcol, "@Active", __FinancialYearTrans.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            //  Adapter.ExecutenNonQuery("USPMemberFinanceYrInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            dt = Adapter.ExecuteDataTable("USPMemberFinanceYrInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt.Rows.Count > 0)
                {
                //HttpContext.Current.Session["FinincialYearID"] = dt.Rows[0]["FinincialYearID"];
                //HttpContext.Current.Session["MemberID"] = dt.Rows[0]["MemberID"];
                HttpContext.Current.Session["FinincialYearID"] = __FinancialYearTrans.FinancialYearID;
                HttpContext.Current.Session["MemberID"] = __FinancialYearTrans.MemberID;
                HttpContext.Current.Session["FinancialYearMemberID"] = dt.Rows[0]["FinancialYearMemberID"];
                }
            }
        //To InsertFinancial Year data entry
        public void InsertFinYrData(FinancialYear __FinYear)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@FromDate", __FinYear.FromDate);
            Adapter.AddParam(pcol, "@ToDate", __FinYear.ToDate);
            Adapter.AddParam(pcol, "@Active", __FinYear.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPFinYrInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }
        //To Login User
        public void UserLogin(User _User)
            {
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            // Adapter.AddParam(pcol, "@UserId", _User.UserId);
            Adapter.AddParam(pcol, "@UserName", _User.UserName);
            Adapter.AddParam(pcol, "@Password", _User.Password);
            dt = Adapter.ExecuteDataTable("USPUserLoginInfo", CommandType.StoredProcedure, Adapter.param(pcol));
            HttpContext.Current.Session["myTable"] = dt;
            if (dt.Rows.Count > 0)
                {
                HttpContext.Current.Session["UserID"] = dt.Rows[0]["UserId"];
                HttpContext.Current.Session["UserName"] = dt.Rows[0]["UserName"];
                HttpContext.Current.Session["Password"] = dt.Rows[0]["Password"];
                HttpContext.Current.Session["SubscriberID"] = dt.Rows[0]["SubscriberID"];
                HttpContext.Current.Session["MemberSubscription"] = dt.Rows[0]["MemberSubscription"];
                HttpContext.Current.Session["MemberCount"] = dt.Rows[0]["MemberCount"];
                HttpContext.Current.Session["RoleId"] = dt.Rows[0]["RoleId"];
                HttpContext.Current.Session["RoleName"] = dt.Rows[0]["Name"];
                }
            else
                {
                string str = "Failed Login";
                }
            }


        public void MemberCheck(Member _Member)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            //// Adapter.AddParam(pcol, "@UserId", _User.UserId);
            Adapter.AddParam(pcol, "@MUserId", CreatedBy);
            dt = Adapter.ExecuteDataTable("USPMemberCount", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt.Rows.Count > 0)
                {
                HttpContext.Current.Session["UserID"] = dt.Rows[0]["UserId"];
                HttpContext.Current.Session["UserName"] = dt.Rows[0]["UserName"];
                HttpContext.Current.Session["Password"] = dt.Rows[0]["Password"];
                HttpContext.Current.Session["SubscriberID"] = dt.Rows[0]["SubscriberID"];
                HttpContext.Current.Session["MemberSubscription1"] = dt.Rows[0]["MemberSubscription"];
                HttpContext.Current.Session["MemberCount1"] = dt.Rows[0]["MemberCount"];
                HttpContext.Current.Session["RoleId"] = dt.Rows[0]["RoleId"];
                HttpContext.Current.Session["RoleName"] = dt.Rows[0]["Name"];
                }
            else
                {
                string str = "Failed Login";
                }
            }



        public void MemberMasterList()
            {

            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            //Adapter.AddParam(pcol, "@UserName", _Member.FamilyID);
            //Adapter.AddParam(pcol, "@Password", _Member.MemberName);
            Adapter.ExecuteReader("USPGetMemberMaster", CommandType.StoredProcedure, Adapter.param(pcol));

            }
        //To Display List of Member Master
        public List<Member> Selectalldata()
            {
            var CreatedBy = HttpContext.Current.Session["UserID"];
            List<Member> Memberlist = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetMemberMaster", Adapter.Connection);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            Memberlist = new List<Member>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Member cobj = new Member();
                cobj.MemberID = Convert.ToInt32(ds.Tables[0].Rows[i]["MemberID"].ToString());
                cobj.FamilyID = Convert.ToInt32(ds.Tables[0].Rows[i]["FamilyID"].ToString());
                cobj.MemberName = ds.Tables[0].Rows[i]["MemberName"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                cobj.Gender = Convert.ToInt32(ds.Tables[0].Rows[i]["Gender"].ToString());
                cobj.ServTax_No = ds.Tables[0].Rows[i]["ServTax_No"].ToString();
                cobj.AadharCardNo = ds.Tables[0].Rows[i]["AadharCardNo"].ToString();
                cobj.PAN = ds.Tables[0].Rows[i]["PAN"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                //cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                Memberlist.Add(cobj);
                }

            return Memberlist;
            }
        //To Display List of Member Master
        public List<Member> SelectallMemberdata()
            {
            var CreatedBy = HttpContext.Current.Session["UserID"];
            List<Member> Memberlist = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllMember", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            Memberlist = new List<Member>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Member cobj = new Member();
                cobj.MemberID = Convert.ToInt32(ds.Tables[0].Rows[i]["MemberID"].ToString());
                cobj.FamilyID = Convert.ToInt32(ds.Tables[0].Rows[i]["FamilyID"].ToString());
                cobj.MemberName = ds.Tables[0].Rows[i]["MemberName"].ToString();
                cobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                cobj.Address = ds.Tables[0].Rows[i]["Address"].ToString();
                cobj.Gender = Convert.ToInt32(ds.Tables[0].Rows[i]["Gender"].ToString());
                cobj.ServTax_No = ds.Tables[0].Rows[i]["ServTax_No"].ToString();
                cobj.AadharCardNo = ds.Tables[0].Rows[i]["AadharCardNo"].ToString();
                cobj.PAN = ds.Tables[0].Rows[i]["PAN"].ToString();
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                //cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                Memberlist.Add(cobj);
                }

            return Memberlist;
            }
        //To Display List of Menu Master
        public List<Menu> DisplayMenuData(Menu _Menu)
            {
            List<Menu> MenuList = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetMenuMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserId", _Menu.UserId);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            MenuList = new List<Menu>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Menu cobj = new Menu();
                cobj.MenuID = Convert.ToInt32(ds.Tables[0].Rows[i]["MenuID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                cobj.UserMenuID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserMenuID"].ToString());
                //  cobj.ParentMenuID = Convert.ToInt32(ds.Tables[0].Rows[i]["ParentMenuID"].ToString());
                //    cobj.url = ds.Tables[0].Rows[i]["url"].ToString(); 
                //cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.Add = Convert.ToBoolean(ds.Tables[0].Rows[i]["Add"]);
                cobj.Edit = Convert.ToBoolean(ds.Tables[0].Rows[i]["Edit"]);
                cobj.Delete = Convert.ToBoolean(ds.Tables[0].Rows[i]["Delete"]);
                cobj.View = Convert.ToBoolean(ds.Tables[0].Rows[i]["View"]);
                MenuList.Add(cobj);
                }

            return MenuList;
            }

        // To Bind Investment Type DropdownList
        public List<MType> BindInvenstmentType(MType _MType)
            {
            List<MType> MTypeList = null;

            DataTable dt = new DataTable();

            //SqlParameterCollection pcol = new SqlCommand().Parameters;

            //Adapter.ExecuteScalar("USPGetMTypeMaster", CommandType.StoredProcedure, Adapter.param(pcol));

            SqlCommand com = new SqlCommand("USPGetMTypeMaster", Adapter.Connection);
            com.Parameters.AddWithValue("@ParentTypeId", 1);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            MTypeList = new List<MType>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MType mobj = new MType();
                mobj.TypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString());
                mobj.ParentTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["ParentTypeId"].ToString());
                mobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                mobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                mobj.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                // mobj.SequenceNo = Convert.ToInt32(ds.Tables[0].Rows[i]["SequenceNo"].ToString()); 
                mobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                //cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                MTypeList.Add(mobj);
                }

            return MTypeList;

            }

        // To Bind Role Master DropdownList
        public List<Role> BindRole()
            {
            List<Role> RoleMasterList = null;

            DataTable dt = new DataTable();

            //SqlParameterCollection pcol = new SqlCommand().Parameters;

            //Adapter.ExecuteScalar("USPGetMTypeMaster", CommandType.StoredProcedure, Adapter.param(pcol));

            SqlCommand com = new SqlCommand("USPGetRoleMaster", Adapter.Connection);
            //  com.Parameters.AddWithValue("@ParentTypeId", 1);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            RoleMasterList = new List<Role>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Role role = new Role();
                role.RoleId = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleId"].ToString());
                role.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                RoleMasterList.Add(role);
                }

            return RoleMasterList;

            }

        public List<Member> GetMemberName()
            {
            List<Member> MemberList = new List<Member>();

            var MemberID = HttpContext.Current.Session["MemberID"];
            SqlCommand com = new SqlCommand("USPGetMemberNM", Adapter.Connection);
            com.Parameters.AddWithValue("@MemberID", MemberID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Member Member = new Member();


                if (ds.Tables[0].Rows.Count <= 1)
                    {
                    Member.MemberName = ds.Tables[0].Rows[i]["MemberName"].ToString();
                    HttpContext.Current.Session["MemberNM"] = Member.MemberName;
                    }
                MemberList.Add(Member);
                }

            return MemberList;
            }

        public List<Sector> GetSectorName(string SectorName)
            {
            List<Sector> SectorList = new List<Sector>();

            var SectorID = HttpContext.Current.Session["SectorName"];
            SqlCommand com = new SqlCommand("USPGetSectorNM", Adapter.Connection);
            com.Parameters.AddWithValue("@SectorID", SectorID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Sector sector = new Sector();


                if (ds.Tables[0].Rows.Count <= 1)
                    {
                    sector.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    HttpContext.Current.Session["SectorNM"] = sector.Name;
                    }
                SectorList.Add(sector);
                }

            return SectorList;
            }


        // To Bind Consultant Master DropdownList
        public List<Consultant> BindConsultantMaster()
            {
            List<Consultant> ConsultantList = null;
            SqlCommand com = new SqlCommand("USPGetConsultantMaster", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            ConsultantList = new List<Consultant>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Consultant consultant = new Consultant();
                consultant.ConsultantID = Convert.ToInt32(ds.Tables[0].Rows[i]["ConsultantID"].ToString());
                consultant.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                HttpContext.Current.Session["ConsultantNM"] = consultant.Name;
                ConsultantList.Add(consultant);
                }
            return ConsultantList;
            }

        // To Bind Broker Master DropdownList
        public List<Account> BindBrokerData(Account _Account)
            {
            List<Account> BrokerList = null;
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearID = HttpContext.Current.Session["FinincialYearID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinancialYearMemberID"];
            SqlCommand com = new SqlCommand("USPGetBrokerList", Adapter.Connection);
            // com.Parameters.AddWithValue("@MemberID", MemberID);
            // com.Parameters.AddWithValue("@FinancialYearMemberID", FinancialYearMemberID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            BrokerList = new List<Account>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Account _account = new Account();
                _account.AccountId = Convert.ToInt32(ds.Tables[0].Rows[i]["AccountId"].ToString());
                _account.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                BrokerList.Add(_account);
                }
            return BrokerList;
            }

        // To Bind Fund Family Master DropdownList
        public List<FundFamily> BindFundFamilyMaster()
            {
            List<FundFamily> FundFamilyList = null;
            SqlCommand com = new SqlCommand("USPGetFundFamilyMaster", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            FundFamilyList = new List<FundFamily>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FundFamily _FundFamily = new FundFamily();
                _FundFamily.MutualFundID = Convert.ToInt32(ds.Tables[0].Rows[i]["MutualFundID"].ToString());
                _FundFamily.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                FundFamilyList.Add(_FundFamily);
                }
            return FundFamilyList;
            }

        // To Bind Demat A/C Master DropdownList
        public List<Demat> BindDematMaster()
            {
            List<Demat> DematList = null;
            SqlCommand com = new SqlCommand("USPGetDematMaster", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            DematList = new List<Demat>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Demat _Demat = new Demat();
                _Demat.DematID = Convert.ToInt32(ds.Tables[0].Rows[i]["DematID"].ToString());
                _Demat.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                DematList.Add(_Demat);
                }
            return DematList;
            }

        // To Bind Payment Type DropdownList
        public List<MType> BindPaymentType()
            {
            List<MType> TypeList = null;
            SqlCommand com = new SqlCommand("USPGetPaymentType", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            TypeList = new List<MType>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MType _Type = new MType();
                _Type.TypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString());
                _Type.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                TypeList.Add(_Type);
                }
            return TypeList;
            }

        // To Bind Scheme DropdownList
        public List<Script> BindScheme(Script _Script)
            {
            List<Script> ScriptList = null;
            SqlCommand com = new SqlCommand("USPGetSchemeDetails", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@MutualFundID", _Script.MutualFundID);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            ScriptList = new List<Script>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Script _script = new Script();
                _script.ScriptID = Convert.ToInt32(ds.Tables[0].Rows[i]["ScriptID"].ToString());
                _script.ScriptName = ds.Tables[0].Rows[i]["Scheme"].ToString();
                _script.BSECode = ds.Tables[0].Rows[i]["Scheme Code"].ToString();
                _script.MFName = ds.Tables[0].Rows[i]["MFF Name"].ToString();
                _script.ISIN = ds.Tables[0].Rows[i]["ISIN"].ToString();

                ScriptList.Add(_script);
                }
            return ScriptList;
            }

        // To Bind Cash / Bank DropdownList
        //public List<MType> BindBankAccount()
        //    {
        //    List<MType> TypeList = null;
        //    SqlCommand com = new SqlCommand("USPGetBankAccount", Adapter.Connection);

        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@GroupID", 7);
        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataSet ds = new DataSet();

        //    da.Fill(ds);
        //    TypeList = new List<MType>();
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //        MType _Type = new MType();
        //        _Type.TypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString());
        //        _Type.Name = ds.Tables[0].Rows[i]["Name"].ToString();
        //        TypeList.Add(_Type);
        //        }
        //    return TypeList;
        //    }

        //To Insert Mutual Fund Manual Entry
        public void InsertMFManualEntry(MutualFundManualEntry __MFEntry)
            {
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearID = HttpContext.Current.Session["FinincialYearID"];
            var CreatedBy = HttpContext.Current.Session["UserID"];

            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@TransactionId", __MFEntry.TransactionId);
            Adapter.AddParam(pcol, "@BillNo", __MFEntry.FolioNo);
            Adapter.AddParam(pcol, "@TranDate", __MFEntry.EntryDate);
            //  Adapter.AddParam(pcol, "@ValanCode", __MFEntry.ValanCode);  
            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.AddParam(pcol, "@AccountType", __MFEntry.Type);
            Adapter.AddParam(pcol, "@BorkerRate", __MFEntry.EntryLoadIn);
            Adapter.AddParam(pcol, "@BorkerAmount", __MFEntry.LoadAmount);
            Adapter.AddParam(pcol, "@Amount", __MFEntry.Amount);
            Adapter.AddParam(pcol, "@NetAmount", __MFEntry.FinalAmount);
            Adapter.AddParam(pcol, "@BookType", __MFEntry.PaymentMode);
            Adapter.AddParam(pcol, "@FinancialYearID", FinancialYearID);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPMFEntryInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));


            }

        //To Insert Mutual Fund Manual Entry
        public void InsertBRDematTransEntry(MutualFundManualEntry __MFEntry)
            {
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearID = HttpContext.Current.Session["FinincialYearID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinancialYearMemberID"];
            var CreatedBy = HttpContext.Current.Session["UserID"];
            HttpContext.Current.Session["TransactionID"] = __MFEntry.TransactionId;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@TransactionId", __MFEntry.TransactionId);
            Adapter.AddParam(pcol, "@BillNo", __MFEntry.FolioNo);
            Adapter.AddParam(pcol, "@TranDate", __MFEntry.EntryDate);
            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.AddParam(pcol, "@FinancialYearID", FinancialYearID);
            Adapter.AddParam(pcol, "@DepositCode", __MFEntry.DematAC);
            Adapter.AddParam(pcol, "@ScriptID", __MFEntry.EntryLoadIn);
            Adapter.AddParam(pcol, "@Qty", __MFEntry.Unit);
            Adapter.AddParam(pcol, "@TranType", __MFEntry.Type);
            Adapter.AddParam(pcol, "@ConsultantID", __MFEntry.FinalAmount);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPBRBillDematTransInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));


            }

        ////To Insert BrokerBillDemat Entry
        public Int64 InsertACTransEntry(MutualFundManualEntry __MFEntry)
            {
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearID = HttpContext.Current.Session["FinincialYearID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinancialYearMemberID"];
            var CreatedBy = HttpContext.Current.Session["UserID"];
            HttpContext.Current.Session["TransactionID"] = __MFEntry.TransactionId;
            SqlParameterCollection pcol = new SqlCommand().Parameters;

            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.AddParam(pcol, "@FinancialYearMemberID", FinancialYearMemberID);
            Adapter.AddParam(pcol, "@ChequeNo", __MFEntry.ChequeNo);
            Adapter.AddParam(pcol, "@TransactionDate", __MFEntry.EntryDate);
            Adapter.AddParam(pcol, "@DebitCode", __MFEntry.DebitCode);
            Adapter.AddParam(pcol, "@CreditCode", __MFEntry.CreditCode);
            Adapter.AddParam(pcol, "@BorkerType", __MFEntry.Type);
            Adapter.AddParam(pcol, "@Amount", __MFEntry.Amount);
            Adapter.AddParam(pcol, "@InvestmentTypeId", __MFEntry.PaymentMode);
            Adapter.AddParam(pcol, "@Reco", __MFEntry.Reco);
            Adapter.AddParam(pcol, "@RecoDate", __MFEntry.PaymentDate);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Int64 TransactionId = Convert.ToInt64(Adapter.ExecuteScalar("USPACTransInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol)));
            return TransactionId;
            }

        ////To Insert BrokerBillTrans Entry
        public void InsertBRTransEntry(MutualFundManualEntry __MFEntry)
            {
            DataTable dt = new DataTable();
            var MemberID = HttpContext.Current.Session["MemberID"];
            var FinancialYearID = HttpContext.Current.Session["FinincialYearID"];
            var FinancialYearMemberID = HttpContext.Current.Session["FinancialYearMemberID"];
            var CreatedBy = HttpContext.Current.Session["UserID"];
            HttpContext.Current.Session["TransactionID"] = __MFEntry.TransactionId;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@TransactionId", __MFEntry.TransactionId);
            Adapter.AddParam(pcol, "@BillNo", __MFEntry.FolioNo);
            Adapter.AddParam(pcol, "@TranType", __MFEntry.Type);
            Adapter.AddParam(pcol, "@AccType", __MFEntry.Scheme);
            Adapter.AddParam(pcol, "@ScriptID", __MFEntry.CreditCode);
            Adapter.AddParam(pcol, "@lotSize", __MFEntry.Amount);
            Adapter.AddParam(pcol, "@LotQty", __MFEntry.PaymentMode);
            Adapter.AddParam(pcol, "@Qty", __MFEntry.Unit);
            Adapter.AddParam(pcol, "@GRate", __MFEntry.PriceNav);
            Adapter.AddParam(pcol, "@Rate", __MFEntry.NavAfterLoad);
            Adapter.AddParam(pcol, "@Amount", __MFEntry.Amount);
            Adapter.AddParam(pcol, "@MemberID", MemberID);
            Adapter.AddParam(pcol, "@FinancialYearID", FinancialYearID);
            Adapter.AddParam(pcol, "@ConsultantID", __MFEntry.Consultant);
            Adapter.AddParam(pcol, "@StrikePrice", __MFEntry.StrikePrice);
            // Adapter.AddParam(pcol, "@OptionType", __MFEntry.OptionType);
            Adapter.AddParam(pcol, "@IsIntraDay", __MFEntry.IsIntraDay);
            Adapter.AddParam(pcol, "@BrokerRate", __MFEntry.BrokerRate);
            Adapter.AddParam(pcol, "@BrokerAmount", __MFEntry.BrokerAmount);
            Adapter.AddParam(pcol, "@ExpRate", __MFEntry.ExpRate);
            Adapter.AddParam(pcol, "@InvestmentTypeId", __MFEntry.PaymentMode);
            Adapter.AddParam(pcol, "@IsFNOBill", __MFEntry.IsFNOBill);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPBRBillTransInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }

        // To Bind Member Name DropdownList
        public List<Member> BindMemberList(Member _Member)
            {
            List<Member> ShowMember = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetMemberListMaster", Adapter.Connection);
            //   com.Parameters.AddWithValue("@ParentTypeId", 1);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ShowMember = new List<Member>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Member mobj = new Member();
                mobj.MemberID = Convert.ToInt32(ds.Tables[0].Rows[i]["MemberID"].ToString());
                mobj.MemberName = ds.Tables[0].Rows[i]["MemberName"].ToString();
                ShowMember.Add(mobj);
                }
            return ShowMember;
            }
        // To Bind Financial Year DropdownList
        public List<FinancialYear> BindFinancialYearList(FinancialYear _FinancialYear)
            {
            List<FinancialYear> ShowFinYear = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetFinancialYrListMaster", Adapter.Connection);
            //   com.Parameters.AddWithValue("@ParentTypeId", 1);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ShowFinYear = new List<FinancialYear>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FinancialYear obj = new FinancialYear();
                obj.FinancialYearID = Convert.ToInt32(ds.Tables[0].Rows[i]["FinancialYearID"].ToString());
                obj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                obj.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FromDate"].ToString());
                obj.ToDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ToDate"].ToString());
                obj.Active = (Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString()));//? "Active" : "Inactive";
                obj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                ShowFinYear.Add(obj);
                }
            return ShowFinYear;

            }
        // To Bind Financial Year DropdownList
        public List<FinancialYear> GetFinYrByID()
            {
            List<FinancialYear> ShowFinYear = null;
            var CreatedBy = HttpContext.Current.Session["UserID"];
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetFinYrById", Adapter.Connection);
            com.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ShowFinYear = new List<FinancialYear>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                FinancialYear obj = new FinancialYear();
                obj.FinancialYearID = Convert.ToInt32(ds.Tables[0].Rows[i]["FinancialYearID"].ToString());
                obj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                obj.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FromDate"].ToString());
                obj.ToDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ToDate"].ToString());
                obj.Active = (Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString()));//? "Active" : "Inactive";
                obj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                ShowFinYear.Add(obj);
                }
            return ShowFinYear;

            }

     
        //Code To Display Upload Data=============================
        public List<Script> UploadData(ScriptUploadDownload _script, HttpPostedFileBase FilePath)
            {

            List<Script> scripdatalist = new List<Script>();

            if (FilePath != null)
                {
                if (FilePath.ContentType == "application/vnd.ms-excel" || FilePath.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                    string filename = FilePath.FileName;
                    HttpContext.Current.Session["filename"] = filename;
                    if (filename.EndsWith(".xlsx") || filename.EndsWith(".xls") || filename.EndsWith(".csv"))
                        {
                        string targetpath = HttpContext.Current.Server.MapPath("~/UploadData/");
                        FilePath.SaveAs(targetpath + filename);

                        string pathToExcelFile = targetpath + filename;
                        HttpContext.Current.Session["pathToExcelFile"] = pathToExcelFile;
                        string pathToExcelFile1 = targetpath + Path.GetFileNameWithoutExtension(filename);
                        string sheetName = "scriptmaster";
                        DataTable dtExcelSchema = new DataTable();
                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var scriptDetails = from a in excelFile.Worksheet<ScriptUploadDownload>(sheetName) select a;
                        string csvData = File.ReadAllText(pathToExcelFile);
                        DataTable dt = new DataTable();
                        DataSet ds = new DataSet();
                        string excelConnectionString = @"Provider='Microsoft.ACE.OLEDB.12.0';Data Source='" + pathToExcelFile + "';Extended Properties='Excel 12.0 Xml; IMEX = 1'"; /*Xml; IMEX = 1*/
                        OleDbConnection oledbconnection = new OleDbConnection(excelConnectionString);

                                                  dtExcelSchema.Columns.AddRange(new DataColumn[10]
                            {
                            new DataColumn("SecurityCode"),
                            new DataColumn("IssuerName"),
                            new DataColumn("SecurityId"),
                            new DataColumn("SecurityName"),
                            new DataColumn("Status"),
                            new DataColumn("Group"),
                            new DataColumn("FaceValue"),
                            new DataColumn("ISINNo"),
                            new DataColumn("Industry"),
                            new DataColumn("Instrument")
                              });

                            foreach (string row in csvData.Split('\n'))
                                {
                                if (!string.IsNullOrEmpty(row))
                                    {
                                    ScriptUploadDownload script = new ScriptUploadDownload();

                                    dtExcelSchema.Rows.Add();

                                    scripdatalist.Add(new Script
                                        {
                                        // SecurityCode = Convert.ToInt32(row.Split(',')[0])
                                        ScriptName = row.Split(',')[3],
                                        BSECode = row.Split(',')[0],
                                        NSECode = row.Split(',')[2],
                                        GroupName = row.Split(',')[5],
                                        FaceValue = row.Split(',')[6],
                                        //ISIN = row.Split(',')
                                        InvestmentType = row.Split(',')[9]
                                        });


                                    var d = scripdatalist;

                                    int i = 0;
                                    //Execute a loop over the columns.  
                                    foreach (string cell in row.Split(','))
                                        {
                                        dtExcelSchema.Rows[dtExcelSchema.Rows.Count - 1][i] = cell;
                                        i++;
                                        }

                                    }

                                }
                            return scripdatalist;
                            }

                        }
                    }
            return scripdatalist;
            }
        //End================

        //To Insert Bulk Script Master data
        public List<Script> InsertBulkScriptMaster(ScriptUploadDownload _Script, HttpPostedFileBase FilePath)
            {

            List<Script> scripdatalist = new List<Script>();

            if (HttpContext.Current.Session["filename"] != null)
                {
                string filename = HttpContext.Current.Session["filename"].ToString();
                string targetpath = HttpContext.Current.Server.MapPath("~/UploadData/");
                // FilePath.SaveAs(targetpath + filename);

                string pathToExcelFile = HttpContext.Current.Session["pathToExcelFile"].ToString();

                DataTable dtExcelSchema = new DataTable();

                string csvData = File.ReadAllText(pathToExcelFile);
                dtExcelSchema.Columns.AddRange(new DataColumn[10]
                {
                            new DataColumn("SecurityCode"),
                            new DataColumn("IssuerName"),
                            new DataColumn("SecurityId"),
                            new DataColumn("SecurityName"),
                            new DataColumn("Status"),
                            new DataColumn("Group"),
                            new DataColumn("FaceValue"),
                            new DataColumn("ISINNo"),
                            new DataColumn("Industry"),
                            new DataColumn("Instrument")
                  });

                foreach (string row in csvData.Split('\n'))
                    {
                    if (!string.IsNullOrEmpty(row))
                        {
                        ScriptUploadDownload script = new ScriptUploadDownload();

                        dtExcelSchema.Rows.Add();

                        scripdatalist.Add(new Script
                            {
                            // SecurityCode = Convert.ToInt32(row.Split(',')[0])
                            ScriptName = row.Split(',')[3],
                            BSECode = row.Split(',')[0],
                            NSECode = row.Split(',')[2],
                            GroupName = row.Split(',')[5],
                            FaceValue = row.Split(',')[6],
                            //ISIN = row.Split(',')
                            InvestmentType = row.Split(',')[9]
                            });


                        var d = scripdatalist;

                        int i = 0;
                        //Execute a loop over the columns.  
                        foreach (string cell in row.Split(','))
                            {
                            dtExcelSchema.Rows[dtExcelSchema.Rows.Count - 1][i] = cell;
                            i++;
                            }

                        }


                    ScriptUploadDownload _script = new ScriptUploadDownload();
                    if (dtExcelSchema.Rows.Count > 0)
                        {
                        for (int i = 1; i < dtExcelSchema.Rows.Count; i++)
                            {
                            SqlParameterCollection pcol = new SqlCommand().Parameters;
                            Adapter.AddParam(pcol, "@ScriptName", dtExcelSchema.Rows[i]["SecurityName"]);
                            Adapter.AddParam(pcol, "@BSECode", dtExcelSchema.Rows[i]["SecurityCode"]);
                            Adapter.AddParam(pcol, "@NSECode", dtExcelSchema.Rows[i]["SecurityId"]);
                            Adapter.AddParam(pcol, "@IndustryID", 0);
                            Adapter.AddParam(pcol, "@GroupName", dtExcelSchema.Rows[i]["Group"]);
                            Adapter.AddParam(pcol, "@InvestmentType", dtExcelSchema.Rows[i]["Instrument"]);
                            Adapter.AddParam(pcol, "@IsMcx", 0);
                            Adapter.AddParam(pcol, "@IsCurrency", 0);
                            Adapter.AddParam(pcol, "@IsNcdx", 0);
                            Adapter.AddParam(pcol, "@IsFO", 0);
                            Adapter.AddParam(pcol, "@FaceValue", dtExcelSchema.Rows[i]["FaceValue"]);
                            Adapter.AddParam(pcol, "@ISIN", dtExcelSchema.Rows[i]["ISINNo"]);
                            Adapter.AddParam(pcol, "@MutualFundID", 0);
                            Adapter.AddParam(pcol, "@Active", 1);
                            Adapter.AddParam(pcol, "@CreatedBy", 0);
                            Adapter.ExecutenNonQuery("USPScriptInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                            }
                        }
                    }
                }
            else
                {
                string pathToExcelFile = HttpContext.Current.Session["Path"].ToString();

                DataTable dtExcelSchema = new DataTable();

                string csvData = File.ReadAllText(pathToExcelFile);
                dtExcelSchema.Columns.AddRange(new DataColumn[10]
                {
                            new DataColumn("SecurityCode"),
                            new DataColumn("IssuerName"),
                            new DataColumn("SecurityId"),
                            new DataColumn("SecurityName"),
                            new DataColumn("Status"),
                            new DataColumn("Group"),
                            new DataColumn("FaceValue"),
                            new DataColumn("ISINNo"),
                            new DataColumn("Industry"),
                            new DataColumn("Instrument")
                  });
                foreach (string row in csvData.Split('\n'))
                    {
                    if (!string.IsNullOrEmpty(row))
                        {
                        ScriptUploadDownload script = new ScriptUploadDownload();

                        dtExcelSchema.Rows.Add();

                        scripdatalist.Add(new Script
                            {

                            ScriptName = row.Split(',')[3],
                            BSECode = row.Split(',')[0],
                            NSECode = row.Split(',')[2],
                            GroupName = row.Split(',')[5],
                            FaceValue = row.Split(',')[6],
                            //ISIN = row.Split(',')
                            InvestmentType = row.Split(',')[9]
                            });


                        var d = scripdatalist;

                        int i = 0;
                        //Execute a loop over the columns.  
                        foreach (string cell in row.Split(','))
                            {
                            dtExcelSchema.Rows[dtExcelSchema.Rows.Count - 1][i] = cell;
                            i++;
                            }

                        }


                    ScriptUploadDownload _script = new ScriptUploadDownload();
                    if (dtExcelSchema.Rows.Count > 0)
                        {
                        for (int i = 1; i < dtExcelSchema.Rows.Count; i++)
                            {
                            SqlParameterCollection pcol = new SqlCommand().Parameters;
                            Adapter.AddParam(pcol, "@ScriptName", dtExcelSchema.Rows[i]["SecurityName"]);
                            Adapter.AddParam(pcol, "@BSECode", dtExcelSchema.Rows[i]["SecurityCode"]);
                            Adapter.AddParam(pcol, "@NSECode", dtExcelSchema.Rows[i]["SecurityId"]);
                            Adapter.AddParam(pcol, "@IndustryID", 0);
                            Adapter.AddParam(pcol, "@GroupName", dtExcelSchema.Rows[i]["Group"]);
                            Adapter.AddParam(pcol, "@InvestmentType", dtExcelSchema.Rows[i]["Instrument"]);
                            Adapter.AddParam(pcol, "@IsMcx", 0);
                            Adapter.AddParam(pcol, "@IsCurrency", 0);
                            Adapter.AddParam(pcol, "@IsNcdx", 0);
                            Adapter.AddParam(pcol, "@IsFO", 0);
                            Adapter.AddParam(pcol, "@FaceValue", dtExcelSchema.Rows[i]["FaceValue"]);
                            Adapter.AddParam(pcol, "@ISIN", dtExcelSchema.Rows[i]["ISINNo"]);
                            Adapter.AddParam(pcol, "@MutualFundID", 0);
                            Adapter.AddParam(pcol, "@Active", 1);
                            Adapter.AddParam(pcol, "@CreatedBy", 0);
                            Adapter.ExecutenNonQuery("USPScriptInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                            }
                        }
                    }

                }

            return scripdatalist;
            }

        //To Display Download Script Master Data

        public List<Script> InsertDownloadScriptMaster(ScriptUploadDownload _script)
            {
            List<Script> scripdatalist = new List<Script>();
            var data = _script.Exchange;
            HttpContext.Current.Session["Exchange"] = data.ToString();
            var Investment = _script.InvestmentType.ToString();
            HttpContext.Current.Session["InvestmentType"] = Investment.ToString();
            string fileName = "";
            // GetExchangeType();
            WebClient Client = new WebClient();
            string sURL = GetURL(ref fileName);
            if (sURL != "")
                {
                string sDownloadPath = "";
                sDownloadPath = GetDownloadPath1(fileName);
                HttpContext.Current.Session["Path"] = sDownloadPath;
                Client.Headers.Add("user-agent", "Only a test!");
                Client.DownloadFile(sURL, sDownloadPath);

                DataTable dtExcelSchema = new DataTable();
                string csvData = File.ReadAllText(sDownloadPath);
                dtExcelSchema.Columns.AddRange(new DataColumn[10]
              {
                            new DataColumn("SecurityCode"),
                            new DataColumn("IssuerName"),
                            new DataColumn("SecurityId"),
                            new DataColumn("SecurityName"),
                            new DataColumn("Status"),
                            new DataColumn("Group"),
                            new DataColumn("FaceValue"),
                            new DataColumn("ISINNo"),
                            new DataColumn("Industry"),
                            new DataColumn("Instrument")
             });

                foreach (string row in csvData.Split('\n'))
                    {
                    if (!string.IsNullOrEmpty(row))
                        {
                        ScriptUploadDownload script = new ScriptUploadDownload();

                        dtExcelSchema.Rows.Add();
                        scripdatalist.Add(new Script
                            {
                            // SecurityCode = Convert.ToInt32(row.Split(',')[0])
                            ScriptName = row.Split(',')[3],
                            BSECode = row.Split(',')[0],
                            NSECode = row.Split(',')[2],
                            GroupName = row.Split(',')[5],
                            FaceValue = row.Split(',')[6],
                            //ISIN = row.Split(',')
                            InvestmentType = row.Split(',')[9]
                            });


                        var d = scripdatalist;

                        int i = 0;
                        //Execute a loop over the columns.  
                        foreach (string cell in row.Split(','))
                            {
                            dtExcelSchema.Rows[dtExcelSchema.Rows.Count - 1][i] = cell;
                            i++;
                            }
                        }
                    }
                return scripdatalist;
                }
            return scripdatalist;
            }

        //End here

        private static string GetDownloadPath1(string fileName)
            {
            string BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string sServerPath = Path.Combine(BaseDirectoryPath, "ScriptMaster");
            Directory.CreateDirectory(sServerPath);

            string sDownloadPath = Path.Combine(sServerPath, fileName);

            if (File.Exists(sDownloadPath))
                File.Delete(sDownloadPath);
            return sDownloadPath;
            }
        //To Get URL 
        private string GetURL(ref string sDownLoadPath)
            {
            ScriptUploadDownload _script = new ScriptUploadDownload();
            string exchange = HttpContext.Current.Session["Exchange"].ToString();
            string Investment = HttpContext.Current.Session["InvestmentType"].ToString();
            int Investmenttype = int.Parse(Investment);
            switch (Investmenttype)
                {
                case 1:
                    if (exchange == "BSE")
                        {
                        sDownLoadPath = "scriptmaster.csv";
                        return "https://irecordinfo.com/Scriptmaster/ListOfScrips.csv";
                        }
                    else if (exchange == "NSE")
                        {
                        sDownLoadPath = "scriptmaster.csv";
                        return "https://irecordinfo.com/Scriptmaster/NSEListOfScrips.csv";
                        }
                    else
                        {
                        return null;
                        }
                //case :
                //   return "https://irecordinfo.com/Scriptmaster/ListOfScrips.csv";
                case 2:
                    sDownLoadPath = "MFScript.csv";
                    return "https://irecordinfo.com/Scriptmaster/MFScript.csv";
                //case 4:
                //    return "https://irecordinfo.com/Scriptmaster/ListOfScrips.csv";
                //case 5:
                //    return "https://irecordinfo.com/Scriptmaster/ListOfScrips.csv";
                default:
                    return "";
                }

            }


        //public List<MenuDisplay> getusermenulist(int ? UserId)
        //    {
        //    using (DbContext db = new DbContext())
        //        {
        //        try
        //            {
        //            //return db.Database.SqlQuery<MenuDisplay>("exec USPGetMenuMaster @UserId",
        //            //new SqlParameter("@UserId", UserId)).ToList();
        //            SqlParameter param1 = new SqlParameter("@UserId", UserId);
        //            var data = db.Database.SqlQuery<MenuDisplay>("exec USPGetMenuMaster @UserId", param1).ToList();

        //            return data;
        //            }
        //        catch (Exception ex)
        //            {

        //            return null;
        //            }

        //        }
        //    }
        //To Insert User Rights Master data
        public List<Menu> InsertRights(Menu _Menu)
            {

            List<Menu> MenuList = new List<Menu>();
            var UserID = HttpContext.Current.Session["UserID"];
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetMenuMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserId", _Menu.UserId);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Menu cobj = new Menu();
                cobj.MenuID = Convert.ToInt32(ds.Tables[0].Rows[i]["MenuID"].ToString());
                cobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                cobj.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                cobj.UserMenuID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserMenuID"].ToString());

                cobj.Add = Convert.ToBoolean(ds.Tables[0].Rows[i]["Add"]);
                cobj.Edit = Convert.ToBoolean(ds.Tables[0].Rows[i]["Edit"]);
                cobj.Delete = Convert.ToBoolean(ds.Tables[0].Rows[i]["Delete"]);
                cobj.View = Convert.ToBoolean(ds.Tables[0].Rows[i]["View"]);
                MenuList.Add(cobj);
                for (var ij = 0; ij < _Menu.MenuList.Count; ij++)
                    {
                    if (_Menu.MenuList[i].Add == true || _Menu.MenuList[i].Edit == true || _Menu.MenuList[i].Delete == true || _Menu.MenuList[i].View == true)
                        {
                        SqlParameterCollection pcol = new SqlCommand().Parameters;
                        Adapter.AddParam(pcol, "@UserId", UserID);
                        Adapter.AddParam(pcol, "@MenuID", cobj.MenuID);
                        Adapter.AddParam(pcol, "@Add", _Menu.MenuList[i].Add);
                        Adapter.AddParam(pcol, "@Edit", _Menu.MenuList[i].Edit);
                        Adapter.AddParam(pcol, "@Delete", _Menu.MenuList[i].Delete);
                        Adapter.AddParam(pcol, "@View", _Menu.MenuList[i].View);
                        Adapter.AddParam(pcol, "@Active", 1);
                        Adapter.AddParam(pcol, "@CreatedBy", UserID);
                        Adapter.ExecutenNonQuery("USPUserMenuTransInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                        }
                    }
                }

            return MenuList;
            }

        // To Search Menu's 
        public List<Menu> SearchVal(string MenuName, Menu _Menu)
            {
            List<Menu> Menulist = new List<Menu>();
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@MenuName", MenuName);
            dt = Adapter.ExecuteDataTable("USPSearchMenuMaster", CommandType.StoredProcedure, Adapter.param(pcol));
            Menulist = new List<Menu>();
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                Menu _menu = new Menu();
                _menu.MenuID = Convert.ToInt32(dt.Rows[i]["MenuID"].ToString());
                _menu.Name = dt.Rows[i]["Name"].ToString();
                Menulist.Add(_menu);
                }

            return Menulist;
            }


        //public static class HtmlExtensions
        //    {
        //    public static IHtmlString Save(this HtmlHelper htmlHelper,  item)
        //        {
        //        var button = new TagBuilder("input");
        //        button.Attributes["type"] = "button";
        //        button.Attributes["value"] = "Reset";
        //        button.AddCssClass("btnresetinvoice");
        //        button.AddCssClass("button");
        //        button.Attributes["data-invoiceid"] = item.InvoiceId.ToString();
        //        if (item.PMApproved)
        //            {
        //            button.Attributes["disabled"] = "disabled";
        //            }
        //        return new HtmlString(button.ToString(TagRenderMode.SelfClosing));
        //        }
        //    }



        }

    }