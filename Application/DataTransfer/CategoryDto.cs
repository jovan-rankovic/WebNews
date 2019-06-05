using System.Collections.Generic;

namespace Application.DataTransfer
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; }

        public IEnumerable<string> ArticlesInCategory { get; set; }
    }
}