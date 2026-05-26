using Auth.Application.Interfaces.Service;
using Auth.Domain.Common;
using Auth.Domain.ValueObjects;

namespace Auth.Infrastructure.Services;

public class BCryptPasswordHasher : IPasswordHasher 
{
    public async Task<Result<string>> HashAsync(Password password)
    {
        try
        {
            return await Task.Run(() => 
                BCrypt.Net.BCrypt.HashPassword(password.Value));
        }
        catch
        {
            return Error.PasswordHasher.CannotConvertInHash;
        }
    }

    public async Task<Result<bool>> VerifyAsync(Password password, string passwordHash)
    {
        try
        {
            if (string.IsNullOrEmpty(passwordHash))
                return Error.PasswordHasher.PasswordHashCannotBeEmpty;
                
            return await Task.Run(() => 
                BCrypt.Net.BCrypt.Verify(password.Value, passwordHash));
        }
        catch
        {
            return Error.PasswordHasher.CannotComparePasswordAndHash;
        }
    }
}