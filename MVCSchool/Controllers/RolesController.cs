using Microsoft.AspNetCore.Mvc;

namespace MVCSchool.Controllers {
    public class RolesController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
