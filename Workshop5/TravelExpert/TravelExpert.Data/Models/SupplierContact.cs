namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SupplierContact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SupplierContactId { get; set; }

        [StringLength(50)]
        public string SupConFirstName { get; set; }

        [StringLength(50)]
        public string SupConLastName { get; set; }

        [StringLength(255)]
        public string SupConCompany { get; set; }

        [StringLength(255)]
        public string SupConAddress { get; set; }

        [StringLength(255)]
        public string SupConCity { get; set; }

        [StringLength(255)]
        public string SupConProv { get; set; }

        [StringLength(255)]
        public string SupConPostal { get; set; }

        [StringLength(255)]
        public string SupConCountry { get; set; }

        [StringLength(50)]
        public string SupConBusPhone { get; set; }

        [StringLength(50)]
        public string SupConFax { get; set; }

        [StringLength(255)]
        public string SupConEmail { get; set; }

        [StringLength(255)]
        public string SupConURL { get; set; }

        [StringLength(10)]
        public string AffiliationId { get; set; }

        public int? SupplierId { get; set; }

        public virtual Affiliation Affiliation { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
