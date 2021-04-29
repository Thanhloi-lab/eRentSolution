﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Utilities.Constants
{
    public class SystemConstant
    {
        public const string MainConnectionString = "eRentSolutionDatabase";
        public const string BackendApiProductUrl = "localhost:5003/product/";
        public const string AdminBackendApiProductUrl = "localhost:5002/product/";
        public const string DefaultAvatar = "default_avatar.png";
        public const string DefaultCategory = "default_category.jpg";
        public const long DefaultAvatarSize = 15131;
        public const long DefaultCategorySize = 3021;
        public class AppSettings
        {
            public const string TokenAdmin = "Token";
            public const string TokenWebApp = "TokenWebApp";
            public const string CurrentUserId = "CurrentUserId";
            public const string AdminRole = "Admin";
            public const string UserAdminRole = "UserAdmin";
            public const string PasswordReseted = "123456aS`";
            public const string NameIdentifierClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        }

        public class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 4;
            public const int NumberOfLastestProducts = 6;
        }
        public class ActionSettings
        {
            public const string HideProduct = "Ẩn sản phẩm";
            public const string UpdateProduct = "Chỉnh sửa sản phẩm";
            public const string CreateProduct = "Tạo sản phẩm";
            public const string ShowProduct = "Hiện sản phẩm";
            public const string InActiveProduct = "Không cho phép hiển thị";
            public const string ActiveProduct = "Cho phép hiển thị";

            public const string UpdateProductDetail = "Chỉnh sửa chi tiết sản phẩm";
            public const string CreateProductDetail = "Thêm chi tiết sản phẩm";
            public const string UpdatePriceProduct = "Chỉnh sửa giá";
            public const string UpdateStockProduct = "Chỉnh sửa tồn kho";
            public const string DeleteDetail = "Xóa chi tiết sản phẩm";

            public const string HideSlide = "Ẩn sản phẩm trình chiếu";
            public const string UpdateSlide = "Chỉnh sửa sản phẩm trình chiếu";
            public const string CreateSlide = "Tạo sản phẩm trình chiếu";
            public const string DeleteSlide = "Xóa sản phẩm trình chiếu";
            public const string ShowSlide = "Hiện sản phẩm trình chiếu";

            public const string CreateFeatureProduct = "Tạo sản phẩm nổi bật";
            public const string DeleteFeatureProduct = "Xóa sản phẩm nổi bật";

            public const string UpdateImage = "Chỉnh sửa hình ảnh sản phẩm";
            public const string CreateImage = "Thêm hình ảnh sản phẩm";
            public const string DeleteImage = "Xóa hình ảnh sản phẩm";
        }
    }
}
