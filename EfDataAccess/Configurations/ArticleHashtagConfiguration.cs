using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfDataAccess.Configurations
{
    public class ArticleHashtagConfiguration : IEntityTypeConfiguration<ArticleHashtag>
    {
        public void Configure(EntityTypeBuilder<ArticleHashtag> builder)
        {
            builder.HasKey(ah => new { ah.ArticleId, ah.HashtagId });
        }
    }
}