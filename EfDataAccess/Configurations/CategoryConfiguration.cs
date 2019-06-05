using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfDataAccess.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.UpdatedAt).HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.Name).HasMaxLength(30).IsRequired();

            builder.HasIndex(c => c.Name).IsUnique();

            builder.HasMany(c => c.CategoryArticles)
                .WithOne(ac => ac.Category)
                .HasForeignKey(ac => ac.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}