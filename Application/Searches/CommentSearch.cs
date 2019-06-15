namespace Application.Searches
{
    public class CommentSearch : Pagination
    {
        public string Text { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}