using System.Linq;
using System.Web.Mvc;
using Raven.Client.Linq;
using RavenDBApplication.Models;

namespace RavenDBApplication.Controllers
{
    public class HomeController : RavenController
    {
         public ActionResult Index()
         {
             var featuredProducts =
                 DocumentSession.Query<Product>().Where(product => product.Featured).ToArray();
             return View(featuredProducts);
         }
    }
}