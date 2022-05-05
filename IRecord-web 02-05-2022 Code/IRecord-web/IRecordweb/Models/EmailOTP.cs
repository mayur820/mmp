using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace IRecordweb.Models
{
    public class EmailOTP
    {
        public void Mail(SUBSCRIBER sb)
        {
            try
            {
                String To = sb.EmailID;
                String From = "algebratechnologies21@gmail.com";
                MailMessage mail = new MailMessage(From, To);
                string mailbody = "Your OTP code is -" + sb.EmailOTP;//+ OTP;
                mail.Subject = "Subscriber Verification Code";
                mail.Body = mailbody;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("algebratechnologies21@gmail.com", "atpl@123");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(mail);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception exc)
            {
            }
        }
        public static void OnbMail(MEMBER mm)
        {
            try
            {
                String To = mm.EmailID;
                String From = "algebratechnologies21@gmail.com";
                MailMessage mail = new MailMessage(From, To);
                string mailbody = "Your OTP code is -" + mm.EmailOTP;//+ OTP;
                mail.Subject = "Subscriber Verification Code";
                mail.Body = mailbody;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("algebratechnologies21@gmail.com", "atpl@123");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(mail);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception exc)
            {
            }
        }
        public void SendmailWithEmail(string Mailid, string Otp)
        {
            try
            {
                String To = Mailid;
                String From = "algebratechnologies21@gmail.com";
                MailMessage mail = new MailMessage(From, To);
                string mailbody = "Your OTP code is -" + Otp;//+ OTP;
                mail.Subject = "Subscriber Verification Code";
                mail.Body = mailbody;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("algebratechnologies21@gmail.com", "atpl@123");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(mail);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception exc)
            {
            }
        }
        public void SMS_MobileOTP(string MobileNumber, string OTP, string Name)
        {
            // SUBSCRIBER  = new SUBSCRIBER();
            mobileotpModel.Rootobject obj = new mobileotpModel.Rootobject();
            obj.listsms = new mobileotpModel.Listsms()
            {
                accountusagetypeid = "1",
                sms = "Dear " + Name + " Your OTP is " + OTP + " to complete registration. Regards iRecord",
                mobiles = "+91" + MobileNumber + "",
                senderid = "iRECOD",
                clientSMSID = "1947692308",
                entityid = "1201162221100747478",
                tempid = "1207162410360646501"
            };
            obj.password = "f7feb3452bXX";
            obj.user = "IRecordS";

            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient("http://mobicomm.dove-sms.com/REST/sendsms/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = new JavaScriptSerializer().Serialize(obj);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //   Console.WriteLine(response.Content);


        }

        public void UserNmPassMail(string EmailID, string MobileNo)
        {
            try
            {
                String To = EmailID;
                String From = "algebratechnologies21@gmail.com";
                MailMessage mail = new MailMessage(From, To);
                string mailbody = "Your Username is -" + EmailID + " & Default Password is -" + MobileNo;//+ OTP;
                mail.Subject = "Useername & Password";
                mail.Body = mailbody;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("algebratechnologies21@gmail.com", "atpl@123");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(mail);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception exc)
            {
            }
        }
    }
}