using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        public string CategoryName { get; set; }
        public int? ParentId { get; set; } 
        public IFormFile ImageFile { get; set; }
    }
}
