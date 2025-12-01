using Microsoft.EntityFrameworkCore;
using MVCSchool.DTO;
using MVCSchool.Models;

namespace MVCSchool.Services {
    public class SubjectService {
        private ApplicationDbContext _dbContext;

        public SubjectService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SubjectDTO>> GetAllAsync() {
            var allSubjects = await _dbContext.Subjects.ToListAsync();
            var subjectDtos = new List<SubjectDTO>();
            foreach (var subject in allSubjects) {
                subjectDtos.Add(ModelToDto(subject));
            }
            return subjectDtos;
        }

        public async Task CreateAsync(SubjectDTO newSubject) {
            await _dbContext.Subjects.AddAsync(DtoToModel(newSubject));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SubjectDTO?> GetByIdAsync(int id) {
            var subject = await _dbContext.Subjects.FirstOrDefaultAsync(sub => sub.Id == id);
            return subject != null ? ModelToDto(subject) : null;
        }

        public async Task UpdateAsync(SubjectDTO updatedSubject) {
            _dbContext.Subjects.Update(DtoToModel(updatedSubject));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var subjectToDelete = await _dbContext.Subjects.FirstOrDefaultAsync(sub => sub.Id == id);
            if (subjectToDelete == null) {
                return;
            }
            _dbContext.Subjects.Remove(subjectToDelete);
            await _dbContext.SaveChangesAsync();
        }

        private SubjectDTO ModelToDto(Subject subject) {
            return new SubjectDTO {
                Id = subject.Id,
                Name = subject.Name,
            };
        }

        private Subject DtoToModel(SubjectDTO subjectDto) {
            return new Subject {
                Id = subjectDto.Id,
                Name = subjectDto.Name,
            };
        }
    }
}
