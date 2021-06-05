using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class ConfirmEmailRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Date { get; set; }
    }
}
