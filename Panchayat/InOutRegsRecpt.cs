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
    
    public partial class InOutRegsRecpt
    {
        public int IORecptID { get; set; }
        public Nullable<int> RegisterTypeID { get; set; }
        public Nullable<System.DateTime> TDate { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Value { get; set; }
    
        public virtual InvItem InvItem { get; set; }
        public virtual RegisterType RegisterType { get; set; }
    }
}
