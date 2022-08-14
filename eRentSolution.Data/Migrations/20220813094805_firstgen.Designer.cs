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
    [Migration("20220813094805_firstgen")]
    partial class firstgen
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
                            ConcurrencyStamp = "0662406b-9145-4a0b-8530-52ade85f6a34",
                            Description = "Administrator role",
                            Name = "Admin",
                            NormalizedName = "admin"
                        },
                        new
                        {
                            Id = new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                            ConcurrencyStamp = "681d57ac-da00-4b3f-bc3b-38c2b42c1451",
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

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

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
                        .HasDefaultValue(2);

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
                            ConcurrencyStamp = "c3205165-8282-4e59-96e5-66722162080c",
                            DateChangePassword = new DateTime(2022, 8, 13, 9, 48, 4, 799, DateTimeKind.Utc).AddTicks(2624),
                            Dob = new DateTime(2000, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "caothanhloi@gmail.com",
                            EmailConfirmed = true,
                            FirstName = "Lợi",
                            LastName = "Cao Thành",
                            LockoutEnabled = false,
                            NormalizedEmail = "caothanhloi@gmail.com",
                            NormalizedUserName = "thanhloi",
                            PasswordHash = "AQAAAAEAACcQAAAAEKejfrxhhzrjh78Y1xAhX1W2T2APENUA8voYfq5A5CHUFzPz2qXkc+tJD8G2oDJ3qA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "",
                            Status = 2,
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

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagePath")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("ImageSize")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("SeoAlias")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SeoDescription")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("SeoTitle")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateCreate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImagePath = "default_category.jpg",
                            ImageSize = 3021L,
                            Name = "HomeStay",
                            SeoAlias = "homestay",
                            SeoDescription = "Loại hình nhà cho thuê và ở chung với chủ nhà.",
                            SeoTitle = "Nhà cho thuê ở cùng chủ hộ",
                            Status = 2
                        },
                        new
                        {
                            Id = 2,
                            DateCreate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImagePath = "default_category.jpg",
                            ImageSize = 3021L,
                            Name = "Khách sạn",
                            SeoAlias = "khach-san",
                            SeoDescription = "Cho thuê, mướn phòng khách sạn",
                            SeoTitle = "Khách sạn",
                            Status = 2
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

                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("NewsId");

                    b.HasIndex("UserId");

                    b.ToTable("Censors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActionId = 1,
                            Date = new DateTime(2022, 8, 13, 9, 48, 4, 824, DateTimeKind.Utc).AddTicks(3020),
                            NewsId = 1,
                            UserId = new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd")
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

            modelBuilder.Entity("eRentSolution.Data.Entities.News", b =>
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
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SeoAlias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeoDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeoTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int")
                        .HasDefaultValue(4);

                    b.Property<int>("ViewCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("StatusId")
                        .IsUnique();

                    b.ToTable("News");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "TP.HCM-Hóc Môn-Xã Tân Thới Nhì-Ấp Dân Thắng 1, 77/3",
                            DateCreated = new DateTime(2022, 8, 13, 9, 48, 4, 823, DateTimeKind.Utc).AddTicks(797),
                            Description = "HomeStay Thanh Loi tại pờ tít",
                            IsFeatured = 1,
                            Name = "HomeStay Thanh Loi",
                            SeoAlias = "HomeStay-thanh-loi",
                            SeoDescription = "HomeStay-thanh-loi",
                            SeoTitle = "HomeStay-thanh-loi",
                            StatusId = 2,
                            ViewCount = 0
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.NewsInCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId", "NewsId");

                    b.HasIndex("NewsId");

                    b.ToTable("NewsInCategories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            NewsId = 1
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.NewsStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("ProductStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            StatusName = "Khóa hoạt động"
                        },
                        new
                        {
                            Id = 2,
                            StatusName = "Hoạt động"
                        },
                        new
                        {
                            Id = 3,
                            StatusName = "Chờ duyệt"
                        },
                        new
                        {
                            Id = 4,
                            StatusName = "Ẩn"
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Product", b =>
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

                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.Property<decimal>("OriginalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.Property<int>("Stock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NewsId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateCreated = new DateTime(2022, 8, 13, 9, 48, 4, 823, DateTimeKind.Utc).AddTicks(6545),
                            Detail = "2 nvs .....",
                            Length = 10,
                            Name = "Phòng 1 chổ nằm",
                            NewsId = 1,
                            OriginalPrice = 100000m,
                            Price = 200000m,
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

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
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

                    b.Property<int>("NewsId")
                        .HasColumnType("int");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("NewsId");

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
                            ActionName = "Chờ duyệt"
                        },
                        new
                        {
                            Id = 5,
                            ActionName = "Hoạt động"
                        },
                        new
                        {
                            Id = 6,
                            ActionName = "Khóa hoạt động"
                        },
                        new
                        {
                            Id = 7,
                            ActionName = "Tạo sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 8,
                            ActionName = "Chỉnh sửa sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 9,
                            ActionName = "Ẩn sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 10,
                            ActionName = "Hiện sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 11,
                            ActionName = "Xóa sản phẩm trình chiếu"
                        },
                        new
                        {
                            Id = 12,
                            ActionName = "Tạo sản phẩm nổi bật"
                        },
                        new
                        {
                            Id = 13,
                            ActionName = "Xóa sản phẩm nổi bật"
                        },
                        new
                        {
                            Id = 14,
                            ActionName = "Ẩn sản phẩm nổi bật"
                        },
                        new
                        {
                            Id = 15,
                            ActionName = "Hiện sản phẩm nổi bật"
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
                        },
                        new
                        {
                            Id = 22,
                            ActionName = "Chỉnh sửa tồn kho"
                        },
                        new
                        {
                            Id = 23,
                            ActionName = "Chỉnh sửa giá"
                        });
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Censor", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.UserAction", "AdminAction")
                        .WithMany("Censors")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eRentSolution.Data.Entities.News", "News")
                        .WithMany("Censors")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eRentSolution.Data.Entities.AppUser", "User")
                        .WithMany("Censors")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdminAction");

                    b.Navigation("News");

                    b.Navigation("User");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.News", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.NewsStatus", "NewsStatus")
                        .WithOne("News")
                        .HasForeignKey("eRentSolution.Data.Entities.News", "StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NewsStatus");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.NewsInCategory", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.Category", "Category")
                        .WithMany("NewsInCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eRentSolution.Data.Entities.News", "News")
                        .WithMany("NewsInCategories")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("News");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Product", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.News", "News")
                        .WithMany("Products")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("News");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.ProductImage", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Slide", b =>
                {
                    b.HasOne("eRentSolution.Data.Entities.News", "News")
                        .WithMany("Slides")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("News");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.AppUser", b =>
                {
                    b.Navigation("Censors");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Category", b =>
                {
                    b.Navigation("NewsInCategories");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.News", b =>
                {
                    b.Navigation("Censors");

                    b.Navigation("NewsInCategories");

                    b.Navigation("Products");

                    b.Navigation("Slides");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.NewsStatus", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.Product", b =>
                {
                    b.Navigation("ProductImages");
                });

            modelBuilder.Entity("eRentSolution.Data.Entities.UserAction", b =>
                {
                    b.Navigation("Censors");
                });
#pragma warning restore 612, 618
        }
    }
}