using Application.Features.Products.Commands;
using Application.Services;

using AutoMapper;

using Common.Requests.Products;

using Domain;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Products.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IMapper> _mockMapper = new();

    public CreateProductCommandHandlerTests()
    {
        _mockProductService = MockProductService.GetProductServiceMocks();
        _mockMapper
            .Setup(m => m.Map<CreateProductRequest, Product>(It.IsAny<CreateProductRequest>()))
            .Returns((CreateProductRequest r) => new Product
            {
                Name = r.Name,
                Description = r.Description,
                Price = r.Price,
                CategoryId = r.CategoryId
            });
    }

    [Theory(DisplayName = "TC1: Create Product with Valid Data")]
    [MemberData(nameof(ProductParamData.GetProductsForCreation), MemberType = typeof(ProductParamData))]
    public async Task CreateProduct_WithValidData_ShouldReturnCreatedProduct(CreateProductRequest request)
    {
        // Arrange
        var handler = new CreateProductCommandHandler(_mockProductService.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new CreateProductCommand { Request = request }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.ShouldNotBeNull();
    }
}
