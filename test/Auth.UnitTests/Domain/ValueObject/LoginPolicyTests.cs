using Auth.Domain.Common;
using Auth.Domain.ValueObjects;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.ValueObject;

public class LoginPolicyTests
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var min = 3;
        var max = 20;
        var numbers = true;
        var letters = true;
        var isSpecialChar = true;
        var specialChar = "[]?";
        var isWords = true;
        HashSet<string> words =
        [
            "admin", "administrator", "root", "system", "support",
            "info", "contact", "help", "api", "auth", "login",
            "register", "password", "user", "users", "test"
        ];
        
        var result = LoginPolicy.Create(
            min,
            max,
            numbers,
            letters,
            isSpecialChar,
            specialChar,
            isWords,
            words
            );
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value!);
        var policy = result.Value;
        Assert.Equal(policy.MinLength, min);
        Assert.Equal(policy.MaxLength, max);
        Assert.Equal(policy.AllowNumbers, numbers);
        Assert.Equal(policy.AllowCapitalLetters, letters);
        Assert.False(!policy.AllowReservedWords == isWords && policy.ReservedWords == words);
        Assert.False(!policy.AllowSpecialCharacters == isSpecialChar && policy.SpecialCharacters == specialChar);
    }

    [Fact]
    public void Create_Invalid_Min_Value_Invalid()
    {
        var min = -1;
        var max = 20;
        var numbers = true;
        var letters = true;
        var isSpecialChar = true;
        var specialChar = "[]?";
        var isWords = true;
        HashSet<string> words =
        [
            "admin", "administrator", "root", "system", "support",
            "info", "contact", "help", "api", "auth", "login",
            "register", "password", "user", "users", "test"
        ];

        var result = LoginPolicy.Create(
            min,
            max,
            numbers,
            letters,
            isSpecialChar,
            specialChar,
            isWords,
            words
        );
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.LoginPolicy.InvalidMinNumber,result.Error);
    }

    [Fact]
    public void Create_Max_More_Than_Min_Invalid()
    {
        var min = 10;
        var max = 5;
        var numbers = true;
        var letters = true;
        var isSpecialChar = true;
        var specialChar = "[]?";
        var isWords = true;
        HashSet<string> words =
        [
            "admin", "administrator", "root", "system", "support",
            "info", "contact", "help", "api", "auth", "login",
            "register", "password", "user", "users", "test"
        ];

        var result = LoginPolicy.Create(
            min,
            max,
            numbers,
            letters,
            isSpecialChar,
            specialChar,
            isWords,
            words
        );
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.LoginPolicy.InvalidNumbersValues,result.Error);
    }
}