using Auth.Application.Interfaces.Repository.Redis;
using Auth.Domain.Common;
using StackExchange.Redis;

namespace Auth.Infrastructure.Persistence.Redis;

public class BlacklistRepository(IDatabase redis) : IBlacklistRepository
{
    private const string Prefix = "sess:bl:";
    private readonly TimeSpan _defaultDuration = TimeSpan.FromMinutes(30);

    public async Task<Result<bool>> AddAsync(string sessionId, TimeSpan? duration = null)
    {
        try
        {
            var expiry = duration ?? _defaultDuration;
            var key = $"{Prefix}{sessionId}";

            return await redis.StringSetAsync(key, "", expiry);
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

    public async Task<Result<bool>> ExistsAsync(string sessionId)
    {
        try
        {
            var key = $"{Prefix}{sessionId}";
            return await redis.KeyExistsAsync(key);
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

    public async Task<Result<bool>> DeleteAsync(string sessionId)
    {
        try
        {
           var key = $"{Prefix}{sessionId}";
           return await redis.KeyDeleteAsync(key);
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