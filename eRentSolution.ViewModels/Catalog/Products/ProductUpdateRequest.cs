using eRentSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductUpdateRequest
    {
        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Mô tả - tìm kiếm")]
        public string SeoDescription { set; get; }
        [Display(Name = "Tiêu đề - tìm kiếm")]
        public string SeoTitle { set; get; }
        [Display(Name = "Từ khóa - tìm kiếm")]
        public string SeoAlias { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
    }
}
