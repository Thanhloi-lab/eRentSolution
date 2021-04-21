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
    }
}
