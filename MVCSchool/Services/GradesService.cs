using MVCSchool.Models;

namespace MVCSchool.Services {
    public class GradesService {
        public ApplicationDbContext _dbContext;

        public GradesService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
    }
}
