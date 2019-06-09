using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfDataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(u => u.UpdatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(u => u.RoleId).HasDefaultValue(2); // 1 reserved for admin

            builder.Property(u => u.FirstName).HasMaxLength(30).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(30).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(60).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(32).IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasOne(u => u.Role).WithMany(r => r.Users);
            builder.HasMany(u => u.Articles).WithOne(a => a.User);
            builder.HasMany(u => u.Comments).WithOne(c => c.User);
        }
    }
}