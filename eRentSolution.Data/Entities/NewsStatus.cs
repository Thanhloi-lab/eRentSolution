using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class NewsStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public News News { get; set; }
    }
}
