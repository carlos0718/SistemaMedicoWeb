using Microsoft.AspNetCore.Mvc;

namespace WebAppSistemaMedico.Controllers
{
    public class Login : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
