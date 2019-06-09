namespace Domain
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }

        public User User { get; set; }
        public Article Article { get; set; }
    }
}