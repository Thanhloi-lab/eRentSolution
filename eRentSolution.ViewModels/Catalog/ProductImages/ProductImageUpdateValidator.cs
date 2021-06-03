using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.ProductImages
{
    public class ProductImageUpdateValidator : AbstractValidator<ProductImageUpdateRequest>
    {
        public ProductImageUpdateValidator()
        {
            RuleFor(x => x.ImageFile.FileName).NotEmpty().WithMessage("Vui lòng chọn ảnh.")
                .MaximumLength(200).WithMessage("Đường dẫn ảnh không vượt quá 200 kí tự.");
        }
    }
}
