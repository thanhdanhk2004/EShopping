using Microsoft.AspNetCore.Mvc;
using Shopping.Reponitory;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Context _context;
        public CategoryController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> index(string slug = "")
        {
            int id = _context.Categories.Where(c => c.Slug == slug).Select(c => c.Id).FirstOrDefault();
            if(id == 0)
                return RedirectToAction("Index");
            var products = _context.Products.Where(p => p.CategoryId == id);
            return View(await products.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
