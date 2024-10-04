using Portal.Models;

namespace Portal.ViewModel
{
    public class CategoryViewModel
    {
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        //public CategoryEntityDictionary? SelectedContent { get; set; }

        public List<Application>? DbApplicationList { get; set; }
        public List<Application>? ApplicationList { get; set; }
        //public string? AddCategoryIds { get; set; }
        //public string? RemoveCategoryIds { get; set; }


        
    }
}
