using System.Collections.Generic;

namespace RelatedProductsApi.Data
{
    public class PagingDataResult
    {
        public IEnumerable<RelatedProductEntity> RelatedProductsEntity { get; set; }
        public int TotalRecords { get; set; }
    }
}
