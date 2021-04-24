using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageDeleteRequest
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public int ProductDetailId { get; set; }
    }
}
