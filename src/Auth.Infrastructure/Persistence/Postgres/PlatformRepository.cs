using Auth.Application.Interfaces.Repository.Postgres;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Auth.Infrastructure.Persistence.Postgres;

public class PlatformRepository(DatabaseContext context) : IPlatformRepository
{
    public async Task<Result<Platform?>> GetByIdAsync(int id)
    {
        try
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
                
            return platform;
        }
        catch (NpgsqlException)
        {
            return Error.Database.ConnectionFailed;
        }
        catch (TimeoutException)
        {
            return Error.Database.TimeoutGateway;
        }
        catch (Exception)
        {
            return Error.Database.InternalServer;
        }
    }

    public async Task<Result<Platform?>> GetByNameAsync(string platformName)
    {
        try
        {
            var platform = await context.Platforms
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == platformName);
                
            return platform;
        }
        catch (NpgsqlException)
        {
            return Error.Database.ConnectionFailed;
        }
        catch (TimeoutException)
        {
            return Error.Database.TimeoutGateway;
        }
        catch (Exception)
        {
            return Error.Database.InternalServer;
        }
    }

    public async Task<Result<int?>> AddAsync(Platform platform)
    {
        try
        {
            await context.Platforms.AddAsync(platform);
            await context.SaveChangesAsync();
        
            return platform.Id;
        }
        catch (NpgsqlException)
        {
            return Error.Database.ConnectionFailed;
        }
        catch (TimeoutException)
        {
            return Error.Database.TimeoutGateway;
        }
        catch (Exception)
        {
            return Error.Database.InternalServer;
        }
    }
}