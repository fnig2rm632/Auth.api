using Auth.Domain.Common;
using Auth.Domain.ValueObjects;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.ValueObject;

public class PasswordPolicyTests
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var minLength = 8;
        var maxLength = 50;
        var letter = true;
        var digit = true;
        var upper = true;
        var lower = true;
        var symbol = true;
        var simbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var result = PasswordPolicy.Create(
            minLength,
            maxLength,
            letter,
            digit,
            upper,
            lower,
            symbol,
            simbols);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var policy = result.Value;
        Assert.Equal(policy.MinLength, minLength);
        Assert.Equal(policy.MaxLength, maxLength);
        Assert.Equal(policy.RequireLetters , letter);
        Assert.Equal(policy.RequireDigit, digit);
        Assert.Equal(policy.RequireUppercase, upper);
        Assert.Equal(policy.RequireLowercase, lower);
        Assert.Equal(policy.RequireNonAlphanumeric, symbol);
    }
    
    [Fact]
    public void Create_Not_Allow_Signe_Invalid()
    {
        int minLength = 8;
        int maxLength = 50;
        bool letter = false; 
        bool digit = false;
        bool upper = false;
        bool lower = false;
        bool symbol = false;
        var simbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var result = PasswordPolicy.Create(
            minLength,
            maxLength,
            letter,
            digit,
            upper,
            lower,
            symbol,
            simbols);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.PasswordPolicy.NoOneTypeChose, result.Error);
    }
    
    [Fact]
    public void Create_Min_Length_Is_Small_Invalid()
    {
        int minLength = 2;
        int maxLength = 50;
        bool letter = false; 
        bool digit = false;
        bool upper = false;
        bool lower = false;
        bool symbol = false;
        var simbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var result = PasswordPolicy.Create(
            minLength,
            maxLength,
            letter,
            digit,
            upper,
            lower,
            symbol,
            simbols);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.PasswordPolicy.MinLengthIsSmall, result.Error);
    }
    
    [Fact]
    public void Create_Max_Length_Is_Big_Invalid()
    {
        int minLength = 8;
        int maxLength = 256;
        bool letter = false; 
        bool digit = false;
        bool upper = false;
        bool lower = false;
        bool symbol = false;
        var simbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var result = PasswordPolicy.Create(
            minLength,
            maxLength,
            letter,
            digit,
            upper,
            lower,
            symbol,
            simbols);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.PasswordPolicy.MaxLengthIsBig, result.Error);
    }
    
    [Fact]
    public void Create_Min_More_Then_Max_Invalid()
    {
        int minLength = 8;
        int maxLength = 2;
        bool letter = false; 
        bool digit = false;
        bool upper = false;
        bool lower = false;
        bool symbol = false;
        var simbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var result = PasswordPolicy.Create(
            minLength,
            maxLength,
            letter,
            digit,
            upper,
            lower,
            symbol,
            simbols);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.PasswordPolicy.MinCannotBeMoreMax, result.Error);
    }
}