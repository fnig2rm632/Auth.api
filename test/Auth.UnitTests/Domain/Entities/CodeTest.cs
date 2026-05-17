using Auth.Domain.Common;
using Auth.Domain.Entities;
using StackExchange.Redis;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.Entities;

public class CodeTest
{
    [Fact]
    public void Create_Valid_Parameter_Valid()
    {
        var value = "123456";
        var сreatedAt = DateTime.UtcNow;

        var result = Code.Create(value, сreatedAt);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var code = result.Value;
        Assert.Equal(code.Value, value);
        Assert.False(code.CreatedAt > DateTime.UtcNow.AddMinutes(1));
        Assert.False(code.IsConfirm);
    }
    
    [Fact]
    public void Create_Code_Is_Empty_Invalid()
    {
        var value = "";
        var сreatedAt = DateTime.UtcNow;

        var result = Code.Create(value, сreatedAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Code.ValueCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Created_At_Date_In_Future_Invalid()
    {
        var value = "123456";
        var сreatedAt = DateTime.UtcNow.AddDays(1);

        var result = Code.Create(value, сreatedAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Code.CreatedAtInFuture, result.Error);
    }
    
    [Fact]
    public void Confirm_Valid()
    {
        var value = "123456";
        var сreatedAt = DateTime.UtcNow;
        var code = Code.Create(value, сreatedAt).Value;

        var result = code!.Confirm();
        
        Assert.True(result.IsSuccess);
        Assert.True(code.IsConfirm);
    }
    
    [Fact]
    public void Confirm_Invalid()
    {
        var value = "123456";
        var сreatedAt = DateTime.UtcNow;
        var code = Code.Create(value, сreatedAt).Value;
        code!.Confirm();

        var result = code.Confirm();
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Code.AlreadyRevoked, result.Error);
    }
    
    [Fact]
    public void Create_From_Hash_Valid_Parameter_Valid()
    {
        var value = "123456";
        var сreatedAt = DateTime.UtcNow;
        var codeTest = Code.Create(value, сreatedAt).Value;
        var hashCode = codeTest!.GetHashEntry().Value;

        var result = Code.Create(hashCode);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var code = result.Value;
        Assert.Equal(code.Value, value);
        Assert.False(code.CreatedAt > DateTime.Now.AddMinutes(1));
        Assert.False(code.IsConfirm);
    }
    
    [Fact]
    public void Create_From_Hash_Hash_Code_Is_Empty_Invalid()
    {
        HashEntry[]? hashCode = null;

        var result = Code.Create(hashCode);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Code.InvalidHashEntry, result.Error);
    }
    
    [Fact]
    public void GetHashEntry_Valid()
    {
        var value = "123456";
        var сreatedAt = DateTime.UtcNow;
        var codeTest = Code.Create(value, сreatedAt).Value;
        
        var result = codeTest!.GetHashEntry();
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var code = Code.Create(result.Value).Value;
        Assert.NotNull(code!.Value);
        Assert.False(code.CreatedAt > DateTime.Now.AddMinutes(1));
        Assert.False(code.IsConfirm);
    }
}