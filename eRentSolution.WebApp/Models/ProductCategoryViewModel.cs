using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;

namespace eRentSolution.WebApp.Models
{
    public class ProductCategoryViewModel
    {
        public PagedResult<ProductViewModel> Products { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
