using FluentValidation;
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
    public class SlideUpdateRequestValidator : AbstractValidator<SlideUpdateRequest>
    {
        public SlideUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Vui lòng nhập tên của sản phẩm trình chiếu.")
                .MaximumLength(200).WithMessage("Tên không vượt quá 200 kí tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Vui lòng nhập mô tả của sản phẩm trình chiếu")
                .MaximumLength(200).WithMessage("Mô tả không vượt quá 200 kí tự.");
        }
    }
}
