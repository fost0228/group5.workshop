using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TravelExpertsData.Models;

namespace TravelExpertsBLL
{
    public static class ProductManager
    {
        public static List<Products> GetAll()
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                List<Products> products = db.Products.ToList();
                return products;
            }
        }

        public static void Add(Products product)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
        }

        public static void Modify(Products product)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.Products.Update(product);
                db.SaveChanges();
            }
        }


        public static void Delete(Products product)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
        }

        public static Products Find(int id)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                Products prod = db.Products.Find(id);
                return prod;
            }
        }

        /// <summary>
        /// Gets Id of a product based on the provided product name
        /// </summary>
        /// <param name="prodName">Product name</param>
        /// <returns>ProductId</returns>
        public static int GetProductId(string prodName)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                Products prod = db.Products.SingleOrDefault(p => p.ProdName == prodName);

                return prod.ProductId;
            }
        }


        /// <summary>
        /// Gets the string name of product based on provided Id
        /// </summary>
        /// <param name="id">ProductID</param>
        /// <returns>Product name</returns>
        public static string GetProductName(int id)
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                Products prod = db.Products.Find(id);

                return prod.ProdName;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IList GetOrderedList()
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {
                var products = db.Products.OrderBy(p => p.ProductId)
                    .Select(p => new { p.ProductId, p.ProdName })
                    .ToList();

                return products;
            }
        }
    }
}
