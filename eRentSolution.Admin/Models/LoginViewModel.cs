using eRentSolution.ViewModels.System.Users;
using eRentSolution.ViewModels.Utilities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Models
{
    public class LoginViewModel
    {
        public UserLoginRequest userLoginRequest { get; set; }
        public List<ContactViewModel> contacts { get; set; }
    }
}
