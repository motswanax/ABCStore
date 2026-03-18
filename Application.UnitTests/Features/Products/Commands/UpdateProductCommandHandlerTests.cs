using Application.Features.Products.Commands;
using Application.Services;

using Common.Requests.Products;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Products.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductService> _mockProductService;

    public UpdateProductCommandHandlerTests()
    {
        _mockProductService = MockProductService.GetProductServiceMocks();
    }

    [Theory(DisplayName = "TC1: Update Product with Valid Data")]
    [MemberData(nameof(ProductParamData.GetProductsForUpdating), MemberType = typeof(ProductParamData))]
    public async Task UpdateProduct_WithValidData_ShouldReturnInt(UpdateProductRequest request)
    {
        // Arrange
        var handler = new UpdateProductCommandHandler(_mockProductService.Object);
        // Act
        var result = await handler.Handle(new UpdateProductCommand { Request = request }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact(DisplayName = "TC2: Update Product with Non-Existent Id")]
    public async Task UpdateProduct_WithNonExistentId_ShouldReturnFailure()
    {
        // Arrange
        var handler = new UpdateProductCommandHandler(_mockProductService.Object);
        var request = new UpdateProductRequest
        {
            Id = 999, // Non-existent Id
            Name = "Non-Existent Product",
            Description = "This product does not exist",
            Price = 0,
            CategoryId = 1
        };
        // Act
        var result = await handler.Handle(new UpdateProductCommand { Request = request }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeFalse();
    }
}