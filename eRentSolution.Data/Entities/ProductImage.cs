﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductDetailId { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public DateTime DateCreated { get; set; }
        public long FileSize { get; set; }
        public ProductDetail ProductDetail { get; set; }
    }
}
