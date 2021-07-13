using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RelatedProductsApi.Configurations;
using RelatedProductsApi.Data;
using RelatedProductsApi.DataProviders.Abstractions;

namespace RelatedProductsApi.DataProviders
{
    public class RelatedProductProvider : IRelatedProductProvider
    {
        private readonly RelatedProductsDbContext _relatedProductsDbContext;
        private readonly int _pageSize;

        public RelatedProductProvider(
            RelatedProductsDbContext relatedProductsDbContext,
            IOptions<Config> options)
        {
            _relatedProductsDbContext = relatedProductsDbContext;
            _pageSize = options.Value.RelatedProductsApi.PageSize;
        }

        public async Task<IEnumerable<RelatedProductEntity>> GetByPageAsync(int page)
        {
            return await Task.Run(() =>
            {
                var skipNumber = page * _pageSize;
                var takeNumber = _pageSize;
                return _relatedProductsDbContext.RelatedProducts.Skip(skipNumber).Take(takeNumber);
            });
        }

        public async Task<RelatedProductEntity> GetByIdAsync(Guid id)
        {
            return await Task.Run(() =>
            {
                return _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Id == id);
            });
        }

        public async Task<RelatedProductEntity> AddAsync(RelatedProductEntity relatedProductEntity)
        {
            var result = await _relatedProductsDbContext.RelatedProducts.AddAsync(relatedProductEntity);
            await _relatedProductsDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Id == id);

            if (result != null)
            {
                _relatedProductsDbContext.RelatedProducts.Remove(result);
                await _relatedProductsDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync(RelatedProductEntity relatedProductEntity)
        {
            var result = _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Id == relatedProductEntity.Id);

            if (result != null)
            {
                _relatedProductsDbContext.RelatedProducts.Update(result);
                await _relatedProductsDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<int> GetPageCounterAsync()
        {
            return await Task.Run(() =>
            {
                double result = _relatedProductsDbContext.RelatedProducts.Count() / _pageSize;
                return (int)Math.Ceiling(result);
            });
        }
    }
}
