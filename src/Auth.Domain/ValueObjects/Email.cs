using System.Net.Mail;
using Auth.Domain.Common;

namespace Auth.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; private init; } = null!;

    private Email() { }

    public static Result<Email> Create(string value)
    {
        var success = ValidateCreationParameters(value);

        if (!success.IsSuccess)
            return success.Error;
        
        var email = new Email
        {
            Value = value.Trim().ToLowerInvariant()
        };

        return email;
    }
    
    public static Result<Email> Conversion(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Email.ValueCannotBeEmpty;

        var email = new Email
        {
            Value = value.ToLowerInvariant()
        };
        
        return email;
    }

    private static Result ValidateCreationParameters(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Error.Email.ValueCannotBeEmpty;
    
        if (email.Length > 254)
            return Error.Email.TooLong(email);
    
        if (!IsValidEmailFormat(email))
            return Error.Email.InvalidAddress(email);
    
        return Result.Success();
    }

    private static bool IsValidEmailFormat(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
    
}