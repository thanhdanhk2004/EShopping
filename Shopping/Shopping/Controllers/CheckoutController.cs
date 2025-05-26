using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
