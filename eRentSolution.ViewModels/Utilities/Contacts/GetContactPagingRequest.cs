using eRentSolution.Data.Enums;
using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Contacts
{
    public class GetContactPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public int? Status { get; set; }
    }
}
