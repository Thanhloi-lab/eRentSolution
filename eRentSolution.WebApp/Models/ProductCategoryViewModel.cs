using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using System;

namespace eRentSolution.WebApp.Models
{
    public class ProductCategoryViewModel
    {
        public PagedResult<ProductViewModel> Products { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
