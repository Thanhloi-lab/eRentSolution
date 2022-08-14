using eRentSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Configurations
{
    public class ProductStatusConfiguration : IEntityTypeConfiguration<NewsStatus>
    {
        public void Configure(EntityTypeBuilder<NewsStatus> builder)
        {
            builder.ToTable("ProductStatuses");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.StatusName).IsRequired().HasMaxLength(200);

        }

    }
}
