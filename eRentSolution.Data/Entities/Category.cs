using eRentSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public Status Status { get; set; }
        public string Name { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string SeoAlias { set; get; }
        public string ImagePath { get; set; }
        public long ImageSize { get; set; }
        public DateTime DateCreate { get; set; }
        public List<NewsInCategory> NewsInCategories { get; set; }
    }
}
