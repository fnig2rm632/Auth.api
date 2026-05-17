using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public class UserIdentity : Entity<Guid>
{
    public Guid UserId { get; private set; }
    public int ProviderId { get; private set; }
    public string ProviderUserId { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public ICollection<OAuthToken> OAuthTokens { get; private set; } = new List<OAuthToken>();
    
    public static Result<UserIdentity> Create(
        Guid id, Guid userId, int providerId, string providerUserId, DateTime createdAt)
    {
        var success = ValidateCreationParameters(id, userId, providerId, providerUserId, createdAt);

        if (!success.IsSuccess)
            return success.Error;
        
        return new UserIdentity()
        {
            Id = id,
            UserId =  userId,
            ProviderId =  providerId,
            ProviderUserId =  providerUserId,
            CreatedAt =  createdAt
        };
    }

    private static Result ValidateCreationParameters(
        Guid id, Guid userId, int providerId, string providerUserId, DateTime createdAt)
    {
        if (id == Guid.Empty)
            return Error.UserIdentity.IdCannotBeEmpty;
        
        if (userId == Guid.Empty)
            return Error.User.IdCannotBeEmpty;
        
        if (providerId <= 0)
            return Error.Provider.IdCannotBeNegative;
        
        if (string.IsNullOrWhiteSpace(providerUserId))
            return Error.UserIdentity.ProviderUserIdCannotBeEmpty ;
    
        if (providerUserId.Length > 255)
            return Error.UserIdentity.ProviderUserIdTooLong;
    
        if (createdAt > DateTime.UtcNow.AddMinutes(5))
            return Error.UserIdentity.CreatedAtInFuture;
        
        return Result.Success();
    }
}