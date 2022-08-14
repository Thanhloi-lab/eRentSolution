using eRentSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Configurations
{
    public class NewsInCategoryConfiguration : IEntityTypeConfiguration<NewsInCategory>
    {
        public void Configure(EntityTypeBuilder<NewsInCategory> builder)
        {
            builder.HasKey(t => new { t.CategoryId, t.NewsId });

            builder.ToTable("NewsInCategories");

            builder.HasOne(t => t.News).WithMany(pc => pc.NewsInCategories)
                .HasForeignKey(pc => pc.NewsId);

            builder.HasOne(t => t.Category).WithMany(pc => pc.NewsInCategories)
              .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
