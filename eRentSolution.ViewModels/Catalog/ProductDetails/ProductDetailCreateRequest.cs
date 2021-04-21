using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductDetails
{
    public class ProductDetailCreateRequest
    {
        //public int ProductId { get; set; }
        //[Display(Name = "Giá")]
        public string ProductDetailName { set; get; }
        public decimal Price { set; get; }
        //[Display(Name = "Giá gốc")]
        public decimal OriginalPrice { set; get; }
        //[Display(Name = "Tồn kho")]
        public int Stock { set; get; }
        //[Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }
    }
}
