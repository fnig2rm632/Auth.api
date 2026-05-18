using System;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.Entities;

public class OAuthTokenTest
{
    [Fact]
    public void Create_Valid_Parameters_Valid()
    {
        var id = Guid.NewGuid();
        var userIdentityId = Guid.NewGuid();
        var accessToken = Guid.NewGuid().ToString();
        var refreshToken = Guid.NewGuid().ToString();
        var expiresAt = DateTime.Now.AddDays(30);
        
        var result = OAuthToken.Create(id, userIdentityId, accessToken, refreshToken, expiresAt);
        
        Assert.True(result.IsSuccess);
        var token = result.Value;
        Assert.NotNull(token); 
        Assert.False(token.Id == Guid.Empty);
        Assert.False(token.UserIdentityId == Guid.Empty);
        Assert.False(string.IsNullOrEmpty(token.AccessToken));
        Assert.False(string.IsNullOrEmpty(token.RefreshToken));
        Assert.False(token.ExpiresAt == DateTime.MinValue);
        Assert.True(DateTime.Now < token.ExpiresAt);
    }
    
    [Fact]
    public void Create_Is_Empty_Id_Invalid()
    {
        var id = Guid.Empty;
        var userIdentityId = Guid.NewGuid();
        var accessToken = Guid.NewGuid().ToString();
        var refreshToken = Guid.NewGuid().ToString();
        var expiresAt = DateTime.Now.AddDays(30);
        
        var result = OAuthToken.Create(id, userIdentityId, accessToken, refreshToken, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.OAuthToken.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Is_Empty_User_Identity_Id_Invalid()
    {
        var id = Guid.NewGuid();
        var userIdentityId = Guid.Empty;
        var accessToken = Guid.NewGuid().ToString();
        var refreshToken = Guid.NewGuid().ToString();
        var expiresAt = DateTime.Now.AddDays(30);
        
        var result = OAuthToken.Create(id, userIdentityId, accessToken, refreshToken, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.UserIdentity.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Is_Empty_Access_Token_Invalid()
    {
        var id = Guid.NewGuid();
        var userIdentityId = Guid.NewGuid();
        var accessToken = "";
        var refreshToken = Guid.NewGuid().ToString();
        var expiresAt = DateTime.Now.AddDays(30);
        
        var result = OAuthToken.Create(id, userIdentityId, accessToken, refreshToken, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.OAuthToken.AccessTokenCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Is_Empty_Refresh_Token_Invalid()
    {
        var id = Guid.NewGuid();
        var userIdentityId = Guid.NewGuid();
        var accessToken = Guid.NewGuid().ToString();
        var refreshToken = "";
        var expiresAt = DateTime.Now.AddDays(30);
        
        var result = OAuthToken.Create(id, userIdentityId, accessToken, refreshToken, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.OAuthToken.RefreshTokenCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Expires_Is_Min_Invalid()
    {
        var id = Guid.NewGuid();
        var userIdentityId = Guid.NewGuid();
        var accessToken = Guid.NewGuid().ToString();
        var refreshToken = Guid.NewGuid().ToString();
        var expiresAt = DateTime.MaxValue;
        
        var result = OAuthToken.Create(id, userIdentityId, accessToken, refreshToken, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.OAuthToken.InvalidExpirationDate, result.Error);
    }
    
    [Fact]
    public void Create_Expires_In_Past_Invalid()
    {
        var id = Guid.NewGuid();
        var userIdentityId = Guid.NewGuid();
        var accessToken = Guid.NewGuid().ToString();
        var refreshToken = Guid.NewGuid().ToString();
        var expiresAt = DateTime.Now.AddDays(-1);
        
        var result = OAuthToken.Create(id, userIdentityId, accessToken, refreshToken, expiresAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.OAuthToken.ExpiresAtCannotBeInPast, result.Error);
    }
}