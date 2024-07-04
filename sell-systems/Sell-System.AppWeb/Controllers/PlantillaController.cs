using Microsoft.AspNetCore.Mvc;

namespace Sell_System.AppWeb.Controllers
{
    public class PlantillaController : Controller
    {
        public IActionResult EnviarClave(string clave, string correo)
        {
            ViewData["Correo"] = correo;
            ViewData["Clave"] = clave;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";

            return View();
        }
        public IActionResult RestablecerClave(string clave)
        {
            return View();
        }
    }
}
