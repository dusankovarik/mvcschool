using Microsoft.AspNetCore.Identity;

namespace MVCSchool.Models {
    public class RoleEdit {
        public required IdentityRole Role { get; set; }
        public required IEnumerable<AppUser> Members { get; set; }
        public required IEnumerable<AppUser> NonMembers { get; set; }
    }
}
