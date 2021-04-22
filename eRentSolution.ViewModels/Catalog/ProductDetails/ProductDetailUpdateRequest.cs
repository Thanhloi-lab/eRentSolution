using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductDetails
{
    public class ProductDetailUpdateRequest
    {
        public int Id { get; set; }
        public string ProductDetailName { set; get; }
        public string Detail { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
    }
}
