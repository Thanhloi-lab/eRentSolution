using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.ViewModels.Utilities.Slides
{
    public class SlideViewModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Url { set; get; }
        public int ProductId { get; set; }
        public int SortOrder { get; set; }
        public string FilePath { get; set; }
    }
}
