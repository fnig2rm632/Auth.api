using Auth.Domain.Common;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Repository.Postgres;

public interface IPlatformRepository
{
    Task<Result<Platform?>> GetByIdAsync(int platformId);
    Task<Result<Platform?>> GetByNameAsync(string platformName);
    Task<Result<int?>> AddAsync(Platform platform);
}