using System.Collections.Generic;

namespace Application.DataTransfer
{
    public class ArticleDto : BaseDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }

        public string Author { get; set; }
        public IEnumerable<string> CategoriesForArticle { get; set; }
    }
}