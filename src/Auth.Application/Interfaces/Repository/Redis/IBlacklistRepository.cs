using Auth.Domain.Common;

namespace Auth.Application.Interfaces.Repository.Redis;

public interface IBlacklistRepository
{
    Task<Result<bool>> AddAsync(string sessionId, TimeSpan? duration = null);
    Task<Result<bool>> ExistsAsync(string sessionId);
    Task<Result<bool>> DeleteAsync(string sessionId);
}