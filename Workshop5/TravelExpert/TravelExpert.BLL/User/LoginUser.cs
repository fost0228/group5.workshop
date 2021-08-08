using System;
using System.ComponentModel.DataAnnotations;

namespace TravelExpert.BLL
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Please input userID")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please input Password")]
        public String Password { get; set; }
    }
}
