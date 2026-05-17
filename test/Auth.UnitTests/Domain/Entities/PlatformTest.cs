using Auth.Domain.Common;
using Auth.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.Entities;

public class PlatformTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var name = "name";
        
        var result = Platform.Create(name);
        
        Assert.True(result.IsSuccess);
        var platform = result.Value;
        Assert.NotNull(platform);
        Assert.False(string.IsNullOrEmpty(name));
        Assert.True(name.Length < 255);
    }
    
    [Fact]
    public void Create_Name_Is_Empty_InValid()
    {
        var name = "";
        
        var result = Platform.Create(name);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Platform.NameCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Name_Is_To_Long_InValid()
    {
        var name = "";
        for (int i = 0; i < 256; i++)
            name += "~";
        
        var result = Platform.Create(name);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Platform.NameTooLong, result.Error);
    }
}