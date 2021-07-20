using RelatedProductsApi.Common.Enums;

namespace RelatedProductsApi.Models.Requests
{
    public class GetByPageRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public SortedTypeEnum SortedType { get; set; }
    }
}
