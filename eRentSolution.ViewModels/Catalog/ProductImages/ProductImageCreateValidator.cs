using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageCreateValidator : AbstractValidator<ProductImageCreateRequest>
    {
        public ProductImageCreateValidator()
        {
            RuleFor(x => x.ImageFile).NotNull().WithMessage("Vui lòng chọn ảnh.");
            RuleFor(x => x.Caption).NotEmpty().WithMessage("Không thể bỏ trống chú thích.")
                .MaximumLength(200).WithMessage("Chú thích không vượt quá 200 kí tự.");

        }
    }
}
