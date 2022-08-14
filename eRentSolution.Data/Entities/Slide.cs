using eRentSolution.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class Slide
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Url { set; get; }
        public int NewsId { get; set; }
        public string ImagePath { get; set; }
        public int SortOrder { get; set; }
        public Status Status { set; get; }
        public News News { get; set; }
    }
}
