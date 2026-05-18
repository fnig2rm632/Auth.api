using System;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.Entities;

public class DeviceTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = Guid.NewGuid();
        var platformId = 1;
        
        var result = Device.Create(id, platformId);
        
        Assert.True(result.IsSuccess);
        var device = result.Value;
        Assert.NotNull(device); 
        Assert.False(device.Id == Guid.Empty);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.True(platformId > -1);
    }
    
    [Fact]
    public void Create_Is_Empty_Id_Invalid()
    {
        var id = Guid.Empty;
        var platformId = 1;
        
        var result = Device.Create(id, platformId);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Device.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Is_Empty_Platform_Id_Invalid()
    {
        var id = Guid.NewGuid();
        var platformId = -1;
        
        var result = Device.Create(id, platformId);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Platform.IdCannotBeNegative, result.Error);
    }
}