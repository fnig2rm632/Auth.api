using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.Domain.ValueObjects;

namespace Auth.Application.Interfaces.Repository.Postgres;

public interface IUserRepository
{
    Task<Result<User?>> GetByIdAsync(Guid id);
    Task<Result<User?>> GetByLoginAsync(Login login);
    Task<Result<User?>> GetByEmailAsync(Email email);
    Task<Result<bool?>> AddAsync(User user);
    Task<Result<bool?>> UpdateAsync(User user);
}