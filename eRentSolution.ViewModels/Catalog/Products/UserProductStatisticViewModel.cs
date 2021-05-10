using eRentSolution.Data.Enums;
using eRentSolution.ViewModels.Catalog.ProductDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class UserProductStatisticViewModel
    {
        //Entity Product
        [Display(Name = "Mã người dùng")]
        public Guid UserId { set; get; }
        [Display(Name = "Tên đăng nhập")]
        public string UserName { set; get; }
        [Display(Name = "Tổng lượt xem")]
        public int ViewCount { set; get; }
        [Display(Name = "Số lượng sản phẩm")]
        public int AmountProducts { set; get; }
    }
}
