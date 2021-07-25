﻿using System;

namespace RelatedProductsApi.Models
{
    public class RelatedProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public string ImageUrl { get; set; }
    }
}
