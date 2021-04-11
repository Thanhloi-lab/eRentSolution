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
            public const string HideProduct = "HideProduct";
            public const string UpdateProduct = "UpdateProduct";
            public const string UpdatePriceProduct = "UpdatePriceProduct";
            public const string UpdateStockProduct = "UpdateStockProduct";
            public const string CreateProduct = "CreateProduct";
        }
    }
}
