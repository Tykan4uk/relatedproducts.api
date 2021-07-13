using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RelatedProductsApi.Data;
using RelatedProductsApi.DataProviders.Abstractions;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Responses;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.Services
{
    public class RelatedProductService : IRelatedProductService
    {
        private readonly IRelatedProductProvider _relatedProductProvider;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;

        public RelatedProductService(IRelatedProductProvider relatedProductProvider)
        {
            _relatedProductProvider = relatedProductProvider;
            _mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<RelatedProduct, RelatedProductEntity>());
            _mapper = new Mapper(_mapperConfiguration);
        }

        public async Task<GetByPageResponse> GetByPageAsync(int page)
        {
            var result = await _relatedProductProvider.GetByPageAsync(page);

            var relatedProducts = _mapper.Map<List<RelatedProduct>>(result);

            return new GetByPageResponse() { RelatedProducts = relatedProducts };
        }

        public async Task<GetByIdResponse> GetByIdAsync(Guid id)
        {
            var result = await _relatedProductProvider.GetByIdAsync(id);

            var relatedProduct = _mapper.Map<RelatedProduct>(result);

            return new GetByIdResponse() { RelatedProduct = relatedProduct };
        }

        public async Task<AddResponse> AddAsync(RelatedProduct relatedProduct)
        {
            var relatedProductEntity = _mapper.Map<RelatedProductEntity>(relatedProduct);

            var result = await _relatedProductProvider.AddAsync(relatedProductEntity);

            var relatedProductOut = _mapper.Map<RelatedProduct>(result);

            return new AddResponse() { RelatedProduct = relatedProductOut };
        }

        public async Task<DeleteResponse> DeleteAsync(Guid id)
        {
            var result = await _relatedProductProvider.DeleteAsync(id);

            return new DeleteResponse() { IsDeleted = result };
        }

        public async Task<UpdateResponse> UpdateAsync(RelatedProduct relatedProduct)
        {
            var relatedProductEntity = _mapper.Map<RelatedProductEntity>(relatedProduct);

            var result = await _relatedProductProvider.UpdateAsync(relatedProductEntity);

            return new UpdateResponse() { IsUpdated = result };
        }

        public async Task<GetPageCounterResponse> GetPageCounterAsync()
        {
            var result = await _relatedProductProvider.GetPageCounterAsync();

            return new GetPageCounterResponse() { PageCounter = result };
        }
    }
}
