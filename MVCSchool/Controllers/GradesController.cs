using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCSchool.DTO;
using MVCSchool.Services;

namespace MVCSchool.Controllers {
    [Authorize]
    public class GradesController : Controller {
        private GradesService _service;

        public GradesController(GradesService service) {
            _service = service;
        }

        public async Task<IActionResult> IndexAsync() {
            var allGrades = await _service.GetAllAsync();
            return View(allGrades);
        }

        public async Task<IActionResult> CreateAsync() {
            var gradeDropdownsData = await _service.GetGradeDropdownsValuesAsync();
            ViewBag.Students = new SelectList(gradeDropdownsData.Students, "Id", "FullName");
            ViewBag.Subjects = new SelectList(gradeDropdownsData.Subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GradeDTO newGrade) {
            await _service.CreateAsync(newGrade);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditAsync(int id) {
            var gradeToEdit = await _service.GetByIdAsync(id);
            if (gradeToEdit == null) {
                return View("NotFound");
            }
            var gradeDropdownsData = await _service.GetGradeDropdownsValuesAsync();
            ViewBag.Students = new SelectList(gradeDropdownsData.Students, "Id", "FullName");
            ViewBag.Subjects = new SelectList(gradeDropdownsData.Subjects, "Id", "Name");
            return View(gradeToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(GradeDTO updatedGrade) {
            await _service.UpdateAsync(updatedGrade);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {
            var gradeToDelete = await _service.GetByIdAsync(id);
            if (gradeToDelete == null) {
                return View("NotFound");
            }
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
