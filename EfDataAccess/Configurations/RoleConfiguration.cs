using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EfDataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(r => r.UpdatedAt).HasDefaultValueSql("GETDATE()");

            builder.Property(r => r.Name).HasMaxLength(15).IsRequired();

            builder.HasIndex(r => r.Name).IsUnique();

            builder.HasMany(r => r.Users).WithOne(u => u.Role);
        }
    }
}