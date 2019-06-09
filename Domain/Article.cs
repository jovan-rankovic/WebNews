using System.Collections.Generic;

namespace Domain
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ArticleCategory> ArticleCategories { get; set; }
        public ICollection<ArticleHashtag> ArticleHashtags { get; set; }
    }
}