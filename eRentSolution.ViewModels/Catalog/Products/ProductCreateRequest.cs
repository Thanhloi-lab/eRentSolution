
using eRentSolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string SeoAlias { get; set; }
        public string SubProductName { get; set; }
        public string Address { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
