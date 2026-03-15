using Application.Services;

using Common.Requests.Products;

using Domain;

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

    [Theory(DisplayName = "TC4: Get Products by Invalid Category ID")]
    [InlineData(3)]
    [InlineData(4)]
    public async Task GetProductsByCategoryId_With_Invalid_Category_Returns_Empty(int categoryId)
    {
        // Act
        var result = await _productService.GetProductsByCategoryIdAsync(categoryId, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact(DisplayName = "TC5: Get All Products")]
    public async Task GetAllProducts_Returns_All_Products()
    {
        // Act
        var result = await _productService.GetAllAsync(CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(_context.Products.Count());
    }

    [Fact(DisplayName = "TC6: Get All Products - No Products Exist")]
    public async Task GetAllProducts_Returns_Empty_Products()
    {
        // Arrange
        _context.Products.RemoveRange(_context.Products);
        await _context.SaveChangesAsync();
        // Act
        var result = await _productService.GetAllAsync(CancellationToken.None);
        // Assert
        result.ShouldBeEmpty();
        result.Count().ShouldBe(0);
    }

    [Fact(DisplayName = "TC7: Get Paginated Products - Valid Page")]
    public async Task GetPaginatedProducts_Returns_Valid_Page()
    {
        // Arrange
        var request = new ProductFilterRequest
        {
            PageNumber = 1,
            PageSize = 1
        };

        // Act
        var result = await _productService.GetPaginatedAsync(request, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull(); 
        result.Data.Count().ShouldBe(1);
    }

    [Theory(DisplayName = "TC8: Create Product - Valid Data")]
    [MemberData(nameof(ProductParamData.GetProductsForCreation), MemberType = typeof(ProductParamData))]
    public async Task CreateProduct_With_Valid_Data_Returns_Created_Product(Product product)
    {
        // Act
        var result = await _productService.CreateAsync(product, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThan(0);
    }

    [Fact(DisplayName = "TC9: Create Product - Invalid Data")]
    public async Task CreateProduct_With_Invalid_Data_Returns_Null()
    {
        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(async () =>
        {
            await _productService.CreateAsync(null, CancellationToken.None);
        });
    }

    public void Dispose() => _context.Dispose();
}
