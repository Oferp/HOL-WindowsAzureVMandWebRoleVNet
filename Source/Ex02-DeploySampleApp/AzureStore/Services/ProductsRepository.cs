using System.Collections.Generic;
using System.Linq;
using AzureStore.Models;

namespace AzureStore.Services
{
    public class ProductsRepository : IProductRepository
    {
        public List<string> GetProducts()
        {
            List<string> products = null;

            using (var context = new AdventureWorksEntities())
            {
                var query = from product in context.Products
                            select product.Name;
                products = query.ToList();
            }

            return products;
        }

        public List<string> Search(string criteria)
        {
            List<string> result;

            using (var context = new AdventureWorksEntities())
            {
                var query = context.ExecuteStoreQuery<string>("Select Name from Production.Product Where Freetext(*,{0})", criteria);

                result = query.ToList();
            }

            return result;
        }
    }
}