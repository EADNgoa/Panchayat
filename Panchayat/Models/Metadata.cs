using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Panchayat
{
    public class Form4Metadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pay Date")]        
        public DateTime PayDate;
    }

    public class VouchersMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pay Date")]        
        public DateTime PayDate;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Held On")]
        [Required]
        public DateTime HeldOn;
    }

    public class ConfigMetadata
    {

        [Display(Name = "Panchayat Name")]
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public String VP;

        [Display(Name = "Demand Increase Percentage")]
        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal DemandIncPerc;

        [Display(Name = "Arrears Percentage")]
        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal ArrearsPerc;

        [Display(Name = "Rows Per Page")]
        [Required]
        [Range(0, 100)]
        public int RowsPerPage;
    }

    public class CitizenMetatdata
    {
        [Display(Name = "Citizen ID")]
        [Required]
        public int CitizenID;

        [Display(Name = "Full Name")]
        [Required]
        [StringLength(150)]
        public int Name;

        [Display(Name = "Mobile No")]
        [Required]
        [StringLength(25)]
        public String Mobile;

        [Display(Name = "Email")]        
        [StringLength(30)]

        public String Email;

        [Display(Name = "Ressidential Address")]
        [Required]
        [StringLength(250)]
        public String ResAddress;



    }

    public class CitizenLedgerDetailsMetatdata
    {
        [Display(Name = "Citizen Ledger Details ID")]
        [Required]
        public int CitizenLedgerDetailsID;

        [Display(Name = "Demand ID ")]
        [Required]
        public int DemandID;

        [Display(Name = "LedgerDetails ID")]
        [Required]
        public int LedgerDetailID;

        [Display(Name = "Details")]
        [Required]
        [StringLength(50)]
        public int CitizenLedgerDetail1;



    }

    public class LedgersMetatdata
    {
        [Display(Name = "Ledger ID")]
        [Required]
        public int LedgerID;

        [Display(Name = "Ledger")]
        [Required]
        [StringLength(20)]
        public String Ledger1;

        [Display(Name = "Is Income(Yes or No)")]
        [Required]        
        public bool IsIncome;

    }

    public class SubLedgersMetatdata
    {
        [Display(Name = "SubLedger ID")]
        [Required]
        public int SubLedgerID;

        [Display(Name = "Ledgers")]
        [Required]
        public int LedgerID;

        [Display(Name = "Sub-Ledger Type(optional)")]
        public int SubLedgerTypeID;

        [Display(Name = "Sub Ledgers")]
        [Required]
        [StringLength(50)]
        public String Ledger1;

    }

    public class SubLedgerTypeMetatdata
    {
        [Display(Name = "Sub-Ledger Type ID")]
        [Required]
        public int SubLedgerTypeID;

        [Display(Name = "Sub-Ledger Type")]
        [Required]
        [StringLength(20)]
        public int SubLedgerType1;

    }

    public class LedgerDetailsMetatdata
    {
        [Display(Name = "Ledger Detail ID")]
        [Required]
        public int LedgerDetailID;

        [Display(Name = "Sub-Ledger")]
        [Required]
        public int SubLedgerID;

        [Display(Name = "Ledger Details")]
        [Required]
        [StringLength(30)]
        public int LedgerDetail1;
    }

  

}