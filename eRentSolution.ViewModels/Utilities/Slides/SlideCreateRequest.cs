using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Slides
{
    public class SlideCreateRequest
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public IFormFile ImageFile { get; set; }
        public int ProductId { get; set; }
    }
}
