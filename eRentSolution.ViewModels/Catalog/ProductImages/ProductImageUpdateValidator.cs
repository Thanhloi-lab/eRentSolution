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
            RuleFor(x => x.ImageFile).NotNull().WithMessage("Vui lòng chọn ảnh.");
        }
    }
}
