using Application.Services;

using Infrastructure.Contexts;
using Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        // Register infrastructure services here (e.g., database context, repositories, etc.)
        return services.AddTransient<ICategoryService, CategoryService>()
                       .AddTransient<IProductService, ProductService>();
    }
}
