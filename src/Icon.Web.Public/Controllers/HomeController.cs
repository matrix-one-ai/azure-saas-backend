using Microsoft.AspNetCore.Mvc;
using Icon.Web.Controllers;

namespace Icon.Web.Public.Controllers
{
    public class HomeController : IconControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}