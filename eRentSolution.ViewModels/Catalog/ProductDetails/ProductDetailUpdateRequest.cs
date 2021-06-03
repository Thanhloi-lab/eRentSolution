using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductDetails
{
    public class ProductDetailUpdateRequest
    {
        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }
        [Display(Name = "Tên chi tiết sản phẩm")]
        public string ProductDetailName { set; get; }
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }
        [Display(Name = "Chiều rộng")]
        public int Width { get; set; }
        [Display(Name = "Chiều dài")]
        public int Length { get; set; }
        [Display(Name = "Giá")]
        public decimal Price { get; set; }
        [Display(Name = "Giá gốc")]
        public decimal OriginalPrice { get; set; }
        [Display(Name = "Tồn kho")]
        public int Stock { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }
    }
}
