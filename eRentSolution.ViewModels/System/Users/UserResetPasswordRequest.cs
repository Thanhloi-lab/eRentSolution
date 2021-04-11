using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserResetPasswordRequest
    {
        public Guid Id { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }

}
