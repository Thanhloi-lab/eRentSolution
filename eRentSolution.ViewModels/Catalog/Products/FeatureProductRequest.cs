using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class FeatureProductRequest
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }
        [Display(Name = "Đường dẫn")]
        public string Url { get; set; }
    }
}
