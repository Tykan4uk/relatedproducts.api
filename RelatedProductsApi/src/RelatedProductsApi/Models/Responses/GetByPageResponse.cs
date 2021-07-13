using System.Collections.Generic;

namespace RelatedProductsApi.Models.Responses
{
    public class GetByPageResponse
    {
        public IEnumerable<RelatedProduct> RelatedProducts { get; set; }
    }
}
