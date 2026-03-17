using Application.Features.Categories.Queries;
using Application.Services;

using AutoMapper;

using Common.Responses.Categories;

using Domain;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Categories.Queries;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Mock<IMapper> _mockMapper = new();

    public GetCategoryByIdQueryHandlerTests()
    {
        _categoryServiceMock = MockCategoryService.GetCategoryServiceMocks();
        _mockMapper
            .Setup(m => m.Map<CategoryResponse>(It.IsAny<Category>()))
            .Returns((Category source) => new CategoryResponse
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
            });
    }

    [Theory(DisplayName = "TC1: Get Category by Valid Id")]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetCategoryById_WithValidId_ShouldReturnCategory(int categoryId)
    {
        // Arrange
        var handler = new GetCategoryByIdQueryHandler(_categoryServiceMock.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new GetCategoryByIdQuery { CategoryId = categoryId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Id.ShouldBe(categoryId);
    }

    [Theory(DisplayName = "TC2: Get Category by Non-Existent Id")]
    [InlineData(999)]
    [InlineData(1000)]
    public async Task GetCategoryById_WithNonExistentId_ShouldReturnFail(int categoryId)
    {
        // Arrange
        var handler = new GetCategoryByIdQueryHandler(_categoryServiceMock.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new GetCategoryByIdQuery { CategoryId = categoryId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.Data.ShouldBeNull();
    }
}
