using eRentSolution.Data.Entities;
using eRentSolution.Data.Enums;
using eRentSolution.Utilities.Constants;
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
                    ParentId = null,
                    Status = Status.Active,
                    Name = "HomeStay",
                    SeoAlias = "homestay",
                    SeoDescription = "Loại hình nhà cho thuê và ở chung với chủ nhà.",
                    SeoTitle = "Nhà cho thuê ở cùng chủ hộ",
                    ImagePath = "default_category.jpg",
                    ImageSize = 3021
                },
                 new Category()
                 {
                     Id = 2,
                     ParentId = null,
                     Status = Status.Active,
                     Name = "Khách sạn",
                     SeoAlias = "khach-san",
                     SeoDescription = "Cho thuê, mướn phòng khách sạn",
                     SeoTitle = "Khách sạn",
                     ImagePath = "default_category.jpg",
                     ImageSize = 3021
                 }) ;

            modelBuilder.Entity<NewsStatus>().HasData(
               new NewsStatus()
               {
                   Id = 1,
                   StatusName = "Khóa hoạt động"
               },
               new NewsStatus()
               {
                   Id = 2,
                   StatusName = "Hoạt động"
               },
               new NewsStatus()
               {
                   Id = 3,
                   StatusName = "Chờ duyệt"
               },
               new NewsStatus()
               {
                   Id = 4,
                   StatusName = "Ẩn"
               });

            modelBuilder.Entity<NewsInCategory>().HasData(
                new NewsInCategory() { NewsId = 1, CategoryId = 1 }
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
                PasswordHash = hasher.HashPassword(null, "123456aS`"),
                SecurityStamp = string.Empty,
                Status = Status.Active,
                AvatarFilePath = "default_avatar.png",
                AvatarFileSize = 15131,
                FirstName = "Lợi",
                LastName = "Cao Thành",
                Dob = new DateTime(2000, 01, 31),
            }) ;

            modelBuilder.Entity<News>().HasData(
           new News()
           {
               Id = 1,
               DateCreated = DateTime.UtcNow,
               ViewCount = 0,
               Name = "HomeStay Thanh Loi",
               SeoAlias = "HomeStay-thanh-loi",
               SeoDescription = "HomeStay-thanh-loi",
               SeoTitle = "HomeStay-thanh-loi",
               Description = "HomeStay Thanh Loi tại pờ tít",
               IsFeatured = Status.InActive,
               Address = "TP.HCM-Hóc Môn-Xã Tân Thới Nhì-Ấp Dân Thắng 1, 77/3",
               StatusId = 2,
           }) ;
            modelBuilder.Entity<Product>().HasData(
           new Product()
           {
               Id = 1,
               DateCreated = DateTime.UtcNow,
               OriginalPrice = 100000,
               Price = 200000,
               Stock = 0,
               Name = "Phòng 1 chổ nằm",
               NewsId = 1,
               Detail = "2 nvs .....",
               Width = 5,
               Length = 10
           }) ;
            modelBuilder.Entity<UserAction>().HasData(
            new UserAction()
            {
                Id = 1,
                ActionName = SystemConstant.ActionSettings.CreateProduct
            },
            new UserAction()
            {
                Id = 2,
                ActionName = SystemConstant.ActionSettings.UpdateProduct
            }, 
            new UserAction()
            {
                Id = 3,
                ActionName = SystemConstant.ActionSettings.HideProduct
            },
            new UserAction()
            {
                Id = 4,
                ActionName = SystemConstant.ActionSettings.ShowProduct
            },
            new UserAction()
            {
                Id = 5,
                ActionName = SystemConstant.ActionSettings.ActiveProduct
            },
            new UserAction()
            {
                Id = 6,
                ActionName = SystemConstant.ActionSettings.InActiveProduct
            },
            new UserAction()
            {
                Id = 7,
                ActionName = SystemConstant.ActionSettings.CreateSlide
            },
            new UserAction()
            {
                Id = 8,
                ActionName = SystemConstant.ActionSettings.UpdateSlide
            },
            new UserAction()
            {
                Id = 9,
                ActionName = SystemConstant.ActionSettings.HideSlide
            },
            new UserAction()
            {
                Id = 10,
                ActionName = SystemConstant.ActionSettings.ShowSlide
            },
            new UserAction()
            {
                Id = 11,
                ActionName = SystemConstant.ActionSettings.DeleteSlide
            },
            new UserAction()
            {
                Id = 12,
                ActionName = SystemConstant.ActionSettings.CreateFeatureProduct
            },
            new UserAction()
            {
                Id = 13,
                ActionName = SystemConstant.ActionSettings.DeleteFeatureProduct
            },
            new UserAction()
            {
                Id = 14,
                ActionName = "Ẩn sản phẩm nổi bật" 
            },
            new UserAction()
            {
                Id = 15,
                ActionName = "Hiện sản phẩm nổi bật" 
            },
            new UserAction()
            {
                Id = 16,
                ActionName = SystemConstant.ActionSettings.UpdateImage
            },
            new UserAction()
            {
                Id = 17,
                ActionName = SystemConstant.ActionSettings.CreateImage
            },
            new UserAction()
            {
                Id = 18,
                ActionName = SystemConstant.ActionSettings.DeleteImage
            },
            new UserAction()
            {
                Id = 19,
                ActionName = SystemConstant.ActionSettings.DeleteDetail
            } ,
            new UserAction()
            {
                Id = 20,
                ActionName = SystemConstant.ActionSettings.CreateProductDetail
            },
            new UserAction()
            {
                Id = 21,
                ActionName = SystemConstant.ActionSettings.UpdateProductDetail
            },
            new UserAction()
            {
                Id = 22,
                ActionName = SystemConstant.ActionSettings.UpdateStockProduct
            },
            new UserAction()
            {
                Id = 23,
                ActionName = SystemConstant.ActionSettings.UpdatePriceProduct
            });
            modelBuilder.Entity<Censor>().HasData(
           new Censor()
           {
               ActionId = 1,
               Date = DateTime.UtcNow,
               UserId = adminId,
               NewsId = 1,
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
