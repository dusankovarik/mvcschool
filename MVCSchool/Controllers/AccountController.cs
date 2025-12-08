using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCSchool.Models;
using MVCSchool.ViewModels;

namespace MVCSchool.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl) {
            LoginViewModel login = new LoginViewModel();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel login) {
            if (ModelState.IsValid) {
                AppUser? appUser = await _userManager.FindByNameAsync(login.UserName!);
                if (appUser != null) {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                        appUser, login.Password!, false, false);
                    if (result.Succeeded) {
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(login.UserName), "Login failed. Invalid username or password.");
            }
            return View(login);
        }

        public async Task<IActionResult> LogoutAsync() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
