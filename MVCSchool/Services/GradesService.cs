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

        public async Task<GradesDropdownsViewModel> GetNewGradeDropdownsValuesAsync() {
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

        private Grade DtoToModel(GradeDTO gradeDto) {
            var student = _dbContext.Students.FirstOrDefault(st => st.Id == gradeDto.StudentId);
            var subject = _dbContext.Subjects.FirstOrDefault(sub => sub.Id == gradeDto.SubjectId);
            if (student == null || subject == null) {
                return null!;
            }
            return new Grade {
                Student = student,
                Subject = subject,
                Date = DateTime.Now,
                Topic = gradeDto.Topic,
                Mark = gradeDto.Mark,
            };
        }
    }
}
