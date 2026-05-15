using Auth.Domain;
using Auth.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainDi(configuration);
        services.AddInfrastructureDi(configuration);
        
        return services;
    }
}