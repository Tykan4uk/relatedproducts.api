using System.Threading.Tasks;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Requests;
using RelatedProductsApi.Models.Responses;

namespace RelatedProductsApi.Services.Abstractions
{
    public interface IRelatedProductService
    {
        Task<GetByPageResponse> GetByPageAsync(GetByPageRequest getByPageRequest);
        Task<GetByIdResponse> GetByIdAsync(GetByIdRequest getByIdRequest);
        Task<AddResponse> AddAsync(AddRequest addRequest);
        Task<DeleteResponse> DeleteAsync(DeleteRequest deleteRequest);
        Task<UpdateResponse> UpdateAsync(RelatedProduct relatedProduct);
    }
}
