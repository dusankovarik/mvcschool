using System.ComponentModel.DataAnnotations;

namespace MVCSchool.ViewModels {
    public class LoginViewModel {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }

        public bool Remember { get; set; }
    }
}
