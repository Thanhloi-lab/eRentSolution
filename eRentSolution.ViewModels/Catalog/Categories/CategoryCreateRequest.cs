using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
        [Display(Name = "Mã danh mục cha")]
        public int? ParentId { get; set; }
        [Display(Name = "Ảnh danh mục")]
        public IFormFile ImageFile { get; set; }
    }
}
