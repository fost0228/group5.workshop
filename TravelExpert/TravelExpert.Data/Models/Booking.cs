namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public int BookingId { get; set; }

        public DateTime? BookingDate { get; set; }

        [StringLength(50)]
        public string BookingNo { get; set; }

        public double? TravelerCount { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(1)]
        public string TripTypeId { get; set; }

        public int? PackageId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Package Package { get; set; }

        public virtual TripType TripType { get; set; }
    }
}
