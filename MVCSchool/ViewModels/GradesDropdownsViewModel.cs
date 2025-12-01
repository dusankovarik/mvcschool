using MVCSchool.Models;

namespace MVCSchool.ViewModels {
    public class GradesDropdownsViewModel {
        public IEnumerable<Student> Students { get; set; } = new List<Student>();
        public IEnumerable<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
