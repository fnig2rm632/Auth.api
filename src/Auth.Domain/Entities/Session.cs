using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public class Session : Entity<Guid>
{
    public Guid UserId { get; private set; }
    public Guid DeviceId { get; private set; }
    public string RefreshTokenHash { get; private set; } = string.Empty;
    public string? IpAddress { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public User? User { get; private init; }
    public Device? Device { get; private init; }

    public static Result<Session> Create(
        Guid id, 
        Guid userId, 
        Guid deviceId, 
        string refreshTokenHash, 
        string ipAddress, 
        DateTime expiresAt,
        DateTime createdAt)
    {
        var success = ValidateCreationParameters(
            id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);

        if (!success.IsSuccess)
            return success.Error;
        
        return new Session
        {
            Id = id,
            UserId = userId,
            DeviceId = deviceId,
            RefreshTokenHash = refreshTokenHash,
            IpAddress = ipAddress,
            ExpiresAt = expiresAt,
            CreatedAt = createdAt
        };
    }

    private static Result ValidateCreationParameters(
        Guid id, 
        Guid userId, 
        Guid deviceId, 
        string refreshTokenHash, 
        string ipAddress, 
        DateTime expiresAt,
        DateTime createdAt)
    {
        if (id == Guid.Empty)
            return Error.Session.IdCannotBeEmpty;
        
        if (userId == Guid.Empty)
            return Error.User.IdCannotBeEmpty;
        
        if (deviceId == Guid.Empty)
            return Error.Device.IdCannotBeEmpty;
        
        if (string.IsNullOrWhiteSpace(refreshTokenHash))
            return Error.Session.RefreshTokenHashCannotBeEmpty;

        if (string.IsNullOrEmpty(ipAddress))
            return Error.Session.IpAddressCannotBeEmpty;
        
        var now = DateTime.UtcNow;
        
        if (expiresAt == DateTime.MinValue || expiresAt == DateTime.MaxValue)
            return Error.Session.InvalidExpirationDate;
        
        if (expiresAt < now.AddMinutes(-5))
            return Error.Session.ExpiresAtCannotBeInPast;
        
        if (createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue)
            return Error.Session.InvalidCreationDate;
        
        if (createdAt > now.AddMinutes(5))
            return Error.Session.CreationDateInFuture;
        
        if (createdAt > expiresAt)
            return Error.Session.CreationDateAfterExpiration;
        
        return Result.Success();
    }   
    
    public Result Revoke(DateTime? revokedAt)
    {
        if (revokedAt!.Value == DateTime.MinValue || revokedAt.Value == DateTime.MaxValue)
            return Error.Session.InvalidRevocationDate;
            
        if (revokedAt.Value < CreatedAt)
            return Error.Session.RevokedBeforeCreation;
            
        if (revokedAt.Value > DateTime.UtcNow.AddMinutes(5))
            return Error.Session.RevocationDateInFuture;
        
        if (IsRevoked)
            return Error.Session.AlreadyRevoked;

        RevokedAt = revokedAt;
        
        return Result.Success();
    }

    public Result ChangeRefreshTokenHash(string newRefreshTokenHash, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(newRefreshTokenHash))
            return Error.Session.RefreshTokenHashCannotBeEmpty;
        
        if (expiresAt < DateTime.UtcNow.AddMinutes(-1))
            return Error.Session.ExpiresAtCannotBeInPast;
        
        if (IsRevoked)
            return Error.Session.Revoked;
        
        RefreshTokenHash = newRefreshTokenHash;
        ExpiresAt = expiresAt;
        
        return Result.Success();
    }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive => !IsExpired && !IsRevoked;
}