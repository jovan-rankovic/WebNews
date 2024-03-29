﻿using System.ComponentModel.DataAnnotations;

namespace Application.DataTransfer
{
    public class CommentDto : BaseDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Text must contain at least 3 characters.")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Display(Name = "Date posted")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy. HH:mm}")]
        public System.DateTime CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy. HH:mm}")]
        public System.DateTime UpdatedAt { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ArticleId { get; set; }

        public string User { get; set; }

        public string Article { get; set; }
    }
}