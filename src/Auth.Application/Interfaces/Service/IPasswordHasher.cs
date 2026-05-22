using Auth.Domain.Common;
using Auth.Domain.ValueObjects;

namespace Auth.Application.Interfaces.Service;

public interface IPasswordHasher
{
    Task<Result<string>> HashAsync(Password password);
    Task<Result<bool>> VerifyAsync(Password password, string passwordHash);
}