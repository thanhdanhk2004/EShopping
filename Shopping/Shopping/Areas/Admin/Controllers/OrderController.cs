using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Reponitory;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly Context _context;
        public OrderController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Orders.ToList());
        }

        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var order_details = await _context.OrderDetails.Include(o => o.Product).Where(o => o.OrderCode == ordercode).ToListAsync();
            return View(order_details);
        }

        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(int status, string ordercode)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
            order.Status = status;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new {success = true, message="Order status update success"});
            } catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the order status");
            }
            return View();
        }
    }
}
