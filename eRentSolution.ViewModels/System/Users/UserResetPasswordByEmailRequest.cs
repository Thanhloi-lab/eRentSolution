using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserResetPasswordByEmailRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        [Display(Name = "Mật khẩu mới")]
        public string Password { get; set; }
        public DateTime Date { get; set; }
    }
    public class UserResetPasswordByEmailValidator : AbstractValidator<UserResetPasswordByEmailRequest>
    {
        public UserResetPasswordByEmailValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới.")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhât 8 kí tự (phải có chữ, số, chữ viết hoa).");
        }
    }
}
