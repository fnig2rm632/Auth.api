using Auth.Domain.Common;

namespace Auth.Domain.ValueObjects;

public sealed class Password : ValueObject
{
    public string Value { get; private init; } = null!;

    private Password() { }

    public static Result<Password> Create(string value, PasswordPolicy policy)
    {
        var success = ValidateCreationParameters(value, policy);
        
        if (!success.IsSuccess)
            return success.Error;

        var password = new Password
        {
            Value = value
        };
        
        return password;
    }

    private static Result ValidateCreationParameters(string password, PasswordPolicy policy)
    {
        if (string.IsNullOrWhiteSpace(password))
            return Error.Password.ValueCannotBeEmpty;

        if (password.Length < policy.MinLength)
            return Error.Password.TooShort(policy.MinLength);

        if (password.Length > policy.MaxLength)
            return Error.Password.TooLong(policy.MaxLength);

        if (password.Any(char.IsWhiteSpace))
            return Error.Password.ContainsWhitespace;

        if (!password.Any(char.IsLetter))
            return Error.Password.LackOfLetter;

        if (!password.Any(char.IsDigit))
            return Error.Password.LackOfNumber;
        
        if (!password.Any(char.IsUpper))
            return Error.Password.LackUppercase;

        if (!password.Any(char.IsLower))
            return Error.Password.LackLowercase;

        if (!password.Any(ch => policy.RequireSimbol.Contains(ch)))
            return Error.Password.LackOfSpecialCharacters;
        
        return Result.Success();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}