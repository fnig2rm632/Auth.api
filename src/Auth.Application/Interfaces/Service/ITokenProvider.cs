using System.Security.Claims;
using Auth.Domain.Common;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Service;

public interface ITokenProvider
{
    Result<string> CreateAccessToken(User user);
    Result<string> CreateRefreshToken(User user);
    Task<Result<ClaimsPrincipal>> ValidateTokenAsync(string token);
}