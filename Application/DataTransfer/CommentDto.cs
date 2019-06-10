using System.ComponentModel.DataAnnotations;

namespace Application.DataTransfer
{
    public class CommentDto : BaseDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Text must contain at least 3 characters.")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public int UserId { get; set; }

        public int ArticleId { get; set; }

        public string User { get; set; }

        public string Article { get; set; }
    }
}