using Application.Services;

using Infrastructure.Contexts;
using Infrastructure.IntegrationTests.Data;
using Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

using Shouldly;

using System.Text.Json;

namespace Infrastructure.IntegrationTests;

public class ProductServiceIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IProductService _productService;

    public ProductServiceIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        _productService = new ProductService(_context);

        // Seed database
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

    [Theory(DisplayName = "TC1: Get Product By Valid ID")]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetProductById_With_Valid_Product_Returns_Product(int productId)
    {
        // Act
        var result = await _productService.GetByIdAsync(productId, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBeEquivalentTo(productId);
    }

    [Theory(DisplayName = "TC2: Get Product By Invalid ID")]
    [InlineData(5)]
    [InlineData(6)]
    public async Task GetProductById_With_Invalid_Product_Returns_Null(int productId)
    {
        // Act
        var result = await _productService.GetByIdAsync(productId, CancellationToken.None);
        // Assert
        result.ShouldBeNull();
    }

    [Theory(DisplayName = "TC3: Get Products by Valid Category ID")]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    public async Task GetProductsByCategoryId_With_Valid_Category_Returns_Products(int categoryId, int expectedCount)
    {
        // Act
        var result = await _productService.GetProductsByCategoryIdAsync(categoryId, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(expectedCount);
    }

    public void Dispose() => _context.Dispose();
}
