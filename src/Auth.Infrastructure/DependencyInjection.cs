using Auth.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDi(this IServiceCollection services, IConfiguration configuration)
    {
        // Registration db context
        var hostDb = configuration["ConnectionPostgres:Host"];;
        var portDb = configuration["ConnectionPostgres:Port"];;
        var databaseDb = configuration["ConnectionPostgres:Database"];;
        var usernameDb = configuration["ConnectionPostgres:Username"];;
        var passwordDb = configuration["ConnectionPostgres:Password"];;
        
        var connectionString = 
            $"Host={hostDb};" +
            $"Port={portDb};" +
            $"Database={databaseDb};" +
            $"Username={usernameDb};" +
            $"Password={passwordDb}";
        
        services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
        
        return services;
    }
}