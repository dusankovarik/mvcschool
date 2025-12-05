using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCSchool.Models;
using MVCSchool.ViewModels;

namespace MVCSchool.Controllers {
    public class UsersController : Controller {
        private UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        public IActionResult Index() {
            return View(_userManager.Users);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserViewModel user) {
            if (ModelState.IsValid) {
                AppUser appUser = new AppUser {
                    UserName = user.Name,
                    Email = user.Email,
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                foreach (IdentityError error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
    }
}
