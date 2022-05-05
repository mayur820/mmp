using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using BAL;
using IRecordweb.Models;

namespace DAL
    {
    public class DbContext : System.Data.Entity.DbContext
        {
        public DbContext() : base("IrecordwebConnection")
            {
            }
        public DbSet<Company> Company { get; set; }
        public DbSet<Subscriber> Subscriber { get; set; }
        public DbSet<Family> Family { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Consultant> Consultant { get; set; }
        public DbSet<Industry> Industry { get; set; }
        public DbSet<Investment> Investment { get; set; }
        public DbSet<Narration> Narration { get; set; }
        public DbSet<Demat> Demat { get; set; }
        public DbSet<FundFamily> FundFamily { get; set; }
        public DbSet<MutualFund> MutualFund { get; set; }
        public DbSet<MutualFundCategory> MutualFundCategory { get; set; }
        public DbSet<Script> Script { get; set; }
        public DbSet<ScriptUploadDownload> ScriptUploadDownload { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MType> MType { get; set; }
        public DbSet<MutualFundManualEntry> MutualFundManualEntry { get; set; }
        public DbSet<FinancialYearTrans> FinancialYearTrans { get; set; }
        public DbSet<FinancialYear> FinancialYear { get; set; }
        public DbSet<TradeFiles> TradeFiles { get; set; }
        public DbSet<BrokerBill> BrokerBill { get; set; }
        public DbSet<User> MemberPartial { get; set; }

        public DbSet<UploadBhavCopy> UploadBhavCopy { get; set; }
        }


    }
