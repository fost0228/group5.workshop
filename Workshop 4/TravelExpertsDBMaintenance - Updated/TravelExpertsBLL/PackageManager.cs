using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelExpertsData.Models;

namespace TravelExpertsBLL
{
    public static class PackageManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        public static void Add(Packages package)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.Packages.Add(package);
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        public static void Modify(Packages package)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.Packages.Update(package);
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        public static void Delete(Packages package)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.Packages.Remove(package);
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Packages Find(string id)
        {
            using (TravelExpertsContext db = new TravelExpertsContext()) // Open database connection
            {
                // Assigning query result as the current selected product
                Packages selectedPackage = db.Packages.SingleOrDefault(p => p.PkgName == id);

                return selectedPackage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IList GetOrderedList()
        {
            using (TravelExpertsContext db = new TravelExpertsContext()) // open db connection
            {
                // Getting ordered list of packages from Packages table
                var packages = db.Packages.OrderBy(p => p.PackageId)
                    .Select(p => new
                    {
                        p.PkgName,
                        p.PkgStartDate,
                        p.PkgEndDate,
                        p.PkgDesc,
                        p.PkgBasePrice,
                        p.PkgAgencyCommission
                    })
                    .ToList();

                return packages;
            }
        }
    }
}
