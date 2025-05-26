﻿using Microsoft.AspNetCore.Mvc;

namespace Shopping.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/index.cshtml");
        }
    }
}
