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
    
    public partial class NocCertificte
    {
        public int NocID { get; set; }
        public string Hno { get; set; }
        public string No { get; set; }
        public Nullable<System.DateTime> AprovedDate { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public string Address { get; set; }
        public string PersonName { get; set; }
        public string ElectDeptAdd { get; set; }
        public string UserID { get; set; }
        public Nullable<int> WEBstatusID { get; set; }
        public Nullable<int> RegisterTypeID { get; set; }
    
        public virtual RegisterType RegisterType { get; set; }
        public virtual WEBstatu WEBstatu { get; set; }
    }
}
