using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Security.Cryptography;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Vml;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using IrecordDAL;
using CsvHelper;

namespace IRecordweb.Controllers
    {
    public class ScriptDataFetchController : Controller
        {
        string URL = ConfigurationManager.AppSettings["ScreenURL"];
        string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        // GET: ScriptDataFetch
        public ActionResult Index()
            {
            return View();
            }
        public List<MTYPE> BindInvenstmentType()
            {
            var ID = 1;
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/MTYPEMASTER?DBAction=ViewById&ID=" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<MTYPE> list = new List<MTYPE>();
            foreach (DataRow dr in data.Tables[0].Rows)
                {
                MTYPE item = new MTYPE();
                item.TypeId = Convert.ToInt32(dr["TypeId"].ToString());
                item.Code = dr["Code"].ToString();
                item.Name = dr["Name"].ToString();
                item.Active = Convert.ToBoolean(dr["Active"].ToString());
                list.Add(item);
                }
            return list;
            }
        [HttpGet]
        public ActionResult ScriptData()
            {
            ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            return View();
            }
        [HttpPost]
        public ActionResult ScriptData(SCRIPTDATA _scriptdata)
            {
            ViewBag.InvestmentType = new SelectList(BindInvenstmentType().ToList(), dataValueField: "TypeId", dataTextField: "Name");
            string dt = _scriptdata.Date.ToString("yyyy-MM-dd");
            string closing = "";
            try
                {
                DataSet dtdata = GetDate(dt);

                if (dtdata.Tables[0].Rows.Count > 0)
                    {
                    closing = dtdata.Tables[0].Rows[0]["Date"].ToString();
                    }

                if (closing.Contains("Sunday") || closing.Contains("Saturday"))
                    {
                    ViewBag.Message = "Holiday on selected Date";
                    return RedirectToAction("ScriptData");
                    }
                string date = _scriptdata.Date.ToString();
                string selectedDate = GetDateBasedonInvestmentType(_scriptdata.InvestmentType, _scriptdata.Exchange, _scriptdata.Date);
                string sDowinloadPath = GetDownloadPath(selectedDate, _scriptdata.InvestmentType, _scriptdata.Exchange, false);
                Session["sDowinloadPath"] = sDowinloadPath;
                string FileName = System.IO.Path.GetFileName(sDowinloadPath);
                Session["filename"] = FileName;
                if (_scriptdata.InvestmentType == "2")
                    {
                    EquityData(selectedDate);
                    using (WebClient client1 = new WebClient())
                        {
                        client1.DownloadFile(Session["URL"].ToString(), Server.MapPath("~/AllScriptData/" + Session["filename"]));
                        string zipPath = Session["sDowinloadPath"].ToString();
                        string FileName1 = System.IO.Path.GetFileName(zipPath);
                        string extractPath = Server.MapPath("~/AllScriptData/");

                        if (System.IO.File.Exists(extractPath))
                            {
                            System.IO.File.Delete(extractPath);
                            }
                            {
                            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
                            }
                        foreach (string file in Directory.GetFiles(extractPath))
                            {

                            SaveDailyRates(file, _scriptdata.Date, _scriptdata.InvestmentType, _scriptdata.Exchange, true);

                            }

                        }
                    ViewBag.Message = "BSE Successfully Uploaded";

                    }
                }
            catch (Exception ex)
                {
                throw;
                }


            return View();
            }

        public void EquityData(string Date)
            {

            try
                {

                ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //string URL = "https://www.bseindia.com/download/BhavCopy/Equity/" + "EQ" + Date + "_CSV.ZIP";
                string fileName = "";
                WebClient Client = new WebClient();
                string URL = "https://www.bseindia.com/corporates/List_Scrips.aspx?expandable=1";
                Session["URL"] = URL;
                if (URL != "")
                    {
                    HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //using (CsvReader csvReader = new CsvReader(response.GetResponseStream, true))
                    //    {
                    //    int fieldCount = csvReader.FieldCount;
                    //    string[] headers = csvReader.GetFieldHeaders();

                    //    while (csvReader.ReadNextRecord())
                    //        {
                    //        //Do work with CSV file data here
                    //        }
                    //    }
                    string sDownloadPath = GetDownloadPath(fileName);
                    Client.Headers.Add("user-agent", "Only a test!");
                    Client.DownloadFile(URL, sDownloadPath);

                   
                    }
                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                //req.Method = "GET";
                //ServicePointManager.ServerCertificateValidationCallback += delegate (
                //object sender,
                //X509Certificate cert,
                //X509Chain chain,
                //SslPolicyErrors sslPolicyErrors)
                //    {
                //        if (sslPolicyErrors == SslPolicyErrors.None)
                //            {
                //            return true;   //Is valid
                //            }

                //        if (cert.GetCertHashString() == "99E92D8447AEF30483B1D7527812C9B7B3A915A7")
                //            {
                //            return true;
                //            }

                //        return false;
                //        };

                //WebResponse respon = req.GetResponse();
                //Stream res = respon.GetResponseStream();

                //string ret = "";
                //byte[] buffer = new byte[1048];
                //int read = 0;
                //while ((read = res.Read(buffer, 0, buffer.Length)) > 0)
                //    {
                //    ret += Encoding.ASCII.GetString(buffer, 0, read);
                //    }
                //if (System.IO.File.Exists(Session["sDowinloadPath"].ToString()))
                //    System.IO.File.Delete(Session["sDowinloadPath"].ToString());
                //System.IO.File.WriteAllBytes(Session["sDowinloadPath"].ToString(), buffer);
                }
            catch (Exception e)
                {
                throw e;
                }

            }

        private static string GetDownloadPath(string fileName)
            {
            string BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string sServerPath = System.IO.Path.Combine(BaseDirectoryPath, "AllScriptData");
            Directory.CreateDirectory(sServerPath);

            string sDownloadPath = System.IO.Path.Combine(sServerPath, "scriptmaster.csv");

            if (System.IO.File.Exists(sDownloadPath))
                System.IO.File.Delete(sDownloadPath);
            return sDownloadPath;
            }
        public string GetDownloadPath(string Date, string SelectedInvestmentTypeIndex, string ExchangeType, bool IsFuture)
            {
            string URL = "";
            string BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string sDailyRatePath = System.IO.Path.Combine(BaseDirectoryPath, "AllScriptData");
            Directory.CreateDirectory(sDailyRatePath);


            if (SelectedInvestmentTypeIndex == "2")
                {
                if (ExchangeType == "BSE") // bse 

                    URL = System.IO.Path.Combine(sDailyRatePath, @"EQ" + Date + "_CSV.ZIP");

                else
                    // URL = Path.Combine(sDailyRatePath, @"cm" + Date + "_CSV.ZIP");  //@"D:\cm" + Date + "_CSV.ZIP";
                    URL = System.IO.Path.Combine(sDailyRatePath, @"cm" + Date + "bhav.csv.ZIP");
                }
            else if (SelectedInvestmentTypeIndex == "3") // F&O
                URL = System.IO.Path.Combine(sDailyRatePath, @"fo" + Date + "_CSV.ZIP"); //@"D:\fo" + Date + "_CSV.ZIP";
            else if (SelectedInvestmentTypeIndex == "4")
                URL = System.IO.Path.Combine(sDailyRatePath, @"MFA" + Date + "_CSV.ZIP");
            else if (SelectedInvestmentTypeIndex == "5")
                URL = System.IO.Path.Combine(sDailyRatePath, @"MCX" + Date + "_CSV.ZIP");
            else if (SelectedInvestmentTypeIndex == "6")
                {
                if (ExchangeType == "BSE") // bse 
                    {
                    if (IsFuture)
                        URL = System.IO.Path.Combine(sDailyRatePath, @"Currency_BSE_Future" + Date + "_CSV.ZIP");
                    else
                        URL = System.IO.Path.Combine(sDailyRatePath, @"Currency_BSE_Option" + Date + "_CSV.ZIP");
                    }
                else
                    {
                    if (IsFuture)
                        URL = System.IO.Path.Combine(sDailyRatePath, @"Currency_NSE_Future" + Date + "_CSV.ZIP");
                    else
                        URL = System.IO.Path.Combine(sDailyRatePath, @"Currency_NSE_Option" + Date + "_CSV.ZIP");
                    }
                }
            else if (SelectedInvestmentTypeIndex == "5")
                URL = System.IO.Path.Combine(sDailyRatePath, @"NCDEX" + Date + "_CSV.ZIP");

            //   Directory.GetFiles(URL);
            return URL;
            }
        public string GetDateBasedonInvestmentType(string SelectedInvestmentTypeIndex, string ExchangeType, DateTime dateString)
            {
            string selectedDate = "";
            if (SelectedInvestmentTypeIndex == "2")
                {
                if (ExchangeType == "BSE")
                    selectedDate = dateString.ToString("ddMMyy").ToUpper();
                else
                    selectedDate = dateString.ToString("ddMMMyyyy").ToUpper();
                }
            else if (SelectedInvestmentTypeIndex == "3" || SelectedInvestmentTypeIndex == "5" || SelectedInvestmentTypeIndex == "6" || SelectedInvestmentTypeIndex == "7")
                selectedDate = dateString.ToString("ddMMMyyyy").ToUpper();
            else if (SelectedInvestmentTypeIndex == "4")
                {
                selectedDate = dateString.ToString("ddMMMyyyy").ToUpper();
                }
            return selectedDate;
            }
        public DataSet GetDate(string Date)
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(URL + "api/Master/SCRIPTDTAFETCHMASTER?DBAction=Getdate&ID=" + Date);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<SCRIPTDATA> list = new List<SCRIPTDATA>();
            foreach (DataRow dr in data.Tables[0].Rows)
                {
                SCRIPTDATA item = new SCRIPTDATA();
                item.Date = Convert.ToDateTime(dr["Date"].ToString());
                list.Add(item);
                }
            return data;

            }
        public bool SaveDailyRates(string FilePath, DateTime dateString, string SelectedInvestmentTypeIndex, string ExchangeTypeIndex, bool IsDownloadClick = false)
            {
            try
                {
                SqlConnection con = Adapter.Connection;
                con.Open();
                bool IsValid = true;

                if (IsValid)
                    {
                    string Data = System.IO.File.ReadAllText(FilePath);
                    string[] ar = new string[14];
                    string sHighRate;
                    SqlCommand cmd = null;
                    SqlDataReader datReader = null;
                    string sExchangeCode = GetExchangeCodeColumn(SelectedInvestmentTypeIndex, ExchangeTypeIndex);
                    bool IsFuture = false;
                    IsFuture = Data.Split('\n')[1].ToUpper().Contains("FUTCUR");
                    string sExchangeRate = GetExchangeRate(SelectedInvestmentTypeIndex, ExchangeTypeIndex, IsFuture);
                    if (SelectedInvestmentTypeIndex == "2" && ExchangeTypeIndex == "BSE")
                        sHighRate = "BSEHighRate";
                    else
                        sHighRate = "NSEHighRate";

                    string bsensecode = "";
                    foreach (string row in Data.Split('\n'))
                        {
                        string r = row;
                        r = r.Replace("\r", "");

                        if (r != " ")
                            if (!string.IsNullOrEmpty(row))
                                {
                                if (SelectedInvestmentTypeIndex == "4")
                                    ar = row.Split(';');
                                else
                                    ar = row.Split(',');

                                string sCommand = "";
                                string Closevalue = "";
                                string Highvalue = "";
                                if (((ar.Length > 5 && ar.Length < 9) && SelectedInvestmentTypeIndex == "4") || (SelectedInvestmentTypeIndex != "4"))
                                    {
                                    string sValue = GetRate(SelectedInvestmentTypeIndex, ExchangeTypeIndex, ar);
                                    if (SelectedInvestmentTypeIndex != "4")
                                        {
                                        if (ExchangeTypeIndex == "BSE")
                                            {
                                            string[] combindvalues = sValue.Split(',');
                                            int index = combindvalues.Length;
                                            if (SelectedInvestmentTypeIndex == "5")
                                                {
                                                Closevalue = combindvalues[0];
                                                }
                                            else
                                                {
                                                Highvalue = combindvalues[0];
                                                Closevalue = combindvalues[1];
                                                }
                                            }
                                        else
                                            {
                                            if (SelectedInvestmentTypeIndex == "2")
                                                {
                                                //if(ar[0]=="HMT")
                                                //{
                                                //    MessageBox.Show("Find script");
                                                //}
                                                //if (ar[0] == "SIL")
                                                //{
                                                //    MessageBox.Show("d");
                                                //}


                                                if (SelectedInvestmentTypeIndex == "2" && ExchangeTypeIndex == "NSE" && (ar[1] == "EQ" || ar[1] == "BZ" || ar[1] == "BE" || ar[1] == "E1"))
                                                    {
                                                    string[] combindvalues = sValue.Split(',');
                                                    Highvalue = combindvalues[0];
                                                    Closevalue = combindvalues[1];
                                                    }
                                                }
                                            else
                                                {
                                                string[] combindvalues = sValue.Split(',');
                                                Closevalue = combindvalues[0];
                                                }
                                            }
                                        }
                                    else
                                        Closevalue = sValue.Split(',')[0];

                                    DataTable Dtdata = GetSelectCommand(ar, sExchangeCode, ref bsensecode, ref sCommand, SelectedInvestmentTypeIndex);


                                    if (Dtdata.Rows.Count > 0)
                                        {

                                        string scrrp = Dtdata.Rows[0]["BSECode"].ToString();


                                        /*fetch scriptcode if exists*/
                                        if (scrrp != "")
                                            {
                                            int iCount = 0;
                                            cmd = null;
                                            string sQuery = "";
                                            if (SelectedInvestmentTypeIndex == "2") // Equity
                                                {
                                                if (SelectedInvestmentTypeIndex == "1" && (ar[1] == "EQ" || ar[1] == "BZ" || ar[1] == "BE" || ar[1] == "E1"))
                                                    {
                                                    //CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, _Bhavcopy.InvestmentType);
                                                    //SaveEquity(con, out cmd, sExchangeRate, Closevalue, scrrp, iCount, out sQuery, dateString, _Bhavcopy.InvestmentType, Highvalue, sHighRate);
                                                    }
                                                else if (SelectedInvestmentTypeIndex == "2")
                                                    {

                                                    string Transdate = dateString.ToString();
                                                    CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, SelectedInvestmentTypeIndex);
                                                    SaveEquity(sExchangeRate, Closevalue, scrrp, iCount, out sQuery, Transdate, SelectedInvestmentTypeIndex, Highvalue, sHighRate);

                                                    }
                                                }
                                            //                    else if (SelectedInvestmentTypeIndex != 2 && SelectedInvestmentTypeIndex != 4) // F & O
                                            //                        {
                                            //                        string sExpireDate = ""; string sStrikePrice = ""; string sOptionType = ""; string Script_Name = "";
                                            //                        if (SelectedInvestmentTypeIndex == 5)     //MCX
                                            //                            Closevalue = GetMCXDataField(ar, Closevalue, out sExpireDate, out sStrikePrice, out sOptionType, out Script_Name);
                                            //                        else if (SelectedInvestmentTypeIndex == 6)  //Currency
                                            //                            GetCurrencyDataField(ar, out sExpireDate, out sStrikePrice, out sOptionType, out Script_Name, IsFuture);
                                            //                        else if (SelectedInvestmentTypeIndex == 7)   // NCDX
                                            //                            sExpireDate = GetNCDEXFields(ar, ref Closevalue);

                                            //                        CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, SelectedInvestmentTypeIndex, sExpireDate, sOptionType, sStrikePrice, IsFuture);
                                            //                        if (iCount == 0)
                                            //                            {
                                            //                            if (SelectedInvestmentTypeIndex == 6)
                                            //                                {
                                            //                                ComFn.SaveDonldCurrencyData(dateString, SelectedInvestmentTypeIndex, scrrp, sExpireDate, sStrikePrice, sOptionType, Closevalue, Script_Name, sExchangeRate);
                                            //                                }
                                            //                            else
                                            //                                {
                                            //                                var Case1 = 3;
                                            //                                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                            //                                Adapter.AddParam(pcol1, "@Action", Case1);
                                            //                                Adapter.AddParam(pcol1, "@TransDate", dateString);
                                            //                                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                                            //                                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                                            //                                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                                            //                                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                                            //                                Adapter.AddParam(pcol1, "@InvestmentType", SelectedInvestmentTypeIndex);
                                            //                                Adapter.AddParam(pcol1, "@NSEFORate", Closevalue);
                                            //                                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                                            //                                Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));
                                            //                                }
                                            //                            }
                                            //                        else
                                            //                            {
                                            //                            if (SelectedInvestmentTypeIndex == 6)
                                            //                                {
                                            //                                ComFn.UpdateDownldCurrencyData(dateString, SelectedInvestmentTypeIndex, scrrp, sExpireDate, sStrikePrice, sOptionType, Closevalue, Script_Name, sExchangeRate);
                                            //                                }
                                            //                            else
                                            //                                {
                                            //                                var Case1 = 4;
                                            //                                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                            //                                Adapter.AddParam(pcol1, "@Action", Case1);
                                            //                                Adapter.AddParam(pcol1, "@TransDate", dateString);
                                            //                                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                                            //                                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                                            //                                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                                            //                                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                                            //                                Adapter.AddParam(pcol1, "@InvestmentType", SelectedInvestmentTypeIndex);
                                            //                                Adapter.AddParam(pcol1, "@NSEFORate", Closevalue);
                                            //                                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                                            //                                Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));
                                            //                                }
                                            //                            }
                                            //                        }
                                            }
                                        }
                                    }
                                }
                        }
                    }
                return IsValid;
                }
            catch (Exception ex)
                {
                //throw new Exception("Error while saving data." + ex.Message.ToString());
                }
            return false;
            }
        public string GetExchangeCodeColumn(string SelectedInvestmentTypeIndex, string SelectedExchangeTypeIndex)
            {
            string sCode = "";
            if (SelectedInvestmentTypeIndex == "2" || SelectedInvestmentTypeIndex == "3") // 2 = eQUITY AND 3 = f&o
                {
                if (SelectedExchangeTypeIndex == "BSE") // bse 
                    sCode = "BSE_Code";
                else
                    sCode = "NSE_Code";   // nse
                }

            else if (SelectedInvestmentTypeIndex == "4") // MF
                sCode = "BSE_Code";
            else if (SelectedInvestmentTypeIndex == "5") // MCX
                sCode = "NSE_Code";
            else if (SelectedInvestmentTypeIndex == "6") // Currency
                sCode = "NSE_Code";
            else if (SelectedInvestmentTypeIndex == "7") // NCDEX
                sCode = "NSE_Code";
            return sCode;
            }
        public string GetExchangeRate(string InvestmentTypeIndex, string ExchangeTypeIndex, bool IsFuture)
            {
            string sRate = "";
            if (InvestmentTypeIndex == "2") // 2 = eQUITY AND 3 = f&o
                {
                if (ExchangeTypeIndex == "BSE") // bse                 
                    sRate = "BSE_Rate";
                else
                    sRate = "NSE_Rate";   // nse                                 
                }
            else if (InvestmentTypeIndex == "6")
                {
                if (IsFuture)
                    {
                    if (ExchangeTypeIndex == "BSE") // bse 
                        sRate = "CurrrencyBSEFURate";
                    else
                        sRate = "CurrrencyNSEFURate";
                    }
                else
                    {
                    if (ExchangeTypeIndex == "BSE") // bse 
                        sRate = "CurrrencyBSEOPRate";
                    else
                        sRate = "CurrrencyNSEOPRate";
                    }
                }
            else if (InvestmentTypeIndex != "2") // F & 0 , NCDEX, MCX
                {
                if (InvestmentTypeIndex == "2" && ExchangeTypeIndex == "BSE") // bse 
                    sRate = "BSE_FO_Rate";
                else
                    sRate = "NSE_FO_Rate";
                }
            else
                sRate = "NSE_Rate";   // nse
            return sRate;
            }
        public static string GetRate(string SelectedInvestmentTypeIndex, string SelectedExchangeTypeIndex, string[] ar)
            {
            string sValue = "";
            string svalueHigh = "";
            if (SelectedInvestmentTypeIndex == "2") //Equity
                {
                if (SelectedExchangeTypeIndex == "BSE")
                    {
                    sValue = SelectedExchangeTypeIndex == "BSE" ? ar[7] : ar[5];
                    svalueHigh = ar[5].ToString();
                    svalueHigh = svalueHigh + "," + ar[7].ToString();
                    sValue = svalueHigh;
                    }
                else
                    {
                    sValue = SelectedExchangeTypeIndex == "NSE" ? ar[5] : ar[7];
                    svalueHigh = ar[3].ToString();
                    svalueHigh = svalueHigh + "," + ar[5].ToString();
                    sValue = svalueHigh;
                    }
                }
            else if (SelectedInvestmentTypeIndex == "4") //Mutual Fund*
                sValue = ar[4];
            else if (SelectedInvestmentTypeIndex == "5")  // MCX
                sValue = ar[9].Replace("\"", "");
            else if (SelectedInvestmentTypeIndex == "6")  //Currency
                sValue = ar[6].Replace("\"", "");
            else if (SelectedInvestmentTypeIndex == "7") //NCDX         
                sValue = ar[9].Replace("\"", "");

            else
                sValue = ar[8].Replace("\"", "");  //F & O

            if (SelectedInvestmentTypeIndex != "2")
                {
                double returnvalue = 0.0;
                bool isNumeric = double.TryParse(sValue, out returnvalue);
                if (isNumeric)
                    return returnvalue.ToString();
                else
                    return "0.0";
                }
            else
                return sValue;

            }
        public DataTable GetSelectCommand(string[] ar, string sExchangeCode, ref string bsensecode, ref string sCommand, string SelectedInvestmentTypeIndex)
            {
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            if (SelectedInvestmentTypeIndex == "2")  // Equity
                {
                if (sExchangeCode == "BSE_Code")
                    {
                    var Action = "GETBSE";
                    Adapter.AddParam(pcol, "@Action", Action);
                    Adapter.AddParam(pcol, "@ExchangeCode", ar[0]);
                    dt = Adapter.ExecuteDataTable("SP_GETSCRIPTBYCODE", CommandType.StoredProcedure, Adapter.param(pcol));
                    }
                else
                    {
                    var Action = "GETNSE";
                    Adapter.AddParam(pcol, "@Action", Action);
                    Adapter.AddParam(pcol, "@ExchangeCode", ar[0]);
                    dt = Adapter.ExecuteDataTable("SP_GETSCRIPTBYCODE", CommandType.StoredProcedure, Adapter.param(pcol));
                    }

                }
            else if (SelectedInvestmentTypeIndex == "6") // Currency
                {
                string example = ar[0];
                string scriptName = example;
                string ExpiryDate = "";
                if (ar[0].Length >= 23)
                    {
                    scriptName = ar[0].Remove(0, 6).Substring(0, 6);
                    ExpiryDate = ar[0].Remove(0, 12);
                    }

                var Action = "GETNSE";
                Adapter.AddParam(pcol, "@Action", Action);
                Adapter.AddParam(pcol, "@ExchangeCode", scriptName);
                dt = Adapter.ExecuteDataTable("SP_GETSCRIPTBYCODE", CommandType.StoredProcedure, Adapter.param(pcol));

                }
            //else if (SelectedInvestmentTypeIndex == 4)  // MF
            //    {
            //    bsensecode = ar[0].ToString();
            //    bsensecode = bsensecode.Replace(" ", string.Empty);
            //    sCommand = "SELECT * FROM M_script where " + sExchangeCode + "= '" + bsensecode + "'";
            //    sCommand = sCommand.Replace("\"", string.Empty).Trim();
            //    }
            else if (SelectedInvestmentTypeIndex == "5")  // MCX
                {
                bsensecode = ar[2].ToString();
                bsensecode = bsensecode.Replace(" ", string.Empty).Replace('"', ' ').Trim();
                var Action = "GETNSE";
                Adapter.AddParam(pcol, "@Action", Action);
                Adapter.AddParam(pcol, "@ExchangeCode", bsensecode);
                dt = Adapter.ExecuteDataTable("SP_GETSCRIPTBYCODE", CommandType.StoredProcedure, Adapter.param(pcol));
                }
            else if (SelectedInvestmentTypeIndex == "7") //NCDEX
                {
                bsensecode = ar[0].ToString();
                bsensecode = bsensecode.Replace(" ", string.Empty).Replace('"', ' ').Trim();

                var Action = "GETNSE";
                Adapter.AddParam(pcol, "@Action", Action);
                Adapter.AddParam(pcol, "@ExchangeCode", bsensecode);
                dt = Adapter.ExecuteDataTable("SP_GETSCRIPTBYCODE", CommandType.StoredProcedure, Adapter.param(pcol));
                }
            return dt;
            }
        private DataTable CheckDailtRatesExists(SqlConnection con, out SqlCommand cmd, string scrrp, out int iCount, DateTime datestring, string SelectedInvestmentTypeIndex, string ExpireDate = "", string OptionType = "", string strikePrice = "", bool IsFuture = false)
            {
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            string sDuplicateCheck = "";
            if (SelectedInvestmentTypeIndex == "2" || SelectedInvestmentTypeIndex == "4")    //2 - Equity & 4 MF
                {
                var Action = "COUNTEQUITY";
                Adapter.AddParam(pcol, "@Action", Action);
                Adapter.AddParam(pcol, "@TransDate", datestring);
                Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                dt = Adapter.ExecuteDataTable("SP_GETCHECKDUPLICATEENTRY", CommandType.StoredProcedure, Adapter.param(pcol));

                }
            else if (SelectedInvestmentTypeIndex == "7")     //NCDEX
                {

                var Action = "COUNTNCDEX";
                Adapter.AddParam(pcol, "@Action", Action);
                Adapter.AddParam(pcol, "@TransDate", datestring);
                Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                Adapter.AddParam(pcol, "@ExpDate", ExpireDate);
                dt = Adapter.ExecuteDataTable("SP_GETCHECKDUPLICATEENTRY", CommandType.StoredProcedure, Adapter.param(pcol));
                }
            else if (SelectedInvestmentTypeIndex == "6")    //Currency
                {
                if (IsFuture)
                    {
                    var Action = "COUNTNCDEX";
                    Adapter.AddParam(pcol, "@Action", Action);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@ExpDate", ExpireDate);
                    dt = Adapter.ExecuteDataTable("SP_GETCHECKDUPLICATEENTRY", CommandType.StoredProcedure, Adapter.param(pcol));
                    }
                else
                    {
                    var Action = "COUNTMCX";
                    string Transdate = datestring.ToString().Substring(0, 10);
                    // string expdate = DateTime.ParseExact(ExpireDate, "ddMMMyyyy", null).ToString("yyyy-MM-dd");
                    Transdate = DateTime.ParseExact(Transdate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                    Adapter.AddParam(pcol, "@Action", Action);
                    Adapter.AddParam(pcol, "@TransDate", Transdate);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@ExpDate", ExpireDate);
                    Adapter.AddParam(pcol, "@StrikePrice", strikePrice);
                    Adapter.AddParam(pcol, "@OptionType", OptionType);
                    dt = Adapter.ExecuteDataTable("SP_GETCHECKDUPLICATEENTRY", CommandType.StoredProcedure, Adapter.param(pcol));
                    }

                }
            else if (SelectedInvestmentTypeIndex == "3" || SelectedInvestmentTypeIndex == "5") // 3 -F&O , 5 - MCX
                {
                var Action = "COUNTMCX";
                string Transdate = datestring.ToString().Substring(0, 10);
                string expdate = DateTime.ParseExact(ExpireDate, "ddMMMyyyy", null).ToString("yyyy-MM-dd");
                Transdate = DateTime.ParseExact(Transdate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                Adapter.AddParam(pcol, "@Action", Action);
                Adapter.AddParam(pcol, "@TransDate", Transdate);
                Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                Adapter.AddParam(pcol, "@ExpDate", expdate);
                Adapter.AddParam(pcol, "@StrikePrice", strikePrice);
                Adapter.AddParam(pcol, "@OptionType", OptionType);
                dt = Adapter.ExecuteDataTable("SP_GETCHECKDUPLICATEENTRY", CommandType.StoredProcedure, Adapter.param(pcol));

                }
            cmd = new SqlCommand(sDuplicateCheck, con);
            iCount = (int)dt.Rows[0]["Column1"];

            return dt;
            }
        private void SaveEquity(string sExchangeRate, string Closevalue, string scrrp, int iCount, out string sQuery, string datestring, string SelectedInvestmentTypeIndex, string Highvalue, string HightRate)
            {
            DataTable dt = new DataTable();
            string Transdate = datestring.Substring(0, 10);
            datestring = DateTime.ParseExact(Transdate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
            if (sExchangeRate == "BSE_Rate")
                {
                if (iCount == 0)
                    {
                    var Action = "INSERTBSE";

                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@Action", Action);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@BSERate", Closevalue);
                    Adapter.AddParam(pcol, "@BSEHighRate", Highvalue);

                    Adapter.ExecutenNonQuery("SP_SCRIPTDATAFETCHMASTER", CommandType.StoredProcedure, Adapter.param(pcol));
                    sQuery = "";
                    }
                else
                    {
                    //sQuery = "UPDATE  DailyRate SET " + sExchangeRate + " =  " + Closevalue + " ,  " + HightRate + "='" + Highvalue + "' Where Trans_Dt = '" + datestring.ToString("MM/dd/yyyy") + "' and Script_Code ='" + scrrp + "'  and InvestmentType ='" + SelectedInvestmentTypeIndex + "'";
                    var Action = "UPDATEBSE";
                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@Action", Action);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@BSERate", Closevalue);
                    Adapter.AddParam(pcol, "@BSEHighRate", Highvalue);

                    Adapter.ExecutenNonQuery("SP_SCRIPTDATAFETCHMASTER", CommandType.StoredProcedure, Adapter.param(pcol));
                    sQuery = "";
                    }
                }

            else
                {
                if (iCount == 0)
                    {
                    var Action = "INSERTNSE";
                    //sQuery = "insert into DailyRate(Trans_Dt,Script_Code," + sExchangeRate + ",InvestmentType," + HightRate + ") values('" + datestring.ToString("MM/dd/yyyy") + "','" + scrrp + "','" + Closevalue + "','" + SelectedInvestmentTypeIndex + "','" + Highvalue + "')";
                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@Action", Action);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@NSERate", Closevalue);
                    Adapter.AddParam(pcol, "@NSEHighRate", Highvalue);

                    Adapter.ExecutenNonQuery("SP_SCRIPTDATAFETCHMASTER", CommandType.StoredProcedure, Adapter.param(pcol));
                    sQuery = "";
                    }
                else
                    {
                    var Action = "UPDATENSE";
                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@Action", Action);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@NSERate", Closevalue);
                    Adapter.AddParam(pcol, "@NSEHighRate", Highvalue);

                    Adapter.ExecutenNonQuery("SP_SCRIPTDATAFETCHMASTER", CommandType.StoredProcedure, Adapter.param(pcol));
                    sQuery = "";
                    }

                }

            //cmd = new SqlCommand(sQuery, con);
            //cmd.ExecuteNonQuery();
            //  return dt;

            }
        }
    }