using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
{
    public class JvEntryModel
    {
        public int ID { get; set; }
        public string Trans_No { get; set; }
        public int Sr_No { get; set; }
        public string Trans_Dt { get; set; }
        public string Member_Code { get; set; }
        public string Year_Code { get; set; }
        public string Vouch_No { get; set; }
        // public string Chq_No { get; set; }
        public string Dr_Code { get; set; }
        public string Cr_Code { get; set; }
        //   public string Sub_Type1 { get; set; }
        //  public string Sub_Type2 { get; set; }
        public string Book_Type1 { get; set; }
        // public string Narr1 { get; set; }
        //  public string Narr2 { get; set; }
        //  public string Narr3 { get; set; }
        public string Internal { get; set; }
        public float Amount { get; set; }
        public int InvestmentType { get; set; }
        public int EntryType { get; set; }
        public int PayAgtBill { get; set; }
        //  public int Reco { get; set; }
        //  public string Reco_Date { get; set; }
        public int isImported { get; set; }
        public string ExpenseType { get; set; }
        public int IsMoneyBack { get; set; }
        public int IsInsurance { get; set; }
        public int IsDisplayInAc { get; set; }
        public string frmpg { get; set; }
        //  public int pay_mode { get; set; }
        //   public bool IsTallyTransfer { get; set; }
        public string Parent_Narr { get; set; }
        public string Counter_Narr { get; set; }
        public string Entry_Narr { get; set; }
        public string Parent_Ac_Code { get; set; }
        public string Parent_Ac_Type { get; set; }
        public Decimal Parent_Amt { get; set; }
    }
}