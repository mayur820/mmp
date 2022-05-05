using BAL;
using IrecordDAL;
using IRecordweb.Models;
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
   public class BrokerBillDAL
        {
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
        // To Bind Investment Type DropdownList
        public List<MType> BindInvenstmentType(MType _MType)
            { 
            List<MType> MTypeList = null;

            DataTable dt = new DataTable();
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
                mobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                MTypeList.Add(mobj);
                }

            return MTypeList;

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

        // To Bind Broker Format DropdownList
        public List<BrokerBill> BindBrokerFormat()
            {
            List<BrokerBill> BrokerBillList = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetBrokerFormat", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            BrokerBillList = new List<BrokerBill>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                BrokerBill _Broker = new BrokerBill();
                _Broker.BrokerBillFormatID = Convert.ToInt32(ds.Tables[0].Rows[i]["BrokerBillFormatID"].ToString());
                _Broker.BrokerFormatName = ds.Tables[0].Rows[i]["Name"].ToString();
                BrokerBillList.Add(_Broker);
                }

            return BrokerBillList;

            }
        public List<BrokerBill> FormData(BrokerBill _Broker)
            {
            List<BrokerBill> BrokerBillList = new List<BrokerBill>();
            string str = "";
            if (_Broker.Consultant == "1")
                {
                str = "";
                }

            SqlCommand com = new SqlCommand("USPGetConsultantNM", Adapter.Connection);
            com.Parameters.AddWithValue("@ConsultantId", _Broker.Consultant);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                BrokerBill broker = new BrokerBill();

                broker.Consultant = ds.Tables[0].Rows[i]["Name"].ToString();

                BrokerBillList.Add(broker);
                }
            //  return ConsultantList;


            HttpContext.Current.Session["Consultant"] = _Broker.Consultant;
            HttpContext.Current.Session["HoldingType"] = _Broker.HoldingType;

            //_Broker.HoldingType = HttpContext.Current.Session["HoldingType"].ToString();
            //  BrokerBillList.Add(_Broker);
            return BrokerBillList;
            }

        // To Bind Demat A/C Master DropdownList
        public List<BrokerBill> BindExpenseData()
            {
            List<BrokerBill> Accountlist = null;
            SqlCommand com = new SqlCommand("USPGetExpnseData", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            Accountlist = new List<BrokerBill>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                BrokerBill _Account = new BrokerBill();
                _Account.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                _Account.Code = ds.Tables[0].Rows[i]["code"].ToString();
                Accountlist.Add(_Account);
                }
            return Accountlist;
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

        }
    }
