using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserResetPasswordRequest
    {
        [Display(Name = "Mã người dùng")]
        public Guid Id { get; set; }
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }
        public string Token { get; set; }
        [Display(Name = "Mật khẩu mới")]
        public UserViewModel User { get; set; }
    }

}
