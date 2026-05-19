using Auth.Domain.Common;
using StackExchange.Redis;

namespace Auth.Application.Interfaces.Repository.Redis;

public interface IRegistrationCodeRepository
{
    Task<Result<bool>> AddAsync(string email, HashEntry[] code, TimeSpan? duration = null);
    Task<Result<List<HashEntry[]>>> GetAllByIdAsync(string email);
    Task<Result<bool>> DeleteAllCodesByIdAsync(string email);

}