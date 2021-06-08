using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryImageUpdateValidator : AbstractValidator<CategoryImageUpdateRequest>
    {
        public CategoryImageUpdateValidator()
        {
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
