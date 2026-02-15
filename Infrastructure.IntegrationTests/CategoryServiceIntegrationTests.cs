using Common.Requests.Categories;

using Domain;

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

    [Fact(DisplayName = "TC6: Get Paginated Categories - Invalid Page")]
    public async Task GetPaginatedCategories_Returns_Empty_When_Page_Out_Of_Range()
    {
        // Arrange
        var request = new CategoryFilterRequest
        {
            PageNumber = 100,
            PageSize = 10
        };
        // Act
        var result = await _categoryService.GetPaginatedAsync(request, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldBeEmpty();
        //Assert.NotNull(result);
        //Assert.NotNull(result.Data);
        //Assert.Empty(result.Data);
    }

    [Theory(DisplayName = "TC7: Create Category - Valid Data")]
    [MemberData(nameof(CategoryParamData.GetValidCategoriesForCreation), MemberType = typeof(CategoryParamData))]
    public async Task CreateCategory_With_Valid_Category_Creates_Category(Category category)
    {
        // Act
        var result = await _categoryService.CreateAsync(category, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(category);
        result.Id.ShouldBeGreaterThan(0);
        result.Name.ShouldBe(category.Name);
        result.Description.ShouldBe(category.Description);
        //Assert.NotNull(result);
        //Assert.True(result.Id > 0);
        //Assert.Equal(newCategory.Name, result.Name);
        //Assert.Equal(newCategory.Description, result.Description);
    }

    [Theory(DisplayName = "TC8: Update Category - Valid Data")]
    [MemberData(nameof(CategoryParamData.GetValidCategoriesForUpdating), MemberType = typeof(CategoryParamData))]
    public async Task UpdateCategory_With_Valid_Category_Updates_Category(Category category)
    {
        // Arrange
        var existingCategory = await _categoryService.GetByIdAsync(category.Id, CancellationToken.None);
        existingCategory.ShouldNotBeNull();
        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        // Act
        var result = await _categoryService.UpdateAsync(existingCategory, CancellationToken.None);
        // Assert
        result.ShouldNotBe(0);
        result.ShouldBeEquivalentTo(category.Id);
        //Assert.NotNull(result);
        //Assert.Equal(category.Id, result.Id);
        //Assert.Equal(category.Name, result.Name);
        //Assert.Equal(category.Description, result.Description);
    }

    public void Dispose() => _context.Dispose();
}
