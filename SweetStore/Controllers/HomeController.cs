using Microsoft.AspNetCore.Mvc;

namespace SweetStore.Controllers  
{
    public class HomeController : Controller
    {
        [HttpGet("/signin")]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}