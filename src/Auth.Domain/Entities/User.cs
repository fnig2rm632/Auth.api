using Auth.Domain.Common;
using Auth.Domain.ValueObjects;

namespace Auth.Domain.Entities;

public sealed class User : Entity<Guid>
{
    public Login Login { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public Email Email { get; private init; } = null!;
    public DateTime CreatedAt { get; private set; }
    
    public ICollection<UserIdentity> UserIdentities { get; private set; } = new List<UserIdentity>();
    public ICollection<Session> Sessions { get; private set; } = new List<Session>(); 

    public static Result<User> Create(Guid id, Login login, Email email, string passwordHash, DateTime createdAt)
    {
        var success = ValidateCreationParameters(id, login, email, passwordHash, createdAt);

        if (!success.IsSuccess)
            return success.Error;
        
        var user = new User
        {
            Id = id,
            Login = login,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = createdAt
        };
        
        return user;
    }
    
    private static Result ValidateCreationParameters(
        Guid id, Login login, Email email, string passwordHash, DateTime createdAt)
    {
        if (id == Guid.Empty)
            return Error.User.IdCannotBeEmpty;

        if (login == null!)
            return Error.User.LoginCannotBeNull;
        
        if (email == null!)
            return Error.User.EmailCannotBeNull;
        
        if (string.IsNullOrEmpty(passwordHash))
            return Error.User.PasswordHashCannotBeNull;
                
        if (createdAt >= DateTime.UtcNow.AddMinutes(2))
            return Error.User.CreatedAtInFuture;

        return Result.Success();
    }

    public Result ResetPassword(string passwordHash)
    {
        if (string.IsNullOrEmpty(passwordHash))
            return Error.User.PasswordHashCannotBeNull;

        if (PasswordHash == passwordHash)
            return Error.User.NewPasswordMustBeDifferent;
            
        return Result.Success();
    }
}
