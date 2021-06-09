using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UpdatePasswordRequestValidator : AbstractValidator<UserUpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Vui lòng nhập mật khẩu hiện tại.")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhât 8 kí tự (phải có chữ, số, chữ viết hoa).");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới.")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhât 8 kí tự (phải có chữ, số, chữ viết hoa).");
            RuleFor(x => x).Custom((request, context) => {
                if (request.NewPassword != request.ConfirmPassword)
                {
                    context.AddFailure("Xác nhận mật khẩu không đúng.");
                }
            }).NotEmpty().WithMessage("Xác nhận mật khẩu không thể để trống.");
        }
    }
}
