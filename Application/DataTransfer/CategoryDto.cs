using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransfer
{
    public class CategoryDto : BaseDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must contain at least 3 letters.")]
        [MaxLength(30, ErrorMessage = "Name can have up to 30 letters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters only.")]
        public string Name { get; set; }

        [Display(GroupName = "Articles in this category")]
        public IEnumerable<string> ArticlesInCategory { get; set; }
    }
}