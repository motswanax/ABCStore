using Application.Services;

using Infrastructure.Contexts;
using Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

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
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
