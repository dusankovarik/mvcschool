namespace MVCSchool.Models {
    public class RoleModification {
        public required string RoleId { get; set; }
        public required string RoleName { get; set; }
        public string[]? IdsToAdd { get; set; }
        public string[]? IdsToDelete { get; set; }
    }
}
