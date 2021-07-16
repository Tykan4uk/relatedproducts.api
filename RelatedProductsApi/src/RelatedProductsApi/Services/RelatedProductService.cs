using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RelatedProductsApi.Data;
using RelatedProductsApi.DataProviders.Abstractions;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Requests;
using RelatedProductsApi.Models.Responses;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.Services
{
    public class RelatedProductService : IRelatedProductService
    {
        private readonly IRelatedProductProvider _relatedProductProvider;
        private readonly IMapper _mapper;

        public RelatedProductService(
            IRelatedProductProvider relatedProductProvider,
            IMapper mapper)
        {
            _relatedProductProvider = relatedProductProvider;
            _mapper = mapper;
        }

        public async Task<GetByPageResponse> GetByPageAsync(GetByPageRequest getByPageRequest)
        {
            var result = await _relatedProductProvider.GetByPageAsync(getByPageRequest.Page, getByPageRequest.PageSize);

            return _mapper.Map<GetByPageResponse>(result);
        }

        public async Task<GetByIdResponse> GetByIdAsync(GetByIdRequest getByIdRequest)
        {
            var result = await _relatedProductProvider.GetByIdAsync(getByIdRequest.Id);

            var relatedProduct = _mapper.Map<RelatedProduct>(result);

            return new GetByIdResponse() { RelatedProduct = relatedProduct };
        }

        public async Task<AddResponse> AddAsync(AddRequest addRequest)
        {
            var relatedProductEntity = _mapper.Map<RelatedProductEntity>(addRequest);

            var result = await _relatedProductProvider.AddAsync(relatedProductEntity);

            var relatedProductOut = _mapper.Map<RelatedProduct>(result);

            return new AddResponse() { RelatedProduct = relatedProductOut };
        }

        public async Task<DeleteResponse> DeleteAsync(DeleteRequest deleteRequest)
        {
            var result = await _relatedProductProvider.DeleteAsync(deleteRequest.Id);

            return new DeleteResponse() { IsDeleted = result };
        }

        public async Task<UpdateResponse> UpdateAsync(RelatedProduct relatedProduct)
        {
            var relatedProductEntity = _mapper.Map<RelatedProductEntity>(relatedProduct);

            var result = await _relatedProductProvider.UpdateAsync(relatedProductEntity);

            return new UpdateResponse() { IsUpdated = result };
        }
    }
}
