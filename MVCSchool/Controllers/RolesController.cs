using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVCSchool.Controllers {
    public class RolesController : Controller {
        private RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
        }

        public IActionResult Index() {
            return View(_roleManager.Roles);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(string name) {
            if (ModelState.IsValid) {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            return View("Create", name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id) {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);
            if (role != null) {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            else {
                ModelState.AddModelError("", "Role not found.");
            }
            return View("Index", _roleManager.Roles);
        }

        private void AddErrors(IdentityResult result) {
            foreach (IdentityError error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
