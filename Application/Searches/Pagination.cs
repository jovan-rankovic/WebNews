namespace Application.Searches
{
    public abstract class Pagination
    {
        public int PerPage { get; set; } = 2;
        public int PageNumber { get; set; } = 1;
    }
}