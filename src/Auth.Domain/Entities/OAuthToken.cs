using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public class OAuthToken : Entity<Guid>
{
    public Guid UserIdentityId { get; private set; }
    public string AccessToken { get; private set; } = string.Empty;
    public string? RefreshToken { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    public UserIdentity? UserIdentity { get; private init; } = new();
 
    public static Result<OAuthToken> Create(
        Guid id, Guid userIdentityId, string accessToken, string refreshToken, DateTime expiresAt)
    {
        var success = ValidateCreationParameter(id, userIdentityId, accessToken, refreshToken, expiresAt);

        if (!success.IsSuccess)
            return success.Error;
        
        return new OAuthToken
        {
            Id = id,
            UserIdentityId = userIdentityId,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        };
    }

    private static Result ValidateCreationParameter(
        Guid id, Guid userIdentityId, string accessToken, string refreshToken, DateTime expiresAt)
    {
        if (id == Guid.Empty)
            return Error.OAuthToken.IdCannotBeEmpty;
        
        if (userIdentityId == Guid.Empty)
            return Error.UserIdentity.IdCannotBeEmpty;
        
        if (string.IsNullOrWhiteSpace(accessToken))
            return Error.OAuthToken.AccessTokenCannotBeEmpty;
    
        if (string.IsNullOrEmpty(refreshToken))
            return Error.OAuthToken.RefreshTokenCannotBeEmpty;
        
        if (expiresAt == DateTime.MinValue || expiresAt == DateTime.MaxValue)
            return Error.OAuthToken.InvalidExpirationDate;
        
        if (expiresAt < DateTime.Now.AddMinutes(-5))
            return Error.Session.ExpiresAtCannotBeInPast;
        
        return Result.Success();
    }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}