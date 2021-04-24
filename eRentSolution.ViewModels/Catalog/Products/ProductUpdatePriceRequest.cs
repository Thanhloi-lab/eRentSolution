using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductUpdatePriceRequest
    {
        public int ProductDetailId { get; set; }
        public decimal NewPrice { get; set; }
    }
}
