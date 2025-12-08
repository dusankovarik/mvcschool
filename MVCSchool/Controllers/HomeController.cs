using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCSchool.Models;

namespace MVCSchool.Controllers {
    public class HomeController : Controller {
        private UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync() {
            AppUser? user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            string message = $"Hello {user.UserName}!";
            return View("Index", message);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
