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
    
    public partial class DemandDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DemandDetail()
        {
            this.DemandLedgerDetails = new HashSet<DemandLedgerDetail>();
            this.DemandPeriods = new HashSet<DemandPeriod>();
            this.DemandYears = new HashSet<DemandYear>();
        }
    
        public int DDID { get; set; }
        public int DemandID { get; set; }
        public int SubLedgerID { get; set; }
    
        public virtual Demand Demand { get; set; }
        public virtual SubLedger SubLedger { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemandLedgerDetail> DemandLedgerDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemandPeriod> DemandPeriods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemandYear> DemandYears { get; set; }
    }
}
