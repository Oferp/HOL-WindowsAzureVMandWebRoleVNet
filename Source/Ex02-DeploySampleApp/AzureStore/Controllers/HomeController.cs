namespace AzureStore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AzureStore.Models;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            return this.View();
        }

        public ActionResult Index()
        {
            return this.Search(null);
        }

        [HttpPost]
        public ActionResult Add(string selectedProduct)
        {
            if (selectedProduct != null)
            {
                List<string> cart = this.Session["Cart"] as List<string> ?? new List<string>();
                cart.Add(selectedProduct);
                this.Session["Cart"] = cart;
            }

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Search(string searchCriteria)
        {
            Services.IProductRepository productRepository = new Services.ProductsRepository();
            var products = string.IsNullOrEmpty(searchCriteria) ?
                productRepository.GetProducts() : productRepository.Search(searchCriteria);

            // add all products currently not in session
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
            var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

            var model = new IndexViewModel()
            {
                Products = filteredProducts,
                SearchCriteria = searchCriteria
            };

            return this.View("Index", model);
        }

        public ActionResult Checkout()
        {
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
            var model = new IndexViewModel()
            {
                Products = itemsInSession
            };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Remove(string products)
        {
            if (products != null)
            {
                var itemsInSession = this.Session["Cart"] as List<string>;
                if (itemsInSession != null)
                {
                    itemsInSession.Remove(products);
                }
            }

            return this.RedirectToAction("Checkout");
        }
    }
}