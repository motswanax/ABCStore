using Application.Features.Categories.Queries;
using Application.Services;

using AutoMapper;

using Common.Responses.Categories;

using Domain;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Categories.Queries;

public class GetCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Mock<IMapper> _mockMapper = new();

    public GetCategoriesQueryHandlerTests()
    {
        _categoryServiceMock = MockCategoryService.GetCategoryServiceMocks();
        _mockMapper
            .Setup(m => m.Map<IEnumerable<CategoryResponse>>(It.IsAny<IEnumerable<Category>>()))
            .Returns((IEnumerable<Category> source) => [.. source.Select(s => new CategoryResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
            })]);
    }

    [Fact(DisplayName = "TC1: Get All Categories")]
    public async Task GetCategories_ShouldReturnAllCategories()
    {
        // Arrange
        var handler = new GetCategoriesQueryHandler(_categoryServiceMock.Object, _mockMapper.Object);
        // Act
        var result = await handler.Handle(new GetCategoriesQuery(), CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.Count().ShouldBeGreaterThan(0);
        result.Data.Count().ShouldBe(5);
    }

    [Fact(DisplayName = "TC2: Get All Categories - No Data Exists")]
    public async Task GetCategories_ShouldReturnEmptyCategories()
    {
        // Arrange
        var emptyCategoryServiceMock = MockCategoryService.GetEmptyCategoryServiceMocks();
        var handler = new GetCategoriesQueryHandler(emptyCategoryServiceMock.Object, _mockMapper.Object);
        var expectedResponse = new List<CategoryResponse>();
        _mockMapper
            .Setup(m => m.Map<IEnumerable<CategoryResponse>>(It.IsAny<IEnumerable<Category>>()))
            .Returns(expectedResponse);
        // Act
        var result = await handler.Handle(new GetCategoriesQuery(), CancellationToken.None);
        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
    }
}
