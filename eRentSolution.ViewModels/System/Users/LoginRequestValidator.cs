using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class LoginRequestValidator : AbstractValidator<UserLoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Vui lòng nhập tài khoản.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Vui lòng nhập mật khẩu.");
        }
    }
}
