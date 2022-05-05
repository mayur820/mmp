using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
    {
   public class MutualFundManualEntry
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrokerBillID { get; set; }
        public Int64 TransactionId { get; set; }
        [Required(ErrorMessage = "Please Enter Folio No")]
        [Display(Name = "Folio No")]
        public string FolioNo { get; set; }
        [Display(Name = "Date")]
        public DateTime EntryDate { get; set; }
        [Display(Name = "Consultant")]
        public string Consultant { get; set; }
        public string ValanCode { get; set; }
        [Display(Name = "Fund Family")]
        public string FundFamily { get; set; }
        public string Scheme { get; set; }
        [Display(Name = "Demat A/C")]
        public string DematAC { get; set; }
        public string Type { get; set; }
        public int Unit { get; set; }
        [Display(Name = "Price/Nav")]
        public int PriceNav { get; set; }
        public int Rate { get; set; }
        public int GRate { get; set; }
        public int Amount { get; set; }
        [Display(Name = "Nav After Load")]
        public int NavAfterLoad { get; set; }
        [Display(Name = "Load Amount")]
        public int LoadAmount { get; set; }
        [Display(Name = "Entry Load In %")]
        public int EntryLoadIn { get; set; }
        [Display(Name = "S.T.T")]
        public int STT { get; set; }
        [Display(Name = "Final Amount")]
        public int FinalAmount { get; set; }
       
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }
        [Display(Name = "Date")]
        public DateTime PaymentDate { get; set; }
        [Display(Name = "Cheque No")]
        public int ChequeNo { get; set; }
        [Display(Name = "Reference No")]
        public int ReferenceNo { get; set; }
        [Display(Name = "Cash/Bank")]
        public string Bank { get; set; }

        public string VoucherNo { get; set; }
        public int DebitCode { get; set; }
        public int CreditCode { get; set; }
        public string BorkerType { get; set; }
        public string Narration1 { get; set; }
        public string Narration2 { get; set; }
        public string Narration3 { get; set; }
        public int InvestmentTypeId { get; set; }
        public int Reco { get; set; }
        public DateTime RecoDate { get; set; }
        public float StrikePrice { get; set; }
        public string OptionType { get; set; }
        public int IsIntraDay { get; set; }
        public float BrokerRate { get; set; }
        public float BrokerAmount { get; set; }
        public float ExpRate { get; set; }
        public int IsFNOBill { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        }
    }
