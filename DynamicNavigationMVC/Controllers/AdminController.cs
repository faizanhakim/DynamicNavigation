using Microsoft.AspNetCore.Mvc;

namespace DynamicNavigationMVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
