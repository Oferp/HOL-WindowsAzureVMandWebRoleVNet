using System.Collections.Generic;

namespace AzureStore.Services
{
    public interface IProductRepository
    {
        List<string> GetProducts();

        List<string> Search(string criteria);
    }
}