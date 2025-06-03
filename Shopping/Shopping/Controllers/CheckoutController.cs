using Microsoft.AspNetCore.Mvc;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Reponitory;
using System.Security.Claims;

namespace Shopping.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly Context _context;

        public CheckoutController(Context context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Checkout()
        {
            var user_email = User.FindFirstValue(ClaimTypes.Email);
            if(user_email == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var odercode = Guid.NewGuid().ToString();
            var oder_item = new OrderModel
            {
                Username = user_email,
                OrderCode = odercode,
                CreatedDate = DateTime.Now,
                Status = 1
            };
            _context.Orders.Add(oder_item);
            _context.SaveChanges();

            List<CartItemModel> carts = HttpContext.Session.get_Json<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            foreach (var cart in carts)
            {
                OrderDetailModel order_detail = new OrderDetailModel
                {
                    Username = user_email,
                    OrderCode = odercode,
                    ProductId = cart.ProductId,
                    Price = cart.Price,
                    Quantity = cart.Quantity,
                };
                _context.OrderDetails.Add(order_detail);
                _context.SaveChanges();
            }
            HttpContext.Session.Remove("Cart");

            TempData["success"] = "Add order success";
            return RedirectToAction("index", "Cart");
        }
    }
}
