using eRentSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Contacts
{
    public class ContactViewModel
    {
        [Display(Name = "Mã số người liên hệ")]
        public int Id { set; get; }
        [Display(Name = "Tên")]
        public string Name { set; get; }
        [Display(Name = "Email")]
        public string Email { set; get; }
        [Display(Name = "Sđt")]
        public string PhoneNumber { set; get; }
        [Display(Name = "Lời nhắn")]
        public string Message { set; get; }
        [Display(Name = "Trạng thái")]
        public Status Status { set; get; }

    }
}
