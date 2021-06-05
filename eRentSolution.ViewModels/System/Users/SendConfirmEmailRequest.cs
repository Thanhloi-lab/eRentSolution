using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class SendConfirmEmailRequest
    {
        public string Email { get; set; }
        public string CurrentDomain { get; set; }
    }
    public class SendConfirmEmailRequestValidator : AbstractValidator<SendConfirmEmailRequest>
    {
        public SendConfirmEmailRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Định dạng mail không hợp lệ");
        }
    }
}
