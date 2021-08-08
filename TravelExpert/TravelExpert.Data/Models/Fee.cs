namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fee()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        [StringLength(10)]
        public string FeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string FeeName { get; set; }

        [Column(TypeName = "money")]
        public decimal FeeAmt { get; set; }

        [StringLength(50)]
        public string FeeDesc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
