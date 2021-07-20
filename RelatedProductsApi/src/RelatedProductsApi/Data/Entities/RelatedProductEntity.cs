﻿using System;

namespace RelatedProductsApi.Data.Entities
{
    public class RelatedProductEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
