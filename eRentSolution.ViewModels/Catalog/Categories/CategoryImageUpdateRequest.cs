using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Catalog.Categories
{
    public class CategoryImageUpdateRequest
    {
        public int CategoryId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
