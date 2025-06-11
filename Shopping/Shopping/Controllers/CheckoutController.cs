using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shopping.Areas.Admin.Reponsitory;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Reponitory;
using System.Security.Claims;

namespace Shopping.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly Context _context;
        private readonly IEmailSender _emailSender;
        public CheckoutController(Context context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        
        public async Task<IActionResult> Checkout()
        {
            var user_email = User.FindFirstValue(ClaimTypes.Email);
            if(user_email == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var odercode = Guid.NewGuid().ToString();
            //Add shipping tu cookies
            var shipping_price_cookies = Request.Cookies["shipping_price"];
            decimal shipping_price = 0;
            if (shipping_price_cookies != null)
            {
                var shipping_price_json = shipping_price_cookies;
                shipping_price = JsonConvert.DeserializeObject<decimal>(shipping_price_json);
            }

            var coupon_cookies = Request.Cookies["CouponTitle"];
            var oder_item = new OrderModel
            {
                Username = user_email,
                OrderCode = odercode,
                CreatedDate = DateTime.Now,
                Status = 1,
                PriceShipping = shipping_price,
                CouponCode = coupon_cookies,
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

                var product = await _context.Products.FindAsync(cart.ProductId);
                if(product != null)
                {
                    product.Quantity = product.Quantity - order_detail.Quantity;
                    product.Sold += order_detail.Quantity;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                }
            }
            HttpContext.Session.Remove("Cart");
            //Gui mail
            //var receiver = "a@gmail.com";
            //var subject = "Order";
            //var message = "Order success";
            //await _emailSender.SendEmailAsync(receiver, subject, message);

            TempData["success"] = "Add order success";
            return RedirectToAction("History", "Account");
        }
    }
}
