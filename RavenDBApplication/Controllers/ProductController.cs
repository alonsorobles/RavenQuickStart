using System.Linq;
using System.Web.Mvc;
using RavenDBApplication.Models;

namespace RavenDBApplication.Controllers
{
    public class ProductController : RavenController
    {
         public ActionResult Index()
         {
             var products = DocumentSession.Query<Product>().ToArray();
             return View(products);
         }

        public ActionResult Add()
        {
            return View(new Product());
        }

        [HttpPost]
        public ActionResult Add(Product productModel)
        {
            if (ModelState.IsValid)
            {
                DocumentSession.Store(productModel);
                return RedirectToAction("Index");
            }
            return View(productModel);
        }

        public ActionResult Edit(int id)
        {
            var product = DocumentSession.Load<Product>(id);
            return View("Add", product);
        }

        [HttpPost]
        public ActionResult Edit(Product productModel)
        {
            if (ModelState.IsValid)
            {
                var product = DocumentSession.Load<Product>(productModel.Id);
                product.Title = productModel.Title;
                product.Description = productModel.Description;
                product.Price = productModel.Price;
                product.Featured = productModel.Featured;
                return RedirectToAction("Index");
            }
            return View("Add", productModel);
        }

        public ActionResult Delete (int id)
        {
            var product = DocumentSession.Load<Product>(id);
            DocumentSession.Delete(product);
            return RedirectToAction("Index");
        }
    }
}