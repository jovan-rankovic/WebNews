using System.Collections.Generic;

namespace Application.Responses
{
    public class PagedResponse<TDto>
    {
        public int TotalCount { get; set; }
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<TDto> Data { get; set; }
    }
}