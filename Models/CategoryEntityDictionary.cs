namespace Portal.Models
{
    public class CategoryEntityDictionary
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LanguageCode { get; set; }

        public int ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        
    }
}
