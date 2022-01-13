using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VeganSepeti.CartApi.Models;
using VeganSepeti.CartApi.Common;

namespace VeganSepeti.CartApi.Business
{
    public class ProductManager
    {
        private static ProductManager instance;
        public static ProductManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductManager();
                }
                return instance;
            }
        }

        private List<Product> productList;
        public List<Product> ProductList
        {
            get
            {
                if (productList == null || productList.Count > 0)
                {
                    productList = GetProductList();
                }
                return productList;
            }
        }

        /// <summary>
        /// Gets all product list from json file
        /// </summary>
        /// <returns></returns>
        private static List<Product> GetProductList()
        {
            string json = string.Empty;
            string productListFilePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.DataPath, Constants.AllProductListFile);
            using (StreamReader streamReader = new(productListFilePath))
            {
                json = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<List<Product>>(json);
        }

        /// <summary>
        /// Get the product from product list
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product GetProduct(int productId)
        {
            return ProductList.Single(product => product.Id == productId);
        }

        /// <summary>
        /// Checks the product existence in the product list
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool IsProductExist(int productId)
        {
            return ProductList.Any(product => product.Id == productId && product.IsActive && product.IsExist);
        }
    }
}
