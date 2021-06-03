
using eRentSolution.Data.Entities;
using eRentSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Display(Name = "Giá")]
        public decimal Price { set; get; }
        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { set; get; }
        [Display(Name = "Tồn kho")]
        public int Stock { set; get; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Chi tiết")]
        public string Detail { set; get; }
        [Display(Name = "Mô tả - tìm kiếm")]
        public string SeoDescription { set; get; }
        [Display(Name = "Tiêu đề - tìm kiếm")]
        public string SeoTitle { set; get; }
        [Display(Name = "Từ khóa - tìm kiếm")]
        public string SeoAlias { get; set; }
        [Display(Name = "Tên chỉ tiết sản phẩm")]
        public string SubProductName { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Chiều rộng")]
        public int Width { get; set; }
        [Display(Name = "Chiều dài")]
        public int Length { get; set; }
        [Display(Name = "Ảnh sản phẩm")]
        public IFormFile ThumbnailImage { get; set; }
        public List<SelectItem> Categories { get; set; } = new List<SelectItem>();
    }
}
