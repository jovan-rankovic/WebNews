namespace Application.Searches
{
    public class ArticleSearch : Pagination
    {
        public string Title { get; set; }
        public string Hashtag { get; set; }
        public int? CategoryId { get; set; }
    }
}