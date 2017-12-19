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
    
    public partial class WEBstatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WEBstatu()
        {
            this.IllegalConstructions = new HashSet<IllegalConstruction>();
            this.PovertyCertificates = new HashSet<PovertyCertificate>();
            this.HouseTaxCerts = new HashSet<HouseTaxCert>();
            this.ConstLicenseCerts = new HashSet<ConstLicenseCert>();
            this.OccupationCertificates = new HashSet<OccupationCertificate>();
            this.NocCertifictes = new HashSet<NocCertificte>();
            this.ResidenceCertificates = new HashSet<ResidenceCertificate>();
            this.CharacterCertificates = new HashSet<CharacterCertificate>();
        }
    
        public int WebStatusID { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IllegalConstruction> IllegalConstructions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PovertyCertificate> PovertyCertificates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HouseTaxCert> HouseTaxCerts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConstLicenseCert> ConstLicenseCerts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OccupationCertificate> OccupationCertificates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NocCertificte> NocCertifictes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResidenceCertificate> ResidenceCertificates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CharacterCertificate> CharacterCertificates { get; set; }
    }
}
