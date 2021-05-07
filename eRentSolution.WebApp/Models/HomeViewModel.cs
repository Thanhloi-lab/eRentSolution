using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using eRentSolution.ViewModels.Utilities.Slides;
using System.Collections.Generic;

namespace eRentSolution.WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideViewModel> Slides { get; set; }
        public PagedResult<ProductViewModel> FeaturedProducts { get; set; }
        public PagedResult<ProductViewModel> PageProducts { get; set; }
    }
}
