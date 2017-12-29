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
    
    public partial class OccupationCertificate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OccupationCertificate()
        {
            this.OccupationCertDetails = new HashSet<OccupationCertDetail>();
        }
    
        public int OccupationCertificateID { get; set; }
        public string PersonName { get; set; }
        public string PersonAddress { get; set; }
        public Nullable<System.DateTime> MeetingDated { get; set; }
        public string ConstLicNo { get; set; }
        public string BuildingDetails { get; set; }
        public Nullable<System.DateTime> ConstLicDate { get; set; }
        public Nullable<System.DateTime> Tdate { get; set; }
        public string SurveyNo { get; set; }
        public string PlotNumber { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> RefDate { get; set; }
        public string HSref { get; set; }
        public Nullable<System.DateTime> HSrefdate { get; set; }
        public Nullable<int> RegisterTypeID { get; set; }
        public Nullable<int> WEBstatusID { get; set; }
        public string UserID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OccupationCertDetail> OccupationCertDetails { get; set; }
        public virtual RegisterType RegisterType { get; set; }
        public virtual WEBstatu WEBstatu { get; set; }
    }
}
