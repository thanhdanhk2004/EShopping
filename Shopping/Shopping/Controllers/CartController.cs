using Microsoft.AspNetCore.Mvc;
using Shopping.Models;
using Shopping.Models.ViewModels;
using Shopping.Reponitory;

namespace Shopping.Controllers
{
    public class CartController : Controller
    {
        private readonly Context _context;
        public CartController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartItemModel> items = HttpContext.Session.get_Json<List<CartItemModel>>("Cart")?? new List<CartItemModel>();
            CartItemViewModel cart_vm = new()
            {
                CartItems = items,
                GrandTotal = items.Sum(x => x.Quantity*x.Price),
            };

            return View(cart_vm);
        }

        [HttpGet]
        [Route("Add")]
        public async Task<IActionResult> Add(int id)
        {
            ProductModel product = await _context.Products.FindAsync(id);
            List<CartItemModel> items = HttpContext.Session.get_Json<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = items.Where(c => c.ProductId == id).FirstOrDefault();

            if(cartItems == null)
            {
                items.Add(new CartItemModel(product));
            }
            else
            {
                cartItems.Quantity += 1;
            }
            HttpContext.Session.set_json("Cart", items);
            return Ok(new { success = true, message = "Add cart success" });
        }

        public async Task<IActionResult> Decreare(int id)
        {
            List<CartItemModel> items = HttpContext.Session.get_Json<List<CartItemModel>>("Cart");

            CartItemModel cartItems = items.Where(c => c.ProductId == id).FirstOrDefault();
            if(cartItems.Quantity > 1)
            {
                --cartItems.Quantity;
            }
            else
            {
                items.RemoveAll(p => p.ProductId == id);
            }

            if(items.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.set_json("Cart", items);
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Increare(int id)
        {
            List<CartItemModel> items = HttpContext.Session.get_Json<List<CartItemModel>>("Cart");

            CartItemModel cartItems = items.Where(c => c.ProductId == id).FirstOrDefault();
            if (cartItems.Quantity >= 1)
            {
                ++cartItems.Quantity;
            }
            else
            {
                items.RemoveAll(p => p.ProductId == id);
            }

            if (items.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.set_json("Cart", items);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            List<CartItemModel> items = HttpContext.Session.get_Json<List<CartItemModel>>("Cart");
            items.RemoveAll(p => p.ProductId == id);

            if (items.Count == 0)
                HttpContext.Session.Remove("Cart");
            else
                HttpContext.Session.set_json("Cart", items);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");   
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/index.cshtml");
        }
    }
}
