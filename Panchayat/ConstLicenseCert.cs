//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ConstLicenseCert
    {
        public int ConstLicenseID { get; set; }
        public string OwnersOfHouse { get; set; }
        public string OwnwersAddress { get; set; }
        public Nullable<System.DateTime> MeetingDated { get; set; }
        public string BuildingType_ { get; set; }
        public string PropertyZone { get; set; }
        public string SurveyNo { get; set; }
        public string SubDivision { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> Tdate { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public Nullable<System.DateTime> ValidUpTo { get; set; }
        public string RecieptNo { get; set; }
        public Nullable<System.DateTime> RecieptDate { get; set; }
        public string DeveloperName { get; set; }
        public string DeveloperAddress { get; set; }
        public Nullable<decimal> ConstFees { get; set; }
        public Nullable<decimal> SanitationFees { get; set; }
        public Nullable<int> RegisterTypeID { get; set; }
        public Nullable<int> WEBstatusID { get; set; }
        public string UserID { get; set; }
    
        public virtual RegisterType RegisterType { get; set; }
        public virtual WEBstatu WEBstatu { get; set; }
    }
}
