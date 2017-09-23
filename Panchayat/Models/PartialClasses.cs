using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc; for dynamic roles
//using Microsoft.AspNet.Identity;for dynamic roles
//using Panchayat.Models;for dynamic roles
//using Microsoft.AspNet.Identity.EntityFramework;for dynamic roles

namespace Panchayat
{

    [MetadataType(typeof(ConfigMetadata))]
    public partial class Config
    {        
    }

    [MetadataType(typeof(CitizenMetatdata))]
    public partial class Citizen
    {
    }

    [MetadataType(typeof(CitizenLedgerDetailsMetatdata))]
    public partial class CitizenLedgerDetail
    {
    }

    [MetadataType(typeof(LedgersMetatdata))]
    public partial class Ledgers
    {
    }

    [MetadataType(typeof(SubLedgersMetatdata))]
    public partial class SubLedgers
    {
    }

    [MetadataType(typeof(SubLedgerTypeMetatdata))]
    public partial class SubLedgerType
    {
    }

    [MetadataType(typeof(LedgerDetailsMetatdata))]
    public partial class LedgerDetail
    {
    }

    [MetadataType(typeof(Form4Metadata))]
    public partial class Form4
    {
    }

    [MetadataType(typeof(VouchersMetadata))]
    public partial class Voucher
    {
    }

    [MetadataType(typeof(AuditMetadata))]
    public partial class Audit
    {
    }

    [MetadataType(typeof(IllegalMetadata))]
    public partial class IllegalConstruction
    {
    }

    [MetadataType(typeof(InwardMetadata))]
    public partial class Inward
    {
    }

    [MetadataType(typeof(MeetingMetadata))]
    public partial class Meeting
    {

    }


    [MetadataType(typeof(OutwardMetadata))]
    public partial class Outward
    {
    }

    [MetadataType(typeof(InOutRegsRecptsMetadata))]
    public partial class InOutRegsRecpt
    {
    }

    [MetadataType(typeof(InOutRegsIssuesMetadata))]
    public partial class InOutRegsIssue
    {
    }

    [MetadataType(typeof(PropertyBookingMetadata))]
    public partial class PropertyBooking
    {
    }
    [MetadataType(typeof(PropertyMetadata))]
    public partial class Property
    {
    }

    public class Form10rpt
    {
        public string Ledger { get; set; }
        public string SubLedger { get; set; }
        public bool isIncome { get; set; }
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Used when Entering a new demand. Needed because we are entering data into 2 tables
    /// </summary>
    public class DemandInitLoad
    {
        public int DemandID { get; set; }
        public Nullable<int> CitizenID { get; set; }
        public Nullable<int> PeriodID { get; set; }
        public string HouseNo { get; set; }
        public Nullable<int> SubLedgerID { get; set; }        
        public Nullable<decimal> Amt { get; set; }
        public Nullable<DateTime> StopDate { get; set; }
        public string Remarks { get; set; }
    }

    public class DemandInitEdit
    {
        public int DemandID { get; set; }
        public int DDID { get; set; }
        public int DemandPeriodID { get; set; }
        public string HouseNo { get; set; }        
        public Nullable<decimal> Amt { get; set; }
        public Nullable<int> StopDate { get; set; }
        public string Remarks { get; set; }
    }

    /// <summary>
    /// The below 2 are used to populate Form7 report
    /// </summary>
    public class Form7Rpt
    {
        public int form7ID { get; set; }
        public int CitID { get; set; }
        public string CitName { get; set; }
        public string House { get; set; }
        public List<TaxDmd> TaxDmd { get; set; }
        public string Remarks { get; set; }
    }

    public class TaxDmd
    {
        public int SLid { get; set; }
        public int DDID { get; set; }
        public string SLname { get; set; }
        public decimal DmdAmt { get; set; } = 0;       
        public Dictionary<int, TaxActuals> TaxActuals { get; set; }
    }

    public class TaxActuals
    {
        public int f8yr { get; set; }
        public decimal Arrears { get; set; } = 0;
        public int ReceiptNo { get; set; } 
        public Nullable<DateTime> PayDate { get; set; } 
        public decimal PaidAmt { get; set; } = 0;
    }

    public class GenericReport
    {
        public int SLid { get; set; }
        public int RVid { get; set; } //Receipt of Voucher ID
        public decimal Amount { get; set; }
        public DateTime Tdate { get; set; } //Transaction date
    }

    public class RVReport
    {
        public int RVid { get; set; } //Receipt of Voucher ID
        public decimal Amount { get; set; }
        public DateTime Tdate { get; set; } //Transaction date
        public Dictionary<string, string> DetailData { get; set; }


    }

    enum SubLedgerNamer : byte
    {
        None=0,
        HouseTax=2
    }

    enum LedgerDetailNamer : byte
    {
        None=0,
        HouseNo=1,
        SignBoardText=2,
        SignBoardSize=3
    }

}