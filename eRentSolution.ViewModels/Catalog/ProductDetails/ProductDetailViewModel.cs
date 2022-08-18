using eRentSolution.ViewModels.Catalog.ProductImages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductDetails
{
    public class ProductDetailViewModel
    {
        [Display(Name = "Mã chi tiết sản phẩm")]
        public int Id { set; get; }
        [Display(Name = "Tên loại sản phẩm")]
        public string ProductDetailName { set; get; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Giá")]
        public decimal Price { set; get; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { set; get; }
        [Display(Name = "Tồn kho")]
        public int Stock { set; get; }
        [Display(Name = "Chiều rộng")]
        public int Width { set; get; }
        [Display(Name = "Chi tiết")]
        public string Detail { set; get; }
        [Display(Name = "Chiều dài")]
        public int Length { set; get; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }
        [Display(Name = "Hình ảnh")]
        public List<ProductImageViewModel> Images { set; get; } = new List<ProductImageViewModel>();
        [Display(Name = "Sản phẩm đại diện")]
        public bool IsThumbnail { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }
    }
}
