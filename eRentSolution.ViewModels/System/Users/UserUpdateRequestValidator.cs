using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Họ không thể để trống.")
                .MaximumLength(200).WithMessage("Họ không vượt quá 200 kí tự.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Tên không thể để trống.")
                .MaximumLength(200).WithMessage("Tên không vượt quá 200 kí tự.");

            RuleFor(x => x.Dob).GreaterThan(DateTime.UtcNow.AddYears(-150)).WithMessage("Tuổi không thể vượt quá 150")
                .LessThan(DateTime.UtcNow.AddYears(-16)).WithMessage("Tuổi phải lớn hơn 16.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại không thể để trống.")
                .Matches("0\\d{9}|\\d{10}$").WithMessage("Số điện thoại không hợp lệ. Số điện thoại phải có 10 chữ số."); ;
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không thể để trống.")
                .EmailAddress().WithMessage("Email không hợp lệ.");
        }
    }
}
