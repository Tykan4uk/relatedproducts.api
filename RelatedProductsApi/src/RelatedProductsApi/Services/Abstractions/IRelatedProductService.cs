using System.Threading.Tasks;
using RelatedProductsApi.Common.Enums;
using RelatedProductsApi.Models.Responses;

namespace RelatedProductsApi.Services.Abstractions
{
    public interface IRelatedProductService
    {
        Task<GetByPageResponse> GetByPageAsync(int page, int pageSize, SortedTypeEnum sortedType);
        Task<GetByIdResponse> GetByIdAsync(string id);
        Task<AddResponse> AddAsync(string name, string description, decimal price);
        Task<DeleteResponse> DeleteAsync(string id);
        Task<UpdateResponse> UpdateNameAsync(string id, string name);
        Task<UpdateResponse> UpdateDescriptionAsync(string id, string description);
        Task<UpdateResponse> UpdatePriceAsync(string id, decimal price);
    }
}
