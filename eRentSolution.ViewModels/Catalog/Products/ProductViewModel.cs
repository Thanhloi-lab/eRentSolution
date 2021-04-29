using eRentSolution.Data.Enums;
using eRentSolution.ViewModels.Catalog.ProductDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        //Entity Product
        [Display(Name = "Mã sản phẩm")]
        public int Id { set; get; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }
        [Display(Name = "Lượt xem")]
        public int ViewCount { set; get; }
        [Display(Name = "Tồn kho")]
        public int Stock { set; get; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }
        [Display(Name = "Sản phẩm nổi bật")]
        public Status? IsFeatured { get; set; }
        [Display(Name = "Hình ảnh đại diện")]
        public string ThumbnailImage { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        // Entity ProductDetail

        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Chi tiết-tìm kiếm")]
        public string SeoDescription { set; get; }
        [Display(Name = "Tiêu đề-tìm kiếm")]
        public string SeoTitle { set; get; }
        [Display(Name = "Từ khóa")]
        public string SeoAlias { get; set; }
        [Display(Name = "Trạng thái")]
        public int StatusId { get; set; }
        [Display(Name = "Trạng thái")]
        public string Status { get; set; }
        [Display(Name = "Những danh mục")]
        public List<string> Categories { get; set; } = new List<string>();
        public List<ProductDetailViewModel> ProductDetailViewModels { get; set; } = new List<ProductDetailViewModel>();
    }
}
