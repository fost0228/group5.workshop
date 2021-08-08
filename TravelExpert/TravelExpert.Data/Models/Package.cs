namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Package()
        {
            Bookings = new HashSet<Booking>();
            Products_Suppliers = new HashSet<Products_Suppliers>();
        }

        public int PackageId { get; set; }

        [Required]
        [StringLength(50)]
        public string PkgName { get; set; }

        public DateTime? PkgStartDate { get; set; }

        public DateTime? PkgEndDate { get; set; }

        [StringLength(50)]
        public string PkgDesc { get; set; }

        [Column(TypeName = "money")]
        public decimal PkgBasePrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? PkgAgencyCommission { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products_Suppliers> Products_Suppliers { get; set; }
    }
}
