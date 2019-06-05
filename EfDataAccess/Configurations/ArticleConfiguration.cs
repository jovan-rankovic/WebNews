﻿using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfDataAccess.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(a => a.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(a => a.UpdatedAt).HasDefaultValueSql("GETDATE()");

            builder.Property(a => a.Title).HasMaxLength(50).IsRequired();
            builder.Property(a => a.Content).HasColumnType("text").IsRequired();
            builder.Property(a => a.ImagePath).HasMaxLength(255).IsRequired();

            builder.HasIndex(a => a.Title).IsUnique();

            builder.HasOne(a => a.User).WithMany(u => u.Articles);
            builder.HasMany(a => a.ArticleCategories)
                .WithOne(ac => ac.Article)
                .HasForeignKey(ac => ac.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}