using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RelatedProductsApi.Data;
using RelatedProductsApi.DataProviders.Abstractions;

namespace RelatedProductsApi.DataProviders
{
    public class RelatedProductProvider : IRelatedProductProvider
    {
        private readonly RelatedProductsDbContext _relatedProductsDbContext;

        public RelatedProductProvider(RelatedProductsDbContext relatedProductsDbContext)
        {
            _relatedProductsDbContext = relatedProductsDbContext;
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

        public async Task<RelatedProductEntity> AddAsync(RelatedProductEntity relatedProductEntity)
        {
            var result = await _relatedProductsDbContext.RelatedProducts.AddAsync(relatedProductEntity);
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
    }
}
