using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace IRecordweb.Models
{
    public class clsLogin
    {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        public void UserLogin(USER _user)
        {
            USER objdata = new USER();
            objdata.UserName = _user.UserName;
            objdata.Password = _user.Password;
            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objdata);
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
               System.Security.Cryptography.X509Certificates.X509Chain chain,
               System.Net.Security.SslPolicyErrors sslPolicyErrors)
    {
        return true; // **** Always accept
    };
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/USERLOGINMASTER?DBAction=View&ID=0");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            HttpContext.Current.Session["DT_UserID_Info"] = data.Tables[0];
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                USER item = new USER();

                HttpContext.Current.Session["UserID"] = dr["UserId"].ToString();
                HttpContext.Current.Session["UserName"] = dr["UserName"].ToString();
                HttpContext.Current.Session["Password"] = dr["Password"].ToString();
                HttpContext.Current.Session["SubscriberID"] = dr["SubscriberID"].ToString();
                HttpContext.Current.Session["MemberSubscription"] = dr["MemberSubscription"].ToString();
                HttpContext.Current.Session["MemberCount"] = dr["MemberCount"].ToString();
                HttpContext.Current.Session["RoleId"] = dr["RoleId"].ToString();
                HttpContext.Current.Session["RoleName"] = dr["Name"].ToString();
                if (dr["Role_Type"] != null)
                {
                    if (dr["Role_Type"].ToString() == "Operator")
                    {
                        HttpContext.Current.Session["Act_User_ID"] = dr["Opertor_ID"].ToString();
                    }
                    if (dr["Role_Type"].ToString() == "Member")
                    {
                        HttpContext.Current.Session["Act_User_ID"] = dr["Memeber_ID"].ToString();
                    }

                }

            }
        }
        public DataTable getSubscriberDetails(string ID)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_USERLOGINDATA_NEW]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewSubscriber";
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = ID;
            // cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = objdata.Password;

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
        public DataTable getOpartorsDetails(string ID)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_USERLOGINDATA_NEW]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewbyOprator";
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = ID;
            // cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = objdata.Password;

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
        public void UserLoginNew(USER _user)
        {
            USER objdata = new USER();
            objdata.UserName = _user.UserName;
            HttpContext.Current.Session["UserNm"] = objdata.UserName;
            objdata.Password = _user.Password;
            String strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[SP_USERLOGINDATA_NEW]";
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEW";
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = objdata.UserName;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = objdata.Password;

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
            if (DT.Rows.Count > 0)
            {
                HttpContext.Current.Session["UserID"]= DT.Rows[0]["UserId"].ToString();
                HttpContext.Current.Session["UserName"] = DT.Rows[0]["UserName"].ToString();
                HttpContext.Current.Session["MemberSubscription"] = DT.Rows[0]["MemberSubscription"];
                HttpContext.Current.Session["MemberCount"] = DT.Rows[0]["MemberCount"];
                HttpContext.Current.Session["OnboardingPhases"] = DT.Rows[0]["OnboardingPhases"].ToString();
                HttpContext.Current.Session["Role_Type"] = DT.Rows[0]["Role_Type"].ToString();
                HttpContext.Current.Session["RoleId"] = DT.Rows[0]["RoleId"].ToString();
                HttpContext.Current.Session["UserLoginData1"] = DT;
                if (DT.Rows[0]["Role_Type"] == DBNull.Value)
                { ////Login as Subscriber
                    DataTable dt = getSubscriberDetails(DT.Rows[0]["UserId"].ToString());
                    HttpContext.Current.Session["SUBSC_OPR_NAME"] = dt.Rows[0]["UserName"].ToString();
                    HttpContext.Current.Session["SUBSC_OPR_ID"] = dt.Rows[0]["UserId"].ToString();
                    HttpContext.Current.Session["Login_Flags"] = "1";
                    //Flags 1=Subscriber,2=Opartors,3=Member
                }
                if (DT.Rows[0]["Role_Type"] != DBNull.Value)
                {
                    if (DT.Rows[0]["Role_Type"].ToString() == "Operator")
                    {
                        ////Login as Opartors

                        DataTable dt = getOpartorsDetails(DT.Rows[0]["UserId"].ToString());
                        HttpContext.Current.Session["SUBSC_OPR_NAME"] = dt.Rows[0]["UserName"].ToString();
                        HttpContext.Current.Session["SUBSC_OPR_ID"] = dt.Rows[0]["UserId"].ToString();
                        HttpContext.Current.Session["Login_Flags"] = "2";
                    }
                    if (DT.Rows[0]["Role_Type"].ToString() == "Member")
                    {
                        ////Login as Member

                     //   DataTable dt = getOpartorsDetails(DT.Rows[0]["UserId"].ToString());
                        HttpContext.Current.Session["SUBSC_OPR_NAME"] = "";
                        HttpContext.Current.Session["SUBSC_OPR_ID"] = "";
                        HttpContext.Current.Session["Login_Flags"] = "3";
                    }
                }
            }


            //foreach (DataRow dr in DT.Rows)
            //{

            //    USER item = new USER();

            //    HttpContext.Current.Session["UserID"] = dr["UserId"].ToString();
            //    HttpContext.Current.Session["UserName"] = dr["UserName"].ToString();
            //    HttpContext.Current.Session["Password"] = dr["Password"].ToString();
            //    HttpContext.Current.Session["SubscriberID"] = dr["SubscriberID"].ToString();
            //    HttpContext.Current.Session["MemberSubscription"] = dr["MemberSubscription"].ToString();
            //    HttpContext.Current.Session["MemberCount"] = dr["MemberCount"].ToString();
            //    HttpContext.Current.Session["RoleId"] = dr["RoleId"].ToString();
            //    HttpContext.Current.Session["RoleName"] = dr["Name"].ToString();
            //    if (dr["Role_Type"] != null)
            //    {
            //        if (dr["Role_Type"].ToString() == "Operator")
            //        {
            //            HttpContext.Current.Session["Act_User_ID"] = dr["Opertor_ID"].ToString();
            //        }
            //        if (dr["Role_Type"].ToString() == "Member")
            //        {
            //            HttpContext.Current.Session["Act_User_ID"] = dr["Memeber_ID"].ToString();
            //        }

            //    }

            //}
        }
        public void SingleUserLogin(DataTable dt)
        {

            foreach (DataRow dr in dt.Rows)
            {
                USER item = new USER();

                HttpContext.Current.Session["UserID"] = dr["UserId"].ToString();
                HttpContext.Current.Session["UserName"] = dr["UserName"].ToString();
                HttpContext.Current.Session["Password"] = dr["Password"].ToString();
                HttpContext.Current.Session["SubscriberID"] = dr["SubscriberID"].ToString();
                HttpContext.Current.Session["MemberSubscription"] = dr["MemberSubscription"].ToString();
                HttpContext.Current.Session["MemberCount"] = dr["MemberCount"].ToString();
                HttpContext.Current.Session["RoleId"] = dr["RoleId"].ToString();
                HttpContext.Current.Session["RoleName"] = dr["Name"].ToString();
                if (dr["Role_Type"] != null)
                {
                    if (dr["Role_Type"].ToString() == "Operator")
                    {
                        HttpContext.Current.Session["Act_User_ID"] = dr["Opertor_ID"].ToString();
                    }
                    if (dr["Role_Type"].ToString() == "Member")
                    {
                        HttpContext.Current.Session["Act_User_ID"] = dr["Memeber_ID"].ToString();
                    }

                }

            }
        }
    }
}