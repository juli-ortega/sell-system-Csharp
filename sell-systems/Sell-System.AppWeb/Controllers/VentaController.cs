using Microsoft.AspNetCore.Mvc;

namespace Sell_System.AppWeb.Controllers
{
    public class VentaController : Controller
    {
        public IActionResult NuevaVenta()
        {
            return View();
        }
        
        public IActionResult HistorialVenta()
        {
            return View();
        }
    }
}
