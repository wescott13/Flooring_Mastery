using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.Data.Data
{
    public class ProductsRepository : IProductsRepository
    {
        string productsPath = @".\Products.txt";
        Dictionary<string, Products> productsDictionary;
        public ProductsRepository()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            string dirProducts = @".\Products";
            Directory.SetCurrentDirectory(dirProducts);

            productsDictionary = new Dictionary<string, Products>();
            var fileProducts = File.ReadAllLines(productsPath);
            foreach (var row in fileProducts)
            {
                string[] columns = row.Split(',');

                try
                {
                    Products products = new Products();
                    products.ProductType = columns[0];
                    products.CostPerSquareFoot = Convert.ToDecimal(columns[1]);
                    products.LaborCostPerSquareFoot = Convert.ToDecimal(columns[2]);

                    productsDictionary.Add(products.ProductType, products);  //ProductType is my key
                }
                catch (Exception ex)
                {
                }
            }
        }
        public List<String> GetAllProducts()
        {
            return productsDictionary.Keys.ToList();  //simplifies foreach
        }

        public Dictionary<string, Products> GetAllProducts2()
        {   
            return productsDictionary;
        }

        public Products LoadProduct(string product)
        {
            if (productsDictionary.ContainsKey(product))
            {
                return productsDictionary[product];
            }
            else
            {
                return null;
            }
        }
    }
}
