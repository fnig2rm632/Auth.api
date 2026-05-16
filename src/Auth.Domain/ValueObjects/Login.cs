using System.Text.RegularExpressions;
using Auth.Domain.Common;

namespace Auth.Domain.ValueObjects;

public sealed class Login : ValueObject
{
    public string Value { get; private init; } = null!;

    private Login() { }

    public static Result<Login> Create(string value, LoginPolicy policy)
    {
        var success = ValidateCreationParameters(value, policy);

        if (!success.IsSuccess)
            return success.Error;

        var login = new Login
        {
            Value = value.ToLowerInvariant()
        };

        return login;
    }

    public static Result<Login> Conversion(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Login.ValueCannotBeEmpty;

        var login = new Login
        {
            Value = value.ToLowerInvariant()
        };
        
        return login;
    }

    private static Result ValidateCreationParameters(string login, LoginPolicy policy)
    {
        if (string.IsNullOrWhiteSpace(login))
            return Error.Login.ValueCannotBeEmpty;

        var trimmedLogin = login.Trim();
        
        if (login.Any(char.IsWhiteSpace))
            return Error.Login.ContainsWhitespace;
        
        if (trimmedLogin.Length < policy.MinLength)
            return Error.Login.TooShort;

        if (trimmedLogin.Length > policy.MaxLength)
            return Error.Login.TooLong;
        
        if (!policy.AllowCapitalLetters && Regex.IsMatch(trimmedLogin, @"[A-Z]"))
            return Error.Login.InvalidCapitalLetters;
        
        if (!policy.AllowNumbers && Regex.IsMatch(trimmedLogin, @"\d"))
            return Error.Login.InvalidNumbers;
        
        if (!policy.AllowSpecialCharacters && Regex.IsMatch(trimmedLogin, $"[{Regex.Escape(policy.SpecialCharacters!)}]"))
            return Error.Login.InvalidSpecialCharacters(policy.SpecialCharacters!);

        if (policy.AllowReservedWords && policy.ReservedWords!.Contains(login.ToLowerInvariant()))
            return Error.Login.ContainsInvalidWords;
        
        return Result.Success();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}