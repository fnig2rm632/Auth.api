using Auth.Domain.Common;
using Auth.Domain.ValueObjects;
using static System.Text.RegularExpressions.Regex;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.ValueObject;

public class LoginTests
{
    [Fact]
    public void Create_Valid_Parameter_Valid()
    {
        var policy = ConfiguratorLoginPolicy(true, true, true, true);
        var loginText = "Qwerty123-";
        
        var result = Login.Create(loginText,policy);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var login = result.Value.Value;
        Assert.True(policy.MinLength <= login.Length);
        Assert.True(policy.MaxLength >= login.Length);
        Assert.DoesNotContain(login, char.IsWhiteSpace);
        Assert.False(!policy.AllowCapitalLetters && IsMatch(login, @"[A-Z]"));
        Assert.False(!policy.AllowNumbers && IsMatch(login, @"\d"));
        Assert.False(!policy.AllowSpecialCharacters && IsMatch(login, $@"{policy.SpecialCharacters}"));
        Assert.False(policy.AllowReservedWords && policy.ReservedWords!.Contains(login.ToLowerInvariant()));
    }
    
    [Fact]
    public void Create_Value_Is_Null_Invalid()
    {
        var policy = ConfiguratorLoginPolicy(true, true, true, true);
        var loginText = "Qwer ty123-";
        
        var result = Login.Create(loginText,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Login.ContainsWhitespace, result.Error);
    }
    
    [Fact]
    public void Create_Contains_White_Space_Invalid()
    {
        var policy = ConfiguratorLoginPolicy(true, true, true, true);
        var loginText = "";
        
        var result = Login.Create(loginText,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Login.ValueCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Invalid_Max_Length_Invalid()
    {
        var policy = ConfiguratorLoginPolicy(true, true, true, true);
        var loginText = "s";
        for (int i = 0; i < policy.MaxLength; i++)
        {
            loginText += "s";
        }
        
        var result = Login.Create(loginText,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Login.TooLong, result.Error);
    }
    
    [Fact]
    public void Create_Invalid_Min_Length_Invalid()
    {
        var policy = ConfiguratorLoginPolicy(true, true, true, true);
        var loginText = "s";
        
        var result = Login.Create(loginText,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Login.TooShort, result.Error);
    }
    
    [Fact]
    public void Create_Not_Allow_Number_Invalid()
    {
        var policy = ConfiguratorLoginPolicy(false, true, true, true);
        var loginText = "Qwerty123-";
        
        var result = Login.Create(loginText,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Login.InvalidNumbers, result.Error);
    }
    
    [Fact]
    public void Create_Not_Allow_Capital_Letters_Invalid()
    {
        var policy = ConfiguratorLoginPolicy(true, false, true, true);
        var loginText = "Qwerty123-";
        
        var result = Login.Create(loginText,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Login.InvalidCapitalLetters, result.Error);
    }
    
    [Fact]
    public void Create_Invalid_Special_Char_Invalid()
    {
        var policy = ConfiguratorLoginPolicy(true, true, false, true);
        var loginText = "Qwerty123_";
            
        var result = Login.Create(loginText,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Login.InvalidSpecialCharacters(policy.SpecialCharacters!), result.Error);
    }
    
    [Fact]
    public void Create_Invalid_Reserved_Words_Invalid()
    {
        var policy = ConfiguratorLoginPolicy(true, true, true, true);
        var loginText = "support";
            
        var result = Login.Create(loginText,policy);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Login.ContainsInvalidWords, result.Error);
    }
    
    private LoginPolicy ConfiguratorLoginPolicy(bool numbers, bool letters, bool specialChar, bool words)
    {
        return LoginPolicy.Create(
            3,
            20,
            numbers,
            letters,
            specialChar,
            "-_.",
            words,
            [
                "admin", "administrator", "root", "system", "support",
                "info", "contact", "help", "api", "auth", "login",
                "register", "password", "user", "users", "test"
            ]).Value!;
    }
}