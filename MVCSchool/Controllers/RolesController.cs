using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCSchool.Models;

namespace MVCSchool.Controllers {
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task<IActionResult> EditAsync(string id) {
            IdentityRole? role = await _roleManager.FindByIdAsync(id);
            if (role == null) {
                return View("NotFound");
            }
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser user in _userManager.Users) {
                var list = await _userManager.IsInRoleAsync(user, role.Name!) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit {
                Role = role,
                Members = members,
                NonMembers = nonMembers,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(RoleModification model) {
            IdentityResult result;
            if (ModelState.IsValid) {
                foreach (string userId in model.IdsToAdd ?? []) {
                    AppUser? user = await _userManager.FindByIdAsync(userId);
                    if (user != null) {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded) {
                            AddErrors(result);
                        }
                    }
                }
                foreach (string userId in model.IdsToDelete ?? []) {
                    AppUser? user = await _userManager.FindByIdAsync(userId);
                    if (user != null) {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded) {
                            AddErrors(result);
                        }
                    }
                }
                if (ModelState.IsValid) {
                    return RedirectToAction("Index");
                }
            }
            return await EditAsync(model.RoleId);
        }

        private void AddErrors(IdentityResult result) {
            foreach (IdentityError error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
