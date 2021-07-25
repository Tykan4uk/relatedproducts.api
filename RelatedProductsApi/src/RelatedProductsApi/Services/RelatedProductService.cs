using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RelatedProductsApi.Common.Enums;
using RelatedProductsApi.Data;
using RelatedProductsApi.DataProviders.Abstractions;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Responses;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.Services
{
    public class RelatedProductService : BaseDataService<RelatedProductsDbContext>, IRelatedProductService
    {
        private readonly IRelatedProductProvider _relatedProductProvider;
        private readonly IMapper _mapper;

        public RelatedProductService(
            IDbContextWrapper<RelatedProductsDbContext> wrapper,
            IRelatedProductProvider relatedProductProvider,
            IMapper mapper,
            ILogger<RelatedProductService> logger)
            : base(wrapper, logger)
        {
            _relatedProductProvider = relatedProductProvider;
            _mapper = mapper;
        }

        public async Task<GetByPageResponse> GetByPageAsync(int page, int pageSize, SortedTypeEnum sortedType)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _relatedProductProvider.GetByPageAsync(page, pageSize, sortedType);

                return _mapper.Map<GetByPageResponse>(result);
            });
        }

        public async Task<GetByIdResponse> GetByIdAsync(string id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _relatedProductProvider.GetByIdAsync(id);

                var relatedProduct = _mapper.Map<RelatedProductModel>(result);

                return new GetByIdResponse() { RelatedProduct = relatedProduct };
            });
        }

        public async Task<AddResponse> AddAsync(string name, string description, decimal price, string imageUrl)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _relatedProductProvider.AddAsync(name, description, price, imageUrl);

                var relatedProductOut = _mapper.Map<RelatedProductModel>(result);

                return new AddResponse() { RelatedProduct = relatedProductOut };
            });
        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _relatedProductProvider.DeleteAsync(id);

                return new DeleteResponse() { IsDeleted = result };
            });
        }

        public async Task<UpdateResponse> UpdateNameAsync(string id, string name)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _relatedProductProvider.UpdateNameAsync(id, name);

                return new UpdateResponse() { IsUpdated = result };
            });
        }

        public async Task<UpdateResponse> UpdateDescriptionAsync(string id, string description)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _relatedProductProvider.UpdateDescriptionAsync(id, description);

                return new UpdateResponse() { IsUpdated = result };
            });
        }

        public async Task<UpdateResponse> UpdatePriceAsync(string id, decimal price)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _relatedProductProvider.UpdatePriceAsync(id, price);

                return new UpdateResponse() { IsUpdated = result };
            });
        }

        public async Task<UpdateResponse> UpdateImageUrlAsync(string id, string imageUrl)
        {
            return await ExecuteSafe(async () =>
            {
                var result = await _relatedProductProvider.UpdateImageUrlAsync(id, imageUrl);

                return new UpdateResponse() { IsUpdated = result };
            });
        }
    }
}
