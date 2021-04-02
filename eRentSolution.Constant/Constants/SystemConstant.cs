using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Utilities.Constants
{
    public class SystemConstant
    {
        public const string MainConnectionString = "eRentSolutionDatabase";
        public class AppSettings
        {
            public const string Token = "Token";
            public const string CurrentUserId = "CurrentUserId";
            public const string AdminRole = "Admin";
            public const string UserAdminRole = "UserAdmin";
        }

        public class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 4;
            public const int NumberOfLastestProducts = 6;
        }
        public class ActionSettings
        {
            public const string DeleteProduct = "DeleteProduct";
            public const string UpdateProduct = "UpdateProduct";
            public const string UpdatePriceProduct = "UpdatePriceProduct";
            public const string UpdateStockProduct = "UpdateStockProduct";
            public const string CreateProduct = "CreateProduct";
        }
    }
}
