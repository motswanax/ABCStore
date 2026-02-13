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

    [Theory]
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

    public void Dispose() => _context.Dispose();
}
