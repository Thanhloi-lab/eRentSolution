using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.System.Users;
using System.Collections.Generic;

namespace eRentSolution.WebApp.Models
{
    public class ProductDetailViewModels
    {
        public List<CategoryViewModel> Categories { get; set; }
        public ProductViewModel Product { get; set; }
        public UserViewModel Owner { get; set; }
    }
}
