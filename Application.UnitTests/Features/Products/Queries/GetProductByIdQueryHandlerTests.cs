using Application.Features.Products.Queries;
using Application.Services;

using AutoMapper;

using Common.Responses.Products;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IMapper> _mockMapper = new();

    public GetProductByIdQueryHandlerTests()
    {
        _mockProductService = MockProductService.GetProductServiceMocks();
        _mockMapper
            .Setup(m => m.Map<ProductResponse>(It.IsAny<Domain.Product>()))
            .Returns((Domain.Product source) => new Common.Responses.Products.ProductResponse
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description ?? string.Empty,
                Price = source.Price,
                CategoryId = source.CategoryId
            });
    }

    [Theory(DisplayName = "TC1: Get Product by Valid Id")]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetProductById_WithValidId_ShouldReturnProduct(int productId)
    {
        // Arrange
        var handler = new GetProductByIdQueryHandler(_mockProductService.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new GetProductByIdQuery { ProductId = productId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Id.ShouldBe(productId);
    }

    [Theory(DisplayName = "TC2: Get Product by Non-Existent Id")]
    [InlineData(999)]
    [InlineData(1000)]
    public async Task GetProductById_WithNonExistentId_ShouldReturnFail(int productId)
    {
        // Arrange
        var handler = new GetProductByIdQueryHandler(_mockProductService.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new GetProductByIdQuery { ProductId = productId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.Data.ShouldBeNull();
    }
}
