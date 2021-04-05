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
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password is at least 8 characters.");
            RuleFor(x => x).Custom((request, context) => {
                if (request.NewPassword != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm password is not match.");
                }
            }).NotEmpty().WithMessage("Confirm password is required.");
        }
    }
}
