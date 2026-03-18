using Application.Features.Categories.Queries;
using Application.Services;

using Common.Requests.Categories;

using Moq;

using Shouldly;

namespace Application.UnitTests.Features.Categories.Queries;

public class GetPaginatedCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Mock<ICategoryService> _emptyCategoryServiceMock;

    public GetPaginatedCategoriesQueryHandlerTests()
    {
        _categoryServiceMock = MockCategoryService.GetCategoryServiceMocks();
        _emptyCategoryServiceMock = MockCategoryService.GetEmptyCategoryServiceMocks();
    }

    [Fact(DisplayName = "TC1: Get Paginated Categories")]
    public async Task GetPaginatedCategories_ShouldReturnValidPage()
    {
        // Arrange
        var handler = new GetPaginatedCategoriesQueryHandler(_categoryServiceMock.Object);
        var request = new CategoryFilterRequest
        {
            PageNumber = 1,
            PageSize = 2,
            SortBy = "id",
            SortDescending = false
        };

        // Act
        var result = await handler.Handle(new GetPaginatedCategoriesQuery { Request = request }, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.Count.ShouldBe(2);
        result.TotalRecords.ShouldBe(5);
        result.PageNumber.ShouldBe(1);
        result.PageSize.ShouldBe(2);
        result.TotalPages.ShouldBe(3);
    }

    [Fact(DisplayName = "TC2: Get Paginated Categories - No Data Exists")]
    public async Task GetPaginatedCategories_ShouldReturnEmptyPage_WhenNoDataExists()
    {
        // Arrange
        var handler = new GetPaginatedCategoriesQueryHandler(_emptyCategoryServiceMock.Object);
        var request = new CategoryFilterRequest
        {
            PageNumber = 1,
            PageSize = 10,
            SortBy = "id",
            SortDescending = false
        };

        // Act
        var result = await handler.Handle(new GetPaginatedCategoriesQuery { Request = request }, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
        result.TotalRecords.ShouldBe(0);
        result.PageNumber.ShouldBe(1);
        result.PageSize.ShouldBe(10);
        result.TotalPages.ShouldBe(0);
    }
}
