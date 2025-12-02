using Microsoft.EntityFrameworkCore;
using MVCSchool.DTO;
using MVCSchool.Models;
using MVCSchool.ViewModels;

namespace MVCSchool.Services {
    public class GradesService {
        public ApplicationDbContext _dbContext;

        public GradesService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<GradesDropdownsViewModel> GetGradeDropdownsValuesAsync() {
            var gradeDropdownsData = new GradesDropdownsViewModel() {
                Students = await _dbContext.Students.OrderBy(st => st.LastName).ToListAsync(),
                Subjects = await _dbContext.Subjects.OrderBy(sub => sub.Name).ToListAsync(),
            };
            return gradeDropdownsData;
        }

        public async Task CreateAsync(GradeDTO newGrade) {
            Grade gradeToInsert = DtoToModel(newGrade);
            if (gradeToInsert != null) {
                await _dbContext.Grades.AddAsync(gradeToInsert);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GradeDTO>> GetAllAsync() {
            List<Grade> grades = await _dbContext.Grades.Include(gr => gr.Student).Include(gr => gr.Subject)
                .ToListAsync();
            List<GradeDTO> gradeDtos = new List<GradeDTO>();
            foreach (var grade in grades) {
                gradeDtos.Add(ModelToDto(grade));
            }
            return gradeDtos;
        }

        public async Task<GradeDTO?> GetByIdAsync(int id) {
            var grade = await _dbContext.Grades.Include(gr => gr.Student).Include(gr => gr.Subject)
                .FirstOrDefaultAsync(gr => gr.Id == id);
            return grade != null ? ModelToDto(grade) : null;
        }

        public async Task UpdateAsync(GradeDTO updatedGrade) {
            _dbContext.Grades.Update(DtoToModel(updatedGrade));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var gradeToDelete = await _dbContext.Grades.FirstOrDefaultAsync(gr => gr.Id == id);
            if (gradeToDelete == null) {
                return;
            }
            _dbContext.Grades.Remove(gradeToDelete);
            await _dbContext.SaveChangesAsync();
        }

        private Grade DtoToModel(GradeDTO gradeDto) {
            var student = _dbContext.Students.FirstOrDefault(st => st.Id == gradeDto.StudentId);
            var subject = _dbContext.Subjects.FirstOrDefault(sub => sub.Id == gradeDto.SubjectId);
            if (student == null || subject == null) {
                return null!;
            }
            return new Grade {
                Id = gradeDto.Id,
                Student = student,
                Subject = subject,
                Date = DateTime.Now,
                Topic = gradeDto.Topic,
                Mark = gradeDto.Mark,
            };
        }

        private GradeDTO ModelToDto(Grade grade) {
            return new GradeDTO {
                Id = grade.Id,
                StudentId = grade.Student.Id,
                SubjectId = grade.Subject.Id,
                Topic = grade.Topic,
                Mark = grade.Mark,
                Date = grade.Date,
                StudentFullName = $"{grade.Student.FirstName} {grade.Student.LastName}",
                SubjectName = grade.Subject.Name,
            };
        }
    }
}
