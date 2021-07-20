using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RelatedProductsApi.Data;
using RelatedProductsApi.Data.Entities;
using RelatedProductsApi.DataProviders.Abstractions;

namespace RelatedProductsApi.DataProviders
{
    public class RelatedProductProvider : IRelatedProductProvider
    {
        private readonly RelatedProductsDbContext _relatedProductsDbContext;

        public RelatedProductProvider(IDbContextFactory<RelatedProductsDbContext> dbContextFactory)
        {
            _relatedProductsDbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<PagingDataResult> GetByPageAsync(int page, int pageSize)
        {
            var totalRecords = await _relatedProductsDbContext.RelatedProducts.CountAsync();
            var pageData = await _relatedProductsDbContext.RelatedProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDataResult() { RelatedProductsEntity = pageData, TotalRecords = totalRecords };
        }

        public async Task<RelatedProductEntity> GetByIdAsync(string id)
        {
            return await _relatedProductsDbContext.RelatedProducts.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<RelatedProductEntity> AddAsync(string name, string description, decimal price)
        {
            var id = Guid.NewGuid().ToString();
            var createDate = DateTime.Now;
            var result = await _relatedProductsDbContext.RelatedProducts.AddAsync(
                new RelatedProductEntity()
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Price = price,
                    CreateDate = createDate
                });
            await _relatedProductsDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteAsync(string id)
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

        public async Task<bool> UpdateNameAsync(string id, string name)
        {
            var result = _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Id == id);

            if (result != null)
            {
                result.Name = name;
                await _relatedProductsDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateDescriptionAsync(string id, string description)
        {
            var result = _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Id == id);

            if (result != null)
            {
                result.Description = description;
                await _relatedProductsDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdatePriceAsync(string id, decimal price)
        {
            var result = _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Id == id);

            if (result != null)
            {
                result.Price = price;
                await _relatedProductsDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
