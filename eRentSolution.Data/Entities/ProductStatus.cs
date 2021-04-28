using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class ProductStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public Product Product { get; set; }
    }
}
