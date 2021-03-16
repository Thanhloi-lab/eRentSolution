using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Products
{
    public class GetProductPagingByCategoryIdRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
