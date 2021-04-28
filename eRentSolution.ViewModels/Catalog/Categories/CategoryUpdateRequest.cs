using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryUpdateRequest
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
