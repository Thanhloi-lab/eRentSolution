﻿using eRentSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Configurations
{
    public class CensorConfiguration : IEntityTypeConfiguration<Censor>
    {
        public void Configure(EntityTypeBuilder<Censor> builder)
        {
            builder.ToTable("Censors");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.Product).WithMany(x => x.Censors).HasForeignKey(x => x.ProductId);
            builder.HasOne(x => x.AdminAction).WithMany(x => x.Censors).HasForeignKey(x => x.ActionId);
            builder.HasOne(x => x.AppUser).WithMany(x => x.Censors).HasForeignKey(x => x.UserId);
        }
    }
}
