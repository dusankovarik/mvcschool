namespace MVCSchool.DTO {
    public class StudentDTO {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
