namespace Portal.Models
{
    public class AppRole
    {
        public int Id { get; set; }
        public string? Role { get; set; }
        public List<AppClaim>? Claims { get; set; }

    }
}
