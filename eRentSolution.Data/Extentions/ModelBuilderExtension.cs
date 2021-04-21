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
               new AppConfig() { Key = "HomeTitle", Value = "This is home page of eRentSolution" },
               new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of eRentSolution" },
               new AppConfig() { Key = "HomeDescription", Value = "This is description of eRentSolution" }
               );
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Status.Active,
                    Name = "HomeStay",
                    SeoAlias = "homestay",
                    SeoDescription = "Loại hình nhà cho thuê và ở chung với chủ nhà.",
                    SeoTitle = "Nhà cho thuê ở cùng chủ hộ"
                },
                 new Category()
                 {
                     Id = 2,
                     IsShowOnHome = true,
                     ParentId = null,
                     SortOrder = 2,
                     Status = Status.Active,
                     Name = "Khách sạn",
                     SeoAlias = "khach-san",
                     SeoDescription = "Cho thuê, mướn phòng khách sạn",
                     SeoTitle = "Khách sạn"
                 });

            
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 }
                );
            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DE");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DD");
            var roleId1 = new Guid("E4DF483B-524D-467B-B6F4-2EE002742987");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "Admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            },
            new AppRole
            {
                Id = roleId1,
                Name = "UserAdmin",
                NormalizedName = "useradmin",
                Description = "User admin role"
            });
            DateTime now = DateTime.UtcNow;
            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "thanhloi",
                NormalizedUserName = "thanhloi",
                Email = "caothanhloi@gmail.com",
                NormalizedEmail = "caothanhloi@gmail.com",
                EmailConfirmed = true,
                DateChangePassword = now,
                PasswordHash = hasher.HashPassword(null, "123456aS`" + now),
                SecurityStamp = string.Empty,
                Status = Status.Active,
                
            });

            modelBuilder.Entity<UserInfo>().HasData(
           new UserInfo()
           {
               UserId = adminId,
               FirstName = "Lợi",
               LastName = "Cao Thành",
               Dob = new DateTime(2000, 01, 31),
           });

            modelBuilder.Entity<Product>().HasData(
           new Product()
           {
               Id = 1,
               DateCreated = DateTime.UtcNow,
               ViewCount = 0,
               Name = "HomeStay Thanh Loi",
               SeoAlias = "HomeStay-thanh-loi",
               SeoDescription = "HomeStay-thanh-loi",
               SeoTitle = "HomeStay-thanh-loi",
               Details = "HomeStay Thanh Loi rộng 1m dài 2m sâu 3m",
               Description = "HomeStay Thanh Loi tại pờ tít",
               IsFeatured = Status.InActive,
               Address = "TP.HCM-Hóc Môn-Xã Tân Thới Nhì-Ấp Dân Thắng 1, 77/3"
           }) ;
            modelBuilder.Entity<ProductDetail>().HasData(
           new ProductDetail()
           {
               Id = 1,
               DateCreated = DateTime.UtcNow,
               OriginalPrice = 100000,
               Price = 200000,
               Stock = 0,
               Name = "Phòng 1 chổ nằm",
               ProductId = 1,
           });
            modelBuilder.Entity<UserAction>().HasData(
           new UserAction()
           {
               Id = 1,
               ActionName = "Tạo sản phẩm"
           },
           new UserAction()
           {
               Id = 2,
               ActionName = "Chỉnh sửa sản phẩm"
           }, 
           new UserAction()
           {
               Id = 3,
               ActionName = "Ẩn sản phẩm"
           },
           new UserAction()
           {
               Id = 4,
               ActionName = "Chỉnh sửa tồn kho"
           },
           new UserAction()
           {
               Id = 5,
               ActionName = "Chỉnh sửa giá"
           },
           new UserAction()
           {
               Id = 6,
               ActionName = "Ẩn sản phẩm trình chiếu"
           },
           new UserAction()
           {
               Id = 7,
               ActionName = "Chỉnh sửa sản phẩm trình chiếu"
           },
           new UserAction()
           {
               Id = 8,
               ActionName = "Tạo sản phẩm trình chiếu"
           },
           new UserAction()
           {
               Id = 9,
               ActionName = "Ẩn sản phẩm nổi bật"
           },
           new UserAction()
           {
               Id = 10,
               ActionName = "Tạo sản phẩm nổi bật"
           },
           new UserAction()
           {
               Id = 11,
               ActionName = "Xóa sản phẩm trình chiếu"
           },
           new UserAction()
           {
               Id = 12,
               ActionName = "Hiện sản phẩm nổi bật"
           },
           new UserAction()
           {
               Id = 13,
               ActionName = "Xóa sản phẩm nổi bật"
           },
           new UserAction()
           {
               Id = 14,
               ActionName = "Hiện sản phẩm trình chiếu"
           },
           new UserAction()
           {
               Id = 15,
               ActionName = "Hiện sản phẩm"
           }
           );
            modelBuilder.Entity<Censor>().HasData(
           new Censor()
           {
               ActionId = 1,
               Date = DateTime.UtcNow,
               UserInfoId = adminId,
               ProductId = 1,
               Id = 1
           });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}
