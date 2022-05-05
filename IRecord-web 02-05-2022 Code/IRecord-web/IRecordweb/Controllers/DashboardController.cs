using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace IRecordweb.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            //ReportViewer reportViewer = new ReportViewer();
            //reportViewer.ProcessingMode = ProcessingMode.Local;
            //reportViewer.SizeToReportContent = true;
            //reportViewer.Width = Unit.Percentage(900);
            //reportViewer.Height = Unit.Percentage(900);
            //var connectionString = ConfigurationManager.ConnectionStrings["DbEmployeeConnectionString"].ConnectionString;
            //SqlConnection conx = new SqlConnection(connectionString);
            //SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Employee_tbt", conx);
            //adp.Fill(ds, ds.Employee_tbt.TableName);
            //reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\MyReport.rdlc";
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MyDataSet", ds.Tables[0]));
            //ViewBag.ReportViewer = reportViewer;
            //return View();

            DataTable DDTDisplayAllUser = DTDisplayAllUser();
            DataTable DDTDisplayAllStatus = DTDisplayAllStatus();
            Session["DDTDisplayAllUser"] = DDTDisplayAllUser;
            Session["DDTDisplayAllStatus"] = DDTDisplayAllStatus;
            return View();
        }
        public DataTable DTDisplayAllUser()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_Dashboard_Reports]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DisplayAllUser";
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["UserID"].ToString();

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
        public DataTable DTDisplayAllStatus()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[dbo].[SP_Dashboard_Reports]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DisplayAllStatus";
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = Session["UserID"].ToString();

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


        public ActionResult Dashboardmarket()
            {
            return View();
            }

        public ActionResult Dashboardlongterm()
            {
            return View();
            }

        public ActionResult DashboardShortTerm()
            {
            return View();
            }
        }
}