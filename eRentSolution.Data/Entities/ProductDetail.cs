using eRentSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public bool IsThumbnail { get; set; }
        public Product Product { get; set; }
        public DateTime DateCreated { set; get; }
        public Status Status { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}
