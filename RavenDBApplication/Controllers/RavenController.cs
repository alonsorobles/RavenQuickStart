using System.Web.Mvc;
using Raven.Client;

namespace RavenDBApplication.Controllers
{
    public class RavenController : Controller
    {
        public const string CurrentRequestDocumentSessionKey = @"CurrentRequestDocumentSession";
        public static IDocumentStore DocumentStore { get; set; }
        public IDocumentSession DocumentSession { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DocumentSession = (IDocumentSession) HttpContext.Items[CurrentRequestDocumentSessionKey];
        }
    }
}