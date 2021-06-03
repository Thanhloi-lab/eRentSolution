using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserLoginRequest
    {
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}
