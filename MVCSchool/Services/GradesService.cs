using Microsoft.EntityFrameworkCore;
using MVCSchool.Models;
using MVCSchool.ViewModels;

namespace MVCSchool.Services {
    public class GradesService {
        public ApplicationDbContext _dbContext;

        public GradesService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<GradesDropdownsViewModel> GetNewGradeDropdownsValues() {
            var gradeDropdownsData = new GradesDropdownsViewModel() {
                Students = await _dbContext.Students.OrderBy(st => st.LastName).ToListAsync(),
                Subjects = await _dbContext.Subjects.OrderBy(sub => sub.Name).ToListAsync(),
            };
            return gradeDropdownsData;
        }
    }
}
