using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelExpert.BLL;
using TravelExpert.Data;
using TravelExpert.Domain;
using TravelExpert.GUI.Models;

namespace TravelExpert.GUI.Controllers
{
    
    public class AccountController : Controller
    {
        //private ISysUserservice _ISysUserservice = null;
        //public AccountController(ISysUserservice iSysUserservice)
        //{
        //    _ISysUserservice = iSysUserservice;
        //}


        /// <summary>
        /// get a  login page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult Login(string returnUrl = null)
        {
            if (returnUrl != null)
                TempData["ReturnUrl"] = returnUrl;
            return View();
        }


        /// <summary>
        /// submit login info
        /// </summary>
        /// <param name="currentuser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoginAsync(CurrentUser currentuser)
        {
            //check customerID and password
            Customer cust = UserManager.Authenticate(currentuser.CustomerId, currentuser.CustPwd);

            if (cust != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                new Claim(ClaimTypes.Name, cust.CustFirstName),


                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");//authentication type
                ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);


                await HttpContext.SignInAsync("Cookies", principal);

                if (TempData["ReturnUrl"] != null)
                    return Redirect(TempData["ReturnUrl"].ToString());
                else
                    return RedirectToAction("Index", "Home");

            }
            else
            {
                return View();
            }

        }

        /// <summary>
        /// register page
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }


        /// <summary>
        /// submit register info
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        //public ActionResult Register(NewCustomer register)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(register);
        //    }
        //    if (!string.Equals(register.CustPwd, register.NewCustPwd, StringComparison.Ordinal))
        //    {
        //        ModelState.AddModelError("NewCustPwd", "Two entry is not equal");
        //        return View(register);
        //    }
        //    else {
        //        UserManager.Register(register);
        //        return RedirectToAction("Index", "Home");

        //    }



        //}


        public async Task<IActionResult> LogoutAsync() 
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index","Home");
        }
    }
}
