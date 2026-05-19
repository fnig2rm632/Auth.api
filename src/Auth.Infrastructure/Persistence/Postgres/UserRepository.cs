using Auth.Application.Interfaces.Repository.Postgres;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.Domain.ValueObjects;
using Auth.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Auth.Infrastructure.Persistence.Postgres;

public class UserRepository(DatabaseContext context) : IUserRepository
{
    public async Task<Result<User?>> GetByIdAsync(Guid id)
    {
        try
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
                
            return user;
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

    public async Task<Result<User?>> GetByLoginAsync(Login login)
    {
        try
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login);
                
            return user;
        }
        catch (NpgsqlException)
        {
            return Error.Database.ConnectionFailed;
        }
        catch (TimeoutException)
        {
            return Error.Database.TimeoutGateway;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Error.Database.InternalServer;
        }
    }

    public async Task<Result<User?>> GetByEmailAsync(Email email)
    {
        try
        {
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
                
            return user;
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

    public async Task<Result<bool?>> AddAsync(User user)
    {
        try
        {
            await context.Users.AddAsync(user);
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

    public async Task<Result<bool?>> UpdateAsync(User user)
    {
        try
        {
            var count = await context.Users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.PasswordHash, user.PasswordHash)
                    .SetProperty(u => u.Email, user.Email)
                    .SetProperty(u => u.Login, user.Login));
    
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