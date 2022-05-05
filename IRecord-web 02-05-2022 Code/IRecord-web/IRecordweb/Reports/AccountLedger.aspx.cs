using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

using System.IO;
using Microsoft.Reporting.WebForms;

namespace IRecordweb.Reports
{
    public partial class AccountLedger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                D1YEAR.Value = Convert.ToDateTime(Convert.ToDateTime(Session["FinFromDate"].ToString()).ToString("yyyy/MM/dd")).Year.ToString();
                D1Months.Value = Convert.ToDateTime(Convert.ToDateTime(Session["FinFromDate"].ToString()).ToString("yyyy/MM/dd")).Month.ToString();
                D1Day.Value = Convert.ToDateTime(Convert.ToDateTime(Session["FinFromDate"].ToString()).ToString("yyyy/MM/dd")).Day.ToString();

                D2YEAR.Value = Convert.ToDateTime(Convert.ToDateTime(Session["FinToDate"].ToString()).ToString("yyyy/MM/dd")).Year.ToString();
                D2Months.Value = Convert.ToDateTime(Convert.ToDateTime(Session["FinToDate"].ToString()).ToString("yyyy/MM/dd")).Month.ToString();
                D2Day.Value = Convert.ToDateTime(Convert.ToDateTime(Session["FinToDate"].ToString()).ToString("yyyy/MM/dd")).Day.ToString();
               
                if(Convert.ToDateTime(Session["FinToDate"].ToString())>=DateTime.Now)
                {
                    txt_toDate.Text= DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    txt_toDate.Text = Convert.ToDateTime(Session["FinToDate"].ToString()).ToString("dd-MMM-yyyy");
                }
                txt_fromDate.Text = Convert.ToDateTime(Session["FinFromDate"].ToString()).ToString("dd-MMM-yyyy");

                ListGroup.DataSource = Dt_GroupList();
                ListGroup.DataTextField = "NAME";
                ListGroup.DataValueField = "ID";
                ListGroup.DataBind();
                foreach (ListItem listItem in ListGroup.Items)
                {
                    listItem.Selected = true;
                   
                }
                ListGroup_SelectedIndexChanged(sender, e);
            }

        }
        public DataTable Dt_GroupList()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_REPORTS_TOOLS]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWGROUP";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = MemberCode;

            cmd.Parameters.Add("@VAR2", SqlDbType.NVarChar).Value = FinancialYearCode;//




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

        protected void ListGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ListGroupvalues = "";
            foreach (ListItem listItem in ListGroup.Items)
            {
                if (listItem.Selected)
                {

                    ListGroupvalues += listItem.Value+",";
                    var val = listItem.Value;
                    var txt = listItem.Text;
                }
            }
            DataTable DT = new DataTable();
            try
            {
                DT = Dt_AccountList(ListGroupvalues.Substring(0, ListGroupvalues.Length - 1));
            }
            catch
            {

            }
             
            ListAccount.DataSource = DT;
            ListAccount.DataTextField = "NAME";
            ListAccount.DataValueField = "ID";
            ListAccount.DataBind();
            foreach (ListItem listItem in ListAccount.Items)
            {
                listItem.Selected = true;

            }
        }
       
        public DataTable Dt_AccountList(string GroupIds)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_REPORTS_TOOLS]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWACCOUNT";
            cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = GroupIds;

            //cmd.Parameters.Add("@VAR2", SqlDbType.NVarChar).Value = FinancialYearCode;//




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
                //throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return DT;
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FileName = Server.MapPath("~/Reports/ReportsSrc/Account_Ledger_Report.rdlc"); 
            //if (!System.IO.File.Exists(FileName))
            //{
            //    FileName = string.Format("{0}Resources\\Account_Ledger_Report.rdlc", Path.GetFullPath(Path.Combine(RunningPath, @"")));

            //}
            DataSet dsCustomers = GetMaindata();
            ReportDataSource datasource = new ReportDataSource("DataSet1", dsCustomers.Tables[0]);
            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.LocalReport.ReportPath = FileName;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.SubreportProcessing +=
