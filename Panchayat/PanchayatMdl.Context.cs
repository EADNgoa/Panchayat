﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Panchayat
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PanchayatEntities : DbContext
    {
        public PanchayatEntities()
            : base("name=PanchayatEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Citizen> Citizens { get; set; }
        public virtual DbSet<Correction> Corrections { get; set; }
        public virtual DbSet<Form1> Form1 { get; set; }
        public virtual DbSet<Form2> Form2 { get; set; }
        public virtual DbSet<Form3> Form3 { get; set; }
        public virtual DbSet<Form4> Form4 { get; set; }
        public virtual DbSet<LedgerDetail> LedgerDetails { get; set; }
        public virtual DbSet<Ledger> Ledgers { get; set; }
        public virtual DbSet<SubLedger> SubLedgers { get; set; }
        public virtual DbSet<SubLedgerType> SubLedgerTypes { get; set; }
        public virtual DbSet<RVdetail> RVdetails { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<DemandDetail> DemandDetails { get; set; }
        public virtual DbSet<DemandPeriod> DemandPeriods { get; set; }
        public virtual DbSet<DemandYear> DemandYears { get; set; }
        public virtual DbSet<PeriodSL> PeriodSLs { get; set; }
        public virtual DbSet<DemandLedgerDetail> DemandLedgerDetails { get; set; }
        public virtual DbSet<Demand> Demands { get; set; }
        public virtual DbSet<CBRunning> CBRunnings { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<RegisterType> RegisterTypes { get; set; }
        public virtual DbSet<Meeting> Meetings { get; set; }
        public virtual DbSet<Outward> Outwards { get; set; }
        public virtual DbSet<Noc> Nocs { get; set; }
        public virtual DbSet<Inward> Inwards { get; set; }
        public virtual DbSet<InvItem> InvItems { get; set; }
        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyBooking> PropertyBookings { get; set; }
        public virtual DbSet<InOutRegsRecpt> InOutRegsRecpts { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InOutRegsIssue> InOutRegsIssues { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<VPRent> VPRents { get; set; }
        public virtual DbSet<VPRentDetail> VPRentDetails { get; set; }
        public virtual DbSet<WEBstatu> WEBstatus { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<IllegalConstruction> IllegalConstructions { get; set; }
        public virtual DbSet<CashInHandReg> CashInHandRegs { get; set; }
        public virtual DbSet<MovementReg> MovementRegs { get; set; }
        public virtual DbSet<CertificateRequirement> CertificateRequirements { get; set; }
        public virtual DbSet<CertSupportDoc> CertSupportDocs { get; set; }
        public virtual DbSet<PovertyCertificate> PovertyCertificates { get; set; }
        public virtual DbSet<ResidenceCertificate> ResidenceCertificates { get; set; }
        public virtual DbSet<HouseTaxCert> HouseTaxCerts { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
    }
}
