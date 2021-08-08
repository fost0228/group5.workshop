namespace TravelExpert.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Agent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agent()
        {
            Customers = new HashSet<Customer>();
        }

        public int AgentId { get; set; }

        [StringLength(20)]
        public string AgtFirstName { get; set; }

        [StringLength(5)]
        public string AgtMiddleInitial { get; set; }

        [StringLength(20)]
        public string AgtLastName { get; set; }

        [StringLength(20)]
        public string AgtBusPhone { get; set; }

        [StringLength(50)]
        public string AgtEmail { get; set; }

        [StringLength(20)]
        public string AgtPosition { get; set; }

        public int? AgencyId { get; set; }

        public virtual Agency Agency { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
