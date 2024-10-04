using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string? IconLocation { get; set; }

        public List<Application>? Applications { get; set; }

        public List<CategoryEntityDictionary>? ContentDictionary { get; set; }

        [NotMapped] 
        public CategoryEntityDictionary? SelectedContent { get; set; }

    }
}
