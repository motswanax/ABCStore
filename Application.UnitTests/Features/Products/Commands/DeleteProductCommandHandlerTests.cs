using Application.Features.Products.Commands;
using Application.Services;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Products.Commands;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductService> _mockProductService;

    public DeleteProductCommandHandlerTests()
    {
        _mockProductService = MockProductService.GetProductServiceMocks();
    }

    [Theory(DisplayName = "TC1: Delete Product with Valid Id")]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteProduct_WithValidId_ShouldReturnInt(int productId)
    {
        // Arrange
        var handler = new DeleteProductCommandHandler(_mockProductService.Object);
        // Act
        var result = await handler.Handle(new DeleteProductCommand { ProductId = productId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Theory(DisplayName = "TC2: Delete Product with Non-Existent Id")]
    [InlineData(999)]
    [InlineData(1000)]
    public async Task DeleteProduct_WithNonExistentId_ShouldReturnFail(int productId)
    {
        // Arrange
        var handler = new DeleteProductCommandHandler(_mockProductService.Object);
        // Act
        var result = await handler.Handle(new DeleteProductCommand { ProductId = productId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeFalse();
    }
}
