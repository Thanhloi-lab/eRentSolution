using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserUpdatePasswordRequest
    {
        public Guid Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
