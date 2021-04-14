using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Slides
{
    public class GetSlidePagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public int? Status { get; set; }
    }
}
