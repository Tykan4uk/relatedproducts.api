using System.Collections.Generic;

namespace RelatedProductsApi.Models.Requests
{
    public class GetByPageRequest
    {
        public IEnumerable<RelatedProduct> RelatedProducts { get; set; }
    }
}
