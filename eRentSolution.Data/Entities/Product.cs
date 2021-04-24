using eRentSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string SeoAlias { get; set; }
        public string Address { get; set; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public Status Status { get; set; }
        public Status? IsFeatured { get; set; }

        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<Censor> Censors { get; set; }
        public List<Slide> Slides { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
    }
}
