using eRentSolution.Data.Entities;
using eRentSolution.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.OriginalPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Stock).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Status).IsRequired().HasDefaultValue(Status.Active);
            builder.Property(x => x.Detail).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Width).IsRequired();
            builder.Property(x => x.Length).IsRequired();

            builder.HasOne(x => x.News).WithMany(x => x.Products).HasForeignKey(x => x.NewsId);
        }
    }
}
