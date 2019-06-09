using System.Collections.Generic;

namespace Domain
{
    public class Hashtag : BaseEntity
    {
        public string Tag { get; set; }

        public ICollection<ArticleHashtag> HashtagArticles { get; set; }
    }
}