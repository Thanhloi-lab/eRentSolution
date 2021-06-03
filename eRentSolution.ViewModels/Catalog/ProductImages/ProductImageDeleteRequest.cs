using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageDeleteRequest
    {
        [Display(Name = "Mã hình ảnh")]
        public int ImageId { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }
        [Display(Name = "Mã chi tiết sản phẩm")]
        public int ProductDetailId { get; set; }
    }
}
