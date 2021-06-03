using eRentSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Slides
{
    public class SlideViewModel
    {
        [Display(Name = "Mã sản phẩm trình chiếu")]
        public int Id { set; get; }
        [Display(Name = "Tên sản phẩm trình chiếu")]
        public string Name { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Đường dẫn sản phẩm")]
        public string Url { set; get; }
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Đường dẫn ảnh")]
        public string FilePath { get; set; }
        [Display(Name = "Trạng thái")]
        public Status Status { get; set; }
    }
}
