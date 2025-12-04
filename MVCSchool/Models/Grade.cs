using System.ComponentModel.DataAnnotations;

namespace MVCSchool.Models {
    public class Grade {
        public int Id { get; set; }
        public required Student Student { get; set; }
        public required Subject Subject { get; set; }
        public required string Topic { get; set; }
        public int Mark { get; set; }
        public DateTime Date { get; set; }
    }
}
