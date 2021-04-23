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
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.")
                .MaximumLength(200).WithMessage("First name cannot over 200 characters.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(200).WithMessage("Last name cannot over 200 characters.");

            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-150)).WithMessage("Noone can live over 150 bro!!")
                .LessThan(DateTime.Now).WithMessage("Birthday cannot be greater than now");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phonenumber is required.")
                .Matches("^[0-9]{10}$").WithMessage("Phonenumber format is not match. Phonenumber must be 10 digits"); ;
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                .Matches("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$").WithMessage("Email format is not match.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");
        }
    }
}
