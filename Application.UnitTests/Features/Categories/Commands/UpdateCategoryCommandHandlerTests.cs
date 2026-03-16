using Application.Features.Categories.Commands;
using Application.Services;

using Common.Requests.Categories;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Categories.Commands;

public class UpdateCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;

    public UpdateCategoryCommandHandlerTests()
    {
        _categoryServiceMock = MockCategoryService.GetCategoryServiceMocks();  
    }

    [Theory(DisplayName = "TC1: Update Category with Valid Data")]
    [MemberData(nameof(CategoryParamData.GetValidCategoryDataForUpdate), MemberType = typeof(CategoryParamData))]
    public async Task UpdateCategory_WithValidData_ShouldReturnInt(UpdateCategoryRequest request)
    {
        // Arrange
        var handler = new UpdateCategoryCommandHandler(_categoryServiceMock.Object);
        // Act
        var result = await handler.Handle(new UpdateCategoryCommand { Request = request }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
    }
}
