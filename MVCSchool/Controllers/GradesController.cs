using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCSchool.DTO;
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

        public async Task<IActionResult> CreateAsync() {
            var gradeDropdownsData = await _service.GetNewGradeDropdownsValuesAsync();
            ViewBag.Students = new SelectList(gradeDropdownsData.Students, "Id", "FullName");
            ViewBag.Subjects = new SelectList(gradeDropdownsData.Subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GradeDTO newGrade) {
            await _service.CreateAsync(newGrade);
            return RedirectToAction("Index");
        }
    }
}
