using System.ComponentModel.DataAnnotations;

namespace MVCSchool.Models {
    public class Student {
        public int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }
    }
}
