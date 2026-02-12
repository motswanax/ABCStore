using Infrastructure.Contexts;
using Infrastructure.IntegrationTests.Data;
using Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

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
        var initializingData = JsonSerializer.Deserialize<DataInitializer>(initializingDataJson);
        _context.Categories.AddRange(initializingData!.Categories);
        _context.Products.AddRange(initializingData.Products);
        _context.SaveChanges();
    }

    public void Dispose() => _context.Dispose();
}
