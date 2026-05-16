using Auth.Domain.Common;

namespace Auth.Domain.ValueObjects;

public class PasswordPolicy : ValueObject
{
    public int MinLength { get; private init; }
    public int MaxLength { get; private init; }
    public bool RequireLetters { get; private init; }
    public bool RequireDigit { get; private init; }
    public bool RequireUppercase { get; private init; }
    public bool RequireLowercase { get; private init; }
    public bool RequireNonAlphanumeric { get; private init; } 
    public string RequireSimbol {get; private init;}
    
    private PasswordPolicy(
        int minLength, 
        int maxLength, 
        bool requireLetters, 
        bool requireDigit, 
        bool requireUppercase, 
        bool requireLowercase, 
        bool requireNonAlphanumeric,
        string requireSimbol)
    {
        MinLength = minLength;
        MaxLength = maxLength;
        RequireLetters = requireLetters;
        RequireDigit = requireDigit;
        RequireUppercase = requireUppercase;
        RequireLowercase = requireLowercase;
        RequireNonAlphanumeric = requireNonAlphanumeric;
        RequireSimbol = requireSimbol;
    }

    public static Result<PasswordPolicy> Create(
        int minLength,
        int maxLength, 
        bool requireLetters, 
        bool requireDigit, 
        bool requireUppercase, 
        bool requireLowercase, 
        bool requireNonAlphanumeric,
        string requireSimbol)
    {
        var policy = new PasswordPolicy(
            minLength, 
            maxLength, 
            requireLetters, 
            requireDigit, 
            requireUppercase, 
            requireLowercase, 
            requireNonAlphanumeric,
            requireSimbol);
        
        var success = ValidateCreationParameters(policy);
        
        if (!success.IsSuccess)
            return success.Error;

        return policy;
    }
    
    private static Result ValidateCreationParameters(PasswordPolicy policy)
    {
        if (policy.MinLength < 4)
            return Error.PasswordPolicy.MinLengthIsSmall;
            
        if (policy.MaxLength > 255)
            return Error.PasswordPolicy.MaxLengthIsBig;
        
        if (policy.MinLength > policy.MaxLength)
            return Error.PasswordPolicy.MinCannotBeMoreMax;
        
        if (policy is
            {
                RequireDigit: false, 
                RequireUppercase: false, 
                RequireLowercase: false, 
                RequireNonAlphanumeric: false, 
                RequireLetters: false
            })
            return Error.PasswordPolicy.NoOneTypeChose;

        if (policy.RequireNonAlphanumeric && string.IsNullOrEmpty(policy.RequireSimbol))
            return Error.PasswordPolicy.RequireSimbolIsNullOrEmpty;
            

        return Result.Success();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MinLength;
        yield return MaxLength;
        yield return RequireDigit;
        yield return RequireLetters;
        yield return RequireUppercase;
        yield return RequireLowercase;
        yield return RequireNonAlphanumeric; 
        yield return RequireSimbol;
    }
    
    public override string ToString() => 
        $"Length: {MinLength}-{MaxLength} , " +
        $"RequireLetters {RequireLetters} , " +
        $"RequireDigit {RequireDigit} , " +
        $"RequireUppercase {RequireUppercase} , " +
        $"RequireLowercase {RequireLowercase}" +
        $"RequireNonAlphanumeric {RequireNonAlphanumeric}" +
        $"RequireSimbol {RequireSimbol}";
}