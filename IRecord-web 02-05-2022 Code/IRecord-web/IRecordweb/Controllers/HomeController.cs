using BAL;
using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
    {
    public class HomeController : Controller
        {
        DAL.Master obj = new DAL.Master();
        public ActionResult Index()
            {

            //USERDATA objdata = new
            //     USERDATA();
            //objdata.NAME = "xyz";
            //objdata.CITYID = "1";
            //objdata.STATEID = "1";
            //objdata.ADDRESS = "1";
            //objdata.ID = "1";
            //var json = new JavaScriptSerializer().Serialize(objdata);
            //var client = new RestClient("http://localhost:64915/api/Master/USERMASTER?DBAction=Insert");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            //request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json",json.ToString(), ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);

            //var client = new RestClient("http://www.bseindia.com/download/BhavCopy/Equity/EQ220421_CSV.ZIP");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);

            //System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //using (WebClient client1 = new WebClient())
            //    {
            //    client1.DownloadFile("http://www.bseindia.com/download/BhavCopy/Equity/EQ220421_CSV.ZIP", Server.MapPath("~/DailyRates/EQ220421_CSV.ZIP"));
            //    }

            UploadBhavCopy objdata = new UploadBhavCopy();
            string data1 = "20210505";
           
            //objdata.date = "20210505";
            //objdata.InstrumentName = "ALL";
            //var json = new JavaScriptSerializer().Serialize(objdata);
            //System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //var client = new RestClient("https://www.mcxindia.com/backpage.aspx/GetDateWiseBhavCopy");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Accept-Encoding", " gzip, deflate, sdch, br");
            //request.AddHeader("Accept-Language", " fr-FR,fr;q=0.8,en-US;q=0.6,en;q=0.4");
            //request.AddHeader("Host", "www.mcxindia.com");
            //request.AddHeader("Referer", "https://www.mcxindia.com/");
            //client.UserAgent = " Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/53.0.2785.143 Chrome/53.0.2785.143 Safari/537.36";
            //request.AddHeader("X-Requested-With", " XMLHttpRequest");
            //request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Cookie", "ASP.NET_SessionId=lkvdd3hl5faxmm4jf33kzvua");
            //request.AddParameter("application/json", "{\r\n    \"Date\": \"20210505\",\r\n    \"InstrumentName\": \"ALL\"\r\n}", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //UploadBhavCopyModel.Rootobject data = new
            //     UploadBhavCopyModel.Rootobject();
            //data = JsonConvert.DeserializeObject<UploadBhavCopyModel.Rootobject>(response.Content.ToString());
            //foreach (UploadBhavCopyModel.Datum dr in data.d.Data)
            //    {
            //    string name = dr.Symbol.ToString();
            //    }
            //Console.WriteLine(response.Content);



            //ServicePointManager.Expect100Continue = true;
            //System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //var client = new RestClient("https://www1.nseindia.com/content/historical/EQUITIES/2021/APR/cm28APR2021bhav.csv.zip");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
            //request.AddHeader("Accept-Language", "fr-FR,fr;q=0.8,en-US;q=0.6,en;q=0.4");
            //request.AddHeader("Host", "www1.nseindia.com");
            //request.AddHeader("Referer", "https://www1.nseindia.com/");
            //client.UserAgent = "Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/53.0.2785.143 Chrome/53.0.2785.143 Safari/537.36";
            //request.AddHeader("X-11-With", "XMLHttpRequest");
            //request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            //request.AddHeader("Cookie", "NSE-TEST-1=1910513674.20480.0000");
            //request.AddParameter("text/plain", "", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //string byteArry = response.Content.ToString();

            //USERDATA objdata = new USERDATA();
            //objdata.NAME = "ABCD";
            //objdata.CITYID = "11";
            //objdata.STATEID = "11";
            //objdata.ADDRESS = "Mumbai";
            //objdata.ID = "5";
            //var json = new JavaScriptSerializer().Serialize(objdata);
            //var client = new RestClient("http://localhost:64915/api/Master/USERMASTER?DBAction=Update&ID=5");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            //request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);


            //USERDATA objdata = new USERDATA();
            //objdata.ID = "4";
            //var json = new JavaScriptSerializer().Serialize(objdata);
            //var client = new RestClient("http://localhost:64915/api/Master/USERMASTER?DBAction=Delete&ID=4");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            //request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);

            //var client = new RestClient("http://localhost:64915/api/Master/USERMASTER?DBAction=View");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", "Basic aXJlY29yZDppcmVjb3Jk");
            //IRestResponse response = client.Execute(request);
            //UserModels.Rootobject obj = new UserModels.Rootobject();
            //obj = JsonConvert.DeserializeObject<UserModels.Rootobject>(response.Content.ToString());
            //foreach (UserModels.Datum dr in obj.Data)
            //    {
            //    string name = dr.NAME.ToString();
            //    }

            return View();
            }
        [HttpPost]
        public ActionResult Index(Member _Member, int id)
            {
            obj.GetMemberName();
            return View();
            }

        public ActionResult About()
            {
            ViewBag.Message = "Your application description page.";

            return View();
            }

        public ActionResult Contact()
            {
            ViewBag.Message = "Your contact page.";

            return View();
            }
        }
    }