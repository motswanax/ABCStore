using Application.Features.Products.Queries;
using Application.Services;

using AutoMapper;

using Common.Responses.Products;

using Domain;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Products.Queries;

public class GetProductsByCategoryIdQueryTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IMapper> _mockMapper = new();

    public GetProductsByCategoryIdQueryTests()
    {
        _mockProductService = MockProductService.GetProductServiceMocks();
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

    [Theory(DisplayName = "TC1: Get Products by Valid Category Id")]
    [InlineData(1, 2)] // CategoryId 1 has 2 products
    [InlineData(2, 1)] // CategoryId 2 has 1 product
    public async Task GetProductsByCategoryId_ShouldReturnProducts(int categoryId, int expectedCount)
    {
        // Arrange
        var handler = new GetProductsByCategoryIdQueryHandler(_mockProductService.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new GetProductsByCategoryIdQuery { CategoryId = categoryId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count().ShouldBe(expectedCount);
    }

    [Theory(DisplayName = "TC2: Get Products by Non-Existent Category Id")]
    [InlineData(999)] // Non-existent CategoryId
    [InlineData(-1)]  // Invalid CategoryId
    public async Task GetProductsByCategoryId_ShouldReturnEmptyProducts(int categoryId)
    {
        // Arrange
        var handler = new GetProductsByCategoryIdQueryHandler(_mockProductService.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new GetProductsByCategoryIdQuery { CategoryId = categoryId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.Data.ShouldBeNull();
    }
}