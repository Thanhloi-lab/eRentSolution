﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eRentSolution.Data.EF;

namespace eRentSolution.Data.Migrations
{
    [DbContext(typeof(eRentDbContext))]
    [Migration("20210424091207_Last_version1.0")]
    partial class Last_version10
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AppRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AppUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("AppUserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("AppUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                            RoleId = new Guid("8d04dce2-969a-435d-bba4-df3f325983de")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("AppUserTokens");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.AppConfig", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.ToTable("AppConfigs");

                    b.HasData(
                        new
                        {
                            Key = "HomeTitle",
                            Value = "This is home page of eRentSolution"
                        },
                        new
                        {
                            Key = "HomeKeyword",
                            Value = "This is keyword of eRentSolution"
                        },
                        new
                        {
                            Key = "HomeDescription",
                            Value = "This is description of eRentSolution"
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.AppRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppRoles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                            ConcurrencyStamp = "08a94b04-540e-4a19-92c6-2519f486a2de",
                            Description = "Administrator role",
                            Name = "Admin",
                            NormalizedName = "admin"
                        },
                        new
                        {
                            Id = new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                            ConcurrencyStamp = "6db6b96b-d4be-4a28-9dc8-0f7252217c72",
                            Description = "User admin role",
                            Name = "UserAdmin",
                            NormalizedName = "useradmin"
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AvatarFilePath")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("AvatarFileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateChangePassword")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                            AccessFailedCount = 0,
                            AvatarFilePath = "default_avatar.png",
                            AvatarFileSize = 15131L,
                            ConcurrencyStamp = "d1a0a6a4-02d6-4bf0-8730-8cfc0b59fd43",
                            DateChangePassword = new DateTime(2021, 4, 24, 9, 12, 6, 322, DateTimeKind.Utc).AddTicks(3538),
                            Email = "caothanhloi@gmail.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "caothanhloi@gmail.com",
                            NormalizedUserName = "thanhloi",
                            PasswordHash = "AQAAAAEAACcQAAAAENXCG0gL/n0YsPYdpe6Pn4WSSoDFIPjfvA8adQFht99lvRbiducpbL54sp31yWyTpw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "",
                            Status = 1,
                            TwoFactorEnabled = false,
                            UserName = "thanhloi"
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImagePath")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("ImageSize")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsShowOnHome")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("SeoAlias")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SeoDescription")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("SeoTitle")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ImagePath = "default_category.jpg",
                            ImageSize = 3021L,
                            IsShowOnHome = true,
                            Name = "HomeStay",
                            SeoAlias = "homestay",
                            SeoDescription = "Loại hình nhà cho thuê và ở chung với chủ nhà.",
                            SeoTitle = "Nhà cho thuê ở cùng chủ hộ",
                            SortOrder = 1,
                            Status = 1
                        },
                        new
                        {
                            Id = 2,
                            ImagePath = "default_category.jpg",
                            ImageSize = 3021L,
                            IsShowOnHome = true,
                            Name = "Khách sạn",
                            SeoAlias = "khach-san",
                            SeoDescription = "Cho thuê, mướn phòng khách sạn",
                            SeoTitle = "Khách sạn",
                            SortOrder = 2,
                            Status = 1
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Censor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserInfoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserInfoId");

                    b.ToTable("Censors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActionId = 1,
                            Date = new DateTime(2021, 4, 24, 9, 12, 6, 352, DateTimeKind.Utc).AddTicks(2211),
                            ProductId = 1,
                            UserInfoId = new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd")
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("IsFeatured")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SeoAlias")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SeoDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeoTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("ViewCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "TP.HCM-Hóc Môn-Xã Tân Thới Nhì-Ấp Dân Thắng 1, 77/3",
                            DateCreated = new DateTime(2021, 4, 24, 9, 12, 6, 350, DateTimeKind.Utc).AddTicks(4943),
                            Description = "HomeStay Thanh Loi tại pờ tít",
                            IsFeatured = 0,
                            Name = "HomeStay Thanh Loi",
                            SeoAlias = "HomeStay-thanh-loi",
                            SeoDescription = "HomeStay-thanh-loi",
                            SeoTitle = "HomeStay-thanh-loi",
                            Status = 0,
                            ViewCount = 0
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.ProductDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("OriginalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("Stock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductDetails");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateCreated = new DateTime(2021, 4, 24, 9, 12, 6, 351, DateTimeKind.Utc).AddTicks(2897),
                            Detail = "2 nvs .....",
                            Length = 10,
                            Name = "Phòng 1 chổ nằm",
                            OriginalPrice = 100000m,
                            Price = 200000m,
                            ProductId = 1,
                            Status = 0,
                            Stock = 0,
                            Width = 5
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Caption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsDefault")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("ProductDetailId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductDetailId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.ProductInCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductInCategories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            ProductId = 1
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ApplyForAll")
                        .HasColumnType("bit");

                    b.Property<decimal?>("DiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("DiscountPercent")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductCategoryIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Slide", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Slides");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.UserAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("UserActions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActionName = "Tạo sản phẩm"
                        },
                        new
                        {
                            Id = 2,
                            ActionName = "Chỉnh sửa sản phẩm"
                        },
                        new
                        {
                            Id = 3,
                            ActionName = "Ẩn sản phẩm"
                        },
                        new
                        {
                            Id = 4,
                            ActionName = "Chỉnh sửa tồn kho"
                        },
                        new
                        {
                            Id = 5,
                            ActionName = "Chỉnh sửa giá"
                        },
                        new
                        {
                            Id = 6,
                            ActionName = "Ẩn sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 7,
                            ActionName = "Chỉnh sửa sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 8,
                            ActionName = "Tạo sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 9,
                            ActionName = "Ẩn sản phẩm nổi bật"
                        },
                        new
                        {
                            Id = 10,
                            ActionName = "Tạo sản phẩm nổi bật"
                        },
                        new
                        {
                            Id = 11,
                            ActionName = "Xóa sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 12,
                            ActionName = "Hiện sản phẩm nổi bật"
                        },
                        new
                        {
                            Id = 13,
                            ActionName = "Xóa sản phẩm nổi bật"
                        },
                        new
                        {
                            Id = 14,
                            ActionName = "Hiện sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 15,
                            ActionName = "Hiện sản phẩm"
                        },
                        new
                        {
                            Id = 16,
                            ActionName = "Chỉnh sửa hình ảnh sản phẩm"
                        },
                        new
                        {
                            Id = 17,
                            ActionName = "Thêm hình ảnh sản phẩm"
                        },
                        new
                        {
                            Id = 18,
                            ActionName = "Xóa hình ảnh sản phẩm"
                        },
                        new
                        {
                            Id = 19,
                            ActionName = "Xóa chi tiết sản phẩm"
                        },
                        new
                        {
                            Id = 20,
                            ActionName = "Thêm chi tiết sản phẩm"
                        },
                        new
                        {
                            Id = 21,
                            ActionName = "Chỉnh sửa chi tiết sản phẩm"
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.UserInfo", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserId");

                    b.ToTable("UserInfos");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                            Dob = new DateTime(2000, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Lợi",
                            LastName = "Cao Thành"
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Censor", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.UserAction", "AdminAction")
                        .WithMany("Censors")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eRentSolution.Data.Entities.Product", "Product")
                        .WithMany("Censors")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eRentSolution.Data.Entities.UserInfo", "UserInfo")
                        .WithMany("Censors")
                        .HasForeignKey("UserInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdminAction");

                    b.Navigation("Product");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.ProductDetail", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.Product", "Product")
                        .WithMany("ProductDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.ProductImage", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.ProductDetail", "ProductDetail")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductDetail");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.ProductInCategory", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.Category", "Category")
                        .WithMany("ProductInCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eRentSolution.Data.Entities.Product", "Product")
                        .WithMany("ProductInCategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Slide", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.Product", "Product")
                        .WithMany("Slides")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.UserInfo", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.AppUser", "AppUser")
                        .WithOne("Person")
                        .HasForeignKey("eRentSolution.Data.Entities.UserInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.AppUser", b =>
                {
                    b.Navigation("Person");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Category", b =>
                {
                    b.Navigation("ProductInCategories");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Product", b =>
                {
                    b.Navigation("Censors");

                    b.Navigation("ProductDetails");

                    b.Navigation("ProductInCategories");

                    b.Navigation("Slides");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.ProductDetail", b =>
                {
                    b.Navigation("ProductImages");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.UserAction", b =>
                {
                    b.Navigation("Censors");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.UserInfo", b =>
                {
                    b.Navigation("Censors");
                });
#pragma warning restore 612, 618
        }
    }
}
