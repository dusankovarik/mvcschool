using Microsoft.AspNetCore.Mvc;
using MVCSchool.Services;

namespace MVCSchool.Controllers {
    public class GradesController : Controller {
        private GradesService _service;

        public GradesController(GradesService service) {
            _service = service;
        }

        public IActionResult Index() {
            return View();
        }
    }
}
