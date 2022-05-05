using BAL;
using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using LinqToExcel;



namespace DAL
    {
    public class UploadBhavCopyDAL
        {

        CommonFn ComFn = new CommonFn();
        // To Bind Investment Type DropdownList
        public List<MType> BindInvenstmentType(MType _MType)
            {
            List<MType> MTypeList = null;

            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetMTypeMaster", Adapter.Connection);
            com.Parameters.AddWithValue("@ParentTypeId", 1);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            MTypeList = new List<MType>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                MType mobj = new MType();
                mobj.TypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["TypeId"].ToString());
                mobj.ParentTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["ParentTypeId"].ToString());
                mobj.Code = ds.Tables[0].Rows[i]["Code"].ToString();
                mobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                mobj.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                mobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                MTypeList.Add(mobj);
                }

            return MTypeList;

            }

        private static string GetMCXDataField(string[] ar, string Closevalue, out string sExpireDate, out string sStrikePrice, out string sOptionType, out string ScriptName)
            {
            sExpireDate = ar[3];
            sStrikePrice = ar[5];
            sOptionType = ar[4];
            sExpireDate = sExpireDate.Replace("\"", "");
            sStrikePrice = sStrikePrice.Replace("\"", "");
            sOptionType = sOptionType.Replace("\"", "");

            ScriptName = ar[2];
            ScriptName = ScriptName.Replace("\"", "").Trim();

            string dd = sExpireDate.Substring(0, 2);
            string mm = sExpireDate.Substring(2, 3);
            string yy = sExpireDate.Substring(5, 4);

            switch (sOptionType)
                {
                case "XX":
                    sOptionType = "";
                    break;

                case "CE":
                    sOptionType = sOptionType + " - CALL";
                    break;

                case "PE":
                    sOptionType = sOptionType + " - PUT";
                    break;

                case "PA":
                    sOptionType = sOptionType + " - PUT";
                    break;

                case "CA":
                    sOptionType = sOptionType + " - CALL";
                    break;
                case "-":
                    sOptionType = "";
                    break;
                }

            Closevalue = Closevalue.Replace("\"", "");
            return Closevalue;
            }


        private static string GetNCDEXFields(string[] ar, ref string Closevalue)
            {
            string sExpireDate = ar[1];
            sExpireDate = sExpireDate.Replace("\"", "");
            string dd = sExpireDate.Substring(0, 2);
            string mm = sExpireDate.Substring(2, 3);
            string yy = sExpireDate.Substring(5, 4);
            // sExpireDate = dd + "-" + mm + "-" + yy;
            Closevalue = Closevalue.Replace("\"", "");
            return sExpireDate;
            }
        public string GetExchangeRate(int InvestmentTypeIndex, string ExchangeTypeIndex, bool IsFuture)
            {
            string sRate = "";
            if (InvestmentTypeIndex == 2) // 2 = eQUITY AND 3 = f&o
                {
                if (ExchangeTypeIndex == "BSE") // bse                 
                    sRate = "BSE_Rate";
                else
                    sRate = "NSE_Rate";   // nse                                 
                }
            else if (InvestmentTypeIndex == 6)
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
            else if (InvestmentTypeIndex != 2) // F & 0 , NCDEX, MCX
                {
                if (InvestmentTypeIndex == 0 && ExchangeTypeIndex == "BSE") // bse 
                    sRate = "BSE_FO_Rate";
                else
                    sRate = "NSE_FO_Rate";
                }
            else
                sRate = "NSE_Rate";   // nse
            return sRate;
            }


        public static string GetRate(int SelectedInvestmentTypeIndex, string SelectedExchangeTypeIndex, string[] ar)
            {
            string sValue = "";
            string svalueHigh = "";
            if (SelectedInvestmentTypeIndex == 2) //Equity
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
            else if (SelectedInvestmentTypeIndex == 4) //Mutual Fund*
                sValue = ar[4];
            else if (SelectedInvestmentTypeIndex == 5)  // MCX
                sValue = ar[9].Replace("\"", "");
            else if (SelectedInvestmentTypeIndex == 6)   //Currency
                sValue = ar[6].Replace("\"", "");
            else if (SelectedInvestmentTypeIndex == 7) //NCDX         
                sValue = ar[9].Replace("\"", "");

            else
                sValue = ar[8].Replace("\"", "");  //F & O

            if (SelectedInvestmentTypeIndex != 2)
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

        public string GetExchangeCodeColumn(int SelectedInvestmentTypeIndex, string SelectedExchangeTypeIndex)
            {
            string sCode = "";
            if (SelectedInvestmentTypeIndex == 2 || SelectedInvestmentTypeIndex == 3) // 2 = eQUITY AND 3 = f&o
                {
                if (SelectedExchangeTypeIndex == "BSE") // bse 
                    sCode = "BSE_Code";
                else
                    sCode = "NSE_Code";   // nse
                }

            else if (SelectedInvestmentTypeIndex == 4) // MF
                sCode = "BSE_Code";
            else if (SelectedInvestmentTypeIndex == 5) // MCX
                sCode = "NSE_Code";
            else if (SelectedInvestmentTypeIndex == 6) // Currency
                sCode = "NSE_Code";
            else if (SelectedInvestmentTypeIndex == 7) // NCDEX
                sCode = "NSE_Code";
            return sCode;
            }


        public DataTable GetSelectCommand(string[] ar, string sExchangeCode, ref string bsensecode, ref string sCommand, int SelectedInvestmentTypeIndex)
            {


            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            if (SelectedInvestmentTypeIndex == 2)  // Equity
                {
                if (sExchangeCode == "BSE_Code")
                    {
                    var Case = 1;
                    Adapter.AddParam(pcol, "@Case", Case);
                    Adapter.AddParam(pcol, "@ExchangeCode", ar[0]);
                    dt = Adapter.ExecuteDataTable("USPGetScriptByCode", CommandType.StoredProcedure, Adapter.param(pcol));
                    }
                else
                    {
                    var Case = 2;
                    Adapter.AddParam(pcol, "@Case", Case);
                    Adapter.AddParam(pcol, "@ExchangeCode", ar[0]);
                    dt = Adapter.ExecuteDataTable("USPGetScriptByCode", CommandType.StoredProcedure, Adapter.param(pcol));
                    }

                }
            else if (SelectedInvestmentTypeIndex == 6) // Currency
                {
                string example = ar[0];
                string scriptName = example;
                string ExpiryDate = "";
                if (ar[0].Length >= 23)
                    {
                    scriptName = ar[0].Remove(0, 6).Substring(0, 6);
                    ExpiryDate = ar[0].Remove(0, 12);
                    }

                var Case = 2;
                Adapter.AddParam(pcol, "@Case", Case);
                Adapter.AddParam(pcol, "@ExchangeCode", scriptName);
                dt = Adapter.ExecuteDataTable("USPGetScriptByCode", CommandType.StoredProcedure, Adapter.param(pcol));

                }
            //else if (SelectedInvestmentTypeIndex == 4)  // MF
            //    {
            //    bsensecode = ar[0].ToString();
            //    bsensecode = bsensecode.Replace(" ", string.Empty);
            //    sCommand = "SELECT * FROM M_script where " + sExchangeCode + "= '" + bsensecode + "'";
            //    sCommand = sCommand.Replace("\"", string.Empty).Trim();
            //    }
            else if (SelectedInvestmentTypeIndex == 5)  // MCX
                {
                bsensecode = ar[2].ToString();
                bsensecode = bsensecode.Replace(" ", string.Empty).Replace('"', ' ').Trim();
                var Case = 2;
                Adapter.AddParam(pcol, "@Case", Case);
                Adapter.AddParam(pcol, "@ExchangeCode", bsensecode);
                dt = Adapter.ExecuteDataTable("USPGetScriptByCode", CommandType.StoredProcedure, Adapter.param(pcol));
                }
            else if (SelectedInvestmentTypeIndex == 7) //NCDEX
                {
                bsensecode = ar[0].ToString();
                bsensecode = bsensecode.Replace(" ", string.Empty).Replace('"', ' ').Trim();

                var Case = 2;
                Adapter.AddParam(pcol, "@Case", Case);
                Adapter.AddParam(pcol, "@ExchangeCode", bsensecode);
                dt = Adapter.ExecuteDataTable("USPGetScriptByCode", CommandType.StoredProcedure, Adapter.param(pcol));
                }
            return dt;
            }

        private void SaveEquity(string sExchangeRate, string Closevalue, string scrrp, int iCount, out string sQuery, string datestring, int SelectedInvestmentTypeIndex, string Highvalue, string HightRate)
            {
            DataTable dt = new DataTable();
            string Transdate = datestring.Substring(0, 10);
            datestring = DateTime.ParseExact(Transdate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
            if (sExchangeRate == "BSE_Rate")
                {
                if (iCount == 0)
                    {
                    var Case = 1;

                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@Case", Case);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@BSERate", Closevalue);
                    Adapter.AddParam(pcol, "@BSEHighRate", Highvalue);

                    Adapter.ExecutenNonQuery("USPUploadBhavCopyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                    sQuery = "";
                    }
                else
                    {
                    //sQuery = "UPDATE  DailyRate SET " + sExchangeRate + " =  " + Closevalue + " ,  " + HightRate + "='" + Highvalue + "' Where Trans_Dt = '" + datestring.ToString("MM/dd/yyyy") + "' and Script_Code ='" + scrrp + "'  and InvestmentType ='" + SelectedInvestmentTypeIndex + "'";
                    var Case = 2;
                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@Case", Case);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@BSERate", Closevalue);
                    Adapter.AddParam(pcol, "@BSEHighRate", Highvalue);

                    Adapter.ExecutenNonQuery("USPUploadBhavCopyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                    sQuery = "";
                    }
                }

            else
                {
                if (iCount == 0)
                    {
                    var Case = 3;
                    //sQuery = "insert into DailyRate(Trans_Dt,Script_Code," + sExchangeRate + ",InvestmentType," + HightRate + ") values('" + datestring.ToString("MM/dd/yyyy") + "','" + scrrp + "','" + Closevalue + "','" + SelectedInvestmentTypeIndex + "','" + Highvalue + "')";
                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@Case", Case);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@NSERate", Closevalue);
                    Adapter.AddParam(pcol, "@NSEHighRate", Highvalue);

                    Adapter.ExecutenNonQuery("USPUploadBhavCopyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                    sQuery = "";
                    }
                else
                    {
                    var Case = 4;
                    SqlParameterCollection pcol = new SqlCommand().Parameters;
                    Adapter.AddParam(pcol, "@Case", Case);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@NSERate", Closevalue);
                    Adapter.AddParam(pcol, "@NSEHighRate", Highvalue);

                    Adapter.ExecutenNonQuery("USPUploadBhavCopyInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
                    sQuery = "";
                    }

                }

            //cmd = new SqlCommand(sQuery, con);
            //cmd.ExecuteNonQuery();
            //  return dt;

            }
        private DataTable CheckDailtRatesExists(SqlConnection con, out SqlCommand cmd, string scrrp, out int iCount, DateTime datestring, int SelectedInvestmentTypeIndex, string ExpireDate = "", string OptionType = "", string strikePrice = "", bool IsFuture = false)
            {
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            DataTable dt = new DataTable();
            string sDuplicateCheck = "";
            if (SelectedInvestmentTypeIndex == 2 || SelectedInvestmentTypeIndex == 4)    //2 - Equity & 4 MF
                {
                var Case = 1;
                Adapter.AddParam(pcol, "@Case", Case);
                Adapter.AddParam(pcol, "@TransDate", datestring);
                Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                dt = Adapter.ExecuteDataTable("USPGetCkeckDuplicateEntry", CommandType.StoredProcedure, Adapter.param(pcol));

                }
            else if (SelectedInvestmentTypeIndex == 7)     //NCDEX
                {

                var Case = 3;
                Adapter.AddParam(pcol, "@Case", Case);
                Adapter.AddParam(pcol, "@TransDate", datestring);
                Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                Adapter.AddParam(pcol, "@ExpDate", ExpireDate);
                dt = Adapter.ExecuteDataTable("USPGetCkeckDuplicateEntry", CommandType.StoredProcedure, Adapter.param(pcol));
                }
            else if (SelectedInvestmentTypeIndex == 6)    //Currency
                {
                if (IsFuture)
                    {
                    var Case = 3;
                    Adapter.AddParam(pcol, "@Case", Case);
                    Adapter.AddParam(pcol, "@TransDate", datestring);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@ExpDate", ExpireDate);
                    dt = Adapter.ExecuteDataTable("USPGetCkeckDuplicateEntry", CommandType.StoredProcedure, Adapter.param(pcol));
                    }
                else
                    {
                    var Case = 2;
                    string Transdate = datestring.ToString().Substring(0, 10);
                    // string expdate = DateTime.ParseExact(ExpireDate, "ddMMMyyyy", null).ToString("yyyy-MM-dd");
                    Transdate = DateTime.ParseExact(Transdate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                    Adapter.AddParam(pcol, "@Case", Case);
                    Adapter.AddParam(pcol, "@TransDate", Transdate);
                    Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                    Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                    Adapter.AddParam(pcol, "@ExpDate", ExpireDate);
                    Adapter.AddParam(pcol, "@StrikePrice", strikePrice);
                    Adapter.AddParam(pcol, "@OptionType", OptionType);
                    dt = Adapter.ExecuteDataTable("USPGetCkeckDuplicateEntry", CommandType.StoredProcedure, Adapter.param(pcol));
                    }

                }
            else if (SelectedInvestmentTypeIndex == 3 || SelectedInvestmentTypeIndex == 5) // 3 -F&O , 5 - MCX
                {
                var Case = 2;
                string Transdate = datestring.ToString().Substring(0, 10);
                string expdate = DateTime.ParseExact(ExpireDate, "ddMMMyyyy", null).ToString("yyyy-MM-dd");
                Transdate = DateTime.ParseExact(Transdate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                Adapter.AddParam(pcol, "@Case", Case);
                Adapter.AddParam(pcol, "@TransDate", Transdate);
                Adapter.AddParam(pcol, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol, "@InvestmentType", SelectedInvestmentTypeIndex);
                Adapter.AddParam(pcol, "@ExpDate", expdate);
                Adapter.AddParam(pcol, "@StrikePrice", strikePrice);
                Adapter.AddParam(pcol, "@OptionType", OptionType);
                dt = Adapter.ExecuteDataTable("USPGetCkeckDuplicateEntry", CommandType.StoredProcedure, Adapter.param(pcol));

                }
            cmd = new SqlCommand(sDuplicateCheck, con);
            iCount = (int)dt.Rows[0]["Column1"];

            return dt;
            }
        private static void GetCurrencyDataField(string[] ar, out string sExpireDate, out string sStrikePrice, out string sOptionType, out string script_name, bool IsFuture)
            {
            string example = ar[0];
            string ExpiryDate = "";
            string StrikePrice = "";
            string OptionType = "";

            if (ar[0].Length >= 23)
                {
                string str = ar[0].Substring(0, 6);
                if (IsFuture)
                    {
                    ExpiryDate = ar[0].Remove(0, 12);
                    StrikePrice = "";
                    OptionType = "";

                    }
                else
                    {
                    ExpiryDate = ar[0].Substring(12, 11);
                    OptionType = ar[0].Substring(23, 2);
                    StrikePrice = ar[0].Substring(ar[0].LastIndexOf(OptionType) + 2);
                    if (OptionType.Equals("CE"))
                        {
                        OptionType = "CE - CALL";
                        }
                    else if (OptionType.Equals("CA"))
                        {
                        OptionType = "CA - CALL";
                        }
                    else if (OptionType.Equals("PE"))
                        {
                        OptionType = "PE - PUT";
                        }
                    else if (OptionType.Equals("PA"))
                        {
                        OptionType = "PA -PUT";
                        }
                    else if (OptionType.Equals("-"))
                        {
                        OptionType = "";
                        }
                    }
                }
            sExpireDate = ExpiryDate;
            sStrikePrice = StrikePrice;
            sOptionType = OptionType == "XX" ? "" : OptionType;
            script_name = ar[0].Remove(0, 6).Substring(0, 6);
            }

        public bool SaveDailyRates(string FilePath, DateTime dateString, int SelectedInvestmentTypeIndex, string ExchangeTypeIndex, bool IsDownloadClick = false)
            {
            try
                {
                SqlConnection con = Adapter.Connection;
                con.Open();
                bool IsValid = true;

                if (IsValid)
                    {
                    string Data = File.ReadAllText(FilePath);
                    string[] ar = new string[14];
                    string sHighRate;
                    SqlCommand cmd = null;
                    SqlDataReader datReader = null;
                    string sExchangeCode = GetExchangeCodeColumn(SelectedInvestmentTypeIndex, ExchangeTypeIndex);
                    bool IsFuture = false;
                    IsFuture = Data.Split('\n')[1].ToUpper().Contains("FUTCUR");
                    string sExchangeRate = GetExchangeRate(SelectedInvestmentTypeIndex, ExchangeTypeIndex, IsFuture);
                    if (SelectedInvestmentTypeIndex == 0 && ExchangeTypeIndex == "BSE")
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
                                if (SelectedInvestmentTypeIndex == 4)
                                    ar = row.Split(';');
                                else
                                    ar = row.Split(',');

                                string sCommand = "";
                                string Closevalue = "";
                                string Highvalue = "";
                                if (((ar.Length > 5 && ar.Length < 9) && SelectedInvestmentTypeIndex == 4) || (SelectedInvestmentTypeIndex != 4))
                                    {
                                    string sValue = GetRate(SelectedInvestmentTypeIndex, ExchangeTypeIndex, ar);
                                    if (SelectedInvestmentTypeIndex != 4)
                                        {
                                        if (ExchangeTypeIndex == "BSE")
                                            {
                                            string[] combindvalues = sValue.Split(',');
                                            int index = combindvalues.Length;
                                            if (SelectedInvestmentTypeIndex == 5)
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
                                            if (SelectedInvestmentTypeIndex == 2)
                                                {
                                                //if(ar[0]=="HMT")
                                                //{
                                                //    MessageBox.Show("Find script");
                                                //}
                                                //if (ar[0] == "SIL")
                                                //{
                                                //    MessageBox.Show("d");
                                                //}


                                                if (SelectedInvestmentTypeIndex == 2 && ExchangeTypeIndex == "NSE" && (ar[1] == "EQ" || ar[1] == "BZ" || ar[1] == "BE" || ar[1] == "E1"))
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
                                            if (SelectedInvestmentTypeIndex == 2) // Equity
                                                {
                                                if (SelectedInvestmentTypeIndex == 1 && (ar[1] == "EQ" || ar[1] == "BZ" || ar[1] == "BE" || ar[1] == "E1"))
                                                    {
                                                    //CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, _Bhavcopy.InvestmentType);
                                                    //SaveEquity(con, out cmd, sExchangeRate, Closevalue, scrrp, iCount, out sQuery, dateString, _Bhavcopy.InvestmentType, Highvalue, sHighRate);
                                                    }
                                                else if (SelectedInvestmentTypeIndex == 2)
                                                    {

                                                    string Transdate = dateString.ToString();
                                                    CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, SelectedInvestmentTypeIndex);
                                                    SaveEquity(sExchangeRate, Closevalue, scrrp, iCount, out sQuery, Transdate, SelectedInvestmentTypeIndex, Highvalue, sHighRate);

                                                    }
                                                }
                                            else if (SelectedInvestmentTypeIndex != 2 && SelectedInvestmentTypeIndex != 4) // F & O
                                                {
                                                string sExpireDate = ""; string sStrikePrice = ""; string sOptionType = ""; string Script_Name = "";
                                                if (SelectedInvestmentTypeIndex == 5)     //MCX
                                                    Closevalue = GetMCXDataField(ar, Closevalue, out sExpireDate, out sStrikePrice, out sOptionType, out Script_Name);
                                                else if (SelectedInvestmentTypeIndex == 6)  //Currency
                                                    GetCurrencyDataField(ar, out sExpireDate, out sStrikePrice, out sOptionType, out Script_Name, IsFuture);
                                                else if (SelectedInvestmentTypeIndex == 7)   // NCDX
                                                    sExpireDate = GetNCDEXFields(ar, ref Closevalue);

                                                CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, SelectedInvestmentTypeIndex, sExpireDate, sOptionType, sStrikePrice, IsFuture);
                                                if (iCount == 0)
                                                    {
                                                    if (SelectedInvestmentTypeIndex == 6)
                                                        {
                                                        ComFn.SaveDonldCurrencyData(dateString, SelectedInvestmentTypeIndex, scrrp, sExpireDate, sStrikePrice, sOptionType, Closevalue, Script_Name, sExchangeRate);
                                                        }
                                                    else
                                                        {
                                                        var Case1 = 3;
                                                        SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                                        Adapter.AddParam(pcol1, "@Case", Case1);
                                                        Adapter.AddParam(pcol1, "@TransDate", dateString);
                                                        Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                                                        Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                                                        Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                                                        Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                                                        Adapter.AddParam(pcol1, "@InvestmentType", SelectedInvestmentTypeIndex);
                                                        Adapter.AddParam(pcol1, "@NSEFORate", Closevalue);
                                                        Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                                                        Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));
                                                        }
                                                    }
                                                else
                                                    {
                                                    if (SelectedInvestmentTypeIndex == 6)
                                                        {
                                                        ComFn.UpdateDownldCurrencyData(dateString, SelectedInvestmentTypeIndex, scrrp, sExpireDate, sStrikePrice, sOptionType, Closevalue, Script_Name, sExchangeRate);
                                                        }
                                                    else
                                                        {
                                                        var Case1 = 4;
                                                        SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                                        Adapter.AddParam(pcol1, "@Case", Case1);
                                                        Adapter.AddParam(pcol1, "@TransDate", dateString);
                                                        Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                                                        Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                                                        Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                                                        Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                                                        Adapter.AddParam(pcol1, "@InvestmentType", SelectedInvestmentTypeIndex);
                                                        Adapter.AddParam(pcol1, "@NSEFORate", Closevalue);
                                                        Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                                                        Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));
                                                        }
                                                    }
                                                }
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

        public bool SaveMCXDwnldDailyRates(string FilePath, DateTime dateString, int SelectedInvestmentTypeIndex, string ExchangeTypeIndex, DataTable dtexc, bool IsDownloadClick = false)
            {
            try
                {
                SqlConnection con = Adapter.Connection;
                con.Open();
                bool IsValid = true;
                int i = 0;
                string DtData = "";
                if (dtexc.Rows.Count > 0)
                    {
                    for (i = 0; i < dtexc.Rows.Count; i++)
                        {
                        string Date = dtexc.Rows[i]["Date"].ToString();
                        string Instrument = dtexc.Rows[i]["Instrument Name"].ToString();
                        string Symbol = dtexc.Rows[i]["Symbol"].ToString();
                        string ExpiryDt = dtexc.Rows[i]["Expiry Date"].ToString();
                        string OptionType = dtexc.Rows[i]["Option Type"].ToString();
                        string StrikePrice = dtexc.Rows[i]["Strike Price"].ToString();
                        string Open = dtexc.Rows[i]["Open"].ToString();
                        string High = dtexc.Rows[i]["High"].ToString();
                        string Low = dtexc.Rows[i]["Low"].ToString();
                        string Close = dtexc.Rows[i]["Close"].ToString();
                        string Volume = dtexc.Rows[i]["Volume(Lots)"].ToString();
                        string Volume_In_Prcntg = dtexc.Rows[i]["Volume(In 000's)"].ToString();
                        DtData = Date + "," + Instrument.Trim() + "," + Symbol + "," + ExpiryDt + "," + OptionType + "," + StrikePrice + "," + Open + "," + High + "," + Low + "," + Close + "," + Volume + "," + Volume_In_Prcntg;

                        // }

                     // }
                    
                    if (IsValid)
                        {

                        string Data = File.ReadAllText(FilePath);
                        string[] ar = new string[14];
                        string sHighRate;
                        SqlCommand cmd = null;
                        SqlDataReader datReader = null;
                        string sExchangeCode = GetExchangeCodeColumn(SelectedInvestmentTypeIndex, ExchangeTypeIndex);
                        bool IsFuture = false;
                        IsFuture = Data.Split('\n')[1].ToUpper().Contains("FUTCUR");
                        string sExchangeRate = GetExchangeRate(SelectedInvestmentTypeIndex, ExchangeTypeIndex, IsFuture);
                        if (SelectedInvestmentTypeIndex == 0 && ExchangeTypeIndex == "BSE")
                            sHighRate = "BSEHighRate";
                        else
                            sHighRate = "NSEHighRate";

                        string bsensecode = ""; string row = "";
                        //foreach (string row in DtData.Split('\n'))
                        //   {
                        row = DtData;

                        string r = row;
                        
                        r = r.Replace("\r", "");

                        if (r != " ")
                            if (!string.IsNullOrEmpty(row))
                                {
                                if (SelectedInvestmentTypeIndex == 4)
                                    ar = row.Split(';');
                                else
                                    ar = row.Split(',');

                                string sCommand = "";
                                string Closevalue = "";
                                string Highvalue = "";
                                if (((ar.Length > 5 && ar.Length < 9) && SelectedInvestmentTypeIndex == 4) || (SelectedInvestmentTypeIndex != 4))
                                    {
                                    string sValue = GetRate(SelectedInvestmentTypeIndex, ExchangeTypeIndex, ar);
                                    if (SelectedInvestmentTypeIndex != 4)
                                        {
                                        if (ExchangeTypeIndex == "BSE")
                                            {
                                            string[] combindvalues = sValue.Split(',');
                                            int index = combindvalues.Length;
                                            if (SelectedInvestmentTypeIndex == 5)
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
                                            if (SelectedInvestmentTypeIndex == 2)
                                                {
                                                //if(ar[0]=="HMT")
                                                //{
                                                //    MessageBox.Show("Find script");
                                                //}
                                                //if (ar[0] == "SIL")
                                                //{
                                                //    MessageBox.Show("d");
                                                //}


                                                if (SelectedInvestmentTypeIndex == 2 && ExchangeTypeIndex == "NSE" && (ar[1] == "EQ" || ar[1] == "BZ" || ar[1] == "BE" || ar[1] == "E1"))
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
                                            if (SelectedInvestmentTypeIndex == 2) // Equity
                                                {
                                                if (SelectedInvestmentTypeIndex == 1 && (ar[1] == "EQ" || ar[1] == "BZ" || ar[1] == "BE" || ar[1] == "E1"))
                                                    {
                                                    //CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, _Bhavcopy.InvestmentType);
                                                    //SaveEquity(con, out cmd, sExchangeRate, Closevalue, scrrp, iCount, out sQuery, dateString, _Bhavcopy.InvestmentType, Highvalue, sHighRate);
                                                    }
                                                else if (SelectedInvestmentTypeIndex == 2)
                                                    {

                                                    string Transdate = dateString.ToString();
                                                    CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, SelectedInvestmentTypeIndex);
                                                    SaveEquity(sExchangeRate, Closevalue, scrrp, iCount, out sQuery, Transdate, SelectedInvestmentTypeIndex, Highvalue, sHighRate);

                                                    }
                                                }
                                            else if (SelectedInvestmentTypeIndex != 2 && SelectedInvestmentTypeIndex != 4) // F & O
                                                {
                                                string sExpireDate = ""; string sStrikePrice = ""; string sOptionType = ""; string Script_Name = "";
                                                if (SelectedInvestmentTypeIndex == 5)     //MCX
                                                    Closevalue = GetMCXDataField(ar, Closevalue, out sExpireDate, out sStrikePrice, out sOptionType, out Script_Name);
                                                else if (SelectedInvestmentTypeIndex == 6)  //Currency
                                                    GetCurrencyDataField(ar, out sExpireDate, out sStrikePrice, out sOptionType, out Script_Name, IsFuture);
                                                else if (SelectedInvestmentTypeIndex == 7)   // NCDX
                                                    sExpireDate = GetNCDEXFields(ar, ref Closevalue);

                                                CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, SelectedInvestmentTypeIndex, sExpireDate, sOptionType, sStrikePrice, IsFuture);
                                                if (iCount == 0)
                                                    {
                                                    if (SelectedInvestmentTypeIndex == 6)
                                                        {
                                                        ComFn.SaveDonldCurrencyData(dateString, SelectedInvestmentTypeIndex, scrrp, sExpireDate, sStrikePrice, sOptionType, Closevalue, Script_Name, sExchangeRate);
                                                        }
                                                    else
                                                        {
                                                        var Case1 = 3;
                                                        SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                                        Adapter.AddParam(pcol1, "@Case", Case1);
                                                        Adapter.AddParam(pcol1, "@TransDate", dateString);
                                                        Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                                                        Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                                                        Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                                                        Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                                                        Adapter.AddParam(pcol1, "@InvestmentType", SelectedInvestmentTypeIndex);
                                                        Adapter.AddParam(pcol1, "@NSEFORate", Closevalue);
                                                        Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                                                        Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));
                                                        }
                                                    }
                                                else
                                                    {
                                                    if (SelectedInvestmentTypeIndex == 6)
                                                        {
                                                        ComFn.UpdateDownldCurrencyData(dateString, SelectedInvestmentTypeIndex, scrrp, sExpireDate, sStrikePrice, sOptionType, Closevalue, Script_Name, sExchangeRate);
                                                        }
                                                    else
                                                        {
                                                        var Case1 = 4;
                                                        SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                                        Adapter.AddParam(pcol1, "@Case", Case1);
                                                        Adapter.AddParam(pcol1, "@TransDate", dateString);
                                                        Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                                                        Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                                                        Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                                                        Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                                                        Adapter.AddParam(pcol1, "@InvestmentType", SelectedInvestmentTypeIndex);
                                                        Adapter.AddParam(pcol1, "@NSEFORate", Closevalue);
                                                        Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                                                        Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));
                                                        }
                                                    }
                                                }
                                            }
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
        public void InsertUploadBhavCopyMaster(UploadBhavCopy _Bhavcopy, HttpPostedFileBase uploadfile)
            {

            SqlConnection con = Adapter.Connection;
            bool IsValid = true;

            if (IsValid)
                {
                string targetpath = HttpContext.Current.Server.MapPath("~/UploadData/");
                string filename = uploadfile.FileName;
                uploadfile.SaveAs(targetpath + filename);

                string pathToExcelFile = targetpath + filename;
                HttpContext.Current.Session["pathToExcelFile"] = pathToExcelFile;
                string pathToExcelFile1 = targetpath + Path.GetFileNameWithoutExtension(filename);

                DataTable dtExcelSchema = new DataTable();
                var excelFile = new ExcelQueryFactory(pathToExcelFile);

                string csvData = File.ReadAllText(pathToExcelFile);

                DataSet ds = new DataSet();

                string[] ar = new string[14];

                string sHighRate;
                SqlCommand cmd = null;

                string sExchangeCode = GetExchangeCodeColumn(_Bhavcopy.InvestmentType, _Bhavcopy.Exchange);
                bool IsFuture = false;
                IsFuture = csvData.Split('\n')[1].ToUpper().Contains("FUTCUR");
                int InvestmentType = _Bhavcopy.InvestmentType;
                string Exchange = _Bhavcopy.Exchange;
                string sExchangeRate = GetExchangeRate(InvestmentType, Exchange, IsFuture);
                if (_Bhavcopy.InvestmentType == 2 && _Bhavcopy.Exchange == "BSE")
                    sHighRate = "BSE_High_Rate";
                else
                    sHighRate = "NSE_High_Rate";

                string bsensecode = "";
                foreach (string row in csvData.Split('\n'))
                    {
                    string r = row;
                    r = r.Replace("\r", "");

                    if (r != " ")
                        if (!string.IsNullOrEmpty(row))
                            {
                            if (_Bhavcopy.InvestmentType == 4)
                                ar = row.Split(';');
                            else
                                ar = row.Split(',');
                            if (ar[0].Contains("SC_CODE"))
                                {
                                continue;
                                }
                            string sCommand = "";
                            string Closevalue = "";
                            string Highvalue = "";
                            if (((ar.Length > 5 && ar.Length < 9) && _Bhavcopy.InvestmentType == 4) || (_Bhavcopy.InvestmentType != 4))
                                {
                                string sValue = GetRate(_Bhavcopy.InvestmentType, _Bhavcopy.Exchange, ar);
                                if (_Bhavcopy.InvestmentType != 4)
                                    {
                                    if (_Bhavcopy.InvestmentType == 2)
                                        {
                                        string[] combindvalues = sValue.Split(',');
                                        int index = combindvalues.Length;
                                        if (_Bhavcopy.InvestmentType == 5)
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
                                        if (_Bhavcopy.InvestmentType == 0)
                                            {

                                            if (_Bhavcopy.InvestmentType == 0 && _Bhavcopy.InvestmentType == 1 && (ar[1] == "EQ" || ar[1] == "BZ" || ar[1] == "BE" || ar[1] == "E1"))
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

                                DataTable Dtdata = GetSelectCommand(ar, sExchangeCode, ref bsensecode, ref sCommand, _Bhavcopy.InvestmentType);


                                if (Dtdata.Rows.Count > 0)
                                    {

                                    string scrrp = Dtdata.Rows[0]["BSECode"].ToString();


                                    /*fetch scriptcode if exists*/
                                    if (scrrp != "")
                                        {
                                        int iCount = 0;
                                        cmd = null;
                                        string sQuery = "";
                                        if (_Bhavcopy.InvestmentType == 2) // Equity
                                            {
                                            if (_Bhavcopy.InvestmentType == 1 && (ar[1] == "EQ" || ar[1] == "BZ" || ar[1] == "BE" || ar[1] == "E1"))
                                                {
                                                //CheckDailtRatesExists(con, out cmd, scrrp, out iCount, dateString, _Bhavcopy.InvestmentType);
                                                //SaveEquity(con, out cmd, sExchangeRate, Closevalue, scrrp, iCount, out sQuery, dateString, _Bhavcopy.InvestmentType, Highvalue, sHighRate);
                                                }
                                            else if (_Bhavcopy.InvestmentType == 2)
                                                {

                                                string Transdate = _Bhavcopy.TransDate.ToString();
                                                CheckDailtRatesExists(con, out cmd, scrrp, out iCount, _Bhavcopy.TransDate, _Bhavcopy.InvestmentType);
                                                SaveEquity(sExchangeRate, Closevalue, scrrp, iCount, out sQuery, Transdate, _Bhavcopy.InvestmentType, Highvalue, sHighRate);

                                                }
                                            }
                                        else if (_Bhavcopy.InvestmentType != 0 && _Bhavcopy.InvestmentType != 2) // F & O
                                            {
                                            string sExpireDate = ""; string sStrikePrice = ""; string sOptionType = ""; string Script_Name = "";
                                            if (_Bhavcopy.InvestmentType == 5)     //MCX
                                                Closevalue = GetMCXDataField(ar, Closevalue, out sExpireDate, out sStrikePrice, out sOptionType, out Script_Name);
                                            else if (_Bhavcopy.InvestmentType == 6)  //Currency
                                                GetCurrencyDataField(ar, out sExpireDate, out sStrikePrice, out sOptionType, out Script_Name, IsFuture);
                                            else if (_Bhavcopy.InvestmentType == 7)   // NCDX
                                                sExpireDate = GetNCDEXFields(ar, ref Closevalue);

                                            CheckDailtRatesExists(con, out cmd, scrrp, out iCount, _Bhavcopy.TransDate, _Bhavcopy.InvestmentType, sExpireDate, sOptionType, sStrikePrice, IsFuture);
                                            if (iCount == 0)
                                                {
                                                if (_Bhavcopy.InvestmentType == 6)
                                                    {
                                                    ComFn.SaveCurrencyData(_Bhavcopy, scrrp, sExpireDate, sStrikePrice, sOptionType, Closevalue, Script_Name, sExchangeRate);
                                                    }
                                                else
                                                    {
                                                    var Case1 = 3;
                                                    SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                                    Adapter.AddParam(pcol1, "@Case", Case1);
                                                    Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                                                    Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                                                    Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                                                    Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                                                    Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                                                    Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                                                    Adapter.AddParam(pcol1, "@NSEFORate", Closevalue);
                                                    Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                                                    Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));
                                                    }
                                                }
                                            else
                                                {
                                                if (_Bhavcopy.InvestmentType == 6)
                                                    {
                                                    ComFn.UpdateCurrencyData(_Bhavcopy, scrrp, sExpireDate, sStrikePrice, sOptionType, Closevalue, Script_Name, sExchangeRate);
                                                    }
                                                else
                                                    {
                                                    var Case1 = 4;
                                                    SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                                    Adapter.AddParam(pcol1, "@Case", Case1);
                                                    Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                                                    Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                                                    Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                                                    Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                                                    Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                                                    Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                                                    Adapter.AddParam(pcol1, "@NSEFORate", Closevalue);
                                                    Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                                                    Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                    }
                }
            }
        }
    }