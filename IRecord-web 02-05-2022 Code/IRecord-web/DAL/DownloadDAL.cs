using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BAL;
using System.Web;
using System.IO.Compression;

namespace DAL
    {
    public class DownloadDAL
        {
        public DataTable GetDate(string date)
            {
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@DayDate", date);
            dt = Adapter.ExecuteDataTable("GetDayDate", CommandType.StoredProcedure, Adapter.param(pcol));
            return dt;
            }

        public string GetDateBasedonInvestmentType(int SelectedInvestmentTypeIndex, string ExchangeType, DateTime dateString)
            {
            string selectedDate = "";
            if (SelectedInvestmentTypeIndex == 2)
                {
                if (ExchangeType == "BSE")
                    selectedDate = dateString.ToString("ddMMyy").ToUpper();
                else
                    selectedDate = dateString.ToString("ddMMMyyyy").ToUpper();
                }
            else if (SelectedInvestmentTypeIndex == 3 || SelectedInvestmentTypeIndex == 5 || SelectedInvestmentTypeIndex == 6 || SelectedInvestmentTypeIndex == 7)
                selectedDate = dateString.ToString("ddMMMyyyy").ToUpper();
            else if (SelectedInvestmentTypeIndex == 4)
                {
                selectedDate = dateString.ToString("ddMMMyyyy").ToUpper();
                }
            return selectedDate;
            }

        private static string GetURLForDownloadBhavCopy(string Date, int SelectedInvestmentTypeIndex, string ExchangeType, DateTime dateString)
            {
            string URL = "";
            string NCDEXdate = dateString.ToString("dd MM yyyy");
            NCDEXdate = NCDEXdate.Replace(" ", string.Empty);
            if (ExchangeType == "BSE")
                ExchangeType = "BSE";
            else
                ExchangeType = "NSE";

            //month = month.Substring(0, 3);
            if (SelectedInvestmentTypeIndex == 2)
                {
                if (ExchangeType == "BSE") // bse 
                    URL = "http://www.bseindia.com/download/BhavCopy/Equity/EQ" + Date + "_CSV.ZIP";
                else
                    URL = "https://www1.nseindia.com/content/historical/EQUITIES/" + dateString.Year
                            + "/" + Date.Substring(2, 3)
                            + "/cm" + Date + "bhav.csv.zip";

                }
            else if (SelectedInvestmentTypeIndex == 3) // F&O  // Currently We are supporting only NSE as BSE does not support F&O
                if (ExchangeType == "NSE") // Nse 
                    URL = @"https://www1.nseindia.com/content/historical/DERIVATIVES/" + dateString.Year
                            + "/" + Date.Substring(2, 3)
                            + "/fo" + Date + "bhav.csv.zip";
                else
                    URL = "";
            else if (SelectedInvestmentTypeIndex == 6) // Currency
                {
                string month = dateString.ToString("MM");
                Date = dateString.ToString("yyyy MM dd");
                Date = Date.Replace(" ", string.Empty);
                if ((ExchangeType == "BSE") || (ExchangeType == "NSE")) // Nse 
                    {
                    Date = dateString.ToString("dd MM yy ");
                    Date = Date.Replace(" ", string.Empty);
                    URL = @"https://www1.nseindia.com/archives/cd/bhav/CD_Bhavcopy" + Date + ".zip";
                    }
                else
                    URL = @"http://www.bseindia.com/bsedata/CIML_bhavcopy/CurrencyBhavCopy_" + Date + ".zip";
                }

            return URL;
            }


        private static string GetURLForServerBhavCopy(string Date, int SelectedInvestmentTypeIndex, string ExchangeType, DateTime dateString, bool IsFuture = false)
            {
            string URL = "";
            return URL;
            }

        public string GetURL(string Date, int SelectedInvestmentTypeIndex, string ExchangeType, DateTime dateString, bool IsDownLoad = false, bool IsFuture = false)
            {
            if (IsDownLoad)

                return GetURLForDownloadBhavCopy(Date, SelectedInvestmentTypeIndex, ExchangeType, dateString);
            else
                return GetURLForServerBhavCopy(Date, SelectedInvestmentTypeIndex, ExchangeType, dateString, IsFuture);
            }

        public string GetDownloadPath(string Date, int SelectedInvestmentTypeIndex, string ExchangeType, bool IsFuture)
            {
            string URL = "";
            string BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string sDailyRatePath = Path.Combine(BaseDirectoryPath, "DailyRates");
            Directory.CreateDirectory(sDailyRatePath);


            if (SelectedInvestmentTypeIndex == 2)
                {
                if (ExchangeType == "BSE") // bse 

                    URL = Path.Combine(sDailyRatePath, @"EQ" + Date + "_CSV.ZIP");

                else
                    // URL = Path.Combine(sDailyRatePath, @"cm" + Date + "_CSV.ZIP");  //@"D:\cm" + Date + "_CSV.ZIP";
                    URL = Path.Combine(sDailyRatePath, @"cm" + Date + "bhav.csv.ZIP");
                }
            else if (SelectedInvestmentTypeIndex == 3) // F&O
                URL = Path.Combine(sDailyRatePath, @"fo" + Date + "_CSV.ZIP"); //@"D:\fo" + Date + "_CSV.ZIP";
            else if (SelectedInvestmentTypeIndex == 4)
                URL = Path.Combine(sDailyRatePath, @"MFA" + Date + "_CSV.ZIP");
            else if (SelectedInvestmentTypeIndex == 5)
                URL = Path.Combine(sDailyRatePath, @"MCX" + Date + "_CSV.ZIP");
            else if (SelectedInvestmentTypeIndex == 6)
                {
                if (ExchangeType == "BSE") // bse 
                    {
                    if (IsFuture)
                        URL = Path.Combine(sDailyRatePath, @"Currency_BSE_Future" + Date + "_CSV.ZIP");
                    else
                        URL = Path.Combine(sDailyRatePath, @"Currency_BSE_Option" + Date + "_CSV.ZIP");
                    }
                else
                    {
                    if (IsFuture)
                        URL = Path.Combine(sDailyRatePath, @"Currency_NSE_Future" + Date + "_CSV.ZIP");
                    else
                        URL = Path.Combine(sDailyRatePath, @"Currency_NSE_Option" + Date + "_CSV.ZIP");
                    }
                }
            else if (SelectedInvestmentTypeIndex == 5)
                URL = Path.Combine(sDailyRatePath, @"NCDEX" + Date + "_CSV.ZIP");

            //   Directory.GetFiles(URL);
            return URL;
            }
        public bool DownloadBhavCopy(int segment, string BSEOrNSECode, DateTime date)
            {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            WebClient Client = new WebClient();

            string selectedDate = GetDateBasedonInvestmentType(segment, BSEOrNSECode, date);
            string URL = GetURL(selectedDate, segment, BSEOrNSECode, date, true);
            HttpContext.Current.Session["URL"] = URL;
            string sDowinloadPath = GetDownloadPath(selectedDate, segment, BSEOrNSECode, false);
            HttpContext.Current.Session["sDowinloadPath"] = sDowinloadPath;
            string FileName = Path.GetFileName(sDowinloadPath);
            HttpContext.Current.Session["FileName"] = FileName;
            // string[] Filename = sDowinloadPath.Split('\\', '6' );


            //if (File.Exists(sDowinloadPath))
            //    File.Delete(sDowinloadPath);

            Client.Headers.Add("user-agent", "Only a test!");

            try
                {


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                req.Method = "GET";
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                //req.

                // Skip validation of SSL/TLS certificate
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                //Start 
                ServicePointManager.ServerCertificateValidationCallback += delegate (
                object sender,
                X509Certificate cert,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
                    {
                        if (sslPolicyErrors == SslPolicyErrors.None)
                            {
                            return true;   //Is valid
                            }

                        if (cert.GetCertHashString() == "99E92D8447AEF30483B1D7527812C9B7B3A915A7")
                            {
                            return true;
                            }

                        return false;
                        };

                //End

                string DirectoryPath1 = ExtractZipFiles(sDowinloadPath);
                foreach (string file in Directory.GetFiles(DirectoryPath1))
                    {
                    if (segment == 3)//F&O
                        {
                        CommonFn.SaveFNODailyRates(file, date, segment);
                        }
                    else
                        {
                        //  objCss.SaveDailyRates(file, date, segment, BSE_Or_NSECode, true);
                        }
                    }

                WebResponse respon = req.GetResponse();
                Stream res = respon.GetResponseStream();

                string ret = "";
                byte[] buffer = new byte[1048];
                int read = 0;
                while ((read = res.Read(buffer, 0, buffer.Length)) > 0)
                    {
                    //Console.Write(Encoding.ASCII.GetString(buffer, 0, read));
                    ret += Encoding.ASCII.GetString(buffer, 0, read);
                    }
                //return ret;
                File.WriteAllBytes(sDowinloadPath, buffer);
                }
            catch (Exception e)
                {
                //  return "Invalid Date";
                return false;
                }


            //////// //Commented by Poonam 29. 04 . 2021 Thursday
          //  string DirectoryPath = ExtractZipFiles(sDowinloadPath);

            //foreach (string filedt in Directory.GetFiles(DirectoryPath))
            //    {
            //    HttpPostedFileBase file = Directory.GetFiles(DirectoryPath)[filedt.ToString()] as HttpPostedFileBase;
            //    if (segment == 1)//F&O
            //        {
            //        CommonFn.SaveDailyRates(file, date, segment);
            //        }
            //    else
            //        {
            //        SaveDailyRates(file, date, segment, BSEOrNSECode, true);
            //        }
            //    }
            return true;
            }

        public string ExtractZipFiles(string sDowinloadPath)
            {
            //string path = "";
            //string filename = Path.GetFileName(sDowinloadPath);
            //ZipStorer zip = ZipStorer.Open(filename, FileAccess.Read);
            //List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

            //foreach (ZipStorer.ZipFileEntry entry in dir)
            //    {
            //    path = Path.Combine(Path.GetDirectoryName(sDowinloadPath), Path.GetFileNameWithoutExtension(sDowinloadPath), Path.GetFileName(entry.FilenameInZip));
            //    if (File.Exists(path))
            //        File.Delete(path);
            //    zip.ExtractFile(entry, path);
            //    }
            //zip.Close();
            //return Path.GetDirectoryName(path);
            string result = "";
            string filename = Path.GetFileName(sDowinloadPath);

            string zipPath =sDowinloadPath;
            string extractPath = "~/DailyRates/";

            //System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath);
            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
            //try
            //    {

            //    using (ZipArchive za = ZipFile.OpenRead(filename))
            //        {
            //        foreach (ZipArchiveEntry zaItem in za.Entries)
            //            {
            //            if (zaItem.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            //                {
            //                result = "Success";
            //                }
            //            else
            //                {
            //                result = "No ico files has been found";
            //                }
            //            }
            //        }
               return sDowinloadPath;
            //    }
            ////  return result;
            //catch {
            //    result = "Oops!. Something went wrong";
            //    return result;
            //    }

            }

        public void DeleteFile(string sDowinloadPath1)
            {
            if (File.Exists(sDowinloadPath1))
                File.Delete(sDowinloadPath1);
            }

        }
    }
