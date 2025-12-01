using Microsoft.AspNetCore.Mvc;
using MVCSchool.DTO;
using MVCSchool.Services;

namespace MVCSchool.Controllers {
    public class StudentsController : Controller {
        private StudentService _service;

        public StudentsController(StudentService service) {
            _service = service;
        }

        public async Task<IActionResult> IndexAsync() {
            var allStudents = await _service.GetAllAsync();
            return View(allStudents);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(StudentDTO newStudent) {
            await _service.CreateAsync(newStudent);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditAsync(int id) {
            var studentToEdit = await _service.GetByIdAsync(id);
            return studentToEdit != null ? View(studentToEdit) : View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(StudentDTO student) {
            await _service.UpdateAsync(student);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id) {
            var studentToDelete = await _service.GetByIdAsync(id);
            if (studentToDelete == null) {
                return View("NotFound");
            }
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
