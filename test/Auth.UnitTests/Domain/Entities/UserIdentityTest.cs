using Auth.Domain.Common;
using Auth.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.Entities;

public class UserIdentityTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var providerId = 1;
        var providerUserId = "providerUserId";
        var createdAt = DateTime.UtcNow;
        
        var result = UserIdentity.Create(id, userId, providerId, providerUserId, createdAt);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var login = result.Value;
        Assert.False(login.Id == Guid.Empty);
        Assert.False(login.UserId == Guid.Empty);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.False(login.ProviderId < 0);
        Assert.False(string.IsNullOrEmpty(login.ProviderUserId));
        Assert.True(login.ProviderUserId.Length < 256);
        Assert.True(login.CreatedAt < DateTime.UtcNow.AddMinutes(5));
    }

    [Fact]
    public void Create_Id_Cannot_Be_Empty_Invalid()
    {
        var id = Guid.Empty;
        var userId = Guid.NewGuid();
        var providerId = 1;
        var providerUserId = "providerUserId";
        var createdAt = DateTime.UtcNow;

        var result = UserIdentity.Create(id, userId, providerId, providerUserId, createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserIdentity.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_User_Id_Cannot_Be_Empty_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.Empty;
        var providerId = 1;
        var providerUserId = "providerUserId";
        var createdAt = DateTime.UtcNow;

        var result = UserIdentity.Create(id, userId, providerId, providerUserId, createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Provider_Id_Cannot_Be_Negative_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var providerId = -1;
        var providerUserId = "providerUserId";
        var createdAt = DateTime.UtcNow;

        var result = UserIdentity.Create(id, userId, providerId, providerUserId, createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Provider.IdCannotBeNegative, result.Error);
    }
    
    [Fact]
    public void Create_Provider_User_Id_Cannot_Be_Empty_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var providerId = 1;
        var providerUserId = "";
        var createdAt = DateTime.UtcNow;

        var result = UserIdentity.Create(id, userId, providerId, providerUserId, createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserIdentity.ProviderUserIdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Provider_User_Id_Too_Long_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var providerId = 1;
        var providerUserId = "";
        for (int i = 0; i < 256; i++)
            providerUserId += "#";
        var createdAt = DateTime.UtcNow;

        var result = UserIdentity.Create(id, userId, providerId, providerUserId, createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserIdentity.ProviderUserIdTooLong, result.Error);
    }
    
    [Fact]
    public void Create_Created_At_In_Future_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var providerId = 1;
        var providerUserId = "providerUserId";
        var createdAt = DateTime.UtcNow.AddDays(1);

        var result = UserIdentity.Create(id, userId, providerId, providerUserId, createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserIdentity.CreatedAtInFuture, result.Error);
    }
}