using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelExpert.Domain
{
    public class NewCustomer
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage="Please input first name")]
        public string CustFirstName { get; set; }
        [Required(ErrorMessage = "Please input last name")]
        public string CustLastName { get; set; }

        [Required(ErrorMessage = "Please input Home Phone")]
        public string CustHomePhone { get; set; }

        [Required(ErrorMessage = "Please input password")]
        public string CustPwd { get; set; }

        [Required(ErrorMessage = "Please confirm password")]
        public string NewCustPwd { get; set; }

        [Required(ErrorMessage = "Please input current address")]
        public string CustAddress { get; set; }

        [Required(ErrorMessage = "Please input city")]
        public string CustCity { get; set; }

        [Required(ErrorMessage = "Please input province")]
        public string CustProv { get; set; }

        [Required(ErrorMessage = "Please input Country")]
        public string CustCountry { get; set; }
        public string CustBusPhone { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Please input postal code")]
        public string CustPostal { get; set; }
    }
}
