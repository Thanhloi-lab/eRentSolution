using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Slides
{
    public class SlideStatusRequest
    {
        [Display(Name = "Mã sản phẩm trình chiếu")]
        public int Id { get; set; }
        [Display(Name = "Ảnh trình chiếu")]
        public string SlideImagePath { get; set; }
    }
}
