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
            RuleFor(x => x.ImageFile.FileName).NotEmpty().WithMessage("Vui lòng chọn ảnh.")
                .MaximumLength(200).WithMessage("Đường dẫn ảnh không vượt quá 200 kí tự.");
            //RuleFor(x => x).Custom((request, context) => {
            //    if (request.NewPassword != request.ConfirmPassword)
            //    {
            //        context.AddFailure("Xác nhận mật khẩu không đúng.");
            //    }
            //}).NotEmpty().WithMessage("Xác nhận mật khẩu không thể để trống.");
        }
    }
}
