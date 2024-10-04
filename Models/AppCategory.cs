namespace Portal.Models
{
    public class AppCategory
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int CategoryId { get; set; }
        public Application? Application { get; set; }
        public Category? Category { get; set; }
    }
}
