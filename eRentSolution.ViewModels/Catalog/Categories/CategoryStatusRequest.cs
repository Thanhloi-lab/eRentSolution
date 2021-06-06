using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryStatusRequest
    {
        [Display(Name = "Mã danh mục")]
        public int Id { get; set; }
        [Display(Name = "Ảnh danh mục")]
        public string CategoryImagePath { get; set; }

        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
    }
}
