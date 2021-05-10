using eRentSolution.ViewModels.Catalog.Products;
using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.AdminApp.Models
{
    public class HomeViewModel
    {
        public PagedResult<ProductViewModel> StatisticProducts { get; set; }
        public PagedResult<UserProductStatisticViewModel> StatisticUserProducts { get; set; }
    }
}
