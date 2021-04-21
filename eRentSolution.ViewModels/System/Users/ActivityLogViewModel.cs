using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class ActivityLogViewModel
    {
        [Display(Name = "Tên")]
        public string UserLastName { get; set; }
        [Display(Name = "Hoạt động")]
        public string ActionName { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Ngày thực hiện")]
        public DateTime Date { get; set; }
    }
}
