using Auth.Domain.Common;

namespace Auth.Domain.ValueObjects;

public class LoginPolicy  : ValueObject
{
    public int MinLength { get; private init; } 
    public int MaxLength { get; private init; }
    public bool AllowNumbers { get; private init; }
    public bool AllowCapitalLetters { get; private init; }
    public bool AllowSpecialCharacters { get; private init; }
    public string? SpecialCharacters { get; private init; }
    public bool AllowReservedWords { get; private init; }
    public HashSet<string>? ReservedWords { get; private init; }

    private LoginPolicy(
        int minLength, 
        int maxLength, 
        bool allowNumbers, 
        bool allowCapitalLetters, 
        bool allowSpecialCharacters, 
        string? specialCharacters, 
        bool allowReservedWords, 
        HashSet<string>? reservedWords)
    {
        MinLength = minLength;
        MaxLength = maxLength;
        AllowNumbers = allowNumbers;
        AllowCapitalLetters = allowCapitalLetters;
        AllowSpecialCharacters = allowSpecialCharacters;
        SpecialCharacters = specialCharacters;
        AllowReservedWords = allowReservedWords;
        ReservedWords = reservedWords;
    }
    
    public static Result<LoginPolicy> Create(
        int minLength, 
        int maxLength, 
        bool allowNumbers, 
        bool allowCapitalLetters, 
        bool allowSpecialCharacters, 
        string? specialCharacters, 
        bool allowReservedWords, 
        HashSet<string>? reservedWords)
    {
        var policy = new LoginPolicy(
            minLength, 
            maxLength, 
            allowNumbers,
            allowCapitalLetters, 
            allowSpecialCharacters,
            specialCharacters,
            allowReservedWords,
            reservedWords);
        
        var success = ValidateCreationParameters(policy);
        
        if (!success.IsSuccess)
            return success.Error;

        return policy;
    }

    private static Result ValidateCreationParameters(LoginPolicy policy)
    {
        if (policy.MinLength <= 0)
            return Error.LoginPolicy.InvalidMinNumber;
        
        if (policy.MaxLength <= policy.MinLength)
            return Error.LoginPolicy.InvalidNumbersValues;

        return Result.Success();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MinLength;
        yield return MaxLength;
        yield return AllowNumbers;
        yield return AllowCapitalLetters;
        yield return AllowSpecialCharacters;
        yield return SpecialCharacters ?? string.Empty;
        yield return AllowReservedWords;
        yield return string.Join(",", ReservedWords!.OrderBy(x => x));
    }

    public override string ToString() =>
        $"LoginPolicy(Min={MinLength}, Max={MaxLength}, " +
        $"Numbers={AllowNumbers}";
}