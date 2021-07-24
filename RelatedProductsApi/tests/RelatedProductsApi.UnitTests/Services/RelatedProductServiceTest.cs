using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using RelatedProductsApi.Common.Enums;
using RelatedProductsApi.Data;
using RelatedProductsApi.Data.Entities;
using RelatedProductsApi.DataProviders.Abstractions;
using RelatedProductsApi.Models;
using RelatedProductsApi.Models.Responses;
using RelatedProductsApi.Services;
using RelatedProductsApi.Services.Abstractions;
using Xunit;

namespace RelatedProductsApi.UnitTests.Services
{
    public class RelatedProductServiceTest
    {
        private readonly IRelatedProductService _relatedProductService;

        private readonly Mock<IRelatedProductProvider> _relatedProductProvider;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<RelatedProductsDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<RelatedProductService>> _logger;

        private readonly PagingDataResult _pagingDataResultsSuccess = new PagingDataResult()
        {
            RelatedProductsEntity = new List<RelatedProductEntity>()
            {
                new RelatedProductEntity()
                {
                    Name = "TestName"
                }
            },
            TotalRecords = 1
        };
        private readonly PagingDataResult _pagingDataResultsFailed = new PagingDataResult()
        {
        };
        private readonly GetByPageResponse _getByPageResponseSuccess = new GetByPageResponse()
        {
            RelatedProducts = new List<RelatedProductModel>()
            {
                new RelatedProductModel()
                {
                    Name = "TestName"
                }
            },
            TotalRecords = 1
        };
        private readonly GetByPageResponse _getByPageResponseFailed = new GetByPageResponse()
        {
            RelatedProducts = null,
            TotalRecords = 1
        };
        private readonly RelatedProductEntity _relatedProductEntitySuccess = new RelatedProductEntity()
        {
            Name = "TestName"
        };
        private readonly RelatedProductModel _relatedProductModelSuccess = new RelatedProductModel()
        {
            Name = "TestName"
        };
        private readonly RelatedProductEntity _relatedProductEntityFailed = new RelatedProductEntity()
        {
        };
        private readonly RelatedProductModel _relatedProductModelFailed = new RelatedProductModel()
        {
        };
        private readonly string _testIdSuccess = "testIdSuccess";
        private readonly string _testIdFailed = "testIdFailed";
        private readonly string _testNameSuccess = "testNameSuccess";
        private readonly string _testNameFailed = "empty";
        private readonly string _testDescriptionSuccess = "testDescriptionSuccess";
        private readonly string _testDescriptionFailed = "empty";
        private readonly decimal _testPriceSuccess = 10.0M;
        private readonly decimal _testPriceFailed = 0.0M;

