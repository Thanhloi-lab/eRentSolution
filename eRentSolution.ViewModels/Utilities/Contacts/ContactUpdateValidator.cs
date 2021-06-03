using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Contacts
{
    public class ContactUpdateValidator : AbstractValidator<ContactUpdateRequest>
    {
        public ContactUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Vui lòng nhập tên.")
                .MaximumLength(200).WithMessage("Tên không vượt quá 200 kí tự.");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Vui lòng nhập lời nhắn.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại không được bỏ trống.")
                .Matches("^[0-9]{10}$").WithMessage("Số điện thoại không hợp lệ. Số điện thoại phải có 10 chữ số."); ;
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được bỏ trống.")
                .Matches("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$").WithMessage("Email không hợp lệ.");
        }
    }
}
