using System.Security.Claims;
using System.Text;
using Auth.Application.Interfaces.Service;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Identity;

public sealed class JwtTokenProvider : ITokenProvider
{
    private readonly JsonWebTokenHandler _tokenHandler = new();
    private readonly SymmetricSecurityKey _key;
    private readonly TokenValidationParameters _validationParameters;
    private readonly int _accessTokenExpiresMinutes;
    private readonly int _refreshTokenExpiresDays;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenProvider(IConfiguration configuration)
    {
        var secret = configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT:Key is missing");

        _issuer = configuration["JWT:Issuer"] ?? throw new InvalidOperationException("JWT:Issuer is missing");

        _audience = configuration["JWT:Audience"] ?? throw new InvalidOperationException("JWT:Audience is missing");

        if (!int.TryParse(configuration["JWT:AccessTokenExpires"], out _accessTokenExpiresMinutes))
        {
            throw new InvalidOperationException("JWT:AccessTokenExpires is invalid");
        }

        if (!int.TryParse(configuration["JWT:RefreshTokenExpires"], out _refreshTokenExpiresDays))
        {
            throw new InvalidOperationException("JWT:RefreshTokenExpires is invalid");
        }

        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }

    public Result<string> CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.UniqueName, user.Login.Value),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expires = DateTime.UtcNow.AddMinutes(_accessTokenExpiresMinutes);

        return CreateToken(claims, expires);
    }

    public Result<string> CreateRefreshToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expires = DateTime.UtcNow
            .AddDays(_refreshTokenExpiresDays);

        return CreateToken(claims, expires);
    }
    
    public async Task<Result<ClaimsPrincipal>> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Error.TokenProvider.InvalidToken;
        }

        var result = await _tokenHandler.ValidateTokenAsync(
            token,
            _validationParameters);

        if (!result.IsValid)
        {
            if (result.Exception is SecurityTokenExpiredException or SecurityTokenInvalidLifetimeException)
            {
                return Error.TokenProvider.ExpiredToken;
            }

            return Error.TokenProvider.InvalidToken;
        }

        if (result.ClaimsIdentity is null)
        {
            return Error.TokenProvider.InvalidToken;
        }

        return new ClaimsPrincipal(result.ClaimsIdentity);
    }
    
    private Result<string> CreateToken(IEnumerable<Claim> claims, DateTime expires)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = expires,

            Issuer = _issuer,
            Audience = _audience,

            SigningCredentials = new SigningCredentials(
                _key,
                SecurityAlgorithms.HmacSha256)
        };

        var token = _tokenHandler.CreateToken(descriptor);

        return token;
    }
}

