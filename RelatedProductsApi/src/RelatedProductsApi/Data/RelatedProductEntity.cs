﻿using System;

namespace RelatedProductsApi.Data
{
    public class RelatedProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
