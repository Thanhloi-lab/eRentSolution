using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserDeleteRequest
    {
        [Display(Name = "Mã người dùng")]
        public Guid Id { get; set; }
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [Display(Name = "Họ")]
        public string LastName { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Sđt")]
        public string PhoneNumber { get; set; }
    }
}
