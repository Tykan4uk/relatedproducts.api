using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RelatedProductsApi.Common.Enums;
using RelatedProductsApi.Common.Exceptions;
using RelatedProductsApi.Data;
using RelatedProductsApi.Data.Entities;
using RelatedProductsApi.DataProviders.Abstractions;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.DataProviders
{
    public class RelatedProductProvider : IRelatedProductProvider
    {
        private readonly RelatedProductsDbContext _relatedProductsDbContext;
        private readonly ILogger<RelatedProductProvider> _logger;

        public RelatedProductProvider(
            IDbContextWrapper<RelatedProductsDbContext> dbContextWrapper,
            ILogger<RelatedProductProvider> logger)
        {
            _relatedProductsDbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<PagingDataResult> GetByPageAsync(int page, int pageSize, SortedTypeEnum sortedType)
        {
            if (page <= 0 || pageSize <= 0)
            {
                _logger.LogError("(RelatedProductsDbContext/GetByPageAsync)Page or page size error!");
                throw new BusinessException("Page or page size error!");
            }

            IQueryable<RelatedProductEntity> query = _relatedProductsDbContext.RelatedProducts;
            switch (sortedType)
            {
                case SortedTypeEnum.CreateDateAscending:
                    query = query.OrderBy(o => o.CreateDate);
                    break;
                case SortedTypeEnum.CreateDateDescending:
                    query = query.OrderByDescending(o => o.CreateDate);
                    break;
                case SortedTypeEnum.PriceAscending:
                    query = query.OrderBy(o => o.Price);
                    break;
                case SortedTypeEnum.PriceDescending:
                    query = query.OrderByDescending(o => o.Price);
                    break;
                default:
                    query = query.OrderBy(o => o.CreateDate);
                    break;
            }

            var totalRecords = await _relatedProductsDbContext.RelatedProducts.CountAsync();
            var pageData = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDataResult() { RelatedProductsEntity = pageData, TotalRecords = totalRecords };
        }

        public async Task<RelatedProductEntity> GetByIdAsync(string id)
        {
            return await _relatedProductsDbContext.RelatedProducts.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<RelatedProductEntity> AddAsync(string name, string description, decimal price, string imageUrl)
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
                    CreateDate = createDate,
                    ImageUrl = imageUrl
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

        public async Task<bool> UpdateImageUrlAsync(string id, string imageUrl)
        {
            var result = _relatedProductsDbContext.RelatedProducts.FirstOrDefault(f => f.Id == id);

            if (result != null)
            {
                result.ImageUrl = imageUrl;
                await _relatedProductsDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
