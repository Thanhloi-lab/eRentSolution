using eRentSolution.ViewModels.Catalog.Categories;
using eRentSolution.ViewModels.Catalog.Products;
using System.Collections.Generic;

namespace eRentSolution.WebApp.Models
{
    public class ProductDetailViewModels
    {
        public List<CategoryViewModel> Categories { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
