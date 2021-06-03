using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductUpdateStockRequest
    {
        public int ProductDetailId { get; set; }
        [Display(Name = "Số lượng")]
        public int AddedQuantity { get; set; }
    }
}
