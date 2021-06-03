using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryUpdateRequest
    {
        [Display(Name = "Mã danh mục")]
        public int CategoryId { get; set; }
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
    }
}
