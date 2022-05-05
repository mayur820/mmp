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
    public partial class StockHolding : System.Web.UI.Page
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

                //ListGroup.DataSource = Dt_GroupList();
                //ListGroup.DataTextField = "NAME";
                //ListGroup.DataValueField = "ID";
                //ListGroup.DataBind();
                //foreach (ListItem listItem in ListGroup.Items)
                //{
                //    listItem.Selected = true;
                   
                //}
                //ListGroup_SelectedIndexChanged(sender, e);
            }

        }
  

      
       
    

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FileName = Server.MapPath("~/Reports/ReportsSrc/RPT_STOCK_HOLDING.rdlc"); 
           
            DataSet dsCustomers = GetMaindata();
            ReportDataSource datasource = new ReportDataSource("DataSet1", dsCustomers.Tables[0]);
            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.LocalReport.ReportPath = FileName;
            reportViewer1.LocalReport.DataSources.Clear();
       
            reportViewer1.LocalReport.DataSources.Add(datasource);
           
        }
       
        private DataSet GetMaindata()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            string constr = strConnString;
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            var MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            var FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            var CreatedById = Session["UserID"].ToString();
            //string ListGroupvalues = "";
            //bool seletectAllOrNot = false;
            //foreach (ListItem listItem in ListGroup.Items)
            //{
            //    if (listItem.Selected)
            //    {

            //        ListGroupvalues += listItem.Value + ",";
            //        var val = listItem.Value;
            //        var txt = listItem.Text;
            //    }
            //    else
            //    {
            //        seletectAllOrNot = true;
            //    }
            //}
            //if(!seletectAllOrNot)
            //{
            //    ListGroupvalues = "";
            //}

            //string ListAcvalues = "";
            //bool seletectAcAllOrNot = false;
            //foreach (ListItem listItem in ListAccount.Items)
            //{
            //    if (listItem.Selected)
            //    {

            //        ListAcvalues += listItem.Value + ",";
            //        var val = listItem.Value;
            //        var txt = listItem.Text;
            //    }
            //    else
            //    {
            //        seletectAcAllOrNot = true;
            //    }
            //}
            //if (!seletectAcAllOrNot)
            //{
            //    ListAcvalues = "";
            //}
            using (SqlConnection con = new SqlConnection(constr))
            {
                
                using (SqlCommand cmd = new SqlCommand("EXEC DBO.ATPL_STOCK_HOLDING @FROM_DATE = '" + Convert.ToDateTime(txt_fromDate.Text).ToString("yyyy/MM/dd") + "', @TO_DATE = '" + Convert.ToDateTime(txt_toDate.Text).ToString("yyyy/MM/dd") + "', @MEMBERID = " + MemberCode + ", @Segment = 0"))
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