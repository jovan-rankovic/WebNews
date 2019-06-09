using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfDataAccess.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.UpdatedAt).HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.Text).HasColumnType("text").IsRequired();

            builder.HasOne(c => c.User).WithMany(u => u.Comments);
            builder.HasOne(c => c.Article).WithMany(a => a.Comments);
        }
    }
}