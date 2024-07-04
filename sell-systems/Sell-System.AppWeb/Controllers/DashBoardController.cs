using Microsoft.AspNetCore.Mvc;

namespace Sell_System.AppWeb.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
