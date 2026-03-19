using Application.Features.Products.Queries;
using Application.Services;

using AutoMapper;

using Common.Responses.Products;

using Domain;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Products.Queries;

public class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IProductService> _emptyMockProductService;
    private readonly Mock<IMapper> _mockMapper = new();
    private readonly Mock<IMapper> _emptyMockMapper = new();
    private readonly List<ProductResponse> _expectedEmptyProductsResponse = [];

    public GetProductsQueryHandlerTests()
    {
        _mockProductService = MockProductService.GetProductServiceMocks();
        _emptyMockProductService = MockProductService.GetEmptyProductServiceMocks();

        _mockMapper
            .Setup(m => m.Map<IEnumerable<ProductResponse>>(It.IsAny<IEnumerable<Product>>()))
            .Returns((IEnumerable<Product> source) => [.. source.Select(s => new ProductResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description ?? string.Empty,
                Price = s.Price,
                CategoryId = s.CategoryId
            })]);
    }

    [Fact(DisplayName = "TC1: Get All Products")]
    public async Task GetProducts_ShouldReturnAllProducts()
    {
        // Arrange
        var handler = new GetProductsQueryHandler(_mockProductService.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count().ShouldBeGreaterThan(0);
        result.Data.Count().ShouldBe(3);
    }

    [Fact(DisplayName = "TC2: Get All Products - No Data Exists")]
    public async Task GetProducts_ShouldReturnEmptyProducts()
    {
        // Arrange
        var handler = new GetProductsQueryHandler(_emptyMockProductService.Object, _emptyMockMapper.Object);
        // Act
        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
    }
}
