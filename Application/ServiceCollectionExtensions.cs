using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly(); // Get the current assembly
        return services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly))
            .AddAutoMapper(cfg => cfg.AddMaps(assembly));
    }
}
