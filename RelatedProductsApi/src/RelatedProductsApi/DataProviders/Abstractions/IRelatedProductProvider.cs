using System.Threading.Tasks;
using RelatedProductsApi.Data;

namespace RelatedProductsApi.DataProviders.Abstractions
{
    public interface IRelatedProductProvider
    {
        Task<PagingDataResult> GetByPageAsync(int page, int pageSize);
        Task<RelatedProductEntity> GetByIdAsync(string id);
        Task<RelatedProductEntity> AddAsync(RelatedProductEntity relatedProductEntity);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(RelatedProductEntity relatedProductEntity);
    }
}
