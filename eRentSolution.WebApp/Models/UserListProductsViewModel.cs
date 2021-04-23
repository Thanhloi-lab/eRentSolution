using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.WebApp.Models
{
    public class UserListProductsViewModel
    {
        public PagedResult<ProductViewModel> Products { get; set; }
        public UserViewModel Owner { get; set; }
    }
}
