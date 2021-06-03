using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Contacts
{
    public class ContactStatusRequest
    {
        [Display(Name = "Mã số người liên hệ")]
        public int Id { get; set; }
    }
}
