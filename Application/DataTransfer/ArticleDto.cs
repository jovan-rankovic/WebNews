using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransfer
{
    public class ArticleDto : BaseDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must contain at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Title can have up to 50 characters.")]
        public string Title { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Content must contain at least 3 characters.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [FileExtensions(ErrorMessage = "Invalid extension. (allowed: jpg, jpeg, png)", Extensions = "jpg,jpeg,png")]
        public string Image { get; set; }

        [Display(Name = "Date posted")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy. HH:mm}")]
        public System.DateTime CreatedAt { get; set; }

        public int AuthorId { get; set; }
        public string Author { get; set; }

        public IEnumerable<string> Comments { get; set; }

        [Display(GroupName = "Categories")]
        public IEnumerable<string> CategoriesForArticle { get; set; }

        [Display(GroupName = "Hashtags")]
        public IEnumerable<string> HashtagsForArticle { get; set; }
    }
}