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
    
    public partial class Outward
    {
        public int OutwardID { get; set; }
        public Nullable<System.DateTime> DateOfDisp { get; set; }
        public string ToWhom { get; set; }
        public string FileReferenceNo { get; set; }
        public Nullable<System.DateTime> FileReferenceDate { get; set; }
        public string SubjectMatter { get; set; }
        public Nullable<decimal> PostageDrawn { get; set; }
        public Nullable<decimal> PostageExtended { get; set; }
        public string Remark { get; set; }
        public int RegisterTypeID { get; set; }
    
        public virtual RegisterType RegisterType { get; set; }
    }
}