        public RelatedProductServiceTest()
        {
            _relatedProductProvider = new Mock<IRelatedProductProvider>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<RelatedProductsDbContext>>();
            _logger = new Mock<ILogger<RelatedProductService>>();

            _relatedProductProvider.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == 1),
                It.Is<int>(i => i == 10),
                It.IsAny<SortedTypeEnum>())).ReturnsAsync(_pagingDataResultsSuccess);

            _relatedProductProvider.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == 1000),
                It.Is<int>(i => i == 10000),
                It.IsAny<SortedTypeEnum>())).ReturnsAsync(_pagingDataResultsFailed);

            _relatedProductProvider.Setup(s => s.GetByIdAsync(
                It.Is<string>(i => i.Contains(_testIdSuccess)))).ReturnsAsync(_relatedProductEntitySuccess);

            _relatedProductProvider.Setup(s => s.GetByIdAsync(
                It.Is<string>(i => i.Contains(_testIdFailed)))).ReturnsAsync(_relatedProductEntityFailed);

            _relatedProductProvider.Setup(s => s.AddAsync(
                It.Is<string>(i => i.Contains(_testNameSuccess)),
                It.Is<string>(i => i.Contains(_testDescriptionSuccess)),
                It.Is<decimal>(i => i.Equals(_testPriceSuccess)))).ReturnsAsync(_relatedProductEntitySuccess);

            _relatedProductProvider.Setup(s => s.AddAsync(
                It.Is<string>(i => i.Contains(_testNameFailed)),
                It.Is<string>(i => i.Contains(_testDescriptionFailed)),
                It.Is<decimal>(i => i.Equals(_testPriceFailed)))).ReturnsAsync(_relatedProductEntityFailed);

            _relatedProductProvider.Setup(s => s.DeleteAsync(
                It.Is<string>(i => i.Contains(_testIdSuccess)))).ReturnsAsync(true);

            _relatedProductProvider.Setup(s => s.DeleteAsync(
                It.Is<string>(i => i.Contains(_testIdFailed)))).ReturnsAsync(false);

            _relatedProductProvider.Setup(s => s.UpdateNameAsync(
                It.Is<string>(i => i.Contains(_testIdSuccess)),
                It.Is<string>(i => i.Contains(_testNameSuccess)))).ReturnsAsync(true);

            _relatedProductProvider.Setup(s => s.UpdateNameAsync(
                It.Is<string>(i => i.Contains(_testIdFailed)),
                It.Is<string>(i => i.Contains(_testNameFailed)))).ReturnsAsync(false);

            _relatedProductProvider.Setup(s => s.UpdateDescriptionAsync(
                It.Is<string>(i => i.Contains(_testIdSuccess)),
                It.Is<string>(i => i.Contains(_testDescriptionSuccess)))).ReturnsAsync(true);

            _relatedProductProvider.Setup(s => s.UpdateDescriptionAsync(
                It.Is<string>(i => i.Contains(_testIdFailed)),
                It.Is<string>(i => i.Contains(_testDescriptionFailed)))).ReturnsAsync(false);

            _relatedProductProvider.Setup(s => s.UpdatePriceAsync(
                It.Is<string>(i => i.Contains(_testIdSuccess)),
                It.Is<decimal>(i => i.Equals(_testPriceSuccess)))).ReturnsAsync(true);

            _relatedProductProvider.Setup(s => s.UpdatePriceAsync(
                It.Is<string>(i => i.Contains(_testIdFailed)),
                It.Is<decimal>(i => i.Equals(_testPriceFailed)))).ReturnsAsync(false);

            _mapper.Setup(s => s.Map<RelatedProductModel>(
                It.Is<RelatedProductEntity>(i => i.Equals(_relatedProductEntitySuccess)))).Returns(_relatedProductModelSuccess);

            _mapper.Setup(s => s.Map<RelatedProductModel>(
                It.Is<RelatedProductEntity>(i => i.Equals(_relatedProductEntityFailed)))).Returns(_relatedProductModelFailed);

            _mapper.Setup(s => s.Map<GetByPageResponse>(
                It.Is<PagingDataResult>(i => i.Equals(_pagingDataResultsSuccess)))).Returns(_getByPageResponseSuccess);

            _mapper.Setup(s => s.Map<GetByPageResponse>(
                It.Is<PagingDataResult>(i => i.Equals(_pagingDataResultsFailed)))).Returns(_getByPageResponseFailed);

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransaction()).Returns(dbContextTransaction.Object);

            _relatedProductService = new RelatedProductService(_dbContextWrapper.Object, _relatedProductProvider.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetByPageAsync_Success()
        {
            // arrange
            var testPage = 1;
            var testPageSize = 10;
            var testSortedType = SortedTypeEnum.CreateDateAscending;

            // act
            var result = await _relatedProductService.GetByPageAsync(testPage, testPageSize, testSortedType);

            // assert
            result.Should().NotBeNull();
            result.RelatedProducts.Should().NotBeNull();
            result.TotalRecords.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetByPageAsync_Failed()
        {
            // arrange
            var testPage = 1000;
            var testPageSize = 10000;
            var testSortedType = SortedTypeEnum.CreateDateAscending;

            // act
            var result = await _relatedProductService.GetByPageAsync(testPage, testPageSize, testSortedType);

            // assert
            result.Should().NotBeNull();
            result.RelatedProducts.Should().BeNull();
            result.TotalRecords.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetByIdAsync_Success()
        {
            // arrange

            // act
            var result = await _relatedProductService.GetByIdAsync(_testIdSuccess);

            // assert
            result.Should().NotBeNull();
            result.RelatedProduct.Should().NotBeNull();
            result.RelatedProduct.Name.Should().Be("TestName");
        }

        [Fact]
        public async Task GetByIdAsync_Failed()
        {
            // arrange

            // act
            var result = await _relatedProductService.GetByIdAsync(_testIdFailed);

            // assert
            result.Should().NotBeNull();
            result.RelatedProduct.Should().NotBeNull();
            result.RelatedProduct.Name.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange

            // act
            var result = await _relatedProductService.AddAsync(_testNameSuccess, _testDescriptionSuccess, _testPriceSuccess);

            // assert
            result.Should().NotBeNull();
            result.RelatedProduct.Should().NotBeNull();
            result.RelatedProduct.Name.Should().Be("TestName");
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange

            // act
            var result = await _relatedProductService.AddAsync(_testNameFailed, _testDescriptionFailed, _testPriceFailed);

            // assert
            result.Should().NotBeNull();
            result.RelatedProduct.Should().NotBeNull();
            result.RelatedProduct.Name.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange

            // act
            var result = await _relatedProductService.DeleteAsync(_testIdSuccess);

            // assert
            result.Should().NotBeNull();
            result.IsDeleted.Should().Be(true);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange

            // act
            var result = await _relatedProductService.DeleteAsync(_testIdFailed);

            // assert
            result.Should().NotBeNull();
            result.IsDeleted.Should().Be(false);
        }

        [Fact]
        public async Task UpdateNameAsync_Success()
        {
            // arrange

            // act
            var result = await _relatedProductService.UpdateNameAsync(_testIdSuccess, _testNameSuccess);

            // assert
            result.Should().NotBeNull();
            result.IsUpdated.Should().Be(true);
        }

        [Fact]
        public async Task UpdateNameAsync_Failed()
        {
            // arrange

            // act
            var result = await _relatedProductService.UpdateNameAsync(_testIdFailed, _testNameFailed);

            // assert
            result.Should().NotBeNull();
            result.IsUpdated.Should().Be(false);
        }

        [Fact]
        public async Task UpdateDeveloperAsync_Success()
        {
            // arrange

            // act
            var result = await _relatedProductService.UpdateDescriptionAsync(_testIdSuccess, _testDescriptionSuccess);

            // assert
            result.Should().NotBeNull();
            result.IsUpdated.Should().Be(true);
        }

        [Fact]
        public async Task UpdateDescriptionAsync_Failed()
        {
            // arrange

            // act
            var result = await _relatedProductService.UpdateDescriptionAsync(_testIdFailed, _testDescriptionFailed);

            // assert
            result.Should().NotBeNull();
            result.IsUpdated.Should().Be(false);
        }

        [Fact]
        public async Task UpdatPriceAsync_Success()
        {
            // arrange

            // act
            var result = await _relatedProductService.UpdatePriceAsync(_testIdSuccess, _testPriceSuccess);

            // assert
            result.Should().NotBeNull();
            result.IsUpdated.Should().Be(true);
        }

        [Fact]
        public async Task UpdatePriceAsync_Failed()
        {
            // arrange

            // act
            var result = await _relatedProductService.UpdatePriceAsync(_testIdFailed, _testPriceFailed);

            // assert
            result.Should().NotBeNull();
            result.IsUpdated.Should().Be(false);
        }
    }
}
