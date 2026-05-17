using Auth.Domain.Common;

namespace Auth.Domain.Entities;

public class Provider : Entity<int>
{
    public string Name { get; private set; } = string.Empty;
    
    public ICollection<UserIdentity> UserIdentities { get; private set; } = new List<UserIdentity>();
    
    public static Result<Provider> Create(int id, string name)
    {
        var success = ValidateCreationParameters(id,name);

        if (!success.IsSuccess)
            return success.Error;
        
        return new Provider
        {
            Name = name
        };
    }

    private static Result ValidateCreationParameters(int id, string name)
    {
        if (id < 0)
            return Error.Provider.IdCannotBeNegative;
        
        if (string.IsNullOrWhiteSpace(name))
            return Error.Provider.NameCannotBeEmpty;

        if (name.Length > 255)
            return Error.Provider.NameTooLong;
        
        return Result.Success();
    }
}