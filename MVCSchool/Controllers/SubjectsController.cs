using Microsoft.AspNetCore.Mvc;
using MVCSchool.DTO;
using MVCSchool.Services;

namespace MVCSchool.Controllers {
    public class SubjectsController : Controller {
        private SubjecstService _service;

        public SubjectsController(SubjecstService service) {
            _service = service;
        }

        public async Task<IActionResult> IndexAsync() {
            var allSubjects = await _service.GetAllAsync();
            return View(allSubjects);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(SubjectDTO newSubject) {
            await _service.CreateAsync(newSubject);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditAsync(int id) {
            var subjectToEdit = await _service.GetByIdAsync(id);
            return subjectToEdit != null ? View(subjectToEdit) : View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(SubjectDTO subject) {
            await _service.UpdateAsync(subject);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {
            var subjectToDelete = await _service.GetByIdAsync(id);
            if (subjectToDelete == null) {
                return View("NotFound");
            }
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
