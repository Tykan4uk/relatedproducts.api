using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RelatedProductsApi.Data;

namespace RelatedProductsApi.DataProviders.Abstractions
{
    public interface IRelatedProductProvider
    {
        Task<IEnumerable<RelatedProductEntity>> GetByPageAsync(int page);
        Task<RelatedProductEntity> GetByIdAsync(Guid guid);
        Task<RelatedProductEntity> AddAsync(RelatedProductEntity game);
        Task<bool> DeleteAsync(Guid guid);
        Task<bool> UpdateAsync(RelatedProductEntity game);
        Task<int> CountProducts();
    }
}
