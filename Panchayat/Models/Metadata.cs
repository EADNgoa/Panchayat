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

    public class WorkMetadata
    {
        [Display(Name = "Name Of Work")]
        [Required]
        [StringLength(300, MinimumLength = 3)]
        public String NameOfWork;

        [Display(Name = "Tech. Sanction No.")]
        [Required]
        [StringLength(300, MinimumLength = 3)]
        public String TechSanctionNo;

        [Display(Name = "Name of Contractor")]
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public String ContractorName;

        [Display(Name = "Percentage Accepted")]        
        [StringLength(50, MinimumLength = 3)]
        public String PercentageAccepted;

        [Display(Name = "Time Limit")]        
        [StringLength(300, MinimumLength = 3)]
        public String TimeLimit;

        [Display(Name = "M. B. No.")]        
        [StringLength(50, MinimumLength = 3)]
        public String MBNo;

        [Display(Name = "Estimated Cost")]
        [Required]
        [Range(0.0, Double.MaxValue)]
        public Decimal EstimatedCost;

        [Display(Name = "Tendered Cost")]        
        [Range(0.0, Double.MaxValue)]
        public Decimal TenderedCost;

        [Display(Name = "Final Value")]        
        [Range(0.0, Double.MaxValue)]
        public Decimal FinalValue;

        [Display(Name = "Net Payment To Contractor")]        
        [Range(0.0, Double.MaxValue)]
        public Decimal NetPaymentToContractor;

        [Display(Name = "Grants Recieved")]        
        [Range(0.0, Double.MaxValue)]
        public Decimal GrantsRecieved;

        [Display(Name = "Grants Utilized")]        
        [Range(0.0, Double.MaxValue)]
        public Decimal GrantsUtilized;

        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Commence Date")]
        public DateTime CommenceDate;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Completion Date")]
        public DateTime CompletionDate;
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

    public class NocertMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Receipt")]
        public DateTime DateOfReciept;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of VP Reg.")]
        public DateTime DateOfVp;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Communication")]
        public DateTime DateOfComm;
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

        [StringLength(128)]
        public string UserID;
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
                
        [Range(0.0, Double.MaxValue)]
        public decimal Value;

     
        [Range(0.0, Double.MaxValue)]
        public decimal Balance;

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
        public string WardNo;

        [Required]
        public string House_No;

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
        [StringLength(50)]

        public string NoOfRes;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime DateOfRes;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DateOfPermision;

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal EstimatedCost;

        [Required]
        [Range(0.0, Double.MaxValue)]

        public decimal AmountPaid;

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

public class MovementMetadata
    {
        [Required]
        [StringLength(150)]
        public string NameAndDes;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime TimeOfDeparture;

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime TimeOfReturn;

        [Required]
        [StringLength(250)]
        public string PlaceAndPurpose;

    }

    public class CashInHandMetadata
    {
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Tdate;

        [Required]
        [StringLength(250)]
        public string NameAndDesg;


        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal CashToDeclareStart;

        [Required]
        [StringLength(250)]
        public string DetailsOfCashExp;

    
        [Range(0.0, Double.MaxValue)]
        public decimal CashToDeclareEnd;

        [Required]
        [StringLength(300)]
        public string Remarks;

    }

    public class PovertyCertificateMetadata
    {
        [Required]
        [StringLength(100)]
        public string PersonName;

        [Required]
        [StringLength(100)]
        public string OtherName;

        [Required]
        [StringLength(250)]
        public string PersonAddress;

        [Required]
        [StringLength(100)]
        public string RequestedBy;

        [Required]
        [StringLength(250)]
        public string AddOfPerReqBy;
    }

    public class ResidenceCertificateMetadata
    {
        [Required]
        [StringLength(100)]
        public string PersonName;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate;
      
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TDate;

        [Required]
        [StringLength(250)]
        public string BirthPlace;


        [Required]
        [StringLength(100)]
        public string NameOfMother;


        [Required]
        [StringLength(100)]
        public string NameOfFather;


        [Required]
        [StringLength(250)]
        public string Address;



    }
    public class HouseTaxCertMetadata
    {
        [Required]
        [StringLength(100)]
        public string PersonName;

        [Required]
        [StringLength(250)]
        public string PersonAddress;

      
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Tdate;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MeetingDate;
        
        [StringLength(100)]
        public string PrevPersonName;

    
        public int Fees;

        [StringLength(100)]
        public string DeveloperName;

        
        [StringLength(100)]
        public string DeveloperAddress;




    }
    public class ConstructionMetadata
    {
        [Required]
        [StringLength(160)]
        public string OwnersOfHouse;

        [Required]
        [StringLength(250)]
        public string OwnwersAddress;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Tdate;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MeetingDated;

        [Required]
        [StringLength(100)]
        public string BuildingType_;

        [Required]
        [StringLength(100)]
        public string PropertyZone;

        [Required]
        [StringLength(100)]
        public string SurveyNo;

        [Required]
        [StringLength(100)]
        public string SubDivision;

        [Required]
        [StringLength(100)]
        public string OrderNo;

        [Required]
        [StringLength(100)]
        public string RefNo;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RefDate;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ValidUpTo;

        [StringLength(100)]
        public string RecieptNo;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RecieptDate;

        [Required]
        [StringLength(100)]
        public string DeveloperName;

        [Required]
        [StringLength(250)]
        public string DeveloperAddress;

        [Range(0.0, Double.MaxValue)]
        public Decimal ConstFees;

        [Range(0.0, Double.MaxValue)]
        public Decimal SanitationFees;




    }
    public class OccupationCertificateMetadata
    {
        [Required]
        [StringLength(100)]
        public string PersonName;

        [Required]
        [StringLength(250)]
        public string PersonAddress;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Tdate;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MeetingDated;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ConstLicDate;


        [Required]
        [StringLength(100)]
        public string ConstLicNo;

        [Required]
        public string BuildingDetails;

        [Required]
        [StringLength(100)]
        public string SurveyNo;

        [Required]
        [StringLength(100)]
        public string PlotNumber;

        [Required]
        [StringLength(100)]
        public string RefNo;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RefDate;

        [Required]
        [StringLength(250)]
        public string HSref;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HSrefdate;

    }

    public class IncomeCertificateMetadata
    {
        [Required]
        [StringLength(100)]
        public string PersonName;

        [Required]
        [StringLength(100)]
        public string RelationName;

        [Required]
        [StringLength(100)]
        public string Address;
        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal IncomeAmt;
        [Required]
        [StringLength(100)]
        public string YearOf;

        [Required]
        [StringLength(100)]
        public string OfficeName;

        [Required]
        [StringLength(150)]
        public string PurposeOf;

        [Required]
        [StringLength(100)]
        public string Inquiry;

        [Required]
        [StringLength(100)]
        public string ReportNo;

     
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InquiryDate;

        [Required]
        [StringLength(50)]
        public string Place;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PrintDate;

    }
    public class DeathCorrCertificateMetadata
    {
        [Required]
        [StringLength(100)]
        public string FromName;
        [Required]
        [StringLength(200)]
        public string FromAddress;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TDate;

        [Required]
        [StringLength(100)]
        public string BirthOf;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BornOn;

        [Required]
        [StringLength(100)]
        public string BirthPlace;

        [Required]
        [StringLength(100)]
        public string FromWrongName;

        [Required]
        [StringLength(100)]
        public string InsteadFWN;


        [Required]
        [StringLength(100)]
        public string NameOfFather;


        [Required]
        [StringLength(100)]
        public string InsteadNF;


        [Required]
        [StringLength(100)]
        public string NameOfMother;


        [Required]
        [StringLength(100)]
        public string InsteadNM;


        [Required]
        [StringLength(100)]
        public string NameOfGrandMother;


        [Required]
        [StringLength(100)]
        public string InsteadNGM;

        [Required]
        [StringLength(100)]
        public string NameOfGrandFather;


        [Required]
        [StringLength(100)]
        public string InsteadNGF;

        [Required]
        [StringLength(100)]
        public string BirthDeathName;



    }
}

