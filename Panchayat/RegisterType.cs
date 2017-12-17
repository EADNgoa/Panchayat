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
    
    public partial class RegisterType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegisterType()
        {
            this.Meetings = new HashSet<Meeting>();
            this.Outwards = new HashSet<Outward>();
            this.Nocs = new HashSet<Noc>();
            this.Inwards = new HashSet<Inward>();
            this.InOutRegsRecpts = new HashSet<InOutRegsRecpt>();
            this.InOutRegsIssues = new HashSet<InOutRegsIssue>();
            this.InvItems = new HashSet<InvItem>();
            this.IllegalConstructions = new HashSet<IllegalConstruction>();
            this.PovertyCertificates = new HashSet<PovertyCertificate>();
            this.CertificateRequirements = new HashSet<CertificateRequirement>();
            this.HouseTaxCerts = new HashSet<HouseTaxCert>();
            this.ConstLicenseCerts = new HashSet<ConstLicenseCert>();
            this.OccupationCertificates = new HashSet<OccupationCertificate>();
            this.NocCertifictes = new HashSet<NocCertificte>();
            this.CharacterCertificates = new HashSet<CharacterCertificate>();
            this.ResidenceCertificates = new HashSet<ResidenceCertificate>();
        }
    
        public int RegisterTypeID { get; set; }
        public string RegisterType1 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Meeting> Meetings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Outward> Outwards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Noc> Nocs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inward> Inwards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InOutRegsRecpt> InOutRegsRecpts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InOutRegsIssue> InOutRegsIssues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvItem> InvItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IllegalConstruction> IllegalConstructions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PovertyCertificate> PovertyCertificates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CertificateRequirement> CertificateRequirements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HouseTaxCert> HouseTaxCerts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConstLicenseCert> ConstLicenseCerts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OccupationCertificate> OccupationCertificates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NocCertificte> NocCertifictes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CharacterCertificate> CharacterCertificates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResidenceCertificate> ResidenceCertificates { get; set; }
    }
}
