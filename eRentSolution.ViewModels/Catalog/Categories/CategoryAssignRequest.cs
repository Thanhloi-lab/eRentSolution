using eRentSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryAssignRequest
    {
        public int Id { get; set; }
        public List<SelectItem> Categories { get; set; } = new List<SelectItem>();
    }
}
