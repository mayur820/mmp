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
    public class PaymantGetwayController : Controller
    {
        string PaymentHandlerHost = ConfigurationManager.AppSettings["PaymentHandlerHost"];
        // GET: PaymantGetway
        public ActionResult PaymentCheckOut()
        {

       
            return View();
        }
        public JsonResult GetValidApplyCoupon(string Coupon)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_M_COUPON]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEWBYCOUPON";
            cmd.Parameters.Add("@CouponCode", SqlDbType.NVarChar).Value = Coupon.ToString();
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
            if (DT.Rows.Count>0)
            {
                return Json(Convert.ToDecimal(DT.Rows[0]["Percentage"].ToString()), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }


        }
        public void  GoToPayment(string Amount,string QtyLicense,string CouponCode)
        {

            DataTable dtgetwayInfo = getPaymentInfo();
            string rowIDENTITY = GetSCOPE_IDENTITY();
            DataTable dtUserInfo = GetUserInfo();
            string paymentreq = "";
            paymentreq += "{";
            paymentreq += "    \"amount\": "+ (Convert.ToDecimal(Amount)* 100) + ",";
            paymentreq += "    \"currency\": \"INR\",";
            paymentreq += "    \"accept_partial\": true,";
            paymentreq += "    \"first_min_partial_amount\": " + (Convert.ToDecimal(Amount) * 100) + ",";
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
            paymentreq += "    \"callback_url\": \"" + PaymentHandlerHost + "PaymantGetway/PaymentHandler?HId=" + rowIDENTITY + "\",";
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
            request.AddHeader("Authorization", "Basic " + dtgetwayInfo.Rows[0]["BASIC_BASE64STR"].ToString());
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
            cmd.Parameters.Add("@PAYMENT_AMOUNT", SqlDbType.Decimal).Value = Amount;
            cmd.Parameters.Add("@COUPON_CODE", SqlDbType.NVarChar).Value = CouponCode;
            cmd.Parameters.Add("@LICENSE_QTY", SqlDbType.Int).Value = QtyLicense;
            
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
        public ActionResult PaymentHandler(string HId, string razorpay_payment_id, string razorpay_payment_link_id, string razorpay_payment_link_reference_id, string razorpay_payment_link_status, string razorpay_signature)
        {
            string strres = "";
            strres += "HId:" + HId;
            strres += "@razorpay_payment_id:" + razorpay_payment_id;
            strres += "@razorpay_payment_link_id:" + razorpay_payment_link_id;
            strres += "@razorpay_payment_link_reference_id:" + razorpay_payment_link_reference_id;
            strres += "@razorpay_payment_link_status:" + razorpay_payment_link_status;
            strres += "@razorpay_signature:" + razorpay_signature;
           DataTable dt=  UpdatePaymentStatus(HId, razorpay_payment_link_status, strres);
            Session["PaymentAmount"] = dt.Rows[0]["PAYMENT_AMOUNT"].ToString();
            Session["TransactionID"] = razorpay_payment_link_reference_id;
            if (razorpay_payment_link_status == "paid")
            {
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                return RedirectToAction("PaymentFaild");
            }

        }

        public ActionResult PaymentSuccess()
        {

            return View();
        }
        public ActionResult PaymentFaild()
        {

            return View();
        }
        public DataTable UpdatePaymentStatus(string Id, string Paymentstatus, string paymentrespose)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[DBO].[SP_PAYMENT_HANDLER]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "UPDATE";
            cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = Id;

            cmd.Parameters.Add("@PAYMENT_STATUS", SqlDbType.Int).Value = (Paymentstatus == "paid" ? 1 : 0);
            cmd.Parameters.Add("@CALL_BACK_RES", SqlDbType.NVarChar).Value = paymentrespose;

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
            //return DT.Rows[0]["ROWID"].ToString();
        }
    }
}