using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.Domain.ValueObjects;
using Xunit;
using Assert = Xunit.Assert;

namespace Auth.UnitTests.Domain.Entities;

public class UserTest
{
    [Fact]
    public void Create_Valid_Parameter_Valid()
    {
        var id = Guid.NewGuid();
        var login = Login.Create("artem", ConfiguratorLoginPolicy()).Value;
        var passwordHash = "5c1468479ed2059de5a0cafdd32384a0";
        var email = Email.Create("email@gmail.com").Value;
        var createdAt = DateTime.UtcNow;

        var result = User.Create(id, login!, email!, passwordHash, createdAt);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var user = result.Value;
        Assert.False(user.Id == Guid.Empty);
        Assert.NotNull(user.Login);
        Assert.NotNull(user.PasswordHash);
        Assert.NotNull(user.Email);
        Assert.False(user.CreatedAt > DateTime.UtcNow.AddMinutes(5));
        
    }
    
    [Fact]
    public void Create_Id_Is_Empty_Invalid()
    {
        var id = Guid.Empty;
        var login = Login.Create("artem", ConfiguratorLoginPolicy()).Value;
        var passwordHash = "5c1468479ed2059de5a0cafdd32384a0";
        var email = Email.Create("email@gmail.com").Value;
        var createdAt = DateTime.UtcNow;

        var result = User.Create(id, login!, email!, passwordHash, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.IdCannotBeEmpty, result.Error);
    }
    
    [Fact]
    public void Create_Login_Is_Null_Invalid()
    {
        var id = Guid.NewGuid();
        Login? login = null;
        var passwordHash = "5c1468479ed2059de5a0cafdd32384a0";
        var email = Email.Create("email@gmail.com").Value;
        var createdAt = DateTime.UtcNow;

        var result = User.Create(id, login!, email!, passwordHash, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.LoginCannotBeNull, result.Error);
    }
    
    [Fact]
    public void Create_Password_Is_Null_Invalid()
    {
        var id = Guid.NewGuid();
        var login = Login.Create("artem", ConfiguratorLoginPolicy()).Value;
        string? passwordHash = null;
        var email = Email.Create("email@gmail.com").Value;
        var createdAt = DateTime.UtcNow;

        var result = User.Create(id, login!, email!, passwordHash!, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.PasswordHashCannotBeNull, result.Error);
    }
    
    [Fact]
    public void Create_Email_Is_Null_Invalid()
    {
        var id = Guid.NewGuid();
        var login = Login.Create("artem", ConfiguratorLoginPolicy()).Value;
        var passwordHash = "5c1468479ed2059de5a0cafdd32384a0";
        Email? email = null;
        var createdAt = DateTime.UtcNow;

        var result = User.Create(id, login!, email!, passwordHash, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.EmailCannotBeNull, result.Error);
    }
    
    [Fact]
    public void Create_Created_At_Date_In_Future_Invalid()
    {
        var id = Guid.NewGuid();
        var login = Login.Create("artem", ConfiguratorLoginPolicy()).Value;
        var passwordHash = "5c1468479ed2059de5a0cafdd32384a0";
        Email? email = Email.Create("email@gmail.com").Value;
        var createdAt = DateTime.UtcNow.AddMinutes(5);

        var result = User.Create(id, login!, email!, passwordHash, createdAt);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.CreatedAtInFuture, result.Error);
    }
    
    [Fact]
    public void ResetPassword_Valid_Parameter_Valid()
    {
        var id = Guid.NewGuid();
        var login = Login.Create("artem", ConfiguratorLoginPolicy()).Value;
        var passwordHash = "5c1468479ed2059de5a0cafdd32384a0";
        var newPasswordHash = "d32384ac14ad9de56847a0c9e50f205d";
        var email = Email.Create("email@gmail.com").Value;
        var createdAt = DateTime.UtcNow;
        var user = User.Create(id, login!, email!, passwordHash, createdAt).Value;
        
        var result = user!.ResetPassword(newPasswordHash);
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(user.PasswordHash);
        Assert.False(newPasswordHash == user.PasswordHash);
    }
    
    [Fact]
    public void ResetPassword_Password_Hash_Is_Empty_Invalid()
    {
        var id = Guid.NewGuid();
        var login = Login.Create("artem", ConfiguratorLoginPolicy()).Value;
        var passwordHash = "5c1468479ed2059de5a0cafdd32384a0";
        var newPasswordHash = "";
        var email = Email.Create("email@gmail.com").Value;
        var createdAt = DateTime.UtcNow;
        var user = User.Create(id, login!, email!, passwordHash, createdAt).Value;
        
        var result = user!.ResetPassword(newPasswordHash);
        
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.PasswordHashCannotBeNull, result.Error);
    }
    
    [Fact]
    public void ResetPassword_Password_Hash_Repeats_Invalid()
    {
        var id = Guid.NewGuid();
        var login = Login.Create("artem", ConfiguratorLoginPolicy()).Value;
        var passwordHash = "5c1468479ed2059de5a0cafdd32384a0";
        var newPasswordHash = "5c1468479ed2059de5a0cafdd32384a0";
        var email = Email.Create("email@gmail.com").Value;
        var createdAt = DateTime.UtcNow;
        var user = User.Create(id, login!, email!, passwordHash, createdAt).Value;
        
        var result = user!.ResetPassword(newPasswordHash);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.User.NewPasswordMustBeDifferent, result.Error);
    }
    
    private LoginPolicy ConfiguratorLoginPolicy()
    {
        return LoginPolicy.Create(
            3,
            20,
            true,
            true,
            true,
            "-_.",
            true,
            [
                "admin", "administrator", "root", "system", "support",
                "info", "contact", "help", "api", "auth", "login",
                "register", "password", "user", "users", "test"
            ]).Value!;
    }
    
}