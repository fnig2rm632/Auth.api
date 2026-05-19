using Auth.Application.Interfaces.Repository.Postgres;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Auth.Infrastructure.Persistence.Postgres;

public class SessionRepository(DatabaseContext context) : ISessionRepository
{
    public async Task<Result<Session?>> GetByIdAsync(Guid id)
    {
        try
        {
            var session = await context.Sessions
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
                
            return session;
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

    public async Task<Result<List<Session?>>> GetListByIdUserAsync(Guid userId)
    {
        try
        {
            var sessions = await context.Sessions
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .ToListAsync();
                
            return sessions!;
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

    public async Task<Result<List<Session?>>> GetListByIdDeviceAsync(Guid deviceId)
    {
        try
        {
            var sessions = await context.Sessions
                .AsNoTracking()
                .Where(s => s.DeviceId == deviceId)
                .ToListAsync();
                
            return sessions!;
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

    public async Task<Result<bool?>> AddAsync(Session session)
    {
        try
        {
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync();
            
            return true;
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

    public async Task<Result<bool?>> UpdateAsync(Session session)
    {
        try
        {
            var count = await context.Sessions
                .Where(u => u.Id == session.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.RefreshTokenHash, session.RefreshTokenHash)
                    .SetProperty(u => u.IpAddress, session.IpAddress)
                    .SetProperty(u => u.RevokedAt, session.RevokedAt)
                    .SetProperty(u => u.ExpiresAt, session.ExpiresAt));
    
            await context.SaveChangesAsync();
            
            return count > 0;
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