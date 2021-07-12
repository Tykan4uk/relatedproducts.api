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

        public async Task<RelatedProductEntity> GetByIdAsync(Guid guid)
        {
            return await Task.Run(() =>
            {
                return _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Guid == guid);
            });
        }

        public async Task<RelatedProductEntity> AddAsync(RelatedProductEntity game)
        {
            var result = await _relatedProductsDbContext.RelatedProducts.AddAsync(game);
            await _relatedProductsDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            var result = _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Guid == guid);

            if (result != null)
            {
                _relatedProductsDbContext.RelatedProducts.Remove(result);
                await _relatedProductsDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync(RelatedProductEntity game)
        {
            var result = _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Guid == game.Guid);

            if (result != null)
            {
                _relatedProductsDbContext.RelatedProducts.Update(result);
                await _relatedProductsDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<int> CountProducts()
        {
            return await Task.Run(() =>
            {
                return _relatedProductsDbContext.RelatedProducts.Count();
            });
        }
    }
}
