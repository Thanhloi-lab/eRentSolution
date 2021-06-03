using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryViewModel
    {

        public int Id { get; set; }
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }
        [Display(Name = "Mã danh mục cha")]
        public int? ParentId { get; set; }
        [Display(Name = "Ảnh danh mục")]
        public string Image { get; set; }
    }
}
