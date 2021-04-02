﻿using eRentSolution.Data.Entities;
using eRentSolution.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Status).IsRequired().HasDefaultValue(Status.Active);

            builder.HasOne(x => x.Person).WithOne(x => x.AppUser).HasForeignKey<Person>(x => x.UserId);
        }
    }
}
