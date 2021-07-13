using System;

namespace RelatedProductsApi.Models
{
    public class RelatedProduct
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
