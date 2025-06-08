using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class ShipingController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
