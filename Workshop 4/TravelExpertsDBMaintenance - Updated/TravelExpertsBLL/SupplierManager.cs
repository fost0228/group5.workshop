using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TravelExpertsData.Models;

namespace TravelExpertsBLL
{
    public static class SupplierManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        public static void Add(Suppliers supplier)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        public static void Delete(Suppliers supplier)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.Suppliers.Remove(supplier);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Suppliers Find(int id)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                Suppliers sup = db.Suppliers.Find(id);
                return sup;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<int> GetListOfIDs()
        {
            List<int> ids = new List<int>();

            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                // Find all supplier IDs
                var supplierIds = db.Suppliers.Select(c => new { ID = c.SupplierId }).ToList();

                foreach (var s in supplierIds)
                {
                    ids.Add(s.ID);
                }

                return ids;

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
                // Getting ordered list of suppliers from Suppliers table
                var suppliers = db.Suppliers.OrderBy(s => s.SupplierId)
                    .Select(s => new { s.SupplierId, s.SupName })
                    .ToList();

                return suppliers;
            }
        }
    }
}
