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
        [Display(Name = "Ảnh đại diện")]
        public IFormFile AvatarFile { get; set; }
    }
}
