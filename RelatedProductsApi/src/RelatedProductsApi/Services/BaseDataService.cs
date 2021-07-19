using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RelatedProductsApi.Data;

namespace RelatedProductsApi.Services
{
    public class BaseDataService
    {
        private readonly RelatedProductsDbContext _relatedProductsDbContext;

        public BaseDataService(IDbContextFactory<RelatedProductsDbContext> dbContextFactory)
        {
            _relatedProductsDbContext = dbContextFactory.CreateDbContext();
        }

        protected async Task<T> ExecuteSafe<T>(Func<Task<T>> action)
        {
            using (var transaction = _relatedProductsDbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await action();
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
