using System;
using System.Threading.Tasks;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Responses;

namespace RelatedProductsApi.Services.Abstractions
{
    public interface IRelatedProductService
    {
        Task<GetByPageResponse> GetByPageAsync(int page);
        Task<GetByIdResponse> GetByIdAsync(Guid id);
        Task<AddResponse> AddAsync(RelatedProduct relatedProduct);
        Task<DeleteResponse> DeleteAsync(Guid id);
        Task<UpdateResponse> UpdateAsync(RelatedProduct relatedProduct);
        Task<GetPageCounterResponse> GetPageCounterAsync();
    }
}
