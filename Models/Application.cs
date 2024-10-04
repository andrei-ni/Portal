using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Models
{
    public class Application
    {
        public int Id { get; set; }

        public string? DevLink { get; set; }

        public string? AppLink { get; set; }

        public string? ManualLink { get; set; }

        public List<Category>? Categories { get; set; }

        public string? IconLocation { get; set; }

        public List<Keyword>? Keywords { get; set; }

        public List<ApplicationEntityDictionary>? ContentDictionary { get; set; }

        [NotMapped]
        public ApplicationEntityDictionary? SelectedContent { get; set; }

    }
}
