using System.ComponentModel.DataAnnotations;

namespace MVCSchool.Models {
    public class Subject {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
