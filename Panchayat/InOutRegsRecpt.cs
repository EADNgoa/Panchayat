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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InOutRegsRecpt()
        {
            this.InOutRegsIssues = new HashSet<InOutRegsIssue>();
        }
    
        public int IORecptID { get; set; }
        public Nullable<int> RegisterTypeID { get; set; }
        public Nullable<System.DateTime> TDate { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> RVno { get; set; }
    
        public virtual InvItem InvItem { get; set; }
        public virtual RegisterType RegisterType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InOutRegsIssue> InOutRegsIssues { get; set; }
    }
}
