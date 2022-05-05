using BAL;
using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DAL;
using System.IO.Compression;
using System.Data;
using System.Net;
using RestSharp;
using System.Web.Script.Serialization;
using IRecordweb.Models;
using Newtonsoft.Json;
using CsvHelper;
using static IRecordweb.Models.UploadBhavCopyModel;
using System.Xml.Linq;
using System.Security.Cryptography;
using ClosedXML.Excel;

namespace IRecordweb.Controllers
    {
    public class UploadBhavCopyController : Controller
        {
        // GET: UploadBhavCopy
        DAL.UploadBhavCopyDAL obj = new DAL.UploadBhavCopyDAL();
        DAL.CommonFn com = new DAL.CommonFn();
        DAL.DownloadDAL ddl = new DownloadDAL();
        MType mtype = new MType();

        public ActionResult Index()
            {
            return View();
            }


        [HttpGet]
        public ActionResult SaveUploadBhavCopy()
            {
            ViewBag.InvestmentType = new SelectList(obj.BindInvenstmentType(mtype).ToList(), dataValueField: "TypeId", dataTextField: "Name");
            return View();
            }
        [HttpPost]
        public ActionResult SaveUploadBhavCopy(UploadBhavCopy _BhavCopy, HttpPostedFileBase UploadFile, string DOWNLOAD)
            {
            ViewBag.InvestmentType = new SelectList(obj.BindInvenstmentType(mtype).ToList(), dataValueField: "TypeId", dataTextField: "Name");

            if (!string.IsNullOrEmpty(DOWNLOAD))
                {
                string dt = _BhavCopy.TransDate.ToString("yyyy-MM-dd");
                string closing = "";
                try
                    {
                    DataTable dtdata = new DataTable();
                    dtdata = ddl.GetDate(dt);

                    if (dtdata.Rows.Count > 0)
                        {
                        closing = dtdata.Rows[0]["Column1"].ToString();
                        }

                    if (closing.Contains("Sunday") || closing.Contains("Saturday"))
                        {
                        ViewBag.Message = "Holiday on selected Date";
                        return RedirectToAction("SaveUploadBhavCopy");
                        }
                    }
                catch
                    {
                    throw;
                    }
                try
                    {
                    ddl.DownloadBhavCopy(_BhavCopy.InvestmentType, _BhavCopy.Exchange, _BhavCopy.TransDate);

                    System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    if (_BhavCopy.InvestmentType == 2 && _BhavCopy.Exchange == "BSE")
                        {
                        using (WebClient client1 = new WebClient())
                            {
                            client1.DownloadFile(Session["URL"].ToString(), Server.MapPath("~/DailyRates/" + Session["filename"]));
                            string zipPath = Session["sDowinloadPath"].ToString();
                            string extractPath = Server.MapPath("~/DailyRates/");

                            //System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath);
                            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);

                            foreach (string file in Directory.GetFiles(extractPath))
                                {

                                obj.SaveDailyRates(file, _BhavCopy.TransDate, _BhavCopy.InvestmentType, _BhavCopy.Exchange, true);

                                }

                            }
                        ViewBag.Message = "BSE Bhav Copy Successfully Uploaded";
                        }
                    else if (_BhavCopy.InvestmentType == 2 && _BhavCopy.Exchange == "NSE")
                        {
                        //  Data(Session["URL"].ToString());
                        Data(Session["URL"].ToString(), UploadFile, _BhavCopy.TransDate, _BhavCopy.InvestmentType, _BhavCopy.Exchange);

                        ViewBag.Message = "NSE Bhav Copy Successfully Uploaded";
                        }
                    else if (_BhavCopy.InvestmentType == 3)
                        {
                        Data(Session["URL"].ToString(), UploadFile, _BhavCopy.TransDate, _BhavCopy.InvestmentType, _BhavCopy.Exchange);
                        ViewBag.Message = "F&O Bhav Copy Successfully Uploaded";

                        }
                    else if (_BhavCopy.InvestmentType == 4)
                        {
                        MFData();
                        ViewBag.Message = "Mutual Fund Bhav Copy Successfully Uploaded";
                        }
                    else if (_BhavCopy.InvestmentType == 5)
                        {
                        MCXData(_BhavCopy);
                        ViewBag.Message = "MCX Bhav Copy Successfully Uploaded";
                        }

                    else if (_BhavCopy.InvestmentType == 6)
                        {
                        Data(Session["URL"].ToString(), UploadFile, _BhavCopy.TransDate, _BhavCopy.InvestmentType, _BhavCopy.Exchange);
                        ViewBag.Message = "Currency Bhav copy Successfully Uploaded";
                        }

                    else if (_BhavCopy.InvestmentType == 7)
                        {
                      string data =  NCDEXData(_BhavCopy);
                        foreach (string r in data.Split('\n'))
                            {
                            string d = r.ToString();
                            }
                        ViewBag.Message = "NCDEX Bhav copy Successfully Uploaded";
                        }

                    }
                catch (Exception ex)
                    {
                    if (ex.Message.ToLower().Contains("not found"))
                        ViewBag.Message = "Data not available for selected date";
                    else
                        ViewBag.Message = "Error while downloading data";
                    }
                return View();
                }
            else
                {
                if (_BhavCopy.InvestmentType == 2)
                    {
                    if (_BhavCopy.Exchange == "true")
                        {
                        //   string filepath = "~/UploadData/" + strPath;
                        //    UploadFile.SaveAs(Server.MapPath(filepath));
                        //function call to get the filename
                        string strPath = UploadFile.FileName;
                        string filename = Path.GetFileName(strPath);
                        filename = Path.GetFileName(strPath);
                        filename = filename.Substring(filename.LastIndexOf("EQ") + 2, filename.LastIndexOf(".") - filename.LastIndexOf("EQ") - 2);

                        DateTime Filedate;
                        if (filename.Length == 9)
                            {
                            Filedate = DateTime.ParseExact(filename, "ddMMMyyyy", null);
                            }
                        else
                            {
                            Filedate = DateTime.ParseExact(filename, "ddMMyy", null);

                            }
                        string CFiledate = Filedate.ToString("dd-MM-yyyy");
                        string SelectedDate = _BhavCopy.TransDate.ToString("dd-MM-yyyy");
                        if (SelectedDate != CFiledate)
                            {
                            ViewBag.Message = "Please Check Respective Date";
                            return View();
                            }
                        }
                    }

                bool isSuccess = false;

                string SelectedDate1 = _BhavCopy.TransDate.ToString("dd-MM-yyyy");
                if (_BhavCopy.InvestmentType == 3)//F&O
                    {
                    string strPath1 = UploadFile.FileName;
                    DateTime Transdate = _BhavCopy.TransDate;
                    int InvestmentType = _BhavCopy.InvestmentType;
                    CommonFn.SaveDailyRates(UploadFile, Transdate, InvestmentType);
                    //}

                    isSuccess = true;
                    }
                else
                    {
                    int BSE_Or_NSECode = 0;
                    //if (_BhavCopy.Exchange.tostrs)
                    //    BSE_Or_NSECode = 0;
                    //else if (rbnse.Checked)
                    //    BSE_Or_NSECode = 1;
                    //  isSuccess = SaveDailyRates(strPath, _BhavCopy);
                    //if (SelectedDate != CFiledate)
                    //    {
                    //    ViewBag.Message = "Please Check Respective Date";
                    //    }
                    //else
                    //    {
                    obj.InsertUploadBhavCopyMaster(_BhavCopy, UploadFile);
                    ViewBag.Message = "File Uploaded Sucessfully";
                    //}
                    }
                return View();
                }
            }
        public void Data(string url, HttpPostedFileBase UploadFile, DateTime TransDate, int InvestmentType, string BSEOrNSECode)
            {
            ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
            request.AddHeader("Accept-Language", "fr-FR,fr;q=0.8,en-US;q=0.6,en;q=0.4");
            request.AddHeader("Host", "www1.nseindia.com");
            request.AddHeader("Referer", "https://www1.nseindia.com/");
            client.UserAgent = "Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/53.0.2785.143 Chrome/53.0.2785.143 Safari/537.36";
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            request.AddHeader("Cookie", "NSE-TEST-1=1910513674.20480.0000");
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            byte[] newByteArray = response.RawBytes;
            string filename = Path.GetFileName(url.Replace("/", "\\"));

            string SaveAsPath = Server.MapPath("~/DailyRates/" + filename);

            if (System.IO.File.Exists(Session["sDowinloadPath"].ToString()))
                System.IO.File.Delete(Session["sDowinloadPath"].ToString());

            System.IO.FileStream stream = new FileStream(SaveAsPath, FileMode.CreateNew);
            System.IO.BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(newByteArray, 0, newByteArray.Length);
            writer.Close();


            // Extract zip file 


            string zipPath = SaveAsPath;
            string extractPath = Server.MapPath("~/DailyRates/");

            //System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath);
            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);

            foreach (string file in Directory.GetFiles(extractPath))
                {
                if (InvestmentType == 3)//F&O
                    {
                    CommonFn.SaveFNODailyRates(file, TransDate, InvestmentType);
                    }
                else
                    {
                    obj.SaveDailyRates(file, TransDate, InvestmentType, BSEOrNSECode, true);
                    }
                }

            }
        public void MCXData(UploadBhavCopy bhavcopy)
            {
            Datum objdata = new Datum();
            string selectedDate = bhavcopy.TransDate.ToString("yyyyMMdd");
            objdata.Date = selectedDate;
            objdata.InstrumentName = "ALL";

            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var json = new JavaScriptSerializer().Serialize(objdata);
            var client = new RestClient("https://www.mcxindia.com/backpage.aspx/GetDateWiseBhavCopy");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept-Encoding", " gzip, deflate, sdch, br");
            request.AddHeader("Accept-Language", " fr-FR,fr;q=0.8,en-US;q=0.6,en;q=0.4");
            request.AddHeader("Host", "www.mcxindia.com");
            request.AddHeader("Referer", "https://www.mcxindia.com/");
            client.UserAgent = " Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/53.0.2785.143 Chrome/53.0.2785.143 Safari/537.36";
            request.AddHeader("X-Requested-With", " XMLHttpRequest");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ASP.NET_SessionId=lkvdd3hl5faxmm4jf33kzvua");
            request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            UploadBhavCopyModel.Rootobject data = new UploadBhavCopyModel.Rootobject();
            data = JsonConvert.DeserializeObject<UploadBhavCopyModel.Rootobject>(response.Content.ToString());
            string datedt = bhavcopy.TransDate.ToString("ddMMMyyyy").ToUpper();
            string FILENAMEOUTEXT = "MCX" + datedt;
            string filename = "MCX" + datedt + ".csv";
           
            string SaveAsPath = Server.MapPath("~/DailyRates/" + filename);
            if (System.IO.File.Exists(SaveAsPath))
                System.IO.File.Delete(SaveAsPath);

            DataTable dtexc = new DataTable();
            dtexc.Columns.Add("Date", typeof(string));
            dtexc.Columns.Add("Instrument Name", typeof(string));
            dtexc.Columns.Add("Symbol", typeof(string));
            dtexc.Columns.Add("Expiry Date", typeof(string));
            dtexc.Columns.Add("Option Type", typeof(string));
            dtexc.Columns.Add("Strike Price", typeof(string));
            dtexc.Columns.Add("Open", typeof(string));
            dtexc.Columns.Add("High", typeof(string));
            dtexc.Columns.Add("Low", typeof(string));
            dtexc.Columns.Add("Close", typeof(string));
            dtexc.Columns.Add("Volume(Lots)", typeof(string));
            dtexc.Columns.Add("Volume(In 000's)", typeof(string));


            for (int i = 0; i < data.d.Data.Count(); i++)
                {
                DataRow dr = dtexc.NewRow();


                string first = data.d.Data[i].Date;
                string second = data.d.Data[i].InstrumentName.ToString();
                //string third = data.d.Data[i].Name.ToString();
                string fourth = data.d.Data[i].Symbol.ToString();
                string five = data.d.Data[i].ExpiryDate.ToString();
                string six;
                if (data.d.Data[i].OptionType.ToString() == "-")
                    {
                    six = "-";
                    }
                else
                    {
                    six = data.d.Data[i].OptionType.ToString();
                    }
                string seven = data.d.Data[i].StrikePrice.ToString();
                string eight = data.d.Data[i].Open.ToString();
                string nine = data.d.Data[i].High.ToString();
                string ten = data.d.Data[i].Low.ToString();
                string eleven = data.d.Data[i].Close.ToString();
                string twelve = data.d.Data[i].PreviousClose.ToString();
                string thirteen = data.d.Data[i].Volume.ToString();
                string fourteen = data.d.Data[i].VolumeInThousands.ToString();
                dr["Date"] = first;
                dr["Instrument Name"] = second;
                dr["Symbol"] = fourth;
                dr["Expiry Date"] = five;
                dr["Option Type"] = six;
                dr["Strike Price"] = seven;
                dr["Open"] = eight;
                dr["High"] = nine;
                dr["Low"] = ten;
                dr["Close"] = eleven;
                dr["Volume(Lots)"] = twelve;
                dr["Volume(In 000's)"] = thirteen;
                dtexc.Rows.Add(dr);
                dtexc.AcceptChanges();
                //  string csvRow = string.Format("{0},{1},{2},{3},{4},{5},{6}, {7},{8},{9},{10},{11},{12}", first, second,fourth,five,six, seven, eight, nine,ten,eleven,twelve, thirteen,fourteen);
                //   stream.WriteLine(csvRow);
                }
            //   }
            using (XLWorkbook wb = new XLWorkbook())
                {
                wb.Worksheets.Add(dtexc, FILENAMEOUTEXT);

                wb.SaveAs(SaveAsPath);
                //using (MemoryStream MyMemoryStream = new MemoryStream())
                //    {
                //    wb.SaveAs(MyMemoryStream)
                //    MyMemoryStream.WriteTo(Response.OutputStream);
                //    Response.Flush();
                //    Response.End();
                //    }
                }
            //System.IO.File.WriteAllText(SaveAsPath, cdata.ToString());
            StringBuilder sb = new StringBuilder();


            //   StreamWriter objWriter = new StreamWriter(SaveAsPath, true);

            //  Console.WriteLine(response.Content);
            obj.SaveMCXDwnldDailyRates(SaveAsPath, bhavcopy.TransDate, bhavcopy.InvestmentType, bhavcopy.Exchange, dtexc, true);

            }
        public string NCDEXData(UploadBhavCopy bhavcopy)
            {
            string date = bhavcopy.TransDate.ToString("dd-MMM-yyyy").ToUpper();
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient("https://ncdex.com/markets/bhavcopy?file_type=provisional&type=bhavcopy_opt&filedate="+date+"&format=csv_file");
            client.Timeout = -1;
            var request = new RestRequest("", Method.GET, DataFormat.Json);
            request.AddHeader("Accept", " text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/e*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Accept-Encoding", " gzip, deflate, br");
            request.AddHeader("Accept-Language", " en-US,en;q=0.9");
            request.AddHeader("Connection", " keep-alive");
            request.AddHeader("Cookie", " _ga=GA1.2.1261762718.1619433707; _gid=GA1.2.1258887555.1620475784; XSRF-TOKEN=eyJpdiI6InA4YUd4dkd1aXNYR0lCNUlySFFZQlE9PSIsInZhbHVlIjoiZXQ5U01Ga1l3T2RIS0xJb1BuUE9qSXJsaFZvQU8rcnVvTTVpa0VUcW5WR0QyOVlXZ1FKSEVTT3F2TnhKR29EQyIsIm1hYyI6IjUxZDM2MzZjYTdjM2M1MmRkODZiZjQ0OTZlZjhjNDIwNTAxNzBiOTMxZTFkZjFkMWFjMjIwY2M5ZDUzNTczMjEifQ%3D%3D; ncdex_session=eyJpdiI6IkpOalwvb2o1MVhGNHl4WVdvR214VDlnPT0iLCJ2YWx1ZSI6InQ2SEV1XC9pUDY4VE9kNEZpaHVDalJyT1Q2RGhiVlVIaVQxd1wvZ1MwVVBQVGZnQnZrWmlZdzNHbFFJdEZBKzdmMiIsIm1hYyI6IjI3NjdjNzNiNDgyOWUwYzk3YTAyNGEzYmFmM2Q3MWJhOTk0MzJjMDQ2MjYyMGRmZGQ5NzAzNGVhZmNmYzE5N2QifQ%3D%3D; XSRF-TOKEN=eyJpdiI6IlYranVDZURzK0xxOUUxMHE1ek1UK0E9PSIsInZhbHVlIjoiMlM1TTRqcHVtUmtQQ0pVYlwveldKSnR1RFh2TDh1UTg2bW9mNEdJSEIzTFVaSlN1dWplek9RaEcwQ1ZRTmo3RTIiLCJtYWMiOiI3OTUxYTE5OGY4YzkwOWRkNjBmZGZjNjM4N2Q0ODMwYzFhYzkzZDgyMzU1ZjEzOWU2MDI4ZjkzMDQ2ZTY4ZDgxIn0%3D; ncdex_session=eyJpdiI6IktEZXBjWitpMEV4WXhobDlzTDFHc0E9PSIsInZhbHVlIjoiNFwvMzFXOUwzTHZaXC9NTWtLYkowaThnRU8rQVJoVXJmSHRSUTAxcWFlZnQrdWN1dm1YQlNNKzdzUWRDc3oxZklxIiwibWFjIjoiOGRjNWY5MmM3ZGI4YTk2OWU2OTkxODBhYWJhMGJiMTFlZGI0NTRmMGYxZmVmOWRjOWE3YWQ4MjlmZWI5NDQyNyJ9");
            request.AddHeader("Host", "ncdex.com");
            request.AddHeader("Referer", " https://ncdex.com/markets/bhavcopy");
            request.AddHeader("sec-ch-ua", " \" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            request.AddHeader("sec-ch-ua-mobile", " ?0");
            request.AddHeader("Sec-Fetch-Dest", " document");
            request.AddHeader("Sec-Fetch-Mode", " navigate");
            request.AddHeader("Sec-Fetch-Site", " same-origin");
            request.AddHeader("Sec-Fetch-User", " ?1");
            request.AddHeader("Upgrade-Insecure-Requests", " 1");
            client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36";
            request.AddHeader("Content-Type", "application/json");
           // request.AddParameter("application/json", "", ParameterType.RequestBody);
            DataTable dt = new DataTable();
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            UploadBhavCopy obj = new UploadBhavCopy();
            
            string d = "";
            var abc = JsonConvert.SerializeObject(response.Content.ToString());
            //dt = (DataTable)response.Content;

            string datedt = bhavcopy.TransDate.ToString("ddMMMyyyy").ToUpper();
            string FILENAMEOUTEXT = "NCDEX" + datedt;
            string filename = "NCDEX" + datedt + ".csv";
            string SaveAsPath = Server.MapPath(@"~/DailyRates/" + filename);
          //  ConvertToCsv(d, SaveAsPath);
            using (var stream = System.IO.File.CreateText(SaveAsPath))
                {
                stream.WriteLine(d.RemoveSpecialCharacters());
                }
            System.IO.File.WriteAllText(SaveAsPath, d.ToString());
            //using (XLWorkbook wb = new XLWorkbook())
            //    {
            //    wb.Worksheets.Add(d);
            //    wb.SaveAs(SaveAsPath);

            //    }              

          
            StringBuilder csvContent = new StringBuilder();
            // Adding Header Or Column in the First Row of CSV  
            csvContent.AppendLine(d);
            //csvContent.AppendLine("'\n'");
            //csvContent.AppendLine(d);
            csvContent.AppendLine("Sathiya,Seelan");

            string textPath = "D:\\CSVTextFile.csv";

            //Here we delete the existing file to avoid duplicate records.  
            if (System.IO.File.Exists(textPath) )
                System.IO.File.Delete(textPath);

            System.IO.File.AppendAllText(textPath, csvContent.ToString());
            // Save or upload CSV format string to Text File (.txt)  
            System.IO.File.AppendAllText(textPath, csvContent.ToString());

            //Download or read all Text within the uploaded Text file.  
            string csvContentStr = System.IO.File.ReadAllText(textPath);

            //This saves content as CSV File.  
          //  this.SaveCSVFile("CSVFileName", csvContentStr);
           


        //DataTable dtProduct  = response.Content.ToString();

        //StringBuilder sb = new StringBuilder();

        //IEnumerable<string> columnNames = dtProduct.Columns.Cast<DataColumn>().
        //                                  Select(column => column.ColumnName);
        //sb.AppendLine(string.Join(",", columnNames));

        //foreach (DataRow row in dtProduct.Rows)
        //    {
        //    IEnumerable<string> fields = row.ItemArray.Select(field =>
        //      string.Concat("\"", field.ToString()c);
        //    sb.AppendLine(string.Join(",", fields));
        //    }
        Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ProductDetails.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(d);
            Response.Flush();
            Response.End();











            return d;
        
            }
        void ConvertToCsv(string sourcefile, string destfile)
            {
            int i, j;
            StreamWriter csvfile;
            string[] lines, cells;
            lines = System.IO.File.ReadAllLines(sourcefile);
            csvfile = new StreamWriter(destfile);
            for (i = 0; i < lines.Length; i++)
                {
                cells = lines[i].Split(new Char[] { '\t', ';' });
                for (j = 0; j < cells.Length; j++)
                    csvfile.Write(cells[j] + ",");
                csvfile.WriteLine();
                }
            csvfile.Close();
            }
        public void MFData()
            {
            System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var client = new RestClient("https://www.amfiindia.com/spages/NAVOpen.txt");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", " text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Accept-Encoding", " gzip, deflate, br");
            request.AddHeader("Accept-Language", " en-US,en;q=0.9");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Cookie", "_ga=GA1.2.1261762718.1619433707; _gid=GA1.2.1258887555.1620475784; XSRF-TOKEN=eyJpdiI6InA4YUd4dkd1aXNYR0lCNUlySFFZQlE9PSIsInZhbHVlIjoiZXQ5U01Ga1l3T2RIS0xJb1BuUE9qSXJsaFZvQU8rcnVvTTVpa0VUcW5WR0QyOVlXZ1FKSEVTT3F2TnhKR29EQyIsIm1hYyI6IjUxZDM2MzZjYTdjM2M1MmRkODZiZjQ0OTZlZjhjNDIwNTAxNzBiOTMxZTFkZjFkMWFjMjIwY2M5ZDUzNTczMjEifQ%3D%3D; ncdex_session=eyJpdiI6IkpOalwvb2o1MVhGNHl4WVdvR214VDlnPT0iLCJ2YWx1ZSI6InQ2SEV1XC9pUDY4VE9kNEZpaHVDalJyT1Q2RGhiVlVIaVQxd1wvZ1MwVVBQVGZnQnZrWmlZdzNHbFFJdEZBKzdmMiIsIm1hYyI6IjI3NjdjNzNiNDgyOWUwYzk3YTAyNGEzYmFmM2Q3MWJhOTk0MzJjMDQ2MjYyMGRmZGQ5NzAzNGVhZmNmYzE5N2QifQ%3D%3D");
            //request.AddHeader("Host", "ncdex.com");
            //request.AddHeader("Referer", "https://ncdex.com/markets/bhavcopy");
            request.AddHeader("Host", "amfiindia.com");
            request.AddHeader("Referer", "https://amfiindia.com/spages/NAVOpen.txt");
            request.AddHeader("sec-ch-ua", " \" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            request.AddHeader("sec-ch-ua-mobile", " ?0");
            request.AddHeader("Sec-Fetch-Dest", " document");
            request.AddHeader("Sec-Fetch-Mode", " navigate");
            request.AddHeader("Sec-Fetch-Site", " same-origin");
            request.AddHeader("Sec-Fetch-User", " ?1");
            request.AddHeader("Upgrade-Insecure-Requests", " 1");
            client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36";
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            }
        }
    }