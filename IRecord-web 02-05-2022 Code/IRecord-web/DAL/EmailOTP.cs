using BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace DAL
    {
    public class EmailOTP
        {
        public static void Mail(Subscriber sb)
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

        public static void MobileOTP()
            {
            //SMS.APIType = SMSGateway.Site2SMS;
            //SMS.MashapeKey = "<Mashape API Key>";
            //SMS.Username = txtNumber.Text.Trim();
            //SMS.Password = txtPassword.Text.Trim();
            //if (txtRecipientNumber.Text.Trim().IndexOf(",") == -1)
            //    {
            //    //Single SMS
            //    SMS.SendSms(txtRecipientNumber.Text.Trim(), txtMessage.Text.Trim());
            //    }

            }

        }
    }

