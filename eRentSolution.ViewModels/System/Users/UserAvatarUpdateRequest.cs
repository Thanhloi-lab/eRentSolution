using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserAvatarUpdateRequest
    {
        [Display(Name = "Mã người dùng")]
        public Guid Id { get; set; }
        [Display(Name = "Tên người dùng")]
        public string Name { get; set; }
        [Display(Name = "Ảnh đại diện mới")]
        public IFormFile AvatarFile { get; set; }
        [Display(Name = "Ảnh đại diện cũ")]
        public string OldAvatarFilePath { get; set; }
    }
    public class UserAvatarUpdateRequestValidator : AbstractValidator<UserAvatarUpdateRequest>
    {
        public UserAvatarUpdateRequestValidator()
        {
            RuleFor(x => x.AvatarFile).NotNull().WithMessage("Không thể để trống ảnh.");
        }
    }
}
