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
    
    public partial class Noc
    {
        public int NocID { get; set; }
        public Nullable<System.DateTime> DateOfReciept { get; set; }
        public string NameAndAdressAppl { get; set; }
        public string NatOfNoc { get; set; }
        public Nullable<System.DateTime> DateOfVp { get; set; }
        public Nullable<int> NoOfResolution { get; set; }
        public Nullable<bool> IssueOrReject { get; set; }
        public string RejectedReason { get; set; }
        public Nullable<System.DateTime> DateOfComm { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> RegisterTypeID { get; set; }
    
        public virtual RegisterType RegisterType { get; set; }
    }
}
