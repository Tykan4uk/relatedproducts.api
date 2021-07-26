using System.Threading.Tasks;
using RelatedProductsApi.Models.Responses;

namespace RelatedProductsApi.Services.Abstractions
{
    public interface IRateLimitService
    {
        Task<CheckRateLimitResponse> CheckRateLimit(string name);
    }
}
