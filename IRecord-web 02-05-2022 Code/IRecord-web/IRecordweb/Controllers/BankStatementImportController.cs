
using IRecordweb.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IRecordweb.Controllers
{
    public class BankStatementImportController : Controller
    {
        private readonly string URL = ConfigurationManager.AppSettings["ScreenURL"];
        private readonly string BasicAuth = ConfigurationManager.AppSettings["Authorization"];
        private readonly string PdfConfig = ConfigurationManager.AppSettings["PdfConfig"];

        // GET: BankStatementImport
        public ActionResult Index()
        {
            ViewBag.BankNameId = new SelectList(GetBankFormat(), dataValueField: "ID", dataTextField: "NAME");
            ViewBag.BankConfig = new SelectList(GetBankFormatConfig(), dataValueField: "ID", dataTextField: "NAME");
            System.Web.HttpContext.Current.Session["member_code"] = "T0001";
            ViewData["asdf"] = "sdf";
            System.Web.HttpContext.Current.Session["year_code"] = "1120202021";
            return View();
        }
        public ActionResult ViewList()
        {
           

            return View();
        }
        [HttpGet]
        public JsonResult ViewFormate(string BankId)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_BANK_FORMATE_UTITLIY]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "VIEWCONFIG";
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@MemberYearId", SqlDbType.Int).Value = FinancialYearCode;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = BankId;
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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(DT);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            List<MTYPE> list = new List<MTYPE>();
            foreach (DataRow dr in DT.Rows)
            {
                MTYPE item = new MTYPE();
                item.TypeId = Convert.ToInt32(dr["ID"].ToString());
                item.Name = dr["NAME"].ToString();
                list.Add(item);
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult Index(BankStatement_Index_Model bank, HttpPostedFileBase fileupload)
        {
            // ViewBankStatement(bank, fileupload);
            Viewbankdata(bank, fileupload);
            return RedirectToAction("ViewBankStatement");
        }
        public JsonResult ReadAllBankData()
        {
            DataTable DT = Session["datadetails"] as DataTable;
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ViewACCOUNT()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_BANKSTATEMENTDATA]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "ViewAccount";
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;
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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
        public JsonResult ViewBnkSteUser()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_BANKSTATEMENTDATA]"
            };
            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "View";
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;
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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
        public List<BankStatement_Index_Model> GetBankFormat()
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();


            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_BANK_FORMATE_UTITLIY]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBANK";
            cmd.Parameters.Add("@MemberId", SqlDbType.Int).Value = MemberCode;
            cmd.Parameters.Add("@MemberYearId", SqlDbType.Int).Value = FinancialYearCode;

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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            List<BankStatement_Index_Model> list = new List<BankStatement_Index_Model>();
            foreach (DataRow dr in DT.Rows)
            {
                BankStatement_Index_Model item = new BankStatement_Index_Model
                {
                    ID = Convert.ToInt32(dr["ID"].ToString()),
                    Name = dr["NAME"].ToString()
                };

                list.Add(item);
            }
            return list;
        }
        public List<BankStatement_Index_Model> GetBankFormatConfig()
        {
            //System.Net.ServicePointManager.SecurityProtocol =SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //RestClient client = new RestClient(URL + "api/Master/BANKFORMATMASTER?DBAction=ViewConfig&ID=0")
            //{
            //    Timeout = -1
            //};
            //RestRequest request = new RestRequest(Method.POST);
            //request.AddHeader("Authorization", BasicAuth);
            //string body = @"";
            //request.AddParameter("text/plain", body, ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //DataSet data = JsonConvert.DeserializeObject<DataSet>(response.Content);
            List<BankStatement_Index_Model> list = new List<BankStatement_Index_Model>();
            //foreach (DataRow dr in data.Tables[0].Rows)
            //{
            //    BankStatement_Index_Model item = new BankStatement_Index_Model
            //    {
            //        ID = Convert.ToInt32(dr["ID"].ToString()),
            //        Name = dr["NAME"].ToString()
            //    };

            //    list.Add(item);
            //}
            //list.Clear();
            return list;
        }

        public List<BankStatement_Index_Model> Viewbankdata(BankStatement_Index_Model objdata, HttpPostedFileBase Fileupload)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(Fileupload.InputStream))
            {
                bytes = br.ReadBytes(Fileupload.ContentLength);
            }
            string _FileName = Path.GetFileName(Fileupload.FileName);
            string fileextenion = Path.GetExtension(Fileupload.FileName);
            string ContentType = Fileupload.ContentType.ToString();
            //  string filebytes = br.Fileupload.ContentLength;
            Session["SeletedUserBankId"] = objdata.BankNameId;
            BankStatementDataTrans.Rootobject datatrn = new BankStatementDataTrans.Rootobject
            {
                Bankcode = objdata.BankConfig,
                fileName = _FileName,
                contentType = ContentType,
                fileextension = fileextenion,
                pdfbytes = Convert.ToBase64String(bytes),
                StartDate = objdata.FromDate,
                EndDate = objdata.ToDate
            };

            System.Net.ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(PdfConfig + "api/PdfProcess/BankFormat")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(datatrn);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(response.Content);
            Session["datadetails"] = myDataSet.Tables[0];
            TempData["Datadetails"] = Session["datadetails"];

            List<BankStatement_Index_Model> list = new List<BankStatement_Index_Model>();
            foreach (DataRow dr in myDataSet.Tables[0].Rows)
            {
                BankStatement_Index_Model item = new BankStatement_Index_Model
                {
                    Date = Convert.ToDateTime(dr["Date"].ToString())
                };
                Session["Date"] = item.Accounts;
                item.Accounts = dr["Accounts"].ToString();
                Session["Accounts"] = item.Accounts;
                item.Accounts_Color = dr["Accounts_color"].ToString();
                Session["Accounts_Color"] = item.Accounts_Color;
                item.Cheque = dr["Cheque"].ToString();
                Session["Cheque"] = item.Cheque;
                item.Debit = dr["Debit"].ToString();
                Session["Debit"] = item.Debit;
                item.Credit = dr["Credit"].ToString();
                Session["Credit"] = item.Credit;
                item.Narration = dr["Narration"].ToString();
                Session["Narration"] = item.Narration;
                item.ACCode = dr["ACcode"].ToString();
                Session["ACcode"] = item.ACCode;
                list.Add(item);
            }
            BankStatement_Index_Model item1 = new BankStatement_Index_Model
            {
                BankFormatList = list
            };
            List<BankStatement_Index_Model> dtdata = item1.BankFormatList;
            TempData["datadt"] = dtdata;
            Session["Finaldata"] = dtdata;
            return list;

        }

        [HttpGet]
        public ActionResult ViewBankStatement()
        {
            object data = TempData["datadt"];
            TempData["FinalData"] = data;
            return View(data);
        }
        [HttpPost]
        public ActionResult ViewBankStatement(string Save, string Add)
        {
            if (!string.IsNullOrEmpty(Save))
            {
                // Savedata();
                ViewBag.Message = "Record Saved Successfully !";
                return View();
            }
            if (!string.IsNullOrEmpty(Add))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
        public int GetGlobalTransactionNumber1(string bktyp)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)
            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            con.Open();
            string dttod = DateTime.Today.ToString("yyyy-MM-dd");
            SqlCommand cmd43 = new SqlCommand("Insert into Global_no(Book_typ, Date, MemberID, FinancialYearMemberID, CreatedBy, CreatedDate) values('" + bktyp + "',getdate()," + MemberCode + "," + FinancialYearCode + "," + CreatedById + ",getdate());SELECT SCOPE_IDENTITY();", con);
            object vchno = cmd43.ExecuteScalar();
            return Convert.ToInt32(vchno);
        }
        public void Savedata(string JsonData)
        {
            DataTable dtfin = Session["Dt_FinancialYear"] as DataTable;//Member Details(MemberId,FinancialYearUserID)

            string MemberCode = dtfin.Rows[0]["MemberId"].ToString();
            string FinancialYearCode = dtfin.Rows[0]["FinancialYearUserID"].ToString();
            string CreatedById = Session["UserID"].ToString();
            DataTable dtdata = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(JsonData);
            int rowcounter = 1;

         
            foreach (DataRow dr in dtdata.Rows)
            {
                string transNo = GetGlobalTransactionNumber1("BS").ToString();
                string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,

                    CommandText = "[DBO].[SP_BANKSTATEMENTDATA]"
                };
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "INSERT";
                cmd.Parameters.Add("@TransDate", SqlDbType.Date).Value = dr["Date"].ToString();
                cmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar).Value = dr["Cheque"].ToString();
                cmd.Parameters.Add("@CreditCode", SqlDbType.VarChar).Value = (dr["Debit"].ToString() == "0" ? Session["SeletedUserBankId"].ToString() : dr["ACcode"].ToString());//
                cmd.Parameters.Add("@DebitCode", SqlDbType.VarChar).Value = (dr["Credit"].ToString() == "0" ? Session["SeletedUserBankId"].ToString() : dr["ACcode"].ToString());//
                cmd.Parameters.Add("@Narration", SqlDbType.VarChar).Value = dr["Narration"].ToString();//
                // cmd.Parameters.Add("@Active", SqlDbType.VarChar).Value = dataInSingleObj.Active;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedById;
                cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = MemberCode;
                cmd.Parameters.Add("@FinancialYearMemberID", SqlDbType.Int).Value = FinancialYearCode;
                cmd.Parameters.Add("@Amount", SqlDbType.Float).Value = (dr["Debit"].ToString() == "0" ? dr["Credit"].ToString() : dr["Debit"].ToString());//
                cmd.Parameters.Add("@Trans_No", SqlDbType.VarChar).Value = transNo;
                cmd.Parameters.Add("@Sr_No", SqlDbType.Int).Value = rowcounter;

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
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
                rowcounter++;
            }






        }

        public ActionResult DisplayData()
        {
            object data = TempData["datadt"];
            return View(data);
        }

        //public JsonResult Get_Header()
        //{
        //    ImportTradefileContract_INDEX_Models data = new ImportTradefileContract_INDEX_Models();
        //    data = Session["data"] as ImportTradefileContract_INDEX_Models;
        //    var json = new JavaScriptSerializer().Serialize(data);
        //    return Json(json.ToString(), JsonRequestBehavior.AllowGet);

        //}


        //public JsonResult Get_Details()
        //{
        //    ImportTradefileContract_INDEX_Models data = new ImportTradefileContract_INDEX_Models();
        //    data = Session["data"] as ImportTradefileContract_INDEX_Models;
        //    var json = new JavaScriptSerializer().Serialize(data);
        //    return Json(json.ToString(), JsonRequestBehavior.AllowGet);
        //}
        public JsonResult Get_Expenses()
        {
            ImportTradefileContract_INDEX_Models data = new ImportTradefileContract_INDEX_Models();
            data = Session["data"] as ImportTradefileContract_INDEX_Models;
            string json = new JavaScriptSerializer().Serialize(data);
            return Json(json.ToString(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void Upload()
        {


            foreach (string key in Request.Files)
            {
                HttpPostedFileBase FilePath = Request.Files[key];
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(FilePath.InputStream))
                {
                    bytes = br.ReadBytes(FilePath.ContentLength);
                }
                string _FileName = Path.GetFileName(FilePath.FileName);
                string fileextenion = Path.GetExtension(FilePath.FileName);
                string ContentType = FilePath.ContentType.ToString();
                Session["FilesString"] = Convert.ToBase64String(bytes);
                Session["FileName"] = _FileName;
                Session["Fileextenion"] = fileextenion;
                Session["ContentType"] = ContentType;
            }

            //return Content("Success");
        }
        [HttpPost]
        public void IndexSave(BankStatement_Index_Model data)
        {
            data.FilesString = Session["FilesString"].ToString();
            data.FileName = Session["FileName"].ToString();
            data.Fileextenion = Session["Fileextenion"].ToString();
            data.ContentType = Session["ContentType"].ToString();
            // string date = obj.FromDate;
            //return View();
            Session["data"] = data;
            //  Session["data"] = myDataSet.Tables[0];
        }

        public ActionResult SaveInfo()
        {
            Savefunc();
            return View("Index");
        }




        public static object DataTableToJSON(System.Data.DataTable table)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(list);
        }
        //FILL BANK DDL

        public JsonResult Get_Header()
        {
            BankStatement_Index_Model data = new BankStatement_Index_Model();
            data = Session["data"] as BankStatement_Index_Model;
            string json = new JavaScriptSerializer().Serialize(data);
            return Json(json.ToString(), JsonRequestBehavior.AllowGet);

        }
        public static int GetGlobalTransactionNumber(string bktyp)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            con.Open();
            string dttod = DateTime.Today.ToString("yyyy-MM-dd");
            SqlCommand cmd43 = new SqlCommand("Insert into Global_no(Book_typ,dt) values('" + bktyp + "','" + dttod + "');SELECT SCOPE_IDENTITY();", con);
            object vchno = cmd43.ExecuteScalar();
            return Convert.ToInt32(vchno);
        }
        public JsonResult Get_Details()
        {
            DataTable dt = Session["datadetails"] as DataTable;
            return Json(DataTableToJSON(dt), JsonRequestBehavior.AllowGet);
        }
        public string debitcode;
        public string creditcode;
        private readonly int decmpl;
        public void Savefunc()
        {
            DataTable dt_details = Session["datadetails"] as DataTable;
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[("IrecordwebConnection")].ConnectionString);
            con.Open();
            transaction = con.BeginTransaction("SampleTransaction");
            try
            {
                //if (validatefn())
                //{
                int saverror = 0;
                string bktyp = "BK";
                int Sr_No = 1;
                string vouchrno = "";
                for (int counter1 = 0; counter1 < (dt_details.Rows.Count); counter1++)
                {
                    int trno = GetGlobalTransactionNumber(bktyp);

                    double Finalamount = 0;
                    double DebitAmount = 0;
                    double CreditAmount = 0;
                    if (dt_details.Rows[counter1]["Debit"] != null)
                    {
                        DebitAmount = Convert.ToDouble(dt_details.Rows[counter1]["Debit"].ToString());
                    }
                    //if (dt_details.Rows[counter1]["Credit"] != null)
                    //    CreditAmount = Convert.ToDouble(dt_details.Rows[counter1]["Credit"].ToString());

                    if (Math.Round(DebitAmount, decmpl) > 0)
                    {
                        Finalamount = Math.Round(DebitAmount, decmpl);
                        creditcode = "";//txtBankName.Tag.ToString();
                        debitcode = dt_details.Rows[counter1]["ACcode"].ToString();
                    }
                    else if (Math.Round(CreditAmount, decmpl) > 0)
                    {
                        Finalamount = Math.Round(CreditAmount, decmpl);
                        debitcode = "";
                        creditcode = dt_details.Rows[counter1]["ACcode"].ToString();
                    }

                    cmd = new SqlCommand(" SELECT * FROM AC_Trans WHERE (Trans_Dt = CONVERT(DATETIME, '" + DateTime.ParseExact(dt_details.Rows[counter1]["Date"].ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy") + "', 102))"
                        + " AND (Member_Code = '" + Session["member_code"] + "') AND (Year_Code = '" + Session["year_code"] + "') AND (Book_Type1 = 'BK') AND (Chq_No = '"
                        + dt_details.Rows[counter1]["Cheque"].ToString().Trim() + "') AND ((Dr_Code = '" + debitcode + "') AND (Cr_Code = '" + creditcode + "')) AND (Amount = " + Finalamount + ")", con)
                    {
                        Transaction = transaction
                    };

                    SqlDataAdapter DataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = cmd
                    };
                    DataAdapter.Fill(ds);

                    if (ds.Tables[0].Rows.Count == 0)
                    {

                        string type = "B";
                        string Mode = "";
                        if (Math.Round(DebitAmount) > 0)
                        {
                            Mode = "P";
                        }
                        else
                        {
                            Mode = "R";
                        }

                        //  MessageBox.Show("1");

                        cmd = new SqlCommand("Select Max(Vouch_No) from AC_Trans where Sub_Type1='" + type + "' and Sub_Type2='"
                            + Mode + "' and Book_Type1='BK' and Member_Code='" + Session["member_code"] + "' and Year_Code='" + Session["year_code"] + "'", con)
                        {
                            Transaction = transaction
                        };
                        SqlDataReader drv = cmd.ExecuteReader();

                        if (drv.HasRows)
                        {
                            if (drv.Read())
                            {

                                if (!drv.IsDBNull(0))

                                /// if (vno != "" && Convert.ToInt32(vno) > 0)

                                {
                                    string vno = "0";
                                    try
                                    {
                                        vno = drv.GetString(0);
                                        // Convert.ToInt32(vno);
                                    }
                                    catch (Exception) { vno = "0"; }
                                    if (vno != "" && vno != "0")       /// if (vno != "" && Convert.ToInt32(vno) > 0)
                                    {
                                        int vouchno = 0;
                                        string[] details = vno.Replace("//", "/").Split('/');
                                        int lengthvno = details.Length;
                                        if (lengthvno == 3)
                                        {
                                            vouchno = Convert.ToInt32(details[2]);
                                        }
                                        else
                                        {
                                            vouchno = Convert.ToInt32(details[1]);
                                        }
                                        // int vouchno = Convert.ToInt32(details[1]);
                                        vouchrno = details[0];
                                        vouchno = vouchno + 1;
                                        if (vouchno < 10)
                                        {
                                            vouchrno = vouchrno + "/" + "0000" + vouchno;
                                        }
                                        else if (vouchno < 100)
                                        {
                                            vouchrno = vouchrno + "/" + "000" + vouchno;
                                        }
                                        else if (vouchno < 1000)
                                        {
                                            vouchrno = vouchrno + "/" + "00" + vouchno;
                                        }
                                        else if (vouchno < 10000)
                                        {
                                            vouchrno = vouchrno + "/" + "0" + vouchno;
                                        }
                                        else
                                        {
                                            vouchrno = "" + vouchno;
                                        }
                                    }
                                    else
                                    {

                                        int v = 1;
                                        vouchrno = type + Mode + "/";
                                        if (v < 10)
                                        {
                                            vouchrno = vouchrno + "0000" + v;
                                        }
                                        else if (v < 100)
                                        {
                                            vouchrno = vouchrno + "000" + v;
                                        }
                                        else if (v < 1000)
                                        {
                                            vouchrno = vouchrno + "00" + v;
                                        }
                                        else if (v < 10000)
                                        {
                                            vouchrno = vouchrno + "0" + v;
                                        }
                                        else
                                        {
                                            vouchrno = "" + v;
                                        }
                                    }
                                }
                                else
                                {

                                    int v = 1;
                                    vouchrno = type + Mode + "/";
                                    if (v < 10)
                                    {
                                        vouchrno = vouchrno + "0000" + v;
                                    }
                                    else if (v < 100)
                                    {
                                        vouchrno = vouchrno + "000" + v;
                                    }
                                    else if (v < 1000)
                                    {
                                        vouchrno = vouchrno + "00" + v;
                                    }
                                    else if (v < 10000)
                                    {
                                        vouchrno = vouchrno + "0" + v;
                                    }
                                    else
                                    {
                                        vouchrno = "" + v;
                                    }
                                }
                            }
                        }
                        else
                        {
                            int v = 1;
                            vouchrno = type + Mode + "/";
                            if (v < 10)
                            {
                                vouchrno = vouchrno + "0000" + v;
                            }
                            else if (v < 100)
                            {
                                vouchrno = vouchrno + "000" + v;
                            }
                            else if (v < 1000)
                            {
                                vouchrno = vouchrno + "00" + v;
                            }
                            else if (v < 10000)
                            {
                                vouchrno = vouchrno + "0" + v;
                            }
                            else
                            {
                                vouchrno = "" + v;
                            }
                        }
                        drv.Close();

                        // MessageBox.Show("2");
                        string narr = "";
                        if (dt_details.Rows[counter1]["Narration"] != null)
                        {
                            narr = dt_details.Rows[counter1]["Narration"].ToString().Trim();
                        }

                        string chq = "";
                        if (dt_details.Rows[counter1]["Cheque"] != null)
                        {
                            chq = dt_details.Rows[counter1]["Cheque"].ToString().Trim();
                        }

                        if (chq.Length > 20)
                        {
                            chq = chq.Substring(0, 20);
                        }

                        string billDate = dt_details.Rows[counter1]["Date"].ToString();
                        DateTime TransDate = DateTime.ParseExact(billDate, "dd/MM/yyyy", null);

                        //cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[Member_Code],[Year_Code],[Vouch_No],"
                        //    + " [Chq_No],[Dr_Code] ,[Cr_Code] ,[Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[Sub_Type1],[Sub_Type2],"
                        //    + " [frmpg],[EntryType],[IsDisplayInAc],[isImported],[pay_mode]) VALUES('" + trno + "','" + Sr_No + "','"
                        //    + TransDate.ToString("yyyy-MM-dd") + "','" + MenuMaster.memcode + "','" + MenuMaster.yearcode + "','" + vouchrno + "','" + chq
                        //    + "','" + debitcode + "','" + creditcode + "','" + bktyp + "','','" + narr + "','','N','" + Finalamount + "','0','" + type + "','"
                        //    + Mode + "','BS','0','1','1','1')", con);

                        cmd = new SqlCommand("INSERT INTO[dbo].[AC_Trans]([Trans_No],[Sr_No],[Trans_Dt],[Member_Code],[Year_Code],[Vouch_No],"
                            + " [Chq_No],[Dr_Code] ,[Cr_Code] ,[Book_Type1],[Narr1] ,[Narr2],[Narr3] ,[Internal],[Amount],[InvestmentType],[Sub_Type1],[Sub_Type2],"
                            + " [frmpg],[EntryType],[IsDisplayInAc],[isImported],[pay_mode]) VALUES('" + trno + "','" + Sr_No + "','"
                            + TransDate.ToString("yyyy-MM-dd") + "','" + Session["member_code"] + "','" + Session["year_code"] + "','" + vouchrno + "','" + chq
                            + "','" + debitcode + "','" + creditcode + "','" + bktyp + "','','" + narr.Replace(" ", "<>").Replace("><", "").Replace("<>", " ") + "','','N','" + Finalamount + "','0','" + type + "','"
                            + Mode + "','BS','0','1','1','1')", con)
                        {
                            Transaction = transaction
                        };

                        int nre = cmd.ExecuteNonQuery();
                        if (nre > 0)
                        {
                            Sr_No = Sr_No + 1;
                        }
                        else
                        {
                            saverror++;
                        }
                    }
                    ds.Clear();
                }

                if (saverror == 0)
                {
                    transaction.Commit();
                    // MessageBox.Show("Records Saved Successfully");
                }
                // }
            }
            catch (Exception)
            {
                //  MessageBox.Show(ex.Message);
                transaction.Rollback();
            }
            con.Close();
        }
        public JsonResult GetAllbank()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_BANK_FORMATE_UTITLIY]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWBANK";

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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetAllbankconfiq()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[DBO].[SP_BANK_FORMATE_UTITLIY]"
            };
            cmd.Parameters.Add("@ACTION", SqlDbType.NVarChar).Value = "VIEWCONFIG";

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
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
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
            return Json(DataTableToJSON(DT), JsonRequestBehavior.AllowGet);

        }
    }
}