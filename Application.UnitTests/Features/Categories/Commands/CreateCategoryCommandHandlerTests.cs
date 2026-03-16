using Application.Features.Categories.Commands;
using Application.Services;

using AutoMapper;

using Common.Requests.Categories;

using Domain;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Categories.Commands;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryService> _mockCategoryService;
    private readonly Mock<IMapper> _mockMapper = new();

    public CreateCategoryCommandHandlerTests()
    {
        _mockCategoryService = MockCategoryService.GetCategoryServiceMocks();

        _mockMapper
            .Setup(m => m.Map<CreateCategoryRequest, Category>(It.IsAny<CreateCategoryRequest>()))
            .Returns((CreateCategoryRequest r) => new Category { Name = r.Name, Description = r.Description });
    }

    [Theory(DisplayName = "TC1: Create Category with Valid Data")]
    [MemberData(nameof(CategoryParamData.GetValidCategoryData), MemberType = typeof(CategoryParamData))]
    public async Task CreateCategory_WithValidData_ShouldReturnCreatedCategory(CreateCategoryRequest request)
    {
        // Arrange
        var handler = new CreateCategoryCommandHandler(_mockCategoryService.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new CreateCategoryCommand { Request = request }, CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.ShouldNotBeNull();
    }
}