using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelExpertsData.Models;

namespace TravelExpertsBLL
{
    public static class SupplierContactManager
    {
        public static void Add(SupplierContacts supplierContact)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.SupplierContacts.Add(supplierContact);
                db.SaveChanges();
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
                var supplierContactIds = db.SupplierContacts.Select(sc =>
                                                    new { ID = sc.SupplierContactId }).ToList();

                foreach (var sc in supplierContactIds)
                {
                    ids.Add(sc.ID);
                }

                return ids;

            }
        }
    }
}
