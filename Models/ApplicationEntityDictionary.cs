namespace Portal.Models
{
    public class ApplicationEntityDictionary
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LanguageCode { get; set; }

        public int ParentApplicationId { get; set; } // For ContentDictionary
        public Application? ParentApplication { get; set; } // For ContentDictionary
       


    }
}
