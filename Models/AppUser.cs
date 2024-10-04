namespace Portal.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string? AdIdentity { get; set; } // Username
        public string? Email { get; set; }
        public string? BadgeNo { get; set; }
        public string? Name { get; set; }
        public int? RoleId { get; set; }
        public AppRole? Role { get; set; }
    }
}
