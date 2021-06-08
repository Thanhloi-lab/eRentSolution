using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class ProductUpdateValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Không thể để trống tên sản phẩm.")
               .MaximumLength(200).WithMessage("Tên sản phẩm không vượt quá 200 kí tự.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được để trống.")
               .MaximumLength(300).WithMessage("Địa chỉ không vượt quá 200 kí tự.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả không được để trống.")
               .MaximumLength(200).WithMessage("Mô tả không vượt quá 200 kí tự.");
        }
    }
}
