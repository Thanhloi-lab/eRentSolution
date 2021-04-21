using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductDetails
{
    public class ProductDetailViewModel
    {
        [Display(Name = "Mã loại sản phẩm")]
        public int Id { set; get; }
        [Display(Name = "Tên loại sản phẩm")]
        public string ProductDetailName { set; get; }
        [Display(Name = "Giá")]
        public decimal Price { set; get; }
        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { set; get; }
        [Display(Name = "Tồn kho")]
        public int Stock { set; get; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }
        [Display(Name = "Sản phẩm đại diện")]
        public bool IsThumbnail { get; set; }
    }
}
