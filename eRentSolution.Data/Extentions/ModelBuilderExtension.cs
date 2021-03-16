using eRentSolution.Data.Entities;
using eRentSolution.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Extentions
{
    public static class ModelBuilderExtensions
    {
        public static void seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
               new AppConfig() { Key = "HomeTitle", Value = "This is home page of eShopSolution" },
               new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of eShopSolution" },
               new AppConfig() { Key = "HomeDescription", Value = "This is description of eShopSolution" }
               );
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Status.Active,
                    Name = "Áo nam",
                    SeoAlias = "ao-nam",
                    SeoDescription = "Sản phẩm áo thời trang nam",
                    SeoTitle = "Sản phẩm áo thời trang nam"
                },
                 new Category()
                 {
                     Id = 2,
                     IsShowOnHome = true,
                     ParentId = null,
                     SortOrder = 2,
                     Status = Status.Active,
                     Name = "Áo nữ",
                     SeoAlias = "ao-nu",
                     SeoDescription = "Sản phẩm áo thời trang nữ",
                     SeoTitle = "Sản phẩm áo thời trang nữ"
                 });

            modelBuilder.Entity<Product>().HasData(
           new Product()
           {
               Id = 1,
               DateCreated = DateTime.Now,
               OriginalPrice = 100000,
               Price = 200000,
               Stock = 0,
               ViewCount = 0,
               Name = "Áo sơ mi nam trắng Việt Tiến",
               SeoAlias = "ao-so-mi-nam-trang-viet-tien",
               SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
               SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
               Details = "Áo sơ mi nam trắng Việt Tiến",
               Description = "Áo sơ mi nam trắng Việt Tiến"
           });
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 }
                );
            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DE");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DD");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "caothanhloi@gmail.com",
                NormalizedEmail = "caothanhloi@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123456aS"),
                SecurityStamp = string.Empty,
                FirstName = "Lợi",
                LastName = "Cao Thành",
                Dob = new DateTime(2000, 01, 31)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}
