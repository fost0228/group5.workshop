using System;
using System.Collections.Generic;
using System.Linq;
using TravelExpert.Data;
using TravelExpert.Domain;

namespace TravelExpert.BLL
{
    public class UserManager
    {
        private static List<Customer> _account;

        public static Customer Authenticate(string UserName, string CustPwd)
        {
            TravelExpertsContext db = new TravelExpertsContext();
            _account = db.Customers.ToList();
            var account = _account.SingleOrDefault(usr => usr.UserName == UserName &&
                                            usr.CustPwd == CustPwd);
            return account;
        }

        //public static void Register(NewCustomer customer)
        //{
        //    TravelExpertsContext db = new TravelExpertsContext();
        //    db.Customers.Add(customer);
        //    db.SaveChanges();
        //}
        public static bool UserExist(String UserName)
        {
            TravelExpertsContext db = new TravelExpertsContext();

            return db.Customers.SingleOrDefault(usr => usr.UserName == UserName) != null;
        }
    }
}
