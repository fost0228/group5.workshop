namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Affiliation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Affiliation()
        {
            SupplierContacts = new HashSet<SupplierContact>();
        }

        [Key]
        [StringLength(10)]
        public string AffilitationId { get; set; }

        [StringLength(50)]
        public string AffName { get; set; }

        [StringLength(50)]
        public string AffDesc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierContact> SupplierContacts { get; set; }
    }
}
