namespace MVCSchool.DTO {
    public class GradeDTO {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public required string Topic { get; set; }
        public int Mark { get; set; }
        public DateTime Date { get; set; }
        public required string StudentFullName { get; set; }
        public required string SubjectName { get; set; }
    }
}