new SubreportProcessingEventHandler(addsubreport);
            reportViewer1.LocalReport.DataSources.Add(datasource);
            //reportViewer1.ReportRefresh();
            //reportViewer1.RefreshReport();
            //reportViewer1.RefreshReport();
            //reportViewer1.RefreshReport();
            //reportViewer1.RefreshReport();
        }
        void addsubreport(object sender, SubreportProcessingEventArgs e)
        {

            string trnsno = e.Parameters[6].Values[0].ToString();
            SqlCommand sqlcomm = new SqlCommand();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataSet dataset = GetSubdata(trnsno);

            //dataAdapter.Fill(dataset.Tables[0]);
            e.DataSources.Add(new ReportDataSource("DataSet1", dataset.Tables[0]));

        }
        private DataSet GetMaindata()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            string constr = strConnString;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            string ListGroupvalues = "";
            bool seletectAllOrNot = false;
            foreach (ListItem listItem in ListGroup.Items)
            {
                if (listItem.Selected)
                {

                    ListGroupvalues += listItem.Value + ",";
                    var val = listItem.Value;
                    var txt = listItem.Text;
                }
                else
                {
                    seletectAllOrNot = true;
                }
            }
            if(!seletectAllOrNot)
            {
                ListGroupvalues = "";
            }

            string ListAcvalues = "";
            bool seletectAcAllOrNot = false;
            foreach (ListItem listItem in ListAccount.Items)
            {
                if (listItem.Selected)
                {

                    ListAcvalues += listItem.Value + ",";
                    var val = listItem.Value;
                    var txt = listItem.Text;
                }
                else
                {
                    seletectAcAllOrNot = true;
                }
            }
            if (!seletectAcAllOrNot)
            {
                ListAcvalues = "";
            }
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("exec rpt_Account_Ledger_Report_ATPL @Yearcode='" + FinancialYearCode + "',@Membercode='" + MemberCode + "',@FromDate='"+Convert.ToDateTime(txt_fromDate.Text).ToString("yyyy/MM/dd")+ "',@ToDate='" + Convert.ToDateTime(txt_toDate.Text).ToString("yyyy/MM/dd") + "',@Accountcode='"+ ListAcvalues + "',@Groupid='"+ ListGroupvalues + "'"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dsMyDs = new DataSet())
                        {
                            sda.Fill(dsMyDs, "DataTable1");
                            return dsMyDs;
                        }
                    }
                }
            }





            //string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            //string constr = strConnString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand("exec rpt_Account_Ledger_Report_ATPL @Yearcode='169',@Membercode='1669',@FromDate='2021-03-01',@ToDate='2021-12-03',@Accountcode='',@Groupid=''"))
            //    {
            //        using (SqlDataAdapter sda = new SqlDataAdapter())
            //        {
            //            cmd.Connection = con;
            //            sda.SelectCommand = cmd;
            //            using (DataSet dsMyDs = new DataSet())
            //            {
            //                sda.Fill(dsMyDs, "DataTable1");
            //                return dsMyDs;
            //            }
            //        }
            //    }
            //}
        }
        private DataSet GetSubdata(string trnsno)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            string constr = strConnString;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            string ListGroupvalues = "";
            bool seletectAllOrNot = false;
            foreach (ListItem listItem in ListGroup.Items)
            {
                if (listItem.Selected)
                {

                    ListGroupvalues += listItem.Value + ",";
                    var val = listItem.Value;
                    var txt = listItem.Text;
                }
                else
                {
                    seletectAllOrNot = true;
                }
            }
            if (!seletectAllOrNot)
            {
                ListGroupvalues = "ALL";
            }

            string ListAcvalues = "";
            bool seletectAcAllOrNot = false;
            foreach (ListItem listItem in ListAccount.Items)
            {
                if (listItem.Selected)
                {

                    ListAcvalues += listItem.Value + ",";
                    var val = listItem.Value;
                    var txt = listItem.Text;
                }
                else
                {
                    seletectAcAllOrNot = true;
                }
            }
            if (!seletectAcAllOrNot)
            {
                ListAcvalues = "ALL";
            }
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("exec rpt_Account_Ledger_Report_ATPL_sub @Year_code='"+ FinancialYearCode + "',@Member_code='"+ MemberCode + "',@From_Date='" + Convert.ToDateTime(txt_fromDate.Text).ToString("yyyy/MM/dd") + "',@To_Date='" + Convert.ToDateTime(txt_toDate.Text).ToString("yyyy/MM/dd") + "',@Account_code='"+ ListAcvalues + "',@Group_id='"+ ListGroupvalues + "', @trans_no = " + trnsno + ""))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dsMyDs = new DataSet())
                        {
                            sda.Fill(dsMyDs, "DataTable1");
                            return dsMyDs;
                        }
                    }
                }
            }
        }
    }
}