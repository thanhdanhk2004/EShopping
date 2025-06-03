using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Shopping.Reponitory;

namespace Shopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/User")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly Context _context;
        private UserManager<AppUserModel> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserController(Context context, RoleManager<IdentityRole> roleManager, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create(UserModel user)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(AppUserModel user)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result = await _userManager.CreateAsync(user, user.PasswordHash);
                if(result.Succeeded)
                {
                    TempData["success"] = "Create success a user";
                    return RedirectToAction("Index");
                }
                foreach(IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);    
            }
            TempData["error"] = "Have a error in model";
            List<string> errors = new List<string>();
            foreach(var value in ModelState.Values)
            {
                foreach(var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return BadRequest(errors);
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["success"] = "Delete success a user";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Delete error a user";
            return RedirectToAction("Index");
        }
    }
}
