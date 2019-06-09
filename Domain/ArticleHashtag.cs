namespace Domain
{
    public class ArticleHashtag
    {
        public int ArticleId { get; set; }
        public int HashtagId { get; set; }

        public Article Article { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}