using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using IRecordweb.Models;

namespace IRecordweb.Controllers
{
    public class PaymentDemoController : Controller
    {
        // GET: PaymentDemo
        public ActionResult Index()
        {
            DataTable dtgetwayInfo = getPaymentInfo();
            string rowIDENTITY = GetSCOPE_IDENTITY();
            DataTable dtUserInfo = GetUserInfo();
            string paymentreq = "";
            paymentreq += "{";
            paymentreq += "    \"amount\": 10000,";
            paymentreq += "    \"currency\": \"INR\",";
            paymentreq += "    \"accept_partial\": true,";
            paymentreq += "    \"first_min_partial_amount\": 10000,";
            paymentreq += "    \"expire_by\": 1691097057,";
            paymentreq += "    \"reference_id\": \"Irecord000" + rowIDENTITY + "\",";
            paymentreq += "    \"description\": \"" + dtgetwayInfo.Rows[0]["DESCRIPTION"].ToString() + "\",";
            paymentreq += "    \"customer\": {";
            paymentreq += "        \"name\": \"" + dtUserInfo.Rows[0]["SubscriberName"] + "\",";
            paymentreq += "        \"contact\": \"+91" + dtUserInfo.Rows[0]["MobileNo"] + "\",";
            paymentreq += "        \"email\": \"" + dtUserInfo.Rows[0]["EmailID"] + "\"";
            paymentreq += "    },";
            paymentreq += "    \"notify\": {";
            paymentreq += "        \"sms\": true,";
            paymentreq += "        \"email\": true";
            paymentreq += "    },";
            paymentreq += "    \"reminder_enable\": true,";
            paymentreq += "    \"notes\": {";
            paymentreq += "        \"policy_name\": \"" + dtgetwayInfo.Rows[0]["POLICY_NAME"].ToString() + "\"";
            paymentreq += "    },";
            paymentreq += "    \"callback_url\": \"https://example-callback-url.com/?HId=" + rowIDENTITY + "\",";
            paymentreq += "    \"callback_method\": \"get\"";
            paymentreq += "}";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
             System.Security.Cryptography.X509Certificates.X509Chain chain,
             System.Net.Security.SslPolicyErrors sslPolicyErrors)
{
  return true; // **** Always accept
};
            var client = new RestClient(dtgetwayInfo.Rows[0]["POST_URL"].ToString());
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic "+ dtgetwayInfo.Rows[0]["BASIC_BASE64STR"].ToString());
            request.AddHeader("Content-Type", "application/json");
            var body = paymentreq;
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            PaymentGetwayRespose.Rootobject objdata = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentGetwayRespose.Rootobject>(response.Content);
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_PAYMENT_HANDLER]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATE_PAYMENT_RESPOSE_LINK";
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = rowIDENTITY;
            cmd.Parameters.Add("@USER_ID", SqlDbType.Int).Value = Session["UserID"].ToString();
            cmd.Parameters.Add("@PAYMENT_REQ", SqlDbType.NVarChar).Value = paymentreq;
            cmd.Parameters.Add("@PAYMENT_RES", SqlDbType.NVarChar).Value = response.Content;
            cmd.Parameters.Add("@PAYMENT_LINK", SqlDbType.NVarChar).Value = objdata.short_url;
            cmd.Parameters.Add("@PAYMENT_STATUS", SqlDbType.Int).Value = 0;
            //  cmd.Parameters.Add("@CALL_BACK_RES", SqlDbType.NVarChar).Value = yourCShap_variable;
            cmd.Parameters.Add("@CREATED_BY", SqlDbType.Int).Value = Session["UserID"].ToString();
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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
            Response.Redirect(objdata.short_url);
            return View();
        }
        public DataTable GetUserInfo()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_PAYMENT_HANDLER]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEW_GET_USER_INFO";
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = Session["UserID"].ToString();
             if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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
        public string GetSCOPE_IDENTITY()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_PAYMENT_HANDLER]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
            cmd.Parameters.Add("@USER_ID", SqlDbType.Int).Value = Session["UserID"].ToString();
            cmd.Parameters.Add("@PAYMENT_REQ", SqlDbType.NVarChar).Value = "";
            cmd.Parameters.Add("@PAYMENT_RES", SqlDbType.NVarChar).Value = "";
            cmd.Parameters.Add("@PAYMENT_LINK", SqlDbType.NVarChar).Value = "";
            cmd.Parameters.Add("@PAYMENT_STATUS", SqlDbType.Int).Value = 0;
            //  cmd.Parameters.Add("@CALL_BACK_RES", SqlDbType.NVarChar).Value = yourCShap_variable;
            cmd.Parameters.Add("@CREATED_BY", SqlDbType.Int).Value = Session["UserID"].ToString();
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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
            return DT.Rows[0]["ROWID"].ToString();
        }
        public DataTable getPaymentInfo()
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_PAYMENT_HANDLER]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEW_PAYMENT_DEATILS";
         
            if (con.State == ConnectionState.Open) con.Close();
            if (con.State == ConnectionState.Closed) con.Open();
            DataTable DT = new DataTable();
            try
            {
                //con.Open();
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
    }
}