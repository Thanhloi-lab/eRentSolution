using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Slides
{
    public class SlideUpdateRequest
    {
        public IFormFile ImageFile { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
    }
}
