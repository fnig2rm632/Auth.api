using Auth.Application;

namespace Auth.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiDi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDi(configuration);
        
        return services;
    }
}