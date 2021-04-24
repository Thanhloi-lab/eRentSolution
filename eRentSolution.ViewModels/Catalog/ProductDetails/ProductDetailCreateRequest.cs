using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductDetails
{
    public class ProductDetailCreateRequest
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { set; get; }
        [Display(Name = "Tên loại sản phẩm")]
        public string ProductDetailName { set; get; }
        [Display(Name = "Giá")]
        public decimal Price { set; get; }
        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { set; get; }
        [Display(Name = "Tồn kho")]
        public int Stock { set; get; }
        [Display(Name = "Chi tiết sản phẩm")]
        public string Detail { set; get; }
        [Display(Name = "Chiều dài")]
        public int Length { set; get; }
        [Display(Name = "Chiều rộng")]
        public int Width { set; get; }
        public IFormFile Image { get; set; }
    }
}
