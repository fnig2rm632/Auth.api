using Auth.Application.Interfaces.Repository.Redis;
using Auth.Domain.Common;
using StackExchange.Redis;

namespace Auth.Infrastructure.Persistence.Redis;

public class RegistrationCodeRepository(IDatabase redis) : IRegistrationCodeRepository
{
    private const string Prefix = "user:rc:";
    private readonly TimeSpan _defaultDuration = TimeSpan.FromMinutes(5);
    private readonly IServer _server = redis.Multiplexer.GetServer(redis.Multiplexer.GetEndPoints().First());
    
    public async Task<Result<bool>> AddAsync(string email, HashEntry[]? code, TimeSpan? duration = null)
    {
        try
        {
            var expiry = duration ?? _defaultDuration;
            var key = $"{Prefix}{email}";
            var transaction = redis.CreateTransaction();
            
            var hashSetTask = transaction.HashSetAsync(key, code!);
            var expireTask = transaction.KeyExpireAsync(key, expiry);
        
            var result = await transaction.ExecuteAsync();
        
            await hashSetTask;
            await expireTask;

            return result;
        }
        catch (RedisConnectionException)
        {
            return Error.Redis.ConnectionFailed;
        }
        catch (RedisTimeoutException)
        {
            return Error.Redis.TimeoutGateway;
        }
        catch (RedisException ex) when (ex.Message.Contains("Out of Memory"))
        {
            return Error.Redis.OutOfMemory;
        }
        catch (Exception)
        {
            return  Error.Redis.InternalServer;
        }
    }

    public async Task<Result<List<HashEntry[]>>> GetAllByIdAsync(string email)
    {
        try
        {
            var key = $"{Prefix}{email}*";
            var result = new List<HashEntry[]>();
        
            await foreach (var pattern in _server.KeysAsync(pattern: key))
            {
                var hashEntries = await redis.HashGetAllAsync(pattern);
                if (hashEntries.Length > 0)
                {
                    result.Add(hashEntries);
                }
            }
        
            return result;
        }
        catch (RedisConnectionException)
        {
            return Error.Redis.ConnectionFailed;
        }
        catch (RedisTimeoutException)
        {
            return Error.Redis.TimeoutGateway;
        }
        catch (Exception)
        {
            return  Error.Redis.InternalServer;
        }
    }

    public async Task<Result<bool>> DeleteAllCodesByIdAsync(string email)
    {
        try
        {
            var pattern = $"{Prefix}{email}*";
            var keys = new List<RedisKey>();
        
            await foreach (var key in _server.KeysAsync(pattern: pattern))
            {
                keys.Add(key);
            }
        
            if (keys.Count == 0)
            {
                return false;
            }
        
            var transaction = redis.CreateTransaction();
            foreach (var key in keys)
            { 
                transaction.KeyDeleteAsync(key);
            }
        
            var committed = await transaction.ExecuteAsync();

            return committed;

        }
        catch (RedisConnectionException)
        {
            return Error.Redis.ConnectionFailed;
        }
        catch (RedisTimeoutException)
        {
            return Error.Redis.TimeoutGateway;
        }
        catch (RedisException ex) when (ex.Message.Contains("Out of Memory"))
        {
            return Error.Redis.OutOfMemory;
        }
        catch (Exception)
        {
            return  Error.Redis.InternalServer;
        }
    }
}