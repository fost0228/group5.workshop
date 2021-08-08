namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CreditCard
    {
        public int CreditCardId { get; set; }

        [Required]
        [StringLength(10)]
        public string CCName { get; set; }

        [Required]
        [StringLength(50)]
        public string CCNumber { get; set; }

        public DateTime CCExpiry { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
