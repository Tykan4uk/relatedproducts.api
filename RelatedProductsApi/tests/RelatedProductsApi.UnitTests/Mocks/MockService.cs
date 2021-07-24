using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RelatedProductsApi.Data;
using RelatedProductsApi.Services;
using RelatedProductsApi.Services.Abstractions;

namespace RelatedProductsApi.UnitTests.Mocks
{
    public class MockService : BaseDataService<RelatedProductsDbContext>
    {
        public MockService(
            IDbContextWrapper<RelatedProductsDbContext> dbContextWrapper,
            ILogger<MockService> logger)
            : base(dbContextWrapper, logger)
        {
        }

        public async Task RunWithException()
        {
            await ExecuteSafe<bool>(() =>
            {
                throw new Exception();
            });
        }

        public async Task RunWithoutException()
        {
            await ExecuteSafe<bool>(() =>
            {
                return Task.FromResult(true);
            });
        }
    }
}
