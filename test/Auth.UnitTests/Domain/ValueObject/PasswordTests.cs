using Auth.Domain.Common;
using Auth.Domain.ValueObjects;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.ValueObject;

public class PasswordTests
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "!sst5s|Sta";
        
        var result = Password.Create(textPassword, policy);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var password = result.Value.Value;
        Assert.True(policy.MinLength <= password.Length);
        Assert.True(policy.MaxLength >= password.Length);
        Assert.False(policy.RequireLetters && !password.Any(char.IsLetter));
        Assert.False(policy.RequireDigit && !password.Any(char.IsDigit));
        Assert.False(policy.RequireLowercase && !password.Any(char.IsLower));
        Assert.False(policy.RequireUppercase && !password.Any(char.IsUpper));
        Assert.False(policy.RequireNonAlphanumeric && !password.Any(char.IsSymbol));
    }
    
    [Fact]
    public void Create_Value_Is_Null_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "";

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.ValueCannotBeEmpty, result.Error);
    }

    [Fact]
    public void Create_Length_Is_Small_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "";
        for (int i = 0; i < policy.MinLength - 1; i++)
        {
            textPassword += "*";
        }

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.TooShort(policy.MinLength), result.Error);
    }
    
    [Fact]
    public void Create_Length_Is_Bigger_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "";
        for (int i = 0; i < policy.MaxLength + 1; i++)
        {
            textPassword += "*";
        }

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.TooLong(policy.MaxLength), result.Error);
    }
    
    [Fact]
    public void Create_Contains_White_Space_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "!sst5 s<Sta";

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.ContainsWhitespace, result.Error);
    }
    
    [Fact]
    public void Create_Lack_Of_Letter_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "!44555}555";

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.LackOfLetter, result.Error);
    }
    
    [Fact]
    public void Create_Lack_Of_Digit_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "!sstss<Sta";

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.LackOfNumber, result.Error);
    }
    
    [Fact]
    public void Create_Lack_Of_Uppercase_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "!sst2s<5ta";

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.LackUppercase, result.Error);
    }
    
    [Fact]
    public void Create_Lack_Of_Lowercase_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "!SSS2S<5SA";

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.LackLowercase, result.Error);
    }
    
    [Fact]
    public void Create_Lack_Of_Special_Characters_Invalid()
    {
        var policy = ConfiguratorPasswordPolicy(true,true,true,true,true);
        var textPassword = "sSSS2Ss5SA";

        var result = Password.Create(textPassword,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Password.LackOfSpecialCharacters, result.Error);
    }
    
    private PasswordPolicy ConfiguratorPasswordPolicy(bool letter, bool digit, bool upper, bool lower, bool symbol)
    {
        return PasswordPolicy.Create(
            8,
            50,
            letter,
            digit,
            upper,
            lower,
            symbol,
            "!@#$%^&*()_+-=[]{}|;:,.<>?"
            ).Value!;
    }
}