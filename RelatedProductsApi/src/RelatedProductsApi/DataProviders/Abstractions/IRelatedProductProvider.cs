using System.Threading.Tasks;
using RelatedProductsApi.Data;

namespace RelatedProductsApi.DataProviders.Abstractions
{
    public interface IRelatedProductProvider
    {
        Task<PagingDataResult> GetByPageAsync(int page, int pageSize);
        Task<RelatedProductEntity> GetByIdAsync(string id);
        Task<RelatedProductEntity> AddAsync(string name, string description, decimal price);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateNameAsync(string id, string name);
        Task<bool> UpdateDescriptionAsync(string id, string description);
        Task<bool> UpdatePriceAsync(string id, decimal price);
    }
}
