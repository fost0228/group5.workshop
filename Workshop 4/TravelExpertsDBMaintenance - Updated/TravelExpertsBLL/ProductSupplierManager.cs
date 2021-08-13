using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravelExpertsData.Models;

namespace TravelExpertsBLL
{
    public static class ProductSupplierManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productSupplier"></param>
        public static void Add(ProductsSuppliers productSupplier)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.ProductsSuppliers.Add(productSupplier);
                db.SaveChanges();
            }
        }


        public static List<ProductsSuppliers> GetListBySupplierID(int id)
        {
            using (TravelExpertsContext db = new TravelExpertsContext()) // Opens database connection
            {              
                List<ProductsSuppliers> prodSuppliers = db.ProductsSuppliers.Where(ps => ps.SupplierId == id)
                    .ToList();

                return prodSuppliers;

            } // close
        }
    }
}
