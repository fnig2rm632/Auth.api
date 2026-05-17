using Auth.Domain.Common;
using Auth.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.Entities;

public class ProviderTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = 1;
        var name = "name";
        
        var result = Provider.Create(id, name);
        
        Assert.True(result.IsSuccess);
        var provider = result.Value;
        Assert.NotNull(provider);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.True(id > -1);
        Assert.False(string.IsNullOrEmpty(name));
        Assert.True(name.Length < 255);
    }

    [Fact]
    public void Create_Id_Is_Negative_InValid()
    {
        var id = -1;
        var name = "name";
        
        var result = Provider.Create(id, name);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Provider.IdCannotBeNegative, result.Error);
    }
    
    [Fact]
    public void Create_Name_Is_Empty_InValid()
    {
        var id = 1;
        var name = "";
        
        var result = Provider.Create(id, name);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Provider.NameCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Name_Is_To_Long_InValid()
    {
        var id = 1;
        var name = "";
        for (int i = 0; i < 256; i++)
            name += "~";
        
        var result = Provider.Create(id, name);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Provider.NameTooLong, result.Error);
    }
}