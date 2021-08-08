namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Bookings = new HashSet<Booking>();
            CreditCards = new HashSet<CreditCard>();
            Customers_Rewards = new HashSet<Customers_Rewards>();
        }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(25)]
        public string CustFirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string CustLastName { get; set; }

        [Required]
        [StringLength(75)]
        public string CustAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string CustCity { get; set; }

        [Required]
        [StringLength(2)]
        public string CustProv { get; set; }

        [Required]
        [StringLength(7)]
        public string CustPostal { get; set; }

        [StringLength(25)]
        public string CustCountry { get; set; }

        [StringLength(20)]
        public string CustHomePhone { get; set; }

        [Required]
        [StringLength(20)]
        public string CustBusPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string CustEmail { get; set; }

        public int? AgentId { get; set; }

        [StringLength(20)]
        public string CustPwd { get; set; }

        public virtual Agent Agent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditCard> CreditCards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customers_Rewards> Customers_Rewards { get; set; }
    }
}
