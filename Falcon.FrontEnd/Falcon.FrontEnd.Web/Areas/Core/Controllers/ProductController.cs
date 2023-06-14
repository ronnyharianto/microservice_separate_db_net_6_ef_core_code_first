using Microsoft.AspNetCore.Mvc;

namespace Falcon.FrontEnd.Web.Areas.Core.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
