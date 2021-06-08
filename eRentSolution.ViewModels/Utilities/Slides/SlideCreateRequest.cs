using FluentValidation;
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
    public class SlideCreateRequestValidator : AbstractValidator<SlideCreateRequest>
    {
        public SlideCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Vui lòng nhập tên của sản phẩm trình chiếu.")
                .MaximumLength(200).WithMessage("Tên không vượt quá 200 kí tự.");
            RuleFor(x=>x.Description).NotEmpty().WithMessage("Vui lòng nhập mô tả của sản phẩm trình chiếu")
                .MaximumLength(200).WithMessage("Mô tả không vượt quá 200 kí tự.");
            RuleFor(x => x.ProductUrl).NotEmpty().WithMessage("Vui lòng nhập mô tả của sản phẩm trình chiếu")
                .MaximumLength(200).WithMessage("Đường dẫn không vượt quá 200 kí tự.");
            RuleFor(x => x.ImageFile).NotNull().WithMessage("Không thể để trống ảnh.");
        }
    }
}
