using eRentSolution.Data.Entities;
using eRentSolution.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.ToTable("News");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.ViewCount).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);
            builder.Property(x => x.IsFeatured).IsRequired().HasDefaultValue(Status.InActive);
            builder.Property(x => x.Address).HasMaxLength(300).IsRequired();
            builder.Property(x => x.StatusId).IsRequired().HasDefaultValue(Status.Private);

            builder.HasOne(x => x.NewsStatus).WithOne(x => x.News).HasForeignKey<News>(x => x.StatusId);
        }
    }
}
