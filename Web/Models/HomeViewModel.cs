using Application.DataTransfer;
using System.Collections.Generic;

namespace Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ArticleDto> Articles { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
    }
}