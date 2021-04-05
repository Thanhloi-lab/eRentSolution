using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.System.Users
{
    public class UserActivityLogRequest : PagingRequestBase
    {
        public Guid Id { get; set; }
        public string Keyword { get; set; }
    }
}
