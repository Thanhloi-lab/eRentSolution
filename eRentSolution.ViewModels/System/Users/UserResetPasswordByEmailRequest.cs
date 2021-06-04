using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserResetPasswordByEmailRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        [Display(Name = "Mật khẩu mới")]
        public string Password { get; set; }
    }
}
