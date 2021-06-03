using System;
using System.ComponentModel.DataAnnotations;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageViewModel
    {
        [Display(Name = "Mã hình ảnh")]
        public int Id { get; set; }
        [Display(Name = "Mã chi tiết sản phẩm")]
        public int ProductDetailId { get; set; }
        [Display(Name = "Đường dẫn")]
        public string ImagePath { get; set; }
        [Display(Name = "Chú thích")]
        public string Caption { get; set; }
        [Display(Name = "Mặc định")]
        public bool IsDefault { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Kích thướt hình ảnh")]
        public long FileSize { get; set; }
    }
}
