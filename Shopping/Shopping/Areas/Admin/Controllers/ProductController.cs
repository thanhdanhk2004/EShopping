using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Reponitory;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly Context _context;
        
        public ProductController(Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.OrderBy(p => p.Id).Include(p => p.Category).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");

            return View();
        }
    }
}
