using eRentSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string Name { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public string Detail { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Stock { set; get; }
        public News News { get; set; }
        public DateTime DateCreated { set; get; }
        public Status Status { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}
