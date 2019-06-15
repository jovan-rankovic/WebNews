namespace Application.Searches
{
    public abstract class Pagination
    {
        public int PageNumber { get; set; } = 1;
        public int PerPage { get; set; } = 2;
    }
}