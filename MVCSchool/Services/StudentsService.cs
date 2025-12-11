using Microsoft.EntityFrameworkCore;
using MVCSchool.DTO;
using MVCSchool.Models;

namespace MVCSchool.Services {
    public class StudentsService {
        private ApplicationDbContext _dbContext;

        public StudentsService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllAsync() {
            var allStudents = await _dbContext.Students.ToListAsync();
            var studentDtos = new List<StudentDTO>();
            foreach (var student in allStudents) {
                studentDtos.Add(ModelToDto(student));
            }
            return studentDtos;
        }

        public async Task CreateAsync(StudentDTO newStudent) {
            await _dbContext.Students.AddAsync(DtoToModel(newStudent));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<StudentDTO?> GetByIdAsync(int id) {
            var student = await _dbContext.Students.FirstOrDefaultAsync(st => st.Id == id);
            return student != null ? ModelToDto(student) : null;
        }

        public async Task UpdateAsync(StudentDTO updatedStudent) {
            _dbContext.Students.Update(DtoToModel(updatedStudent));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var studentToDelete = await _dbContext.Students.FirstOrDefaultAsync(st => st.Id == id);
            if (studentToDelete == null) {
                return;
            }
            _dbContext.Students.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public object GetByName(string query) {
            string[] nameParts = query.Split(',');
            List<Student> studentsThatMatch = new List<Student>();
            List<StudentDTO> returnedStudents = new List<StudentDTO>();
            if (nameParts.Length > 1) {
                studentsThatMatch = _dbContext.Students.Where(st => st.LastName == nameParts[0].Trim())
                    .Where(st => st.FirstName == nameParts[1].Trim()).ToList();
            }
            else {
                studentsThatMatch = _dbContext.Students.Where(st => st.LastName == nameParts[0].Trim() ||
                st.FirstName == nameParts[0].Trim()).ToList();
            }
            foreach (var student in studentsThatMatch) {
                returnedStudents.Add(ModelToDto(student));
            }
            return returnedStudents;
        }

        private StudentDTO ModelToDto(Student student) {
            return new StudentDTO {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth
            };
        }

        private Student DtoToModel(StudentDTO studentDto) {
            return new Student {
                Id = studentDto.Id,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                DateOfBirth = studentDto.DateOfBirth
            };
        }
    }
}
