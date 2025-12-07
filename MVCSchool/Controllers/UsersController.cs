using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCSchool.Models;
using MVCSchool.ViewModels;

namespace MVCSchool.Controllers {
    public class UsersController : Controller {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        private IPasswordValidator<AppUser> _passwordValidator;

        public UsersController(
            UserManager<AppUser> userManager,
            IPasswordHasher<AppUser> passwordHasher,
            IPasswordValidator<AppUser> passwordValidator) {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
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
                AddErrors(result);
            }
            return View(user);
        }

        public async Task<IActionResult> EditAsync(string id) {
            AppUser? userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit == null) {
                return View("NotFound");
            }
            EditUserViewModel userToEditVM = new EditUserViewModel {
                Id = userToEdit.Id,
                Name = userToEdit.UserName!,
                Email = userToEdit.Email!,
            };
            return View(userToEditVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditUserViewModel user) {
            if (!ModelState.IsValid) {
                return View(user);
            }
            AppUser? appUser = await _userManager.FindByIdAsync(user.Id);
            if (appUser == null) {
                ModelState.AddModelError("", "User not found.");
                return View(user);
            }
            IdentityResult passwordValidation = await _passwordValidator.ValidateAsync(_userManager, appUser, user.Password);
            if (!passwordValidation.Succeeded) {
                AddErrors(passwordValidation);
                return View(user);
            }
            appUser.Email = user.Email;
            appUser.PasswordHash = _passwordHasher.HashPassword(appUser, user.Password);
            IdentityResult result = await _userManager.UpdateAsync(appUser);
            if (result.Succeeded) {
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id) {
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user != null) {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            else {
                 ModelState.AddModelError("", "User not found.");
            }
            return View("Index", _userManager.Users);
        }

        private void AddErrors(IdentityResult result) {
            foreach (IdentityError error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
