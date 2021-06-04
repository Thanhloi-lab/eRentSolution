using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.ViewModels.System.Users
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
        public string CurrentDomain { get; set; }
    }
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Định dạng mail không hợp lệ");
        }
    }
}
