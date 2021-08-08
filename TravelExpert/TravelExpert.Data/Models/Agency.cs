namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Agency
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agency()
        {
            Agents = new HashSet<Agent>();
        }

        public int AgencyId { get; set; }

        [StringLength(50)]
        public string AgncyAddress { get; set; }

        [StringLength(50)]
        public string AgncyCity { get; set; }

        [StringLength(50)]
        public string AgncyProv { get; set; }

        [StringLength(50)]
        public string AgncyPostal { get; set; }

        [StringLength(50)]
        public string AgncyCountry { get; set; }

        [StringLength(50)]
        public string AgncyPhone { get; set; }

        [StringLength(50)]
        public string AgncyFax { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agent> Agents { get; set; }
    }
}
