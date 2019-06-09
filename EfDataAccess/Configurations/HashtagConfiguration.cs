using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfDataAccess.Configurations
{
    public class HashtagConfiguration : IEntityTypeConfiguration<Hashtag>
    {
        public void Configure(EntityTypeBuilder<Hashtag> builder)
        {
            builder.Property(h => h.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(h => h.UpdatedAt).HasDefaultValueSql("GETDATE()");

            builder.Property(h => h.Tag).HasMaxLength(20).IsRequired();

            builder.HasIndex(h => h.Tag).IsUnique();

            builder.HasMany(h => h.HashtagArticles)
                .WithOne(ah => ah.Hashtag)
                .HasForeignKey(ah => ah.HashtagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
