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
    
    public partial class IncomeCertificate
    {
        public int IncomeCertificateID { get; set; }
        public string PersonName { get; set; }
        public string RelationName { get; set; }
        public string Address { get; set; }
        public Nullable<decimal> IncomeAmt { get; set; }
        public string YearOf { get; set; }
        public string OfficeName { get; set; }
        public string PurposeOf { get; set; }
        public string Inquiry { get; set; }
        public string ReportNo { get; set; }
        public Nullable<System.DateTime> InquiryDate { get; set; }
        public string Place { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public string UserID { get; set; }
        public Nullable<int> RegisterTypeID { get; set; }
        public Nullable<int> WEBstatusID { get; set; }
    
        public virtual RegisterType RegisterType { get; set; }
        public virtual WEBstatu WEBstatu { get; set; }
    }
}
