using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class CN_Post_Model
        {
        public class Rootobject
            {
            public Header[] Header { get; set; }
            public Detail[] Details { get; set; }
            public Expense[] Expense { get; set; }
            public Runningbalance[] RunningBalance { get; set; }
            }

        public class Header
            {
            public string Date { get; set; }
            public string BillNo { get; set; }
            public string SettlementNo { get; set; }
            }

        public class Detail
            {
            public string ConsultantCode { get; set; }
            public string Consultant { get; set; }
            public string HoldingType { get; set; }
            public object HoldingTypeId { get; set; }

            public object Demat { get; set; }
            public object DematCode1 { get; set; }
            public object StrikePrice { get; set; }
            public object OptionType { get; set; }
            public object ExpiryDate { get; set; }
            public object BFCF { get; set; }
            public object StockType { get; set; }
            public string Type { get; set; }
            public string Qty { get; set; }
            public string GrossRate { get; set; }
            public string GrossAmt { get; set; }
            public string brkpunit { get; set; }
            public string BrokAmt { get; set; }
            public string Rate { get; set; }
            public string Amount { get; set; }
            public object Lott { get; set; }
            public object LotQty { get; set; }
            public string ScriptName { get; set; }
            public string ScriptCode { get; set; }
            public string Script_color { get; set; }
            public bool IntraDay { get; set; }



            }

        public class Expense
            {
            public object Expense_Name { get; set; }
            public object Exp_Amount { get; set; }
            public string extype { get; set; }
            public string expnm { get; set; }
            public string expcd { get; set; }
            public string ExpAmount { get; set; }
            }

        public class Runningbalance
            {
            public string txtBrokerage { get; set; }
            public string textBox5 { get; set; }
            public string txtInvestment { get; set; }
            public string txtGrossNetAmt { get; set; }
            public string txtNetAmt { get; set; }
            public string txtExpenses { get; set; }
            public string label3 { get; set; }
            public string label4 { get; set; }
            public string lblAmountReceivable { get; set; }
            }

        }
    }