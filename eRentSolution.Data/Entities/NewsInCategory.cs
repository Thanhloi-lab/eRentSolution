using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class NewsInCategory
    {
        public int NewsId { get; set; }
        public int CategoryId { get; set; }
        public News News { get; set; }
        public Category Category { get; set; }
    }
}
