using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductUpdatePriceRequest
    {
        [Display(Name = "Mã chi tiết sản phẩm")]
        public int ProductDetailId { get; set; }
        [Display(Name = "Giá mới")]
        public decimal NewPrice { get; set; }
    }
}
