using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Họ không được bỏ trống.")
                .MaximumLength(200).WithMessage("Họ không thể vượt quá 200 kí tự.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Tên không được bỏ trống")
                .MaximumLength(200).WithMessage("Tên không thể vượt quá 200 kí tự.");

            RuleFor(x => x.Dob).GreaterThan(DateTime.UtcNow.AddYears(-150)).WithMessage("Tuổi không thể vượt quá 150.")
                .LessThan(DateTime.UtcNow.AddYears(-16)).WithMessage("Tuổi phải lớn hơn 16.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại không được bỏ trống")
                .Matches("^0\\d{9}|\\d{10}$").WithMessage("Số điện thoại không hợp lệ. Số điện thoại phải có 10 chữ số.");;
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được bỏ trống.")
                .EmailAddress().WithMessage("Email không hợp lệ.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Tài khoản không được bỏ trống.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được bỏ trống")
                /*.Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[`@_!?,:*-./\\$€¢£])[a-zA-Z\\d]{8,20}$")*/.MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8-20 kí tự (bắt buộc có cả số, chữ và chữ viết hoa).");
                 
            RuleFor(x => x).Custom((request, context) => {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Xác nhận mật khẩu không chính xác.");
                }
            });
        }
    }
}
