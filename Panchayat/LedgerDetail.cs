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
    
    public partial class LedgerDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LedgerDetail()
        {
            this.DemandLedgerDetails = new HashSet<DemandLedgerDetail>();
            this.RVdetails = new HashSet<RVdetail>();
        }
    
        public int LedgerDetailID { get; set; }
        public Nullable<int> SubLedgerID { get; set; }
        public string LedgerDetail1 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemandLedgerDetail> DemandLedgerDetails { get; set; }
        public virtual SubLedger SubLedger { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RVdetail> RVdetails { get; set; }
    }
}
