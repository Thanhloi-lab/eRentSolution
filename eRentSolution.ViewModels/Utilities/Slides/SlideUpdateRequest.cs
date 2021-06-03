using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Slides
{
    public class SlideUpdateRequest
    {
        //public IFormFile ImageFile { get; set; }
        [Display(Name = "Tên sản phẩm trình chiếu")]
        public string Name { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        //public int ProductId { get; set; }
        [Display(Name = "Mã sản phẩm trình chiếu")]
        public int Id { get; set; }
    }
}
