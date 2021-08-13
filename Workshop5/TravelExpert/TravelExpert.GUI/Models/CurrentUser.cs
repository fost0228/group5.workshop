using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelExpert.GUI.Models
{
    public class CurrentUser
    {
        public int CustomerId { get; set; }
        public string CustFirstName { get; set; }
        public string CustLastName { get; set; }
        public string CustHomePhone { get; set; }
        public string CustPwd { get; set; }

        public string CustAddress { get; set; }

        public string CustCity { get; set; }
        public string CustProv { get; set; }
        public string CustCountry { get; set; }
        public string CustBusPhone { get; set; }

        public string Email { get; set; }
        public string CustPostal { get; set; }

        public int AgentId { get; set; }
        public DateTime LoginTime { get; set; }

    }
}
