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
    
    public partial class Building
    {
        public int BuildingID { get; set; }
        public string WardNo { get; set; }
        public string House_No { get; set; }
        public string OwnerName { get; set; }
        public string NameOfConstructioin { get; set; }
        public Nullable<System.DateTime> DateOfAppl { get; set; }
        public string NoOfRes { get; set; }
        public Nullable<System.DateTime> DateOfRes { get; set; }
        public Nullable<System.DateTime> DateOfPermision { get; set; }
        public Nullable<decimal> EstimatedCost { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public Nullable<System.DateTime> DateOfCompletion { get; set; }
        public Nullable<System.DateTime> DateOfOcccp { get; set; }
        public Nullable<System.DateTime> DateOfAsses { get; set; }
        public Nullable<decimal> HouseTax { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> ReceiptNo { get; set; }
    
        public virtual Form4 Form4 { get; set; }
    }
}
