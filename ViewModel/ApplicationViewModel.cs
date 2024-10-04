using Portal.Models;

namespace Portal.ViewModel
{
    public class ApplicationViewModel
    {

        public Application? Application { get; set; }
        public List<Application>? Applications { get; set; }
        public List<Category>? DbCategoryList { get; set; }
        public List<Category>? CategoryList { get; set; }
        //public string? AddApplicationIds { get; set; }
        //public string? RemoveApplicationIds { get; set; }

        public List<Keyword>? KeywordList { get; set; }
    }
}
