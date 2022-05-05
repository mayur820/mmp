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
using System.Web.WebPages;

namespace IRecordweb.Reports
{
    public partial class GainLoss : System.Web.UI.Page
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

                if (Convert.ToDateTime(Session["FinToDate"].ToString()) >= DateTime.Now)
                {
                    txt_toDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    txt_toDate.Text = Convert.ToDateTime(Session["FinToDate"].ToString()).ToString("dd-MMM-yyyy");
                }
                txt_fromDate.Text = Convert.ToDateTime(Session["FinFromDate"].ToString()).ToString("dd-MMM-yyyy");

                BindScriptList();
                BindEntryList();
                BindHoldingList();
            }
        }
        public void BindScriptList()
        {
            try
            {
                string strsql = "Select A.* from (select Distinct -1 as ScriptID , 'All' as ScriptName from M_script union all select Distinct ScriptID , ScriptName from M_script ) A";
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand(strsql, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                adp.Fill(dt);
                ListSCRIPT.DataSource = dt;
                ListSCRIPT.DataTextField = "ScriptName";
                ListSCRIPT.DataValueField = "ScriptID";
                ListSCRIPT.DataBind();
                foreach (ListItem listItem in ListSCRIPT.Items)
                {
                    listItem.Selected = true;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void BindHoldingList()
        {
            try
            {
                string strsql = "Select A.* from (Select -1 as ID , 'All' as Holding_Type union all Select 1 as ID , 'T-Stock In Trade' as Holding_Type union all Select 0 as ID , 'I-Share Invenstment' as Holding_Type ) A";
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand(strsql, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                adp.Fill(dt);
                LstHoldingType.DataSource = dt;
                LstHoldingType.DataTextField = "Holding_Type";
                LstHoldingType.DataValueField = "ID";
                LstHoldingType.DataBind();
                foreach (ListItem listItem in LstHoldingType.Items)
                {
                    listItem.Selected = true;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void BindEntryList()
        {
            try
            {
                string strsql = "select A.* from (select Distinct -1 as  SR_NO,'ALL' as Entry_Type from Ent_EntryTypes union all select SR_NO,Entry_Type from Ent_EntryTypes) A";
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand(strsql,con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                adp.Fill(dt);
                lstentryType.DataSource = dt;
                lstentryType.DataTextField = "Entry_Type";
                lstentryType.DataValueField = "SR_NO";
                lstentryType.DataBind();
                foreach (ListItem listItem in lstentryType.Items)
                {
                    listItem.Selected = true;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable Dt_SCRIPTList()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_GETSCRIPTBYCODE]";
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWALL";
            //cmd.Parameters.Add("@VAR1", SqlDbType.NVarChar).Value = MemberCode;

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
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return DT;
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
            string FileName = Server.MapPath("~/Reports/ReportsSrc/Report_Gain_Loss.rdlc"); 
            //if (!System.IO.File.Exists(FileName))
            //{
            //    FileName = string.Format("{0}Resources\\Account_Ledger_Report.rdlc", Path.GetFullPath(Path.Combine(RunningPath, @"")));

            //}
            DataSet dsCustomers = GetMaindata();
            ReportDataSource datasource = new ReportDataSource("DataSet1", dsCustomers.Tables[0] as DataTable);
            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.LocalReport.ReportPath = FileName;
            reportViewer1.LocalReport.DataSources.Clear();
            try
            {
                reportViewer1.LocalReport.DataSources.Add(datasource);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            
            
            //reportViewer1.ReportRefresh();
            //reportViewer1.RefreshReport();
            //reportViewer1.RefreshReport();
            //reportViewer1.RefreshReport();
            //reportViewer1.RefreshReport();
        }
       
        private DataSet GetMaindata()
        {
            string Entry = "";
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            string constr = strConnString;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            string ListSrptvalues = "";
            string ListEntryvalues = "";
            string ListHoldvalues = "";
            bool seletectAllOrNot = false;

            if (LstHoldingType.SelectedValue == "-1")
            {
                ListHoldvalues = "All";
                seletectAllOrNot = true;
            }
            else
            {
                foreach (ListItem listItem in LstHoldingType.Items)
                {
                    if (listItem.Selected)
                    {

                        ListHoldvalues += listItem.Value + ",";
                        var val = listItem.Value;
                        var txt = listItem.Text;
                    }
                    else
                    {
                        seletectAllOrNot = true;
                    }
                }
            }
            if (!seletectAllOrNot)
            {
                ListHoldvalues = "";
            }

            if (ListSCRIPT.SelectedValue == "-1")
            {
                ListSrptvalues = "All";
                seletectAllOrNot = true;
            }
            else
            {
                foreach (ListItem listItem in ListSCRIPT.Items)
                {
                    if (listItem.Selected)
                    {

                        ListSrptvalues += listItem.Value + ",";
                        var val = listItem.Value;
                        var txt = listItem.Text;
                    }
                    else
                    {
                        seletectAllOrNot = true;
                    }
                }
            }
            if (!seletectAllOrNot)
            {
                ListSrptvalues = "";
            }
            if (lstentryType.SelectedValue == "-1")
            {
                ListEntryvalues = "All";
                seletectAllOrNot = true;
            }
            else
            {
                foreach (ListItem listItem in lstentryType.Items)
                {
                    if (listItem.Selected)
                    {

                        ListEntryvalues += listItem.Text + ",";
                        var val = listItem.Value;
                        var txt = listItem.Text;
                    }
                    else
                    {
                        seletectAllOrNot = true;
                    }
                }
            }

            if (!seletectAllOrNot)
            {
                ListEntryvalues = "";
            }

          
            using (SqlConnection con = new SqlConnection(constr))
            {
                ListEntryvalues = ListEntryvalues.TrimEnd(',');
                string strSql = "exec [dbo].[SP_Gain_Loss_Main] @MEMBERID= " + MemberCode + ", @Entry_Type = '" + ListEntryvalues + "', @fin_Year = " + FinancialYearCode + ", @SCRIPT_ID = '" + ListSrptvalues + "', @Holding_Type = '"+ ListHoldvalues +"', @from_Date = '"+ Convert.ToDateTime(txt_fromDate.Text).ToString("yyyy/MM/dd") + "', @To_Date = '" + Convert.ToDateTime(txt_toDate.Text).ToString("yyyy/MM/dd") + "'";
                //string strold= "exec SP_Gain_Loss_Main  @MEMBERID = " + MemberCode + " , @Entry_Type = '" + lstentryType.SelectedValue.ToString() + "', @fin_Year = " + FinancialYearCode + ", @SCRIPT_ID = '" + ListSrptvalues + "'"";
                using (SqlCommand cmd = new SqlCommand(strSql))
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
    
    }
}