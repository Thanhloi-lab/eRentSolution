using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductUpdateStockRequest
    {
        public int ProductDetailId { get; set; }
        public int AddedQuantity { get; set; }
    }
}
