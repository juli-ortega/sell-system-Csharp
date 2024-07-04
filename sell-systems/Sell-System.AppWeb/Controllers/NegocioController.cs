using Microsoft.AspNetCore.Mvc;

namespace Sell_System.AppWeb.Controllers
{
    public class NegocioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
