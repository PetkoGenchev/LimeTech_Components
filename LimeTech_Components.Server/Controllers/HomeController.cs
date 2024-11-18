using Microsoft.AspNetCore.Mvc;

namespace LimeTech_Components.Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
