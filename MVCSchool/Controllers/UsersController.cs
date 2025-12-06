using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCSchool.Models;
using MVCSchool.ViewModels;

namespace MVCSchool.Controllers {
    public class UsersController : Controller {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher) {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
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

        public async Task<IActionResult> EditAsync(string id) {
            AppUser? userToEdit = await _userManager.FindByIdAsync(id);
            return userToEdit != null ? View(userToEdit) : View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, string email, string password) {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user != null) {
                if (!string.IsNullOrEmpty(email)) {
                    user.Email = email;
                }
                else {
                    ModelState.AddModelError("", "Email cannot be empty.");
                }
                if (!string.IsNullOrEmpty(password)) {
                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                }
                else {
                    ModelState.AddModelError("", "Password cannot be empty.");
                }
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password)) {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) {
                        return RedirectToAction("Index");
                    }
                    AddErrors(result);
                }
            }
            else {
                ModelState.AddModelError("", "User not found.");
            }
            return View(user);
        }

        private void AddErrors(IdentityResult result) {
            foreach (IdentityError error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
