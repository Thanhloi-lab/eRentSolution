using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserUpdatePasswordRequest
    {
        [Display(Name = "Mã người dùng")]
        public Guid Id { get; set; }
        [Display(Name = "Tên người dùng")]
        public string FirstName { get; set; }
        [Display(Name = "Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }
        [Display(Name = "Xác nhận mật khẩu mới")]
        public string ConfirmPassword { get; set; }
    }
}
