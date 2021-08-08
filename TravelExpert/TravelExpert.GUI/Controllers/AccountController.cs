using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpert.BLL;

namespace TravelExpert.GUI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// login
        /// </summary>
        /// 
        [HttpPost]
        public ActionResult Login(LoginUser user)
        {
            return View();
        }
    }
}
