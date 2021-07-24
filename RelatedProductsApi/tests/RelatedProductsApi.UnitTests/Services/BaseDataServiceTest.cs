using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using RelatedProductsApi.Data;
using RelatedProductsApi.Services.Abstractions;
using RelatedProductsApi.UnitTests.Mocks;
using Xunit;

namespace RelatedProductsApi.UnitTests.Services
{
    public class BaseDataServiceTest
    {
        private Mock<IDbContextWrapper<RelatedProductsDbContext>> _dbContextWrapper;
        private Mock<IDbContextTransaction> _dbContextTransaction;
        private Mock<ILogger<MockService>> _logger;
        private MockService _mockService;

        public BaseDataServiceTest()
        {
            _dbContextWrapper = new Mock<IDbContextWrapper<RelatedProductsDbContext>>();
            _dbContextTransaction = new Mock<IDbContextTransaction>();
            _logger = new Mock<ILogger<MockService>>();

            _dbContextWrapper.Setup(s => s.BeginTransaction()).Returns(_dbContextTransaction.Object);

            _mockService = new MockService(_dbContextWrapper.Object, _logger.Object);
        }

        [Fact]
        public async Task ExecuteSafe_Success()
        {
            // arrange

            // act
            await _mockService.RunWithoutException();

            // assert
            _dbContextTransaction.Verify(t => t.Commit(), Times.Once);
            _dbContextTransaction.Verify(t => t.Rollback(), Times.Never);
        }

        [Fact]
        public async Task ExecuteSafe_Failed()
        {
            // arrange

            // act
            await _mockService.RunWithException();

            // assert
            _dbContextTransaction.Verify(t => t.Commit(), Times.Never);
            _dbContextTransaction.Verify(t => t.Rollback(), Times.Once);

            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()
                        .Contains($"transaction is rollbacked")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
