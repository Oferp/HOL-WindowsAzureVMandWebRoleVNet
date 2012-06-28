using System.Collections.Generic;

namespace AzureStore.Models
{
    public class IndexViewModel
    {
        public string SearchCriteria { get; set; }

        public IEnumerable<string> Products { get; set; }
    }
}