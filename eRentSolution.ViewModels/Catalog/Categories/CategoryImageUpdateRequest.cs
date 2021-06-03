using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryImageUpdateRequest
    {
        [Display(Name = "Mã danh mục")]
        public int CategoryId { get; set; }
        [Display(Name = "Ảnh danh mục")]
        public IFormFile ImageFile { get; set; }
    }
}
