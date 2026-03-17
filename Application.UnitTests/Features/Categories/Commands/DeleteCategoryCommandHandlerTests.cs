using Application.Features.Categories.Commands;
using Application.Services;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Categories.Commands;

public class DeleteCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;

    public DeleteCategoryCommandHandlerTests()
    {
        _categoryServiceMock = MockCategoryService.GetCategoryServiceMocks();
    }

    [Theory(DisplayName = "TC1: Delete Category with Valid Id")]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteCategory_WithValidId_ShouldReturnInt(int categoryId)
    {
        // Arrange
        var handler = new DeleteCategoryCommandHandler(_categoryServiceMock.Object);
        // Act
        var result = await handler.Handle(new DeleteCategoryCommand { CategoryId = categoryId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }

    [Theory(DisplayName = "TC2: Delete Category with Non-Existent Id")]
    [InlineData(999)]
    [InlineData(1000)]
    public async Task DeleteCategory_WithNonExistentId_ShouldReturnFail(int categoryId)
    {
        // Arrange
        var handler = new DeleteCategoryCommandHandler(_categoryServiceMock.Object);
        // Act
        var result = await handler.Handle(new DeleteCategoryCommand { CategoryId = categoryId }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeFalse();
    }
}
