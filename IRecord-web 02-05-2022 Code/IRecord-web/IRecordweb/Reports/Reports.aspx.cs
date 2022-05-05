using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace IRecordweb.Reports
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/ReportsSrc/IRECORD_HOLDING_REPORT.rdlc");

                ReportDataSource datasource = new ReportDataSource("DataSet1", GetData().Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);
            }
        }
        private DataSet GetData()
        {
            string constr = @"Data Source=DESKTOP-AVM4CQQ;Initial Catalog=MWork;User Id=sa;Password=chin@10;";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("EXEC [dbo].[ATPL_SP_HOLDING_REPORT] @Member_Code = 'm0001', @Trans_no = 'ALL'"))
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