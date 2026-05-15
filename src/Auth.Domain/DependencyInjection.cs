using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainDi(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}