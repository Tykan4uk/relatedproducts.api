using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.Services
{
    public abstract class BaseDataService<T>
        where T : DbContext
    {
        private readonly IDbContextWrapper<T> _dbContextWrapper;
        private readonly ILogger<BaseDataService<T>> _logger;

        public BaseDataService(
            IDbContextWrapper<T> dbContextWrapper,
            ILogger<BaseDataService<T>> logger)
        {
            _dbContextWrapper = dbContextWrapper;
            _logger = logger;
        }

        protected async Task<T1> ExecuteSafe<T1>(Func<Task<T1>> action)
        {
            using (var transaction = _dbContextWrapper.BeginTransaction())
            {
                try
                {
                    var result = await action();
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, $"transaction is rollbacked");
                    return default(T1);
                }
            }
        }
    }
}
