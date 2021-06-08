using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Vui lòng nhập tên danh mục.")
                .MaximumLength(200).WithMessage("Tên danh mục không vượt quá 200 kí tự.");

            RuleFor(x => x.ImageFile).NotNull().WithMessage("Vui lòng chọn ảnh.");
            //RuleFor(x => x).Custom((request, context) => {
            //    if (request.NewPassword != request.ConfirmPassword)
            //    {
            //        context.AddFailure("Xác nhận mật khẩu không đúng.");
            //    }
            //}).NotEmpty().WithMessage("Xác nhận mật khẩu không thể để trống.");
        }
    }
}
