using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult index()
        {
            return View();
        }
    }
}
