using Auth.Domain.Common;
using Auth.Domain.ValueObjects;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.ValueObject;

public class EmailTests
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var textEmail = "AdReSs@MaIl.Ru";
        
        var result = Email.Create(textEmail);
        
        Assert.True(result.IsSuccess);
        var email = result.Value;
        Assert.NotNull(email); 
        Assert.Equal(textEmail.ToLower(), email.ToString());
    }
    
    [Fact]
    public void Create_Is_Empty_Invalid()
    {
        var textEmail = "";
        
        var result = Email.Create(textEmail);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Email.ValueCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Is_Long_Invalid()
    {
        var textEmail = "address" ;
        for (int i = 0; i < 40; i++)
            textEmail += "address";
        textEmail += "@mail.ru";
        
        var result = Email.Create(textEmail);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Email.TooLong(textEmail), result.Error);
    }
    
    [Fact]
    public void Create_Is_Not_Email_Invalid()
    {
        var textEmail = "address" ;
        
        var result = Email.Create(textEmail);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Email.InvalidAddress(textEmail), result.Error);
    }
}