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
    
    public partial class Form3
    {
        public int CashBookID { get; set; }
        public Nullable<System.DateTime> PayDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> LedgerID { get; set; }
        public Nullable<int> SubLedgerID { get; set; }
        public Nullable<int> TransNo { get; set; }
        public string TransPerson { get; set; }
    
        public virtual Ledger Ledger { get; set; }
        public virtual SubLedger SubLedger { get; set; }
    }
}
