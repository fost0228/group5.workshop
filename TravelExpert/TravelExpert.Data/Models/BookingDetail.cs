namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookingDetail
    {
        public int BookingDetailId { get; set; }

        public double? ItineraryNo { get; set; }

        public DateTime? TripStart { get; set; }

        public DateTime? TripEnd { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Destination { get; set; }

        [Column(TypeName = "money")]
        public decimal? BasePrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? AgencyCommission { get; set; }

        public int? BookingId { get; set; }

        [StringLength(5)]
        public string RegionId { get; set; }

        [StringLength(5)]
        public string ClassId { get; set; }

        [StringLength(10)]
        public string FeeId { get; set; }

        public int? ProductSupplierId { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual Class Class { get; set; }

        public virtual Fee Fee { get; set; }

        public virtual Products_Suppliers Products_Suppliers { get; set; }

        public virtual Region Region { get; set; }
    }
}
