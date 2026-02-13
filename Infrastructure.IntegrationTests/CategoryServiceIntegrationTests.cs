using Common.Requests.Categories;

using Infrastructure.Contexts;
using Infrastructure.IntegrationTests.Data;
using Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using System.Text.Json;

namespace Infrastructure.IntegrationTests;

public class CategoryServiceIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly CategoryService _categoryService;

    public CategoryServiceIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        _categoryService = new CategoryService(_context);

        // Seed data
        var initializingDataJson = File.ReadAllText("Data\\seeddata.json");
        var initializingData = JsonSerializer.Deserialize<DataInitializer>(
            initializingDataJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        initializingData.ShouldNotBeNull();
        initializingData.Categories.ShouldNotBeNull();
        initializingData.Products.ShouldNotBeNull();

        _context.Categories.AddRange(initializingData.Categories);
        _context.Products.AddRange(initializingData.Products);
        _context.SaveChanges();
    }

    [Theory(DisplayName = "TC1: Get Category By Valid ID")]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetCategoryById_With_Valid_Category_Returns_Category(int categoryId)
    {
        // Arrange
        //var category = _context.Categories.First();
        // Act
        var result = await _categoryService.GetByIdAsync(categoryId, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBeEquivalentTo(categoryId);

        //Assert.NotNull(result);
        //Assert.Equal(category.Id, result!.Id);
        //Assert.Equal(category.Name, result.Name);
    }

    [Theory(DisplayName = "TC2: Get Category By Invalid ID")]
    [InlineData(3)]
    [InlineData(4)]
    public async Task GetCategoryById_With_Invalid_CategoryId_Returns_Null(int categoryId)
    {
        // Act
        var result = await _categoryService.GetByIdAsync(categoryId, CancellationToken.None);
        // Assert
        result.ShouldBeNull();
        //Assert.Null(result);
    }

    [Fact(DisplayName = "TC3: Get All Categories - Data exists")]
    public async Task GetAllCategories_Returns_All_Categories()
    {
        // Arrange
        var expectedCount = _context.Categories.Count();
        // Act
        var result = await _categoryService.GetAllAsync(CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(expectedCount);
        //Assert.NotNull(result);
        //Assert.Equal(expectedCount, result.Count());
    }

    [Fact(DisplayName = "TC4: Get All Categories - No Data")]
    public async Task GetAllCategories_Returns_Empty_List_When_No_Categories()
    {
        // Arrange
        _context.Categories.RemoveRange(_context.Categories);
        _context.SaveChanges();
        // Act
        var result = await _categoryService.GetAllAsync(CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
        //Assert.NotNull(result);
        //Assert.Empty(result);
    }

    [Fact(DisplayName = "TC5: Get Paginated Categories - Valid Page")]
    public async Task GetPaginatedCategories_Returns_Paginated_Categories()
    {
        // Arrange
        var request = new CategoryFilterRequest
        {
            PageNumber = 1,
            PageSize = 1
        };
        // Act
        var result = await _categoryService.GetPaginatedAsync(request, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.Count().ShouldBe(1);
        //Assert.NotNull(result);
        //Assert.NotNull(result.Data);
        //Assert.Single(result.Data);
    }

    public void Dispose() => _context.Dispose();
}
