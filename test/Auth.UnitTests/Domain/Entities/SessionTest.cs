using Auth.Domain.Common;
using Auth.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.Entities;

public class SessionTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddDays(30); 
        var createdAt = DateTime.UtcNow; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.True(result.IsSuccess);
        var session = result.Value;
        Assert.NotNull(session);
        Assert.False(session.Id == Guid.Empty);
        Assert.False(session.UserId == Guid.Empty);
        Assert.False(session.DeviceId == Guid.Empty);
        Assert.False(string.IsNullOrEmpty(session.RefreshTokenHash));
        Assert.False(string.IsNullOrEmpty(session.IpAddress));
        Assert.False(createdAt == DateTime.MinValue);
        Assert.False(createdAt > DateTime.Now.AddMinutes(5));
        Assert.False(createdAt > expiresAt);
        
    }
    
    [Fact]
    public void Create_Id_Is_Empty_Invalid()
    {
        var id = Guid.Empty;
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddDays(30); 
        var createdAt = DateTime.UtcNow; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_User_Id_Is_Empty_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.Empty;
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddDays(30); 
        var createdAt = DateTime.UtcNow; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Device_Id_Is_Empty_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.Empty;
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddDays(30); 
        var createdAt = DateTime.UtcNow; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Device.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Refresh_Token_Hash_Is_Empty_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = "";
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddDays(30); 
        var createdAt = DateTime.UtcNow; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.RefreshTokenHashCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Ip_Address_Is_Empty_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "";
        var expiresAt = DateTime.UtcNow.AddDays(30); 
        var createdAt = DateTime.UtcNow; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.IpAddressCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Expires_At_Is_Max_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.MaxValue; 
        var createdAt = DateTime.UtcNow; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.InvalidExpirationDate, result.Error);
    }
    
    [Fact]
    public void Create_Expires_At_In_Past_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddMinutes(-10); 
        var createdAt = DateTime.UtcNow; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.ExpiresAtCannotBeInPast, result.Error);
    }
    
    [Fact]
    public void Create_Created_At_Is_Max_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddDays(30); 
        var createdAt = DateTime.MaxValue; 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.InvalidCreationDate, result.Error);
    }
    
    [Fact]
    public void Create_Creation_Date_In_Future_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddDays(30); 
        var createdAt = DateTime.UtcNow.AddHours(1); 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.CreationDateInFuture, result.Error);
    }
    
    [Fact]
    public void Create_Creation_Date_After_Expiration_Invalid()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var refreshTokenHash = Guid.NewGuid().ToString();
        var ipAddress = "127.0.0.1";
        var expiresAt = DateTime.UtcNow.AddMinutes(1); 
        var createdAt = DateTime.UtcNow.AddMinutes(2); 
        
        var result = Session.Create(id, userId, deviceId, refreshTokenHash, ipAddress, expiresAt, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.CreationDateAfterExpiration, result.Error);
    }
    
    [Fact]
    public void Revoke_Valid_Parameters_Valid()
    {
        var session = ConfiguratorSession();
        var createdAt = DateTime.UtcNow; 
        
        var result = session!.Revoke(createdAt);

        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void Revoke_Invalid_Revocation_Date_Invalid()
    {
        var session = ConfiguratorSession();
        var createdAt = DateTime.MaxValue; 
        
        var result = session!.Revoke(createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.InvalidRevocationDate,result.Error);
    }
    
    [Fact]
    public void Revoke_Revoked_Before_Creation_Invalid()
    {
        var session = ConfiguratorSession();
        var createdAt = DateTime.UtcNow.AddMinutes(-1); 
        
        var result = session!.Revoke(createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.RevokedBeforeCreation,result.Error);
    }
    
    [Fact]
    public void Revoke_Revocation_Date_In_Future_Invalid()
    {
        var session = ConfiguratorSession();
        var createdAt = DateTime.UtcNow.AddMinutes(6); 
        
        var result = session!.Revoke(createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.RevocationDateInFuture, result.Error);
    }
    
    [Fact]
    public void Revoke_Already_Revoked_Invalid()
    {
        var session = ConfiguratorSession();
        var createdAt = DateTime.UtcNow; 
        session!.Revoke(createdAt);
        
        var result = session.Revoke(createdAt);

        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.AlreadyRevoked, result.Error);
    }

    [Fact]
    public void Change_Refresh_Token_Hash_Valid_Parameters_Valid()
    {
        var session = ConfiguratorSession();
        var newRefreshTokenHash = Guid.NewGuid().ToString();
        var expiresAt =  DateTime.UtcNow.AddDays(30);

        var result = session!.ChangeRefreshTokenHash(newRefreshTokenHash, expiresAt);
        
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void Change_Refresh_Token_Hash_Cannot_Be_Empty_Invalid()
    {
        var session = ConfiguratorSession();
        var newRefreshTokenHash = "";
        var expiresAt =  DateTime.UtcNow.AddDays(30);

        var result = session!.ChangeRefreshTokenHash(newRefreshTokenHash, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.RefreshTokenHashCannotBeEmpty,result.Error);
    }
    
    [Fact]
    public void Change_Refresh_Token_Hash_Expires_At_Cannot_Be_In_Past_Invalid()
    {
        var session = ConfiguratorSession();
        var newRefreshTokenHash = Guid.NewGuid().ToString();
        var expiresAt =  DateTime.UtcNow.AddDays(-1);

        var result = session!.ChangeRefreshTokenHash(newRefreshTokenHash, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.ExpiresAtCannotBeInPast,result.Error);
    }
    
    [Fact]
    public void Change_Refresh_Token_Hash_Session_Revoked_Invalid()
    {
        var session = ConfiguratorSession();
        session!.Revoke(DateTime.UtcNow);
        var newRefreshTokenHash = Guid.NewGuid().ToString();
        var expiresAt = DateTime.UtcNow.AddDays(30);

        var result = session!.ChangeRefreshTokenHash(newRefreshTokenHash, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.Session.Revoked,result.Error);
    }

    private Session? ConfiguratorSession()
    {
        return Session.Create(
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            Guid.NewGuid().ToString(), 
            "127.0.0.1", 
            DateTime.UtcNow.AddDays(30), 
            DateTime.UtcNow).Value;
    }
}