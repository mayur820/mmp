using BAL;
using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
    {
    public class TradeFileDAL
        {
        public void SaveImport(HttpPostedFileBase FilePath)
            {
            if (FilePath != null)
                {
                string filename = FilePath.FileName;
                //  string filePath = FilePath.ToString();
                string ext = Path.GetExtension(filename);
                if (ext.ToLower() == ".pdf")
                    {
                    //pdf_to_text_converter(Convert.ToInt16(txtFormatNo.Text), txtimport.Text, isTradefile);
                    }
                }
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

        }
    }
