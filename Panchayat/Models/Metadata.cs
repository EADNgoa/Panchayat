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
    public class AuditMetadata
    {
        [Required]
        public int AuditNo;

        [Required]
        public string ListOfAuditObjection;

        public string ActionsTaken;
    }

    public class IllegalMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfComp;

        [StringLength(100)]
        [Required]
        public string NameOfPr;

        [StringLength(250)]
        [Required]
        public string AddressOfPr;

        [Required]
        [StringLength(300)]
        public string NatOfCon;

        [Required]
        [StringLength(50)]
        public string OccasOfCons;

        [Required]
        [StringLength(350)]
        public string ActionTaken;

        [Required]
        [StringLength(350)]
        public string Remarks;

    }

    public class InwardMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfReciept;

        [Required]
        [StringLength(150)]
        public string FromWhereRec;

        [Required]
        [StringLength(30)]
        public string NoOfLett;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfLett;

        [Required]
        public int FileNo;


        [Required]
        [StringLength(150)]
        public string SubjectMatter;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ActionTaken;

        [Required]
        [StringLength(350)]
        public string Remark;

    }

    public class MeetingMetadata
    {
        [Required]
        [StringLength(350)]
        public string Subject;

        [Required]
        [StringLength(100)]
        public string ProposerName;

        [Required]
        public bool PropOrAmend;

        [Required]
        public int For;

        [Required]
        public int Against;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MeetingDate;

        [Required]
        public string Resolution;

        [Required]
        public string Remark;


    }

    public class OutwardMetadata
    {

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfDisp;

        [Required]
        [StringLength(250)]
        public string ToWhom;


        [Required]
        [StringLength(30)]
        public string FileReferenceNo;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FileReferenceDate;

        [Required]
        [StringLength(350)]
        public string SubjectMatter;

        [Range(0.0, Double.MaxValue)]
        public Decimal PostageDrawn;

        [Range(0.0, Double.MaxValue)]
        public Decimal PostageExtended;

        [Required]
        public string Remark;

    }


    public class InOutRegsRecptsMetadata
    {
        [DataType(DataType.Date)] 
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TDate;


        public string Qty;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal Value;
    }

    public class InOutRegsIssuesMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TDate;

   
        public int Qty;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal Value;

     
        [Range(0.0, Double.MaxValue)]
        public decimal Balance;

        [Required]
        [StringLength(350)]
        public string IssuedTo;

        [Required]
        [StringLength(350)]
        public string Remarks;

    }



    public class PropertyBookingMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Booked For date")]
        [Required]
        public DateTime HDate;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Booked For date")]
        [Required]
        public DateTime TDate;


        [StringLength(250)]
        [Required]
        public string NameOfApplicant;


        [StringLength(15)]
        [Required]
        public string PhoneNo;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal AdvAmount;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal SecurityDepositAmt;

        [StringLength(250)]
        [Required]
        public string ApplForSDRefund;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal FullPayAmount;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal PaymentOfLuxTax;


        [StringLength(250)]
        [Required]
        public string Remarks;

    }

    public class PropertyMetadata
    {
        [StringLength(100)]
        [Required]
        public string PropertyName;


        [StringLength(250)]
        [Required]
        public string Remarks;
    }

    public class BuildingMetadata
    {
        [Required]
        public int WardNo;

        [Required]
        public int House_No;

        [StringLength(100)]
        [Required]
        public string OwnerName;

        [StringLength(250)]
        [Required]
        public string NameOfConstructioin;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)] 
        [Required]
        public DateTime DateOfAppl;

        [Required]
        public int NoOfRes;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DateOfPermision;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal EstimatedCost;

        [Required]
        public int AmountPaid;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfCompletion;


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfOcccp;


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfAsses;

        [Range(0.0, Double.MaxValue)]
        public decimal HouseTax;

        [Required]
        [StringLength(250)]
        public string Remarks;


    }




}

