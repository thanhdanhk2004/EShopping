using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Shopping.Models;
using Shopping.Reponitory;
using Shopping.Models.ViewModels;
namespace Shopping.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signInManager;
        private Context _context;
        
        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login(string url)
        {
            return View(new LoginViewModel { ReturnUrl = url});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                if(result.Succeeded)
                {
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid username and password");
            }
            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {
            if(ModelState.IsValid)
            {
                AppUserModel new_user = new AppUserModel { UserName = user.Username, Email = user.Email };
                IdentityResult result = await _userManager.CreateAsync(new_user, user.Password);
                if (result.Succeeded)
                {
                    return Redirect("/Account/login");
                }
                foreach(IdentityError error in result.Errors) 
                    ModelState.AddModelError("", error.Description);

            }
            return View(user);
        }
    }
}
