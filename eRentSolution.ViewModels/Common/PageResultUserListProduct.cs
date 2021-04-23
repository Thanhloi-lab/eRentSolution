using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Common
{
    public class PageResultUserListProduct : PagedResultBase
    {
        public Guid UserId { get; set; }
    }
}
