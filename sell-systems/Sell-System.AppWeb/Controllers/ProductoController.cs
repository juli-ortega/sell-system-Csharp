using Microsoft.AspNetCore.Mvc;

namespace Sell_System.AppWeb.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
