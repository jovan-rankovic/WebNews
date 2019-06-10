using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransfer
{
    public class HashtagDto : BaseDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Tag must contain at least 4 characters.")]
        [MaxLength(20, ErrorMessage = "Tag can have up to 20 characters.")]
        [RegularExpression(@"^#[a-zA-Z]+$", ErrorMessage = "Hashtag format only.")]
        public string Tag { get; set; }

        [Display(GroupName = "Articles with this hashtag")]
        public IEnumerable<string> ArticlesWithHashtag { get; set; }
    }
}