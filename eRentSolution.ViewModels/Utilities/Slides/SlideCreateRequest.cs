using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Slides
{
    public class SlideCreateRequest
    {
        [Display(Name = "Tên")]
        public string Name { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Ảnh")]
        public IFormFile ImageFile { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }
        [Display(Name = "Đường dẫn sản phẩm")]
        public string ProductUrl { get; set; }
    }
}
