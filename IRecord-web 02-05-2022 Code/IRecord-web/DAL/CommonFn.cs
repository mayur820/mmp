using BAL;
using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
    {
    public class CommonFn
        {
        public static DataTable ReadCsvFile(string filePath, int i)
            {
            DataTable dtCsv = new DataTable();

            string[] rows = File.ReadAllText(filePath).Split('\n'); //split full file text into rows
            int start = i;
            for (; i < rows.Count() - 1; i++)
                {
                List<string> values = new List<string>();
                while (rows[i] != "")
                    {
                    if (rows[i][0] == '"')
                        {
                        int index = rows[i].IndexOf('"', 1);
                        values.Add(rows[i].Substring(1, index - 1));
                        if (index == rows[i].Length - 1)
                            {
                            rows[i] = "";
                            }
                        else
                            {
                            rows[i] = rows[i].Substring(index + 2);
                            }
                        }
                    else
                        {
                        int index = rows[i].IndexOf(',');
                        if (index == -1)
                            {
                            values.Add(rows[i]);
                            rows[i] = "";
                            }
                        else
                            {
                            values.Add(rows[i].Substring(0, index));
                            rows[i] = rows[i].Substring(index + 1);
                            }

                        }
                    }
                string[] rowValues = values.ToArray<string>();//rows[i].Split(','); //split each row with comma to get individual values  
                /*if (rows[i].Contains("\",\""))
                {
                    rowValues = rows[i].Substring(1, rows[i].Length - 1).Split(new[] { "\",\"" }, StringSplitOptions.None);
                }
                else
                {
                    rowValues = rows[i].Split(',');
                }*/
                if (i == start)
                    {
                    for (int j = 0; j < rowValues.Count(); j++)
                        {
                        dtCsv.Columns.Add(rowValues[j]); //add headers  
                        }
                    }
                else
                    {
                    DataRow dr = dtCsv.NewRow();
                    for (int k = 0; k < rowValues.Count(); k++)
                        {
                        dr[k] = rowValues[k].ToString();
                        }
                    dtCsv.Rows.Add(dr); //add other rows  
                    }
                }
            return dtCsv;
            }

        public static void SaveDailyRates(HttpPostedFileBase FilePath, DateTime SelectedDate, int SelectedInvestmentTypeIndex)
            {
            try
                {
                string targetpath = HttpContext.Current.Server.MapPath("~/UploadData/");
                string filename = FilePath.FileName;
                FilePath.SaveAs(targetpath + filename);

                string pathToExcelFile = targetpath + filename;
                HttpContext.Current.Session["pathToExcelFile"] = pathToExcelFile;
                DataTable CsvFile = CommonFn.ReadCsvFile(pathToExcelFile, 0);

                SqlConnection con = Adapter.Connection;
                con.Open();
                SqlCommand cmd;
                SqlDataReader reader;
                string Query;

                string selectedDate = SelectedDate.ToString("yyyy-MM-dd");

                //DELETE Previous
                switch (SelectedInvestmentTypeIndex)
                    {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        //Query = "DELETE FROM DailyRate WHERE InvestmentType = 1 AND Trans_Dt = '" + selectedDate + "'";
                        //cmd = new SqlCommand(Query, con);
                        //cmd.ExecuteNonQuery();
                        var Case = 1;
                        SqlParameterCollection pcol = new SqlCommand().Parameters;
                        Adapter.AddParam(pcol, "@Case", Case);
                        Adapter.AddParam(pcol, "@TransDate", selectedDate);
                        Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol));
                        break;
                    }

                //INSERT Previous
                switch (SelectedInvestmentTypeIndex)
                    {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        int rowCount = CsvFile.Rows.Count;
                        string PreviousScript = "";
                        foreach (DataRow Row in CsvFile.Rows)
                            {
                            string ScriptName = Row["SYMBOL"].ToString();
                            //if (!PreviousScript.Equals(ScriptName))
                                {
                                //  Query = "SELECT DISTINCT Script_Code FROM Mst_Script WHERE NSE_CODE = '" + ScriptName + "'";
                                //  cmd = new SqlCommand(Query, con);
                                DataTable dt = new DataTable();
                                var Case = 2;
                                SqlParameterCollection pcol = new SqlCommand().Parameters;
                                Adapter.AddParam(pcol, "@Case", Case);
                                Adapter.AddParam(pcol, "@ScriptName", ScriptName);
                                dt = Adapter.ExecuteDataTable("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol));


                                DataTable ScriptTable = new DataTable();
                                //   ScriptTable.Load(Adapter.ExecuteReader("USPBhavCopyFNO"));
                                ScriptTable = dt;
                                string ExpireDate = "";
                                try
                                    {
                                    ExpireDate = DateTime.ParseExact(Row["EXPIRY_DT"].ToString(), "dd-MMM-yyyy", null)
                                    .ToString("MM/dd/yyyy");
                                    }
                                catch (Exception e)
                                    {
                                    ExpireDate = DateTime.Parse(Row["EXPIRY_DT"].ToString()).ToString("MM/dd/yyyy");
                                    }

                                string StrikePrice = Row["STRIKE_PR"].ToString();
                                StrikePrice = StrikePrice.Equals("0") ? "" : StrikePrice;

                                string OptionType = Row["OPTION_TYP"].ToString().Trim(); ;
                                switch (OptionType)
                                    {
                                    case "XX":
                                        OptionType = "";
                                        break;

                                    case "CE":
                                        OptionType = OptionType + " - CALL";
                                        break;

                                    case "PE":
                                        OptionType = OptionType + " - PUT";
                                        break;

                                    case "PA":
                                        OptionType = OptionType + " - PUT";
                                        break;

                                    case "CA":
                                        OptionType = OptionType + " - CALL";
                                        break;
                                    }

                                string Rate = Row["CLOSE"].ToString();

                                for (int iScript = 0; iScript < ScriptTable.Rows.Count; iScript++)
                                    {
                               
                                    var Case1 = 3;
                                    SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                    Adapter.AddParam(pcol1, "@Case", Case1);
                                    Adapter.AddParam(pcol1, "@TransDate", selectedDate);
                                    Adapter.AddParam(pcol1, "@ScriptCode", ScriptTable.Rows[iScript][0]);
                                    Adapter.AddParam(pcol1, "@ExpDate", ExpireDate);
                                    Adapter.AddParam(pcol1, "@StrikePrice", StrikePrice);
                                    Adapter.AddParam(pcol1, "@OptionType", OptionType);
                                    Adapter.AddParam(pcol1, "@InvestmentType", SelectedInvestmentTypeIndex);
                                    Adapter.AddParam(pcol1, "@NSEFORate", Rate);
                                    Adapter.AddParam(pcol1, "@ScriptName", ScriptName);
                                    Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));


                                    }
                                }
                            PreviousScript = ScriptName;
                            }
                        break;
                    }
                }
            catch (Exception ex)
                {
                //  MessageBox.Show(ex.StackTrace);
                }
            }

        public static void SaveFNODailyRates(string FilePath, DateTime SelectedDate, int SelectedInvestmentTypeIndex)
            {
            try
                {
               
                DataTable CsvFile = CommonFn.ReadCsvFile(FilePath, 0);

                SqlConnection con = Adapter.Connection;
                con.Open();
                SqlCommand cmd;
                SqlDataReader reader;
                string Query;

                string selectedDate = SelectedDate.ToString("yyyy-MM-dd");

                //DELETE Previous
                switch (SelectedInvestmentTypeIndex)
                    {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        //Query = "DELETE FROM DailyRate WHERE InvestmentType = 1 AND Trans_Dt = '" + selectedDate + "'";
                        //cmd = new SqlCommand(Query, con);
                        //cmd.ExecuteNonQuery();
                        var Case = 1;
                        SqlParameterCollection pcol = new SqlCommand().Parameters;
                        Adapter.AddParam(pcol, "@Case", Case);
                        Adapter.AddParam(pcol, "@TransDate", selectedDate);
                        Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol));
                        break;
                    }

                //INSERT Previous
                switch (SelectedInvestmentTypeIndex)
                    {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        int rowCount = CsvFile.Rows.Count;
                        string PreviousScript = "";
                        foreach (DataRow Row in CsvFile.Rows)
                            {
                            string ScriptName = Row["SYMBOL"].ToString();
                            //if (!PreviousScript.Equals(ScriptName))
                                {
                                //  Query = "SELECT DISTINCT Script_Code FROM Mst_Script WHERE NSE_CODE = '" + ScriptName + "'";
                                //  cmd = new SqlCommand(Query, con);
                                DataTable dt = new DataTable();
                                var Case = 2;
                                SqlParameterCollection pcol = new SqlCommand().Parameters;
                                Adapter.AddParam(pcol, "@Case", Case);
                                Adapter.AddParam(pcol, "@ScriptName", ScriptName);
                                dt = Adapter.ExecuteDataTable("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol));


                                DataTable ScriptTable = new DataTable();
                                //   ScriptTable.Load(Adapter.ExecuteReader("USPBhavCopyFNO"));
                                ScriptTable = dt;
                                string ExpireDate = "";
                                try
                                    {
                                    ExpireDate = DateTime.ParseExact(Row["EXPIRY_DT"].ToString(), "dd-MMM-yyyy", null)
                                    .ToString("MM/dd/yyyy");
                                    }
                                catch (Exception e)
                                    {
                                    ExpireDate = DateTime.Parse(Row["EXPIRY_DT"].ToString()).ToString("MM/dd/yyyy");
                                    }

                                string StrikePrice = Row["STRIKE_PR"].ToString();
                                StrikePrice = StrikePrice.Equals("0") ? "" : StrikePrice;

                                string OptionType = Row["OPTION_TYP"].ToString().Trim(); ;
                                switch (OptionType)
                                    {
                                    case "XX":
                                        OptionType = "";
                                        break;

                                    case "CE":
                                        OptionType = OptionType + " - CALL";
                                        break;

                                    case "PE":
                                        OptionType = OptionType + " - PUT";
                                        break;

                                    case "PA":
                                        OptionType = OptionType + " - PUT";
                                        break;

                                    case "CA":
                                        OptionType = OptionType + " - CALL";
                                        break;
                                    }

                                string Rate = Row["CLOSE"].ToString();

                                for (int iScript = 0; iScript < ScriptTable.Rows.Count; iScript++)
                                    {

                                    var Case1 = 3;
                                    SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                                    Adapter.AddParam(pcol1, "@Case", Case1);
                                    Adapter.AddParam(pcol1, "@TransDate", selectedDate);
                                    Adapter.AddParam(pcol1, "@ScriptCode", ScriptTable.Rows[iScript][0]);
                                    Adapter.AddParam(pcol1, "@ExpDate", ExpireDate);
                                    Adapter.AddParam(pcol1, "@StrikePrice", StrikePrice);
                                    Adapter.AddParam(pcol1, "@OptionType", OptionType);
                                    Adapter.AddParam(pcol1, "@InvestmentType", SelectedInvestmentTypeIndex);
                                    Adapter.AddParam(pcol1, "@NSEFORate", Rate);
                                    Adapter.AddParam(pcol1, "@ScriptName", ScriptName);
                                    Adapter.ExecutenNonQuery("USPBhavCopyFNO", CommandType.StoredProcedure, Adapter.param(pcol1));


                                    }
                                }
                            PreviousScript = ScriptName;
                            }
                        break;
                    }
                }
            catch (Exception ex)
                {
                //  MessageBox.Show(ex.StackTrace);
                }
            }


        public void SaveCurrencyData(UploadBhavCopy _Bhavcopy ,  string scrrp,  string sExpireDate,  string sStrikePrice,  string sOptionType,  string Closevalue,  string Script_Name, string sExchangeRate)
            {
            if (sExchangeRate == "CurrrencyBSEFURate")
                {
                var Case = 1;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyBSEFURate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrency", CommandType.StoredProcedure, Adapter.param(pcol1));
             }

           else if (sExchangeRate == "CurrrencyNSEFURate")
                {
                var Case = 2;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyNSEFURate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrency", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyBSEOPRate")
                {
                var Case = 3;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyBSEOPRate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrency", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyNSEOPRate")
                {
                var Case = 4;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyNSEOPRate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrency", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            }

        public void UpdateCurrencyData(UploadBhavCopy _Bhavcopy, string scrrp, string sExpireDate, string sStrikePrice, string sOptionType, string Closevalue, string Script_Name, string sExchangeRate)
            {
            if (sExchangeRate == "CurrrencyBSEFURate")
                {
                var Case = 1;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyBSEFURate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrencyUpdate", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyNSEFURate")
                {
                var Case = 2;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyNSEFURate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrencyUpdate", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyBSEOPRate")
                {
                var Case = 3;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyBSEOPRate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrencyUpdate", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyNSEOPRate")
                {
                var Case = 4;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", _Bhavcopy.TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", _Bhavcopy.InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyNSEOPRate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrencyUpdate", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            }

        public void SaveDonldCurrencyData(DateTime TransDate, int InvestmentType,  string scrrp, string sExpireDate, string sStrikePrice, string sOptionType, string Closevalue, string Script_Name, string sExchangeRate)
            {
            if (sExchangeRate == "CurrrencyBSEFURate")
                {
                var Case = 1;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyBSEFURate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrency", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyNSEFURate")
                {
                var Case = 2;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyNSEFURate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrency", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyBSEOPRate")
                {
                var Case = 3;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyBSEOPRate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrency", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyNSEOPRate")
                {
                var Case = 4;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyNSEOPRate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrency", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            }

        public void UpdateDownldCurrencyData(DateTime TransDate, int InvestmentType, string scrrp, string sExpireDate, string sStrikePrice, string sOptionType, string Closevalue, string Script_Name, string sExchangeRate)
            {
            if (sExchangeRate == "CurrrencyBSEFURate")
                {
                var Case = 1;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyBSEFURate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrencyUpdate", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyNSEFURate")
                {
                var Case = 2;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyNSEFURate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrencyUpdate", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyBSEOPRate")
                {
                var Case = 3;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyBSEOPRate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrencyUpdate", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            else if (sExchangeRate == "CurrrencyNSEOPRate")
                {
                var Case = 4;
                SqlParameterCollection pcol1 = new SqlCommand().Parameters;
                Adapter.AddParam(pcol1, "@Case", Case);
                Adapter.AddParam(pcol1, "@TransDate", TransDate);
                Adapter.AddParam(pcol1, "@ScriptCode", scrrp);
                Adapter.AddParam(pcol1, "@ExpDate", sExpireDate);
                Adapter.AddParam(pcol1, "@StrikePrice", sStrikePrice);
                Adapter.AddParam(pcol1, "@OptionType", sOptionType);
                Adapter.AddParam(pcol1, "@InvestmentType", InvestmentType);
                Adapter.AddParam(pcol1, "@CurrrencyNSEOPRate", Closevalue);
                Adapter.AddParam(pcol1, "@ScriptName", Script_Name);
                Adapter.ExecutenNonQuery("USPBhavCopyCurrencyUpdate", CommandType.StoredProcedure, Adapter.param(pcol1));
                }

            }
        }
    }
